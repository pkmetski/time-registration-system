using DAL.Interfaces;
using System.Collections.Generic;
using Model;
using NHibernate;
using NHibernate.Cfg;
using System;

namespace DAL
{
    public class NHRegistrationRepository : IRegistrationRepository
    {
        private static ISessionFactory _sessionFactory;

        public NHRegistrationRepository()
        {
            _sessionFactory = new Configuration().Configure().BuildSessionFactory();
        }

        /// <summary>
        /// Inject a preconfigured session factory
        /// </summary>
        /// <param name="sessionFactory"></param>
        public NHRegistrationRepository(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public int Insert(Registration registration)
        {
            return InvokeSessionOperation(session => (int)session.Save(registration));
        }

        public Registration Get(int id)
        {
            return InvokeSessionOperation(session => session.Get<Registration>(id));
        }

        public IEnumerable<Registration> Get(QueryArguments args)
        {
            return InvokeSessionOperation(session =>
                BuildQuery(session.QueryOver<Registration>(), args)
                    .List());
        }

        private T InvokeSessionOperation<T>(Func<ISession, T> func)
        {
            using (ISession session = _sessionFactory.OpenSession())
            {
                return func(session);
            }
        }

        private IQueryOver<Registration, Registration> BuildQuery(IQueryOver<Registration, Registration> query, QueryArguments args)
        {
            if (args.Id.HasValue)
            {
                query = query.Where(r => r.Id == args.Id);
            }
            if (args.FromDate.HasValue)
            {
                query = query.Where(r => r.Date >= args.FromDate);
            }
            if (args.ToDate.HasValue)
            {
                query = query.Where(r => r.Date <= args.ToDate);
            }
            if (!string.IsNullOrEmpty(args.Project))
            {
                query = query.Where(r => r.Project == args.Project);
            }
            if (!string.IsNullOrEmpty(args.Customer))
            {
                query = query.Where(r => r.Customer == args.Customer);
            }
            if (args.Hours.HasValue)
            {
                query = query.Where(r => r.Hours == args.Hours);
            }
            return query;
        }
    }
}
