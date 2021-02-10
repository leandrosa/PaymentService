#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Payments.Presentation/Payments.Presentation.csproj", "Payments.Presentation/"]
COPY ["Payments.Infrastructure/Payments.Infrastructure.csproj", "Payments.Infrastructure/"]
COPY ["Payments.Application/Payments.Application.csproj", "Payments.Application/"]
COPY ["Payments.Domain/Payments.Domain.csproj", "Payments.Domain/"]
RUN dotnet restore "Payments.Presentation/Payments.Presentation.csproj"
COPY . .
WORKDIR "/src/Payments.Presentation"
RUN dotnet build "Payments.Presentation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Payments.Presentation.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Payments.Presentation.dll"]
