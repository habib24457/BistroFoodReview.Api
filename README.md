# Running the Project

Step 0: Setup Database
1.	Make sure PostgreSQL is installed and running.
2.	Create a database named bistrodb.

Or:  
A Dockerfile for the API is provided.
Note: The PostgreSQL container via Docker Compose is not fully configured.
To run the API, connect to a local PostgreSQL instance and adjust the connection string in appsettings.json and Program.cs.  
- How to run the API with Docker:  
1. Build the Docker Image: docker build -t bistrofoodreview-api  
2. Run the docker container: docker run -p 5175:8080 bistrofoodreview-api  
The API will be available at: http://localhost:5175/api  

***Note:*** If you had a PostgreSQL database in Docker, add a docker-compose.yml like  

```json
version: '3.9'
services:
  db:
    image: postgres
    environment:
      POSTGRES_USER: postgres
      POSTGRES_PASSWORD: password
      POSTGRES_DB: BistroReviewDb
    ports:
      - "5432:5432"

  api:
    build: .
    ports:
      - "5175:8080"
    depends_on:
      - db
```
Finally, run the API and the DB: docker compose up --build  
--------------------------------------------------------------------
Whether you choose to run locally or in a docker container, choose your connection string  
and also update the name in the Program.cs in the Database connection.  

```json
"ConnectionStrings": {
"DefaultConnection": "Host=localhost;Port=5432;Database=bistrodb;Username=yourusername;SSL Mode=Disable;Trust Server Certificate=true",
"DockerConnection": "Host=host.docker.internal;Port=5432;Database=bistrodb;Username=yourusername;Password=mysecretpassword;SSL Mode=Disable;Trust Server Certificate=true"
}
```
Step 1: Clone the repository
- git clone  
- Update the connection string in appsettings.json with your PostgreSQL username:

Step 2: Restore dependencies  
- cd BistroFoodReview.Api
- dotnet restore

Step 3: Build the project  
- dotnet build

Step 4: Apply EF Core migrations  
- dotnet ef database update  

Note: If you are on Visual Studio IDE. Please go->  
Tools->Nuget Package manager -> Package manager console  

Command: Update-Database  

If you are using Rider IDE. Select the project & Right click on the mouse  
and select option:  
Entity Framework Core -> Update Database...  


Step 5: Run the API  
•	dotnet run  
•	The API will be available at:  http://localhost:5175/swagger/index.html  




--------------------------------------------  

!!!Important: Initially the data database will be empty.  
After running the EFCore migration successfully, the database will be  
populated with some initial data.!!!  

--------------------------------------------  

# How to test the endpoints  

1. How to insert a new rating for a meal?  
-In swagger UI go to section rating and select Post request  
/api/Rating/saveRating  

- saveRating endpoint expects 3 arguments:  
    userId: remove the default GUID id and you have to take from existing user (api/User/allUsers)
    mealId: (you have to take from saved meal from existing meal (api/Meal/dailyMenu)  
    stars: which is a double you can insert between 1-5  
 
2. How to test autocomplete endpoint (/api/autocmplete)  
- Enter any first letters from the existing meal names. The endpoint will return max 3  
meal name suggestions in an array, if there is exisitng mealName with the prefix.  
- For example: If you seed the DB: there will be three meal name with EditName1, EditName2, EditName3  
- If you enter in the endpoint 'ed' and execute. The endpoint will return the three meal names.  



