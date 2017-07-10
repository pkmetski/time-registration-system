using NHibernate;
using NHibernate.Cfg;
using Services.Interfaces;

namespace Services
{
    public class SessionHelper : ISessionHelper
    {
        private ISessionFactory _sessionFactory;
        public SessionHelper()
        {
            _sessionFactory = new Configuration().Configure().BuildSessionFactory();
        }

        private ISession _session = null;
        public ISession GetCurrentSession()
        {
            if (_session == null)
            {
                _session = _sessionFactory.OpenSession();
            }
            return _session;
        }

        public void Dispose()
        {
            if (_session != null)
            {
                _session.Dispose();
                _session = null;
            }
        }
    }
}
