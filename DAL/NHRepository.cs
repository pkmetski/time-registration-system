using DAL.Interfaces;
using Model.Arguments;
using NHibernate;
using System.Collections.Generic;

namespace DAL
{
    public abstract class NHRepository<TModel> : IRepository<TModel> where TModel : class
    {
        private static ISession _session;
        public NHRepository(ISession session)
        {
            _session = session;
        }

        public int Insert(TModel model)
        {
            return (int)_session.Save(model);
        }

        public TModel Get(int id)
        {
            return _session.Get<TModel>(id);
        }

        public void SaveOrUpdate(TModel model)
        {
            _session.SaveOrUpdate(model);
            _session.Flush();
        }

        public virtual IEnumerable<TModel> Get(QueryArguments args)
        {
            return BuildQuery(_session.QueryOver<TModel>(), args)
                .List();
        }

        protected abstract IQueryOver<TModel, TModel> BuildQuery(IQueryOver<TModel, TModel> query, QueryArguments args);
    }
}
