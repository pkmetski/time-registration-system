using Model;
using Model.Arguments;
using NHibernate;

namespace DAL
{
    public class NHInvoiceRepository : NHRepository<Invoice>
    {
        public NHInvoiceRepository(ISession session) : base(session) { }

        protected override IQueryOver<Invoice, Invoice> BuildQuery(IQueryOver<Invoice, Invoice> query, QueryArguments args)
        {
            return query;
        }
    }
}
