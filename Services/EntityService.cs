using DAL.Interfaces;
using Model.Arguments;
using NHibernate;
using Services.Interfaces;
using System;
using System.Collections.Generic;

namespace Services
{
    public abstract class EntityService<TModel> : IEntityService<TModel> where TModel : class
    {
        protected readonly IRepository<TModel> _repository;

        public EntityService(IRepository<TModel> repository)
        {
            _repository = repository;
        }

        public virtual TModel Get(int id)
        {
            return _repository.Get(id);
        }

        public virtual IEnumerable<TModel> Get(QueryArguments args)
        {
            return _repository.Get(args);
        }

        public virtual int Insert(TModel model)
        {
            try
            {
                return _repository.Insert(model);
            }
            catch (PropertyValueException ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        public virtual void SaveOrUpdate(TModel model)
        {
            _repository.SaveOrUpdate(model);
        }
    }
}
