using PucharApi.GraphQL;
using PucharApi.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>();

var app = builder.Build();

app.MapGraphQL("/graphql");
app.MapGet("/", () => "PucharApi działa. Wejdź na /graphql");

app.Run();
