using contactmanagmentAPI.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace contactmanagmentAPI.Repositories
{
    public class ContactRepository: IContactRepository
    {
        private readonly string _jsonFilePath;

        public ContactRepository(IOptions<Data> options)
        {
            _jsonFilePath = options.Value.ContactList
                         ?? throw new ArgumentException("JSON file path not configured.");
        }

        public async Task<IEnumerable<Contact>> GetAllContactsAsync()
        {
            if (!File.Exists(_jsonFilePath)) return new List<Contact>();
            var json = await File.ReadAllTextAsync(_jsonFilePath);
            return JsonSerializer.Deserialize<List<Contact>>(json) ?? new List<Contact>();
        }

        public async Task<Contact?> GetContactByIdAsync(int id)
        {
            var contacts = await GetAllContactsAsync();
            return contacts.FirstOrDefault(c => c.Id == id);
        }

        public async Task AddContactAsync(Contact contact)
        {
            var contacts = (await GetAllContactsAsync()).ToList();
            contact.Id = contacts.Any() ? contacts.Max(c => c.Id) + 1 : 1;
            contacts.Add(contact);
            await SaveContactsAsync(contacts);
        }

        public async Task UpdateContactAsync(Contact contact)
        {
            var contacts = (await GetAllContactsAsync()).ToList();
            var index = contacts.FindIndex(c => c.Id == contact.Id);
            if (index != -1)
            {
                contacts[index] = contact;
                await SaveContactsAsync(contacts);
            }
        }

        public async Task DeleteContactAsync(int id)
        {
            var contacts = (await GetAllContactsAsync()).ToList();
            contacts.RemoveAll(c => c.Id == id);
            await SaveContactsAsync(contacts);
        }

        private async Task SaveContactsAsync(IEnumerable<Contact> contacts)
        {
            var json = JsonSerializer.Serialize(contacts, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_jsonFilePath, json);
        }
    }
    }
