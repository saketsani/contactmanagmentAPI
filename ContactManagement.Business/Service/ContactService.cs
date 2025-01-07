using ContactManagement.Business.Interfaces;
using ContactManagement.Data.Models;
using ContactManagement.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Business.Service
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _repository;

        public ContactService(IContactRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<Contact>> GetAllContactsAsync()
        {
            return _repository.GetAllContactsAsync();
        }

        public Task<Contact?> GetContactByIdAsync(int id)
        {
            return _repository.GetContactByIdAsync(id);
        }

        public Task AddContactAsync(Contact contact)
        {
            return _repository.AddContactAsync(contact);
        }

        public Task UpdateContactAsync(Contact contact)
        {
            return _repository.UpdateContactAsync(contact);
        }

        public Task DeleteContactAsync(int id)
        {
            return _repository.DeleteContactAsync(id);
        }
    }
}
