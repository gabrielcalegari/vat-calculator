# Austrian VAT Calculator

A microservice to calculate Value Added Tax (VAT) for Austria.

## How does it work?

The API receives one of the `netAmount`, `grossAmount` or `vatAmount` and additionally a valid Austrian `vatRate` (10%, 13%, 20%). The other two amounts (net/gross/VAT) are calculated by the system and returned to the client.

### Example

Request:

```
curl --location 'https://localhost:8081/valueaddedtax' \
--header 'Content-Type: application/json' \
--data '{
    "grossAmount": 1100,
    "vatRate": 10
}'
```

Response:

```
{
    "grossAmount": 1100,
    "netAmount": 1000,
    "vatAmount": 100,
    "vatRate": 10
}
```

## How to run?

To run this application, clone this project, open the root folder in the terminal, and follow one of the instructions below:

- If you have a .NET 8 environment in your machine:

```
 dotnet run --project .\src\VatCalculator.WebApi\VatCalculator.WebApi.csproj
```

- If you have Docker installed in your machine:

```
docker-compose up
```

## Architecture

The proposed solution is composed of a three-layer architecture: WebApi, Application, and Domain, using a Clean Architecture approach. 

### WebApi layer
The WebApi layer is the startup layer and exposes a REST API. It registers the application layer. It also registers a global exception handler.

### Application layer
The Application layer exposes the Use Cases provided by the application and used by the WebApi layer. To make validation in a fluent and decoupled way, I've used the library FluentValidation. For handling predictable errors, the Result pattern is being used through the ErrorOr library. The library Mapperly is used to automate the mapping between Application types and Domain types.

### Domain layer

The Domain layer contains the business logic which is orchestred by the application layer.

### Considerations

- For unit testing, I've chosen to work with xUnit as the runner, and NSubstitute for mocking.

- I only implemented Unit Testing for the Domain and Application layers. For the Presentation layer, I think Integration Testing will bring more benefits but it still was not implemented.

- All logs will be the output in the console. In case we need a structured log, we could use a library such as Serilog.

- I have not added but could be a benefit to add some Monitoring besides logs, such as Health Checkers, APM metrics and Tracing.