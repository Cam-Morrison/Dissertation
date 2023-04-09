## Disclaimer
The project contains two applications which work together. 
I certify that the intellectual property is my own other than Angular and .NET included libraries and the following dependencies,

### Angular:
  ApexCharts - Used to chart data.
  NGX-Picture - Image optimisation specifically for the news page.
  Angular Material - UI design.
 
### .NET: 
  ProfanityFilter - Blocks rude words in usernames and watchlist titles.
  EntityFramework - Used to map code-first .NET objects to MySQL database.
  ML.NET - Used for sentiment analysis and time series forecasting. The sentiment model was built with Model Builder, whereas the time series model was unique. 
  Swagger - Used for API endpoint testing. 
  ConfigCat - Industry recognised feature flag management.
  Azure Key Vault - Secrets management to hide connection strings.
  AspNetCoreRateLimit - Stop malicious users spamming the server.

### Other
Additionally, partial content within the application from Docker, EntityFramework, ML.NET and Angular was automatically generated. 

### Data
Data used within the application as outlined within the dissertation is obtained from the following sources:
  Yahoo Finance
  Polygon.io
  Finnhub.io
  Kaggle 
  
 ## Installation guide
 An Integrated Development Environment (IDE) software is required to run the code. The recommended application used for this project is Visual Studio Code, a lightweight and highly customisable editor. To download it, see the link. 
https://code.visualstudio.com/
To help with the navigation of the project, the Material Icon theme is recommended as it makes the distinct types of files stand out from each other. This is an extension for Visual Studio code, and you can find a quick install tutorial here. 
https://youtu.be/XBO4rt3aKFE?t=37
To run the Node Package Manager (NPM) which is responsible for installing dependencies and running the front-end Angular application you will need the Node JavaScript run time which includes NPM. 
https://nodejs.org/en/download
Next, you will need to install the .NET six run time and SDK. Here is a YouTube tutorial, which demonstrates the steps required. 
https://www.youtube.com/watch?v=AC5UWby16sg
To install the environment for Entity Framework to scaffold a database, you will need to install the MySQL server. When running the MySQL installer, click “Developer default” which will only download the client command line. For ease of installation, set the port number to 3306 and management user to root if it is not already as default. 
https://dev.mysql.com/downloads/installer/

Now, unzip the project folder and using Visual Studio code at the top left corner click “File” -> “Open folder”.

You should then see the following file structure. The backend contains the .NET server-side application and Frontend contains the Angular client-side application. 

Now you have opened the project, go to the top navigation bar of the editor, and click “Terminal” followed by “New Terminal”. 
At the bottom of your editor, you should now see a command prompt terminal, click on it and type: 

**cd frontend**

This will take you to the frontend folder where we will set up Angular, check its working and install dependencies for the project.
First check that Node Package Manager has been installed properly by running the following command. If a version is returned, then it has worked. If not revisit the steps or search for the error code online. 

**npm --version**

Now run this command to install Angular.

**npm install -g @angular/cli**

Now, run the following command to install dependencies. One dependency for injection may not be included in the install by default so run the second command separately.

**npm i
npm i ngx-window-token**

Finally, after the dependencies are installed, start the front end by running this command.

**npm start**

The front end should now be running on the local device port 4200. Before navigating to the local host website. Start up the backend to give it the functionality. At the top right of the active terminal, create a new PowerShell window as seen in the image.

Run the following commands individually to navigate to the server-side .NET application’s directory, then check the version of the .NET install. If a number is not returned, then there is an issue with the installation. The first number should be six. Finally, build the .NET application.

**Cd backend
Dotnet --version
Dotnet build backend.csproj**

Now run the following commands one at a time. This will install the entity framework command for .NET, and then run the database update command to create the tables.

**Dotnet tool install –global dotnet-ef
dotnet ef database update --context backend.entity.dbcontext**

You should now be able to access both the backend and frontend applications.
Angular app: http://localhost:4200/
Swagger user interface: https://localhost:7299/swagger/index.html
The Swagger endpoints will not work unless you register an account (or log in if you have an existing one) and copy the JWT response into the authorization popup located at the top right of the page with the word bearer i.e, “bearer [TOKEN]”. 
By default, any accounts generated on the newly installed version of the application will be users. Creating an administrator account will require custom MySQL statements in the command line application.


Signed 18/04/2023
*Cameron Morrison*
