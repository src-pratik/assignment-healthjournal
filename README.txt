==========================================================================================================
Assumptions 
==========================================================================================================
1. User Mangement is not available in system, i.e there is no user interface built for creating or updating users.
2. ASP net Identity is used for user management, its database ERD is not explained.

==========================================================================================================
Frameworks/Dependency/Dev tools
==========================================================================================================
1. .Net Core 6.0 framework (SDK) for build. Net Core 6.0 runtime if only used for execution after build.
2. SQL Server (above 2012) - Developer edition
3. Node (Version v18.14.0 or above)
4. npm package manager 9.3.1

Dev Tools
1. Visual Studio 2022 - For backend project
2. Visual Studio Code - For frontend project


The application is setup under 2 main projects
1. Backend (.Net Core 6.0)
2. Frontend (Angular 15)

=========================================================================================
Backend (.Net Core)
=========================================================================================

Configuration changes : 

1. FileStorage
	Kindly change the path value to match a folder available on the machine, this path is used for filestorage provider.

"FileStorageProvider": {
    "Path": "c:\\temp"
  },

2. Database Connection String :
		Kindly change the server name, currently its set to . (selfhost). If the SQL server is running on the same machine
 this config can be kept as it is or if a different server change the value to the server name.

  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=JournalPortal;Trusted_Connection=True;MultipleActiveResultSets=true"
  },

Optional change : The local port assigned to the backed project is 7125. This can be changed if needed.

To build, test and run the project kindly use the below steps.

navigate to code\backend folder in terminal

1. Dependencies : to restore the dependencies,  execute the below command
	dotnet restore
2. To build the project execute the below command
	dotnet build
3. To execute tests run the below command
	dotnet test -t
4. To run the project execute the below command
	dotnet run --project portal.Service

=============================================================================================
Database
=============================================================================================
Database Migration :

The system is configured to install the latest changes on Startup. It will apply all migrations automatically.

Optionally: The database scripts are available in Database folder.

=============================================================================================
Data Seed 
=============================================================================================

Default users are already added to the system. The system seeds the below users in 2 different roles 
1. User
2. Published

Data seeded for logins,

role => user
	user1@portal.com 
	user2@portal.com
role=> publisher
	pub1@portal.com
	pub2@portal.com

password = "P@$$w0rd";


Data can be found in portal.Security.Identity project, under IdentitySeed.cs file


=============================================================================================
Front End
=============================================================================================


Configuration changes : 

If the backend service port has been changed to other than 7125, kindly change the below value in 
code\frontend\ngPortal\src\environments\environment.development.ts

update the apiBaseUrl to match with new value that is set.

export const environment = {
    apiBaseUrl: " https://localhost:7125"
};


To build, test and run the project kindly use the below steps.
navigate to code\backend folder in terminal

1. Dependencies : to restore the dependencies,  execute the below command
	npm install
2. To build the project execute the below command
	ng build
3. To execute tests run the below command
	ng serve