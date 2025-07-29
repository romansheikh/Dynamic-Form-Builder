Dynamic Form Builder - ASP.NET Core MVC This project is a home-based assignment for QIA Management Consultants Ltd., designed to demonstrate skills in ASP.NET Core MVC, SQL Server, AJAX, and component-based architecture.

✅ Features
Dynamic form creation with multiple fields
Support for dropdowns, labels, and required flag
Reusable View Component for dropdown rendering
AJAX form submission via API endpoint
Form history list with preview functionality
Bootstrap 5 styling for modern UI
🏗️ Technologies Used
ASP.NET Core MVC (.NET 6 / .NET 7)
SQL Server
ADO.NET for database operations
JavaScript + Fetch API
View Components
Bootstrap 5
SweetAlert2
📁 Project Structure
/Controllers FormController.cs FormApiController.cs

/Models Form.cs Field.cs

/Views /Form Index.cshtml Create.cshtml CreateViaAjax.cshtml Preview.cshtml /Shared/Components/DropdownField Default.cshtml

/ViewComponents DropdownFieldViewComponent.cs

🛠️ Setup Instructions
Clone or download this repository.
Create a local SQL Server database named: FormBuilderDB
Run the following SQL to create necessary tables:
sql

CREATE TABLE Forms ( FormId INT PRIMARY KEY IDENTITY, Title NVARCHAR(255), CreatedAt DATETIME DEFAULT GETDATE() );

CREATE TABLE FormFields ( FieldId INT PRIMARY KEY IDENTITY, FormId INT FOREIGN KEY REFERENCES Forms(FormId), Label NVARCHAR(255), IsRequired BIT, SelectedOption NVARCHAR(255) ); Update appsettings.json with your SQL Server credentials:

"ConnectionStrings": { "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=FormBuilderDB;User Id=your_user;Password=your_password;TrustServerCertificate=True;" }

DynamicFormBuilder/ │ ├── README.md ├── FormBuilderDB.bak

Run the project in Visual Studio (Ctrl + F5)

🌐 Pages Summary Page Description /Form/Index Lists all forms with preview links /Form/Create Create form using traditional POST /Form/CreateViaAjax Create form using AJAX + API /Form/Preview/{id} Preview single form

🎯 Bonus Features Implemented ViewComponent for dropdown fields API controller for AJAX-based form submission SweetAlert2 feedback message

Dynamic field addition via Fetch and partial rendering

📞 Contact If you have any questions, feel free to contact:

Roman Sheikh 📧 Email: romansheikh3@gmail.com 📱 Phone: 01912200321 🗓️ Submission Date: 23 July 2025
