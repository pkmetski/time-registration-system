using DAL;
using Model;
using Services.Interfaces;

namespace Services.Factories
{
    public class ServicesFactory
    {
        private ISessionHelper _sessionHelper;
        public ServicesFactory(ISessionHelper sessionHelper)
        {
            _sessionHelper = sessionHelper;
        }

        public IEntityService<Registration> GetRegistrationService()
        {
            return new RegistrationService(new NHRegistrationRepository(_sessionHelper.GetCurrentSession()));
        }

        public IInvoicingService GetInvoicingServince()
        {
            return new InvoicingService(new NHInvoiceRepository(_sessionHelper.GetCurrentSession()), GetRegistrationService());
        }
    }
}
