FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY ["BookingService/Domain/Domain.csproj", "BookingService/Domain/"]
COPY ["BookingService/Application/Application.csproj", "BookingService/Application/"]
COPY ["BookingService/Infrastructure/Infrastructure.csproj", "BookingService/Infrastructure/"]
COPY ["BookingService/Presentation/Presentation.csproj", "BookingService/Presentation/"]

RUN dotnet restore BookingService/Presentation/Presentation.csproj

COPY . .

WORKDIR /src/BookingService/Presentation
RUN dotnet publish "Presentation.csproj" -c Release -o /app/publish --no-restore

# Stage 2
FROM mcr.microsoft.com/dotnet/aspnet:10.0

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080

ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080

USER app

ENTRYPOINT ["dotnet", "Presentation.dll"]
