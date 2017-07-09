using Model;
using System.Collections.Generic;

namespace Services.Interfaces
{
    public interface IPricingService
    {
        double GetPrice(IEnumerable<Registration> registrations);
    }
}
