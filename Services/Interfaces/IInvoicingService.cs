using Model;
using Model.Arguments;

namespace Services.Interfaces
{
    public interface IInvoicingService : IEntityService<Invoice>
    {
        int CreateInvoice(QueryArguments args);
    }
}
