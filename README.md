# BookStoreAPI
API for Book Store

Includes REST API for managing Book's and Client's using GET/POST/PUT/DELETE/PATCH methods.

`rentedBooks` is determined by Book's with ClientId set, **setting them to Client will not update any books**.
Book **doesn't** need to have an active client, history **will** be updated accordingly when changing ClientId *(who has the book)* to other Client or null *(nobody)*.


**You are free to use this project for your purposes, but if you modify it include the original repository where it came from.**
