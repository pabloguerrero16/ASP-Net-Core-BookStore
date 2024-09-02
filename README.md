<a id="readme-top"></a>

[![LinkedIn][linkedin-shield]][linkedin-url]

<!-- PROJECT LOGO -->
<br />
<div align="center">
<h3 align="center">Online Book Store</h3>

  <p align="center">
    A project to Showcase my skills using ASP.NET Core 8 + SQL Server Management Studio
    <br />
    <br />
    <br />
  </p>
</div>

<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About the Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
        <li><a href="#development-tools">Development Tools</a></li>
      </ul>
    </li>
    <li><a href="#objective">Objective</a></li>
    <li><a href="#database">Database</a></li>
    <li>
      <a href="#project-features-regular-customer">Project Features (Regular Customer)</a>
      <ul>
        <li><a href="#login--registration">Login & Registration</a></li>
        <li><a href="#edit-profilepassword">Edit Profile/Password</a></li>
        <li><a href="#product-list--filters--search-bar">Product List + Filters & Search Bar</a></li>
        <li><a href="#product-details">Product Details</a></li>
        <li><a href="#shopping-cart">Shopping Cart</a></li>
        <li><a href="#paypal-payment">PayPal Payment</a></li>
        <li><a href="#shopping-history">Shopping History</a></li>
        <li><a href="#invoice">Invoice</a></li>
      </ul>
    </li>
    <li>
      <a href="#project-features-admin">Project Features (Admin)</a>
      <ul>
        <li><a href="#admin-panel">Admin Panel</a></li>
        <li><a href="#book-list">Book List</a></li>
        <li><a href="#add--edit-book">Add & Edit Book</a></li>
        <li><a href="#delete-book">Delete Book</a></li>
      </ul>
    </li>
    <li><a href="#contact">Contact</a></li>
  </ol>
</details>

<!-- ABOUT THE PROJECT -->
## About the Project

![Product Name Screen Shot][product-screenshot]

This project involves the development of an online bookstore, built using .NET Core 8, with a focus on implementing technologies and practices used in real-world contexts. The project is divided into two main parts: a web application (MVC) and a web API, both developed in .NET Core 8. The database is managed using SQL Server.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

### Built With

* [![C#][C#-logo]][C#-url]
* [![.Net][NET-logo]][NET-url]
* [![SSMS][SQL-logo]][SQL-url]
* [![HTML][html-logo]][html-url]
* [![CSS][css-logo]][css-url]
* [![Bootstrap][Bootstrap.com]][Bootstrap-url]

### Development Tools
#### Postman
* Used for the development and testing of APIs, ensuring efficient and correct communication between the frontend and backend.

#### Cloudinary
* Used for the storage of images using Cloud services. This allow us to avoid storing images in the project's folder or the database, which saves space.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- OBJECTIVE -->
## Objective
* The main objective of this project is to familiarize with various technologies and practices that are common in real-world development environments. By integrating payment systems, cloud image management, and a robust shopping cart and invoicing system, this project provides a solid foundation for developing complete and functional web applications.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- DATABASE -->
## Database
* This is the diagram of the database that I built for my project.
* ![Database Diagram][db]

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- PROJECT FEATURES (REGULAR CUSTOMER) -->
## Project Features (Regular Customer)

In this section I'll describe the main features of my project from a regular customer's Point Of View.

### Login & Registration

* Users can Login or Register in the website.
* ![Login & Registration][register]

### Edit Profile/Password
* Users can update their account details, they can also update their password.
* ![Update Info][update-info]

### Product List + Filters & Search Bar

* Users can visualize all products available, they can apply filters to show only want they want to see.
* ![Main Page][main]

### Product Details

* This page shows all the info from the selected book.
* ![Book Info][detail]

### Shopping Cart

* This is the shopping cart. User's have the option to delete items from the shopping cart. They also get the option to pay using PayPal.
* ![Shopping Cart][cart]

### PayPal Payment

* The default payment option for the website is PayPal. This was made possible using PayPal API.
* ![PayPal][paypal]

### Shopping History

* User's can access their shopping history
* ![Shopping History][history]

### Invoice

* From the shopping history, users can access a invoice for every purchase they've made.
* ![Invoice][invoice]

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- PROJECT FEATURES (ADMIN) -->
## Project Features (Admin)

In this section I'll describe the main features of my project from an Admin Point Of View.

### Admin Panel

* If the user has the admin Role, they'll get access to the admin Panel. They can access the Crud for books, authors, and genres.
* ![Admin Dashboard][dash]

* Since Authors and Genres are a basic model (Just the name), I'll just list the books CRUD, since that's the one with most information.

### Book List

* Admins can get access to the Books list, where they can Perform any of the CRUD options.
* ![Admin Dashboard][dash]

### Add & Edit Book
* Admins can add and update books. The add and update view is the same, the main difference is that when updating the info will be auto filled with the current info on the database.
* ![Add & Update Book][add]

### Delete Book
* Admins can delete a book if they desire. This will show up a confirmation before doing so to avoid deleting wrong products.
* ![Delete Book][delete]

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- CONTACT -->
## Contact

Pablo Guerrero - [LinkedIn](www.linkedin.com/in/pabloguerrero1611) - guerreropablo313@gmail.com

Project Link: [BookStore .Net Core 8](https://github.com/pabloguerrero16/ASP-Net-Core-BookStore)

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- MARKDOWN LINKS & IMAGES -->
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: www.linkedin.com/in/pabloguerrero1611
[product-screenshot]: Images/product.png
[register]: Images/register.png
[main]: Images/main.png
[detail]: Images/detail.png
[cart]: Images/cart.png
[invoice]: Images/invoice.png
[history]: Images/history.png
[db]: Images/db.png
[add]: Images/add.png
[delete]: Images/delete.png
[paypal]: Images/paypal.png
[dash]: Images/dash.png
[update-info]: Images/update-info.png
[C#-logo]: https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white
[C#-url]: https://dotnet.microsoft.com/en-us/languages/csharp
[NET-logo]: https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white
[NET-url]: https://dotnet.microsoft.com/en-us/
[SQL-logo]:  	https://img.shields.io/badge/Microsoft%20SQL%20Server-CC2927?style=for-the-badge&logo=microsoft%20sql%20server&logoColor=white
[SQL-url]:https://learn.microsoft.com/en-us/sql/ssms/sql-server-management-studio-ssms?view=sql-server-ver16
[html-logo]:  	https://img.shields.io/badge/HTML-239120?style=for-the-badge&logo=html5&logoColor=white
[html-url]: https://developer.mozilla.org/en-US/docs/Web/HTML
[css-logo]:https://img.shields.io/badge/CSS-239120?&style=for-the-badge&logo=css3&logoColor=white
[css-url]: https://developer.mozilla.org/en-US/docs/Web/CSS
[js-logo]:https://img.shields.io/badge/Javascript-323330?style=for-the-badge&logo=javascript&logoColor=F7DF1E
[js-url]: https://developer.mozilla.org/en-US/docs/Web/JavaScript
[Bootstrap.com]: https://img.shields.io/badge/Bootstrap-563D7C?style=for-the-badge&logo=bootstrap&logoColor=white
[Bootstrap-url]: https://getbootstrap.com
