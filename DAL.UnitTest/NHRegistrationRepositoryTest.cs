using DAL.Interfaces;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Model;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Reflection;
using System.Linq;

namespace DAL.UnitTest
{
    [TestFixture]
    public class NHRegistrationRepositoryTest
    {
        private IRegistrationRepository _registrationRepository;
        private static SQLiteConnection _connection;

        [SetUp]
        public void Setup()
        {
            _registrationRepository = new NHRegistrationRepository(GetInMemorySessionFactory());
        }

        [TearDown]
        public void Teardown()
        {
            _connection.Dispose();
        }

        private ISessionFactory GetInMemorySessionFactory()
        {
            string connectionString = "FullUri=file:memorydb.db?mode=memory&cache=shared";
            var config = Fluently.Configure()
                    .Database(SQLiteConfiguration.Standard.ConnectionString(connectionString))
                    .Mappings(m => m.HbmMappings.AddFromAssembly(Assembly.Load("Model")))
                    //.ExposeConfiguration(cfg => cfg.SetProperty("current_session_context_class", "call"))
                    .BuildConfiguration();

            var schemaExport = new SchemaExport(config);
            _connection = new SQLiteConnection(connectionString);
            _connection.Open();
            schemaExport.Execute(false, true, false, _connection, null);

            return config.BuildSessionFactory();
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

        private static IEnumerable<TestCaseData> RegistrationQueryArgumentsTestCases
        {
            get
            {
                Func<Registration, bool> nonExistentCustomerCondition = reg => !string.Equals(reg.Customer, "Customer3");
                yield return new TestCaseData(new QueryArguments { Customer = "Customer3" }, nonExistentCustomerCondition, 0);

                Func<Registration, bool> customerCondition = reg => string.Equals(reg.Customer, "Customer2");
                yield return new TestCaseData(new QueryArguments { Customer = "Customer2" }, customerCondition, 3);

                Func<Registration, bool> hoursCondition = reg => reg.Hours == 8;
                yield return new TestCaseData(new QueryArguments { Hours = 8 }, hoursCondition, 1);

                Func<Registration, bool> projectCondition = reg => string.Equals(reg.Project, "Future project");
                yield return new TestCaseData(new QueryArguments { Project = "Future project" }, projectCondition, 1);

                Func<Registration, bool> dateFomCondition = reg => reg.Date >= new DateTime(2007, 2, 1);
                yield return new TestCaseData(new QueryArguments { FromDate = new DateTime(2007, 2, 1) }, dateFomCondition, 3);

                Func<Registration, bool> dateToCondition = reg => reg.Date <= new DateTime(2020, 1, 1);
                yield return new TestCaseData(new QueryArguments { ToDate = new DateTime(2020, 1, 1) }, dateToCondition, 3);

                Func<Registration, bool> dateFromToCondition = reg => reg.Date >= new DateTime(2007, 1, 1) && reg.Date <= new DateTime(2007, 12, 31);
                yield return new TestCaseData(new QueryArguments { FromDate = new DateTime(2007, 1, 1), ToDate = new DateTime(2007, 12, 31) }, dateFromToCondition, 2);

                Func<Registration, bool> allPropsCondition = reg =>
                    reg.Date >= new DateTime(2007, 1, 1) &&
                    reg.Date <= new DateTime(2007, 12, 31) &&
                    string.Equals(reg.Project, "Past project") &&
                    string.Equals(reg.Customer, "Customer2") &&
                    reg.Hours == 6;
                yield return new TestCaseData(new QueryArguments
                {
                    FromDate = new DateTime(2007, 1, 1),
                    ToDate = new DateTime(2007, 12, 31),
                    Project = "Past project",
                    Customer = "Customer2",
                    Hours = 6
                },
                allPropsCondition, 1);
            }
        }

        [Test]
        [TestCaseSource(nameof(RegistrationQueryArgumentsTestCases))]
        public void WhenDatabaseIsQueriedWithQueryArgumentsThenCorrectResultsAreRetrieved(QueryArguments args, Func<Registration, bool> condition, int expectedCount)
        {
            //Arrange
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

            //Act
            var result = _registrationRepository.Get(args);

            //Assert
            Assert.That(result.ToList().Count, Is.EqualTo(expectedCount));
            Assert.True(result.All(condition));
        }
    }
}
