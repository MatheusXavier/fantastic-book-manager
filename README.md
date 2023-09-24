# Fantastic Book Manager
Implementation of the [user story](#user-story) following the requirements:
- .NET C#
- Web API
- Clean Architecture
- Test-Driven Development (TDD) methodologies
- Without using the entity framework
- Create a user, login as the user, and
ensure that the user information is stored in the database.

# User Story
"As a user, I want to be able to manage a collection of books, including adding new books, updating book details, removing books, and viewing a list of all my books. Additionally, I want to have the ability to create an account, log in, and ensure that my book collection is private to me."

### Book Management
Users can perform **CRUD** operations (**C**reate, **R**ead, **U**pdate, **D**elete) on a collection of books. 

### User Authentication
Users can create an account, which includes user registration with email and password.

Users can log in to access their book collection.

User authentication ensures that only authorized users can access and modify their book collection.

### Privacy and Authorization
The user story implies that each user's book collection is private to them, meaning users can only see and modify their own books.

Authorization checks should be in place to ensure that users can only manipulate their own data.

### List of Books
Users should be able to view a list of all their books, with book **title**, **author**, and **genre**.

# Technologies
* [ASP.NET Core 7](https://docs.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core)
* [FluentValidation](https://fluentvalidation.net/) & [GuardClauses](https://github.com/ardalis/GuardClauses)
* [MediatR](https://github.com/jbogard/MediatR)
* [Docker](https://github.com/docker)
* [ASP.NET Core Identity](https://github.com/dotnet/AspNetCore/tree/main/src/Identity) through [NetDevPack.Identity](https://github.com/NetDevPack/Security.Identity)
* [Dapper](https://github.com/DapperLib/Dapper) & [DbUp](https://github.com/DbUp/DbUp)
* [xUnit](https://github.com/xunit/xunit), [NetArchTest.Rules](https://github.com/BenMorris/NetArchTest), [Moq](https://github.com/moq) & [FluentAssertions](https://github.com/fluentassertions/fluentassertions)

# Taken decisions

## Entity Framework
One of the [requirements](#fantastic-book-manager) was to not use the Entity Framework, so [Dapper](https://github.com/DapperLib/Dapper) was used, which has performance as one of its main features and allows us to write queries in raw SQL, but some tool would still be needed to manage changes to the database, so [DbUp](https://github.com/DbUp/DbUp) was chosen, allowing us to write changes to the database in SQL Scripts and [DbUp](https://github.com/DbUp/DbUp) tracks which SQL scripts have already been executed and executes the change scripts necessary to update our database.

## User management / Security
As requested, some form of user authentication was necessary, for this we could follow three different ways:
1. Use an external authentication tool such as [Firebase Authentication](https://code-maze.com/dotnet-firebase-authentication/)
2. Writing this entire user creation and authentication part manually
3. Use a well-known tool that has login functionality

**Alternative (1)** is a good one, but there is a requirement regarding user information being saved in database, so it could be seen as a requirement violation.

**Alternative (2)** would be like trying to reinvent the wheel and we would certainly run into problems such as:
- Time Constraints: Creating our own authentication system from scratch is a time-consuming process
- Expertise: Familiarity with authentication and security best practices is important
- Scalability and maintenance: Long-term maintenance and scalability of our application
- Security: Creating our own system would require a deep understanding of security principles to ensure our system is as secure as possible.

**Alternative (3)** which was chosen because we understood that it would be the best for this scenario, using a tool already known and widely used by the community and which already provides us with resources such as login functionality, user management, passwords, profile data, claims, tokens, email confirmation, and more.

## Using NetDevPack.Identity
To implement the Identity API I chose to use the [NetDevPack.Identity](https://github.com/NetDevPack/Security.Identity) which is a library that already adds several basic implementations of [ASP.NET Identity](https://github.com/dotnet/AspNetCore/tree/main/src/Identity) such as JWT, Claims, Validation and other facilities, this helped **_save time_** and make the Identity API **_extremely simple_**.