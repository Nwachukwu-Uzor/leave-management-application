FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["./HR.LeaveManagement.Api/HR.LeaveManagement.Api.csproj", "HR.LeaveManagement.Api/"]
COPY ["./HR.LeaveManagement.Application/HR.LeaveManagement.Application.csproj", "HR.LeaveManagement.Application/"]
COPY ["./HR.LeaveManagement.Domain/HR.LeaveManagement.Domain.csproj", "HR.LeaveManagement.Domain/"]
COPY ["./HR.LeaveManagement.Infrastructure/HR.LeaveManagement.Infrastructure.csproj", "HR.LeaveManagement.Infrastructure/"]
COPY ["./HR.LeaveManagement.Persistence/HR.LeaveManagement.Persistence.csproj", "HR.LeaveManagement.Persistence/"]
RUN dotnet restore "./HR.LeaveManagement.Api/HR.LeaveManagement.Api.csproj"
COPY . .
WORKDIR "/src/HR.LeaveManagement.Api"
RUN dotnet build "HR.LeaveManagement.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HR.LeaveManagement.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HR.LeaveManagement.Api.dll"]