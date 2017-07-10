using Model.Arguments;
using Services;
using Services.Factories;
using Services.Interfaces;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TimeRegistrationSystem.Controllers
{
    public class InvoicingController : ApiController
    {
        private readonly IInvoicingService _invoicingService;
        private ISessionHelper _sessionHelper;

        public InvoicingController()
        {
            _sessionHelper = new SessionHelper();
            _invoicingService = new ServicesFactory(_sessionHelper).GetInvoicingServince();
        }

        [Route("api/invoices/id/{id}")]
        [HttpGet]
        public HttpResponseMessage GetById(int id)
        {
            try
            {
                var invoice = _invoicingService.Get(id);

                if (invoice == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Invoice with ID {id} not found");
                }
                return Request.CreateResponse(HttpStatusCode.OK, invoice);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [Route("api/invoices")]
        [HttpPost]
        public HttpResponseMessage CreateInvoice([FromBody] QueryArguments args)
        {
            try
            {
                var id = _invoicingService.CreateInvoice(args);
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _sessionHelper.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
