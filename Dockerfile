# Stage 1: Build and restore dependencies
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY . .
WORKDIR "/src/HR.LeaveManagement.Api"
RUN dotnet restore "HR.LeaveManagement.Api.csproj"

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "HR.LeaveManagement.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Create final image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HR.LeaveManagement.Api.dll"]