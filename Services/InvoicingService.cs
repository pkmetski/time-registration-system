using Services.Interfaces;
using System.Linq;
using Model.Arguments;
using Model;
using System.Collections.Generic;
using DAL.Interfaces;
using System;

namespace Services
{
    public class InvoicingService : EntityService<Invoice>, IInvoicingService
    {
        IEntityService<Registration> _registrationService;
        public InvoicingService(IRepository<Invoice> repository, IEntityService<Registration> registrationService) : base(repository)
        {
            _registrationService = registrationService;
        }

        public int CreateInvoice(QueryArguments args)
        {
            var registrations = GetNonInvoicedRegistrations(args);
            if (registrations.Count() == 0)
            {
                throw new ArgumentException("Invalid arguments. Query needs to match at least one non-invoiced registration.");
            }
            var price = CalculatePrice(registrations);
            var invoice = new Invoice
            {
                Registrations = new HashSet<Registration>(registrations),
                Amount = price
            };

            registrations = UpdateRegistrationsWithInvoice(registrations, invoice);

            SaveOrUpdate(invoice);
            return invoice.Id;
        }

        private IEnumerable<Registration> GetNonInvoicedRegistrations(QueryArguments args)
        {
            return _registrationService
                .Get(args)
                .Where(reg => reg.Invoice == null)
                .ToList();
        }

        private IEnumerable<Registration> UpdateRegistrationsWithInvoice(IEnumerable<Registration> registrations, Invoice invoice)
        {
            foreach (var reg in registrations)
            {
                reg.Invoice = invoice;
            }
            return registrations;
        }

        private double CalculatePrice(IEnumerable<Registration> registrations)
        {
            //price can be based on hours, project, customer
            //to keep it simple, it is hard-coded
            return 42;
        }
    }
}
