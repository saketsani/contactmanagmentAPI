using contactmanagmentAPI.Models;
using contactmanagmentAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace contactmanagmentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IContactRepository _repository;

        public ContactsController(IContactRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllContacts()
        {
            var contacts = await _repository.GetAllContactsAsync();
            return Ok(contacts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContact(int id)
        {
            var contact = await _repository.GetContactByIdAsync(id);
            if (contact == null)
                throw new KeyNotFoundException("Contact not found.");

            return Ok(contact);
        }

        [HttpPost]
        public async Task<IActionResult> AddContact([FromBody] Contact contact)
        {
            if (!TryValidateModel(contact))
                throw new ValidationException("Invalid input.");

            await _repository.AddContactAsync(contact);
            return CreatedAtAction(nameof(GetContact), new { id = contact.Id }, contact);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContact(int id, [FromBody] Contact contact)
        {
            if (id != contact.Id)
                throw new ArgumentException("ID mismatch.");

            if (!TryValidateModel(contact))
                throw new ValidationException("Invalid input.");

            await _repository.UpdateContactAsync(contact);

            return CreatedAtAction(nameof(GetContact), new { id = contact.Id }, contact);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            var contact = await _repository.GetContactByIdAsync(id);
            if (contact == null)
                throw new KeyNotFoundException("Contact not found.");

            await _repository.DeleteContactAsync(id);
            return NoContent();
        }
    }
}
