using DAL;
using Services.Interfaces;

namespace Services
{
    public static class ServicesFactory
    {
        public static IRegistrationService GetRegistrationService()
        {
            return new RegistrationService(new NHRegistrationRepository());
        }
    }
}
