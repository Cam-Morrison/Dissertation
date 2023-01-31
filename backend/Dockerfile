# Use the official Microsoft .NET image as the base image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env

# Set the working directory
WORKDIR /app

# Copy the project files to the working directory
COPY . ./

# Restore the dependencies
RUN dotnet restore

# Build the .NET API
RUN dotnet build -c Release -o out

# Use the official Microsoft .NET runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0

# Set the working directory
WORKDIR /app

# Copy the built .NET API from the build-env
COPY --from=build-env /app/out .

# Expose the mapped port
EXPOSE 7299

# Set the entrypoint to run the .NET API
ENTRYPOINT ["dotnet", "backend.dll"]
