using NHibernate;
using System;

namespace Services.Interfaces
{
    public interface ISessionHelper : IDisposable
    {
        ISession GetCurrentSession();
    }
}
