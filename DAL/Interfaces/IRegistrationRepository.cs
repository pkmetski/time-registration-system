using Model;
using System.Collections.Generic;

namespace DAL.Interfaces
{
    public interface IRegistrationRepository
    {
        int Insert(Registration registration);

        Registration Get(int id);

        IEnumerable<Registration> Get(QueryArguments args);
    }
}
