using Model;
using System.Collections.Generic;

namespace Services.Interfaces
{
    public interface IRegistrationService
    {
        int RegisterTime(Registration registration);

        IEnumerable<Registration> GetRegistrations(QueryArguments args);
    }
}
