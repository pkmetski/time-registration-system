using DAL.Interfaces;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Model;
using Model.Arguments;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using System;
using System.Data.SQLite;
using System.Reflection;
using System.Linq;

namespace DAL.UnitTest
{
    [TestFixture]
    public class NHRegistrationRepositoryTest
    {
        private IRepository<Registration> _registrationRepository;
        private static SQLiteConnection _connection;

        [SetUp]
        public void Setup()
        {
            _registrationRepository = new NHRegistrationRepository(GetInMemorySessionFactorySession());
        }

        [TearDown]
        public void Teardown()
        {
            _connection.Dispose();
        }

        private ISession GetInMemorySessionFactorySession()
        {
            string connectionString = "FullUri=file:memorydb.db?mode=memory&cache=shared";
            var config = Fluently.Configure()
                    .Database(SQLiteConfiguration.Standard.ConnectionString(connectionString))
                    .Mappings(m => m.HbmMappings.AddFromAssembly(Assembly.Load("Model")))
                    .BuildConfiguration();

            var schemaExport = new SchemaExport(config);
            _connection = new SQLiteConnection(connectionString);
            _connection.Open();
            schemaExport.Execute(false, true, false, _connection, null);

            return config.BuildSessionFactory().OpenSession();
        }

        [Test]
        public void WhenNonNullableFieldsAreNullThenException()
        {
            //Arrange
            var registration = new Registration { Hours = null };

            //Act, Assert
            Assert.Throws<PropertyValueException>(() => _registrationRepository.Insert(registration));
        }

        [Test]
        public void WhenRegistrationIsInsertedThenItIsPersistedInDb()
        {
            //Arrange
            var inputReg = new Registration
            {
                Date = new DateTime(2017, 1, 31),
                Hours = 5.4,
                Project = "Time reg",
                Customer = "e-conomic"
            };

            //Act
            var id = _registrationRepository.Insert(inputReg);

            //Assert
            var persistedReg = _registrationRepository.Get(id);
            Assert.That(persistedReg.Date, Is.EqualTo(inputReg.Date));
            Assert.That(persistedReg.Hours, Is.EqualTo(inputReg.Hours));
            Assert.That(persistedReg.Project, Is.EqualTo(inputReg.Project));
            Assert.That(persistedReg.Customer, Is.EqualTo(inputReg.Customer));
        }

        [Test]
        public void WhenDatabaseIsQueriedWithNonExistentCustomerThenNoResults()
        {
            //Arrange
            InsertRegistrations();
            var args = new QueryArguments { Customer = "Customer3" };

            //Act
            var result = _registrationRepository.Get(args);

            //Assert
            Assert.That(result.ToList().Count, Is.EqualTo(0));
            Assert.True(result.All(reg => !string.Equals(reg.Customer, args.Customer)));
        }

        [Test]
        public void WhenDatabaseIsQueriedWithExistingCustomerThenCorrectResults()
        {
            //Arrange
            InsertRegistrations();
            var args = new QueryArguments { Customer = "Customer2" };

            //Act
            var result = _registrationRepository.Get(args);

            //Assert
            Assert.That(result.ToList().Count, Is.EqualTo(3));
            Assert.True(result.All(reg => string.Equals(reg.Customer, args.Customer)));
        }

        [Test]
        public void WhenDatabaseIsQueriedWithExistingHoursThenCorrectResults()
        {
            //Arrange
            InsertRegistrations();
            var args = new QueryArguments { Hours = 8 };

            //Act
            var result = _registrationRepository.Get(args);

            //Assert
            Assert.That(result.ToList().Count, Is.EqualTo(1));
            Assert.True(result.All(reg => reg.Hours == args.Hours));
        }

        [Test]
        public void WhenDatabaseIsQueriedWithProjectThenCorrectResults()
        {
            //Arrange
            InsertRegistrations();
            var args = new QueryArguments { Project = "Future project" };

            //Act
            var result = _registrationRepository.Get(args);

            //Assert
            Assert.That(result.ToList().Count, Is.EqualTo(1));
            Assert.True(result.All(reg => string.Equals(reg.Project, args.Project)));
        }

        [Test]
        public void WhenDatabaseIsQueriedWithFromDateThenCorrectResults()
        {
            //Arrange
            InsertRegistrations();
            var args = new QueryArguments { FromDate = new DateTime(2007, 2, 1) };

            //Act
            var result = _registrationRepository.Get(args);

            //Assert
            Assert.That(result.ToList().Count, Is.EqualTo(3));
            Assert.True(result.All(reg => reg.Date >= args.FromDate));
        }

        [Test]
        public void WhenDatabaseIsQueriedWithToDateThenCorrectResults()
        {
            //Arrange
            InsertRegistrations();
            var args = new QueryArguments { ToDate = new DateTime(2020, 1, 1) };

            //Act
            var result = _registrationRepository.Get(args);

            //Assert
            Assert.That(result.ToList().Count, Is.EqualTo(3));
            Assert.True(result.All(reg => reg.Date <= args.ToDate));
        }

        [Test]
        public void WhenDatabaseIsQueriedWithDateRangeThenCorrectResults()
        {
            //Arrange
            InsertRegistrations();
            var args = new QueryArguments { FromDate = new DateTime(2007, 1, 1), ToDate = new DateTime(2007, 12, 31) };

            //Act
            var result = _registrationRepository.Get(args);

            //Assert
            Assert.That(result.ToList().Count, Is.EqualTo(2));
            Assert.True(result.All(reg => reg.Date >= args.FromDate && reg.Date <= args.ToDate));
        }

        [Test]
        public void WhenDatabaseIsQueriedWithMultipleArgsValuesThenCorrectResults()
        {
            //Arrange
            InsertRegistrations();
            var args = new QueryArguments
            {
                FromDate = new DateTime(2007, 1, 1),
                ToDate = new DateTime(2007, 12, 31),
                Project = "Past project",
                Customer = "Customer2",
                Hours = 6
            };

            //Act
            var result = _registrationRepository.Get(args);

            //Assert
            Assert.That(result.ToList().Count, Is.EqualTo(1));
            Assert.True(result.All(reg => reg.Date >= args.FromDate &&
                    reg.Date <= args.ToDate &&
                    string.Equals(reg.Project, args.Project) &&
                    string.Equals(reg.Customer, args.Customer) &&
                    reg.Hours == args.Hours));
        }

        private void InsertRegistrations()
        {
            _registrationRepository.Insert(new Registration
            {
                Date = new DateTime(2017, 1, 31),
                Hours = 5.4,
                Project = "Short project",
                Customer = "Customer1"
            });
            _registrationRepository.Insert(new Registration
            {
                Date = new DateTime(2007, 1, 31),
                Hours = 8,
                Project = "Past project",
                Customer = "Customer2"
            });
            _registrationRepository.Insert(new Registration
            {
                Date = new DateTime(2007, 2, 1),
                Hours = 6,
                Project = "Past project",
                Customer = "Customer2"
            });
            _registrationRepository.Insert(new Registration
            {
                Date = new DateTime(2047, 2, 1),
                Hours = 5.4,
                Project = "Future project",
                Customer = "Customer2"
            });
        }
    }
}
