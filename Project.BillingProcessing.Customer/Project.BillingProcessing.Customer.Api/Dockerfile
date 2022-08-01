#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Project.BillingProcessing.Customer.Api/Project.BillingProcessing.Customer.Api.csproj", "Project.BillingProcessing.Customer.Api/"]
RUN dotnet restore "Project.BillingProcessing.Customer.Api/Project.BillingProcessing.Customer.Api.csproj"
COPY . .
WORKDIR "/src/Project.BillingProcessing.Customer.Api"
RUN dotnet build "Project.BillingProcessing.Customer.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Project.BillingProcessing.Customer.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Project.BillingProcessing.Customer.Api.dll"]