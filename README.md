# classeviva-net
A .NET wrapper for Classeviva school registry

# Features
-Get a student's profile including their first and last name and school

-Get a student's grades

-Get assigned homework

-Get content published by teachers

# Documentation
## ClassevivaNet.Classeviva
### Main object that you should use to run all functions
### Properties:
-Name: A student's name

-Surname: A student's surname

## ClassevivaNet.Classeviva.LoginAsync(email, password)
### Logins into your Classeviva account and returns a Classeviva instance
### Parameters:
-email: A student's email used to access Classeviva

-password: A student's password used to access Classeviva
### Returns:
An instance of the Classeviva class
