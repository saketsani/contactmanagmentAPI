using contactmanagmentAPI.Controllers;
using contactmanagmentAPI.Models;
using contactmanagmentAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit;

namespace ContactApp.Tests
{
    [TestFixture]
    public class ContactsControllerTests
    {
        private Mock<IContactRepository> _mockRepository;
        private ContactsController _controller;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new Mock<IContactRepository>();
            _controller = new ContactsController(_mockRepository.Object);
        }

        [Test]
        public async Task GetAllContacts_ReturnsOkResult_WithContacts()
        {
            // Arrange
            var contacts = new List<Contact>
            {
                new Contact { Id = 1, Firstname = "saket", Lastname = "sani", Email = "saketsani123@gmail.com" },
               new Contact { Id = 1, Firstname = "sourav", Lastname = "kumar", Email = "souravkumar@gmail.com" },
            };
            _mockRepository.Setup(repo => repo.GetAllContactsAsync()).ReturnsAsync(contacts);

            // Act
            var result = await _controller.GetAllContacts();

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnValue = okResult.Value as IEnumerable<Contact>;
            Assert.AreEqual(2, returnValue.Count());
        }


        [Test]
        public async Task AddContact_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var newContact = new Contact { Id = null, Firstname = "saket", Lastname = "sani", Email = "saketsani123@gmail.com" };
            _mockRepository.Setup(repo => repo.AddContactAsync(It.IsAny<Contact>())).Returns(Task.CompletedTask);
            _controller.ModelState.Clear(); // Ensure ModelState is clear before setting it

            // Act
            var result = await _controller.AddContact(newContact);

            // Assert
            var createdAtActionResult = result as CreatedAtActionResult;
            Assert.IsNotNull(createdAtActionResult);
            Assert.AreEqual("GetContact", createdAtActionResult.ActionName);
        }

        [Test]
        public async Task UpdateContact_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var updatedContact = new Contact { Id = 1, Firstname = "hhh", Lastname = "dzdzs", Email = "saketsani1@gmail.com" };
            _mockRepository.Setup(repo => repo.UpdateContactAsync(It.IsAny<Contact>())).Returns(Task.CompletedTask);


            // Act
            var result = await _controller.UpdateContact(1, updatedContact);

            // Assert
            var createdAtActionResult = result as CreatedAtActionResult;
            Assert.IsNotNull(createdAtActionResult);
            Assert.AreEqual("GetContact", createdAtActionResult.ActionName);
        }

        [Test]
        public async Task DeleteContact_ReturnsNoContentResult()
        {
            // Arrange
            var contactId = 1;
            var contact = new Contact { Id = 1, Firstname = "saket", Lastname = "sani", Email = "saketsani123@gmail.com" };
            _mockRepository.Setup(repo => repo.GetContactByIdAsync(contactId)).ReturnsAsync(contact);
            _mockRepository.Setup(repo => repo.DeleteContactAsync(contactId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteContact(contactId);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }

    }



}
