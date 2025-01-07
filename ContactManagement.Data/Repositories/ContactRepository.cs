using ContactManagement.Data.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace ContactManagement.Data.Repositories
{
    /// <summary>
    /// Repository class responsible for managing contacts data.
    /// Handles CRUD operations by reading from and writing to a JSON file.
    /// </summary>
    public class ContactRepository : IContactRepository
    {
        private readonly string _jsonFilePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactRepository"/> class.
        /// </summary>
        /// <param name="options">The configuration options containing the path to the contacts JSON file.</param>
        /// <exception cref="ArgumentException">Thrown if the JSON file path is not configured.</exception>
        public ContactRepository(IOptions<ContactData> options)
        {
            if (options.Value.ContactList == null)
            {
                throw new ArgumentException("JSON file path not configured.");
            }
            _jsonFilePath = options.Value.ContactList;
        }

        /// <summary>
        /// Retrieves all contacts from the JSON file asynchronously.
        /// </summary>
        /// <returns>A collection of contacts.</returns>
        public async Task<IEnumerable<Contact>> GetAllContactsAsync()
        {
            if (!File.Exists(_jsonFilePath)) return new List<Contact>();
            var json = await File.ReadAllTextAsync(_jsonFilePath);
            return JsonSerializer.Deserialize<List<Contact>>(json) ?? new List<Contact>();
        }

        /// <summary>
        /// Retrieves a contact by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the contact.</param>
        /// <returns>The contact with the specified ID or null if not found.</returns>
        public async Task<Contact?> GetContactByIdAsync(int id)
        {
            var contacts = await GetAllContactsAsync();
            return contacts.FirstOrDefault(c => c.Id == id);
        }

        /// <summary>
        /// Adds a new contact to the list and saves it to the JSON file asynchronously.
        /// </summary>
        /// <param name="contact">The contact to be added.</param>
        public async Task AddContactAsync(Contact contact)
        {
            var contacts = (await GetAllContactsAsync()).ToList();
            contact.Id = contacts.Any() ? contacts.Max(c => c.Id) + 1 : 1;
            contacts.Add(contact);
            await SaveContactsAsync(contacts);
        }

        /// <summary>
        /// Updates an existing contact in the list and saves it to the JSON file asynchronously.
        /// </summary>
        /// <param name="contact">The contact with updated data.</param>
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

        /// <summary>
        /// Deletes a contact from the list and saves the updated list to the JSON file asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the contact to be deleted.</param>
        public async Task DeleteContactAsync(int id)
        {
            var contacts = (await GetAllContactsAsync()).ToList();
            contacts.RemoveAll(c => c.Id == id);
            await SaveContactsAsync(contacts);
        }

        /// <summary>
        /// Saves the list of contacts to the JSON file asynchronously.
        /// </summary>
        /// <param name="contacts">The collection of contacts to be saved.</param>
        private async Task SaveContactsAsync(IEnumerable<Contact> contacts)
        {
            var json = JsonSerializer.Serialize(contacts, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_jsonFilePath, json);
        }
    }
}
