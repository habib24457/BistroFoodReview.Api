Running the Project

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
    userId: remove the default GUID id and you have to take from existing user (api/User/allUsers)
    mealId: (you have to take from saved meal from existing meal (api/Meal/dailyMenu)  
    stars: which is a double you can insert between 1-5  
 
2. How to test autocomplete endpoint (/api/autocmplete)  
- Enter any first letters from the existing meal names. The endpoint will return max 3  
meal name suggestions in an arry, if there is exisitng mealName with the prefix.  
- For example: If you seed the DB: there will be three meal name with EditName1, EditName2, EditName3  
- If you enter in the endpoint 'ed' and execute. The endpoint will return the three meal names.  



