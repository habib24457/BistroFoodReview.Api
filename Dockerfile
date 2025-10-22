# Stage 1: Build and Test
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution file
COPY *.sln ./

# Copy API project
COPY BistroFoodReview.Api/*.csproj ./BistroFoodReview.Api/
# Copy test project
COPY BistroFoodReview.Api.Test/*.csproj ./BistroFoodReview.Api.Test/

# Restore dependencies
RUN dotnet restore

# Copy the rest of the project files
COPY BistroFoodReview.Api/. ./BistroFoodReview.Api/
COPY BistroFoodReview.Api.Test/. ./BistroFoodReview.Api.Test/

# Run tests
RUN dotnet test ./BistroFoodReview.Api.Test/BistroFoodReview.Api.Test.csproj --no-build --logger "trx;LogFileName=test_results.trx"

# Publish API project
RUN dotnet publish -c Release -o /app/out ./BistroFoodReview.Api

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Copy published output from build stage
COPY --from=build /app/out .

# Expose port
EXPOSE 80

# Run the application
ENTRYPOINT ["dotnet", "BistroFoodReview.Api.dll"]