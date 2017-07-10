Time Registration System v1.0

This project implements WebApi RESTful ednpoints for registering and retrieving
project time registrations data.

**Features**

-   create new time registration

-   retrieve time registration(s)

-   create new invoice

-   retrieve invoice by id

 

**Technologies**

-   C\# WebApi REST services

-   Data layer using NHibernate

-   SQL database for production code

-   In-memory SQLite database for testing

 

**Prerequisites**

An SQL server installation with database named *TimeRegistrationSystem*.

 

**Usage**

 

*Registrations*

 

Endpoint:

http://localhost:65046/api/registrations/

 

-   Create new registration (POST)

The request body is a JSON representation of a Registration instance:

{

"Date": "2034-12-31",

"Hours": 62,

"Project": "project1",

"Customer": "customer3"

}

In order to create a new registration, a POST request is made against the
following endpoint:

http://localhost:65046/api/registrations

If the request succeeds, HTTP code 201 is returned, together with the ID of the
newly created record.

 

-   Retrieve registration by ID (GET)

In order to retrieve a specific record, whose ID is known, a GET request is
issued against the following endpoint:

http://localhost:65046/api/registrations/id/1

This will return a JSON representation of the entity with ID 1, or 404 in case
such entity doesn’t exist.

 

-   Retrieve registrations matching search criteria (GET)

It is possible to issue a more complex queries against the API:

http://localhost:65046/api/registrations?fromDate=2000-01-31&toDate=2007-01-01&project=project1&customer=customer3

This query will match all registration records with date between 31st January
2000 and 1st January 2007, with project “project1”, and customer “customer3”.
All the fields need to match in order for a record to be returned. It is
possible to omit any (or all) parameters. Issuing the query with no parameters
will apply no filtering to the data set and all records will be returned.

 

*Invoices*

 

Endpoint:

http://localhost:65046/api/invoices/

 

-   Create a new invoice (POST)

The POST request expects the following JSON string in its body:

{

"FromDate": "2024-12-31",

"ToDate": "2034-12-31",

"Project": "project1",

"Customer": "customer3"

}

This is to be read as follows: create an invoice for all registrations between
the dates provided, for the project and customer provided. If none of the
registrations in the system meet the requirements set by the query, the server
returns 400, Bad request. None of these properties is required. Providing an
empty JSON object will result in all non-invoiced registrations, regardless of
their customer and project, being assigned to the same invoice.

The return value is the ID of the newly created invoice.

 

-   Retrieve an invoice by ID (GET)

A single invoice record can be retrieved by its ID by Issuing a GET request
against http://localhost:65046/api/invoices/1

This will return invoice with ID 1

For example an invoice with two registrations can look as follows:

{

"Id": 1,

"Amount": 42,

"RegistrationsIds": [

2,

3

]

}

 

**NOTE**: Currently all invoices get the same hard-coded price of 42.
