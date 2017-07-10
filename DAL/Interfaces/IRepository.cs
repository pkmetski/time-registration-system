using Model.Arguments;
using System.Collections.Generic;

namespace DAL.Interfaces
{
    public interface IRepository<TModel>
    {
        int Insert(TModel model);

        TModel Get(int id);

        IEnumerable<TModel> Get(QueryArguments args);

        void SaveOrUpdate(TModel model);
    }
}
