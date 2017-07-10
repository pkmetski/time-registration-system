using Model;
using Model.Arguments;
using Moq;
using NUnit.Framework;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Hosting;
using TimeRegistrationSystem.Controllers;

namespace TimeRegistrationSystem.UnitTest
{
    [TestFixture]
    public class RegistrationsControllerTest
    {
        private RegistrationsController _registrationsController;
        private Mock<IEntityService<Registration>> _registrationServiceMock;

        [SetUp]
        public void Setup()
        {
            _registrationServiceMock = new Mock<IEntityService<Registration>>();
            _registrationsController = new RegistrationsController(_registrationServiceMock.Object);
            _registrationsController.Request = new HttpRequestMessage()
            {
                Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }
            };
        }

        [Test]
        public void WhenRegistrationAreRetrievedByArgumentsThenCorrectValuesAreReturned()
        {
            //Arrange
            var fromDate = new DateTime(2000, 1, 1);
            var toDate = new DateTime(2020, 2, 2);
            var project = "proj";
            var customer = "cust";
            var registrations = new List<Registration>();
            _registrationServiceMock
                .Setup(rsm => rsm.Get(It.IsAny<QueryArguments>()))
                .Returns(registrations);

            //Act
            var result = _registrationsController.GetByArgs(fromDate, toDate, project, customer);

            //Assert
            _registrationServiceMock.Verify(rsm => rsm.Get(
                It.Is<QueryArguments>(qa => qa.FromDate == fromDate &&
                qa.ToDate == toDate &&
                qa.Project == project &&
                qa.Customer == customer)), Times.Once);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(result.Content.ReadAsAsync<IEnumerable<Registration>>().Result, Is.EqualTo(registrations));
        }

        [Test]
        public void WhenRegistrationAreRetrievedByArgumentsAndExceptionOccursThenCorrectResponseIsReturned()
        {
            //Arrange
            _registrationServiceMock
                .Setup(rsm => rsm.Get(It.IsAny<QueryArguments>()))
                .Throws(new Exception());

            //Act
            var result = _registrationsController.GetByArgs(new DateTime(), new DateTime(), "", "");

            //Assert
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
        }

        [Test]
        public void WhenRegistrationIsRetrievedByIdThenCorrectRecordIsReturned()
        {
            //Arrange
            var id = 42;
            var registration = new Registration();
            _registrationServiceMock
                .Setup(rsm => rsm.Get(id))
                .Returns(registration);

            //Act
            var result = _registrationsController.GetById(id);

            //Assert
            _registrationServiceMock.Verify(rsm => rsm.Get(id), Times.Once);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(result.Content.ReadAsAsync<Registration>().Result, Is.EqualTo(registration));
        }

        [Test]
        public void WhenNonExistentRegistrationIsRetrievedByIdThenNoRecordIsReturned()
        {
            //Arrange
            var id = 42;
            _registrationServiceMock
                .Setup(rsm => rsm.Get(id))
                .Returns<Registration>(null);

            //Act
            var result = _registrationsController.GetById(id);

            //Assert
            _registrationServiceMock.Verify(rsm => rsm.Get(id), Times.Once);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public void WhenRegistrationIsPersistedThenCorrectResponseIsReturned()
        {
            //Arrange
            var registration = new Registration();
            var id = 42;
            _registrationServiceMock
                .Setup(rsm => rsm.Insert(registration))
                .Returns(id);

            //Act
            var result = _registrationsController.Save(registration);

            //Assert
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(result.Content.ReadAsAsync<int>().Result, Is.EqualTo(id));
        }

        [TestCase(typeof(ArgumentException), HttpStatusCode.BadRequest)]
        [TestCase(typeof(Exception), HttpStatusCode.InternalServerError)]
        public void WhenRegistrationIsPersistedAndExceptionOccursThenCorrectResponseIsReturned(Type exceptionType, HttpStatusCode statusCode)
        {
            //Arrange
            var registration = new Registration();
            _registrationServiceMock
                .Setup(rsm => rsm.Insert(registration))
                .Throws((Exception)Activator.CreateInstance(exceptionType));

            //Act
            var result = _registrationsController.Save(registration);

            //Assert
            Assert.That(result.StatusCode, Is.EqualTo(statusCode));
        }
    }
}
