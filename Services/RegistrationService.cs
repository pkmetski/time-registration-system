using Model;
using DAL.Interfaces;

namespace Services
{
    public class RegistrationService : EntityService<Registration>
    {
        public RegistrationService(IRepository<Registration> registrationRepository) : base(registrationRepository)
        {
        }
    }
}
