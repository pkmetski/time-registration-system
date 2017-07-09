using Model;
using Services;
using Services.Interfaces;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TimeRegistrationSystem.Controllers
{
    public class RegistrationsController : ApiController
    {
        private readonly IRegistrationService _registrationService;

        public RegistrationsController()
        {
            _registrationService = ServicesFactory.GetRegistrationService();
        }

        public RegistrationsController(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        [Route("api/registrations")]
        [HttpGet]
        public HttpResponseMessage GetByArgs(DateTime? fromDate = null, DateTime? toDate = null, string project = null, string customer = null)
        {
            try
            {
                var args = new QueryArguments
                {
                    FromDate = fromDate,
                    ToDate = toDate,
                    Project = project,
                    Customer = customer
                };

                var regs = _registrationService.GetRegistrations(args);
                return Request.CreateResponse(HttpStatusCode.OK, regs);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [Route("api/registrations/id/{id}")]
        [HttpGet]
        public HttpResponseMessage GetById(int id)
        {
            try
            {
                var registration = _registrationService.GetRegistration(id);

                if (registration == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Registration with ID {id} not found");
                }
                return Request.CreateResponse(HttpStatusCode.OK, registration);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [Route("api/registrations")]
        [HttpPost]
        public HttpResponseMessage Save([FromBody] Registration registration)
        {
            try
            {
                var id = _registrationService.RegisterTime(registration);
                return Request.CreateResponse(HttpStatusCode.Created, id);
            }
            catch (ArgumentException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
