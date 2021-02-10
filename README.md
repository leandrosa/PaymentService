 # Payment Gateway API Service
![.NET](https://github.com/mathos819/PaymentService/workflows/.NET/badge.svg)

The project aims to create a payment gateway to perform payment transactions between merchants and bank issuers.

### Technologies

* ASP.NET Core 3.1
* Entity Framework Core 3.1 (SQL Server or in-memory data storage)
* MediatR
* AutoMapper
* FluentValidation
* Polly (HTTP Client resilience)
* Docker
* Monitoring (Elasticsearch & Kibana)
* Healthchecks
* Swagger
* Identity
* Authentication and authorization through JWT Tokens

### Getting Started

1. Install the latest [.NET 3.1 SDK](https://dotnet.microsoft.com/download/dotnet-core/3.1)
2. Install the latest [Docker Desktop](https://www.docker.com/products/docker-desktop)

### Docker Configuration

In order to get Docker working, you will need to install a new SSL certificate.
You can find more about it on the [Microsoft Docs](https://docs.microsoft.com/en-us/aspnet/core/security/docker-https?view=aspnetcore-3.1).

The following will need to be executed from your terminal to create a cert
`dotnet dev-certs https -ep %USERPROFILE%\.aspnet\https\aspnetapp.pfx -p Checkout_cert123`
`dotnet dev-certs https --trust`

If you already have a valid SSL certificate, you can change the environment variables on the **docker-compose.yml** file.

To build and run the docker containers, execute `docker-compose up --build` from the solution's root.

The script will create the following containers:

* Payment Gateway API Service (https://localhost:5001/swagger)
* SQL Server - latest (localhost,1433)
* Elasticsearch - 7.9.2 (http://localhost:9200)
* Kibana - 7.9.2 (http://localhost:5601)

In order to view the application logs through the Kibana, an index pattern must be created. You can read more about it [here](https://www.elastic.co/guide/en/kibana/current/index-patterns.html).

### Database Configuration

The application supports two types of data storage, SQL Server and in-memory. To switch between them, you need to change your **application.json** file.

```json
  "UseInMemoryDatabase": false,
```

By default, the application will use the SQL Server as its database, and the connection string is set to the local container created on the step before.

The database migration will occur when you run the application. You don't need to worry about it.

### Authentication and authorization

The application counts with authentication and authorization features.

When you first run the service, two users will be created when the database migration occurs, and you can use them to generate a valid token through the API.

| Username | Password | Role |
| :---: | :---: | :---: |
| checkout | Pa$$w0rd. | Administrator |
| user | Pa$$w0rd. | User |

### Service endpoints

1. `POST /payment/v1/authenticate` - Authenticates the specified user on the identity service and, if successful, returns a JWT token, which is necessary to access the other endpoints.
2. `POST /payment/v1/payments` - Creates a new payment transaction. You can find an example body request on the Swagger.
3. `GET /payment/v1/payments/{merchantId}` - Get a list of transaction payments occurred to the **merchantId** sent. *Requires **administrator** role*.
4. `GET /payment/v1/payments/{merchantId}/{paymentId}` - GET a transaction payment occurred to the merchant and payment identifier sent.

### Useful links

* [Swagger UI](https://localhost:5001/swagger) - Provides a display framework that reads an OpenAPI specification document and generates an interactive documentation website.
* [Kibana](http://localhost:5601/) - Provides search and data visualization capabilities for data indexed in Elasticsearch (logs and monitoring data).
* [Health checks](https://localhost:5001/healthchecks) - REST API to check service readiness probes.

### Technical debits

* Creation of unit, integration, and performance tests.
* Create a shared kernel project to remove the dependence of the presentation layer to the infrastructure layer.
* Include Symmetric Encryption.
* Fix health checks UI.
