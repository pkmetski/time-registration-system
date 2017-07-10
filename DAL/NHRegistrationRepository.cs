using Model;
using NHibernate;
using Model.Arguments;

namespace DAL
{
    public class NHRegistrationRepository : NHRepository<Registration>
    {
        /// <summary>
        /// Inject a preconfigured session factory
        /// </summary>
        /// <param name="session"></param>
        public NHRegistrationRepository(ISession session) : base(session) { }

        protected override IQueryOver<Registration, Registration> BuildQuery(IQueryOver<Registration, Registration> query, QueryArguments args)
        {
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
