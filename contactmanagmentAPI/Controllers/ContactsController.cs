using contactmanagmentAPI.Models;
using contactmanagmentAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace contactmanagmentAPI.Controllers
{
    /// <summary>
    /// Controller responsible for managing contacts.
    /// Provides endpoints for CRUD operations on contacts.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IContactRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactsController"/> class.
        /// </summary>
        /// <param name="repository">The contact repository to interact with the data layer.</param>
        public ContactsController(IContactRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Retrieves a list of all contacts.
        /// </summary>
        /// <returns>A list of all contacts in the system.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllContacts()
        {
            var contacts = await _repository.GetAllContactsAsync();
            return Ok(contacts);
        }

        /// <summary>
        /// Retrieves a contact by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the contact.</param>
        /// <returns>The contact with the specified ID if found.</returns>
        /// <response code="200">Returns the contact details if found.</response>
        /// <response code="404">If the contact is not found.</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetContact(int id)
        {
            var contact = await _repository.GetContactByIdAsync(id);
            if (contact == null)
                throw new KeyNotFoundException("Contact not found.");

            return Ok(contact);
        }

        /// <summary>
        /// Adds a new contact to the list.
        /// </summary>
        /// <param name="contact">The contact data to be added.</param>
        /// <returns>A response with the newly created contact.</returns>
        /// <response code="201">Returns the created contact.</response>
        /// <response code="400">If the input data is invalid.</response>
        [HttpPost]
        public async Task<IActionResult> AddContact([FromBody] Contact contact)
        {
            if (!ModelState.IsValid)
                throw new ValidationException("Invalid input.");

            await _repository.AddContactAsync(contact);
            return CreatedAtAction(nameof(GetContact), new { id = contact.Id }, contact);
        }

        /// <summary>
        /// Updates existing contact.
        /// </summary>
        /// <param name="id">The unique identifier of the contact to be updated.</param>
        /// <param name="contact">The contact data to update.</param>
        /// <returns>The updated contact data.</returns>
        /// <response code="200">Returns the updated contact.</response>
        /// <response code="400">If the input data is invalid or ID mismatch.</response>
        /// <response code="404">If the contact is not found.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContact(int id, [FromBody] Contact contact)
        {
            if (id != contact.Id)
                throw new ArgumentException("ID mismatch.");

            if (!ModelState.IsValid)
                throw new ValidationException("Invalid input.");

            await _repository.UpdateContactAsync(contact);
            return CreatedAtAction(nameof(GetContact), new { id = contact.Id }, contact);
        }

        /// <summary>
        /// Deletes an existing contact.
        /// </summary>
        /// <param name="id">The unique identifier of the contact to be deleted.</param>
        /// <returns>A no content response indicating successful deletion.</returns>
        /// <response code="204">If the contact is successfully deleted.</response>
        /// <response code="404">If the contact is not found.</response>
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
