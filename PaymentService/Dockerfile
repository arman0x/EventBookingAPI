FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY ["PaymentService/Domain/Domain.csproj", "PaymentService/Domain/"]
COPY ["PaymentService/Application/Application.csproj", "PaymentService/Application/"]
COPY ["PaymentService/Infrastructure/Infrastructure.csproj", "PaymentService/Infrastructure/"]
COPY ["PaymentService/Presentation/Presentation.csproj", "PaymentService/Presentation/"]

RUN dotnet restore PaymentService/Presentation/Presentation.csproj

COPY . .

WORKDIR /src/PaymentService/Presentation/
RUN dotnet publish "Presentation.csproj" -c Release -o /app/publish --no-restore

# Stage 2

FROM mcr.microsoft.com/dotnet/aspnet:10.0

WORKDIR /app

COPY --from=build /app/publish .

RUN mv appsettings.Example.json appsettings.json
ENV ASPNETCORE_URLS=http://+:8080

ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080

USER app

ENTRYPOINT ["dotnet", "Presentation.dll"]