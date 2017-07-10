using Model.Arguments;
using System.Collections.Generic;

namespace Services.Interfaces
{
    public interface IEntityService<TModel>
    {
        int Insert(TModel registration);

        TModel Get(int id);

        IEnumerable<TModel> Get(QueryArguments args);

        void SaveOrUpdate(TModel model);
    }
}
