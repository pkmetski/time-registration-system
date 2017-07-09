using System.Net.Http;
using System.Web.Http;

namespace TimeRegistrationSystem.Controllers
{
    public class HomeController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Index()
        {
            return new HttpResponseMessage
            {
                Content = new StringContent("Server is running")
            };
        }
    }
}
