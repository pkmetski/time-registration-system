using DAL.Interfaces;
using Model;
using Moq;
using NHibernate;
using NUnit.Framework;
using Services.Interfaces;
using System;
using System.Collections.Generic;

namespace Services.UnitTest
{
    [TestFixture]
    public class RegistrationServiceTest
    {
        private Mock<IRegistrationRepository> _registrationRepositoryMock;
        private IRegistrationService _registrationService;

        [SetUp]
        public void Setup()
        {
            _registrationRepositoryMock = new Mock<IRegistrationRepository>();
            _registrationService = new RegistrationService(_registrationRepositoryMock.Object);
        }

        private Registration GetRegistration()
        {
            return new Registration
            {
                Customer = "cust",
                Project = "proj",
                Date = new DateTime(2017, 4, 2),
                Hours = 42
            };
        }

        [Test]
        public void WhenTimeIsRegisteredThenRepositoryIsInvoked()
        {
            //Arrange
            var registration = GetRegistration();
            var resultingId = 1;
            _registrationRepositoryMock
                .Setup(rrm => rrm.Insert(registration))
                .Returns(resultingId);

            //Act
            var result = _registrationService.RegisterTime(registration);

            //Assert
            Assert.That(result, Is.EqualTo(resultingId));
        }

        [Test]
        public void WhenTimeIsRegisteredAndPropertyValueExceptionIsThrownThenArgumentExceptionIsRethrown()
        {
            //Arrange
            var registration = GetRegistration();
            _registrationRepositoryMock
                .Setup(rrm => rrm.Insert(registration))
                .Throws(new PropertyValueException("exception msg", "", "prop"));

            //Act, Assert
            Assert.Throws<ArgumentException>(() => _registrationService.RegisterTime(registration));
        }

        [Test]
        public void WhenTimeRegistrationsAreRetrievedThenRepositoryIsInvoked()
        {
            //Arrange
            var args = new QueryArguments();
            var registrationsList = new List<Registration>() { GetRegistration() };
            _registrationRepositoryMock
                .Setup(rrm => rrm.Get(args))
                .Returns(registrationsList);

            //Act
            var result = _registrationService.GetRegistrations(args);

            //Assert
            Assert.That(result, Is.EqualTo(registrationsList));
        }

        [Test]
        public void WhenRegistrationIsRetrievedByIdThenRegistrationRepositoryIsQueried()
        {
            //Arrange
            var id = 9;
            var registration = new Registration();
            _registrationRepositoryMock
                .Setup(rrm => rrm.Get(id))
                .Returns(registration);

            //Act
            var result = _registrationService.GetRegistration(id);

            //Assert
            Assert.That(result, Is.EqualTo(registration));
        }
    }
}
