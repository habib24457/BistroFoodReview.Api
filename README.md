Running the Project

Step 0: Setup Database  
Step 0: Setup Database
1.	Make sure PostgreSQL is installed and running.
2.	Create a database named bistrodb.

Step 1: Clone the repository
- git clone  
- Update the connection string in appsettings.json with your PostgreSQL username:  
  "ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=bistrodb;Username=<YOUR_DB_USERNAME>;SSL Mode=Disable;Trust Server Certificate=true"
  }

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

Step 6: Run unit tests
•	cd BistroFoodReview.Api.Test  
•	dotnet test  
--------------------------------------------  

!!!Important: Initially the data database will be empty  
I have created a helper class for initial seeding  
to seed the database with data.  
Go to: Program.cs file of the project.  
uncomment line from 45-50. This will seed some data, so that you  
start testing the endpoints in swagger.  !!!  

--------------------------------------------  

How to test the endpoints  
1. How to insert a new rating for a meal?  
-In swagger UI go to section rating and select Post request  
/api/Rating/saveRating  
- saveRating endpoint expects 3 arguments:  
    userId: (which should be already in the db)  
    mealId: (which should be already in the db)  
    stars: which is a double you can choose between 1-5  
- Note: to get the userId and mealId (for today). run endpoint (/api/User/allUsers)  
for mealId run the endpoint (api/Meal/dailyMenu)  

