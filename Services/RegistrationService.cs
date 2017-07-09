using Services.Interfaces;
using System.Collections.Generic;
using Model;
using DAL.Interfaces;
using NHibernate;
using System;
using System.Linq;

namespace Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IRegistrationRepository _registrationRepository;

        public RegistrationService(IRegistrationRepository registrationRepository)
        {
            _registrationRepository = registrationRepository;
        }

        public Registration GetRegistration(int id)
        {
            return _registrationRepository.Get(id);

        }

        public IEnumerable<Registration> GetRegistrations(QueryArguments args)
        {
            return _registrationRepository.Get(args);
        }

        public int RegisterTime(Registration registration)
        {
            try
            {
                return _registrationRepository.Insert(registration);
            }
            catch (PropertyValueException ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
    }
}
