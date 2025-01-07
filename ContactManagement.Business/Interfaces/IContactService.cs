using ContactManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Business.Interfaces
{
    public interface IContactService
    {
        Task<IEnumerable<Contact>> GetAllContactsAsync();
        Task<Contact?> GetContactByIdAsync(int id);
        Task AddContactAsync(Contact contact);
        Task UpdateContactAsync(Contact contact);
        Task DeleteContactAsync(int id);
    }
}
