# classeviva-net
A .NET wrapper for Classeviva school registry

# Features
-Get a student's profile including their first and last name and school

-Get a student's grades

-Get assigned homework

-Get content published by teachers

# Documentation
## Functions
### ClassevivaNet.Classeviva.LoginAsync(email, password)
#### Logins into your Classeviva account and returns a Classeviva instance
##### Parameters:
-email: A `string` containing a student's email used to access Classeviva

-password: A `string` containing student's password used to access Classeviva
##### Returns:
An instance of the Classeviva class
##### Example:
```cs
Classeviva classeviva = await Classeviva.LoginAsync("email", "password");
```

### ClassevivaNet.Classeviva.GetSchoolAsync()
#### Returns a student's school name
##### Returns:
A `string` containing a student's school name
##### Example:
```cs
string school = await classeviva.GetSchoolAsync();
```

### ClassevivaNet.Classeviva.GetFullNameAsync()
#### Returns a student's full name 
**WARNING: This method is significantly slower than Classeviva's Name and Surname properties, but is included for consistency**
##### Returns:
A `string` containing a student's school name
##### Example:
```cs
string name = await classeviva.GetSchoolAsync();
```

### ClassevivaNet.Classeviva.GetGradesAsync()
#### Returns a student's entire grade history
##### Returns:
An array of `Grade` objects containing grade data
##### Example:
```cs
Grade[] grades = await classeviva.GetGradesAsync();
foreach (Grade grade in grades)
{
    Console.WriteLine(grade.Subject);
}
```

### ClassevivaNet.Classeviva.GetFilesAsync()
#### Returns a student's school material files
##### Returns:
An array of `MaterialFile` objects containing file data
##### Example:
```cs
MaterialFile[] files = await classeviva.GetFilesAsync();
foreach (MaterialFile file in files)
{
    Console.WriteLine(file.Link);
}
```

### ClassevivaNet.Classeviva.GetHomeworkAsync(startDate, endDate)
#### Returns a student's entire grade history
##### Parameters:
-startDate: A `DateTime` object representing the start date of a homework span

-endDate: A `DateTime` object representing the end date of a homework span
##### Returns:
An array of `Homework` objects containing data
##### Example:
```cs
DateTime startDate = new DateTime(2019, 9, 1);
DateTime endDate = new DateTime.Now;
Homework[] homework = await classeviva.GetHomeworkAsync(startDate, endDate);
foreach (Grade homeworkUnit in homework)
{
    Console.WriteLine(grade.GetGradeString());
}
```

### ClassevivaNet.Grade.GetGradeString()
#### Returns the stringified version of a grade. Useful when dealing with both numerical and literal grades
##### Returns:
A `string` containing the stringified version of a grade
##### Example:
```cs
Grade[] grades = await classeviva.GetGradesAsync();
foreach (Grade grade in grades)
{
    Console.WriteLine(grade.GetGradeString());
}
```

## Classes

### ClassevivaNet.Classeviva
#### Main object that you should use to run all functions
##### Properties:
-Name: A `string` containing a student's name

-Surname: A `string` containing a student's surname
#### Example
```cs
Classeviva classeviva = await Classeviva.LoginAsync("email", "password");
Console.WriteLine(classeviva.Name);
Console.WriteLine(Classeviva.Surname);
classeviva.GetGradesAsync();
```

### ClassevivaNet.Grade
#### Grade object that holds all the grade data
##### Properties:
-Comment: A `string` containing a teacher's comment

-Date: A `DateTime` object containing the date on which the grade was gotten

-Subject: A `string` containing the subject the grade was gotten in

-Type: A `string` containing the type of the grade (Oral, Written, Test, etc.)

**NOTE:** This is a string due to unknown amount of grade types

-CountsTowardsAverage: A `bool` that tells whether this grade counts towards the average grade
#### Example
```cs
Grade[] grades = await classeviva.GetGradesAsync();
foreach (Grade grade in grades)
{
    Console.WriteLine(grade.Comment);
    Console.WriteLine(grade.Date.ToString());
    Console.WriteLine(grade.Subject);
    Console.WriteLine(grade.Type);
    Console.WriteLine(grade.CountsTowardsAverage);
}
```

### ClassevivaNet.Homework
#### Homework object that holds all the homework data
##### Properties:
-Id: A `string` containing the ID of the assignment

-Title: A `string` containing the title of the assignment

-StartDate: A `DateTime` object containing the start date of the assignment

-EndDate: A `DateTime` object containing the end date of the assignment

-IsAllDay: A `bool` that tells whether this assignment is for the entire day

-CreationDate: A `DateTime` containing the date on which the assignment was created

-Author: A `string` containing the name of the teacher that gave the assignment

-Content: A `string` containing the content/description of the assignment
#### Example
```cs
Homework[] homework = await classeviva.GetHomeworkAsync(new DateTime(2019, 9, 1), DateTime.Now);
foreach (Homework assignment in homework)
{
    Console.WriteLine(assignment.Id);
    Console.WriteLine(assignment.Title);
    Console.WriteLine(assignment.StartDate.ToString());
    Console.WriteLine(assignment.EndDate.ToString());
    Console.WriteLine(grade.IsAllDay);
    Console.WriteLine(grade.CreationDate);
    Console.WriteLine(grade.Author);
    Console.WriteLine(grade.Content);
}
```

### ClassevivaNet.MaterialFile
#### File object representing files published by teachers
##### Properties:
-Author: A `string` containing the file's author

-Description: A `string` containing the description of the file

-Link: A `string` containing the link to the file

-RootFolder: A `string` containing the name of the folder the file is in

-Date: A `DateTime` object containing the date this file was created on

-MaterialFileType: A `MaterialFileType` enum that defines the type of the file; can be `File` or `Link`
#### Example
```cs
MaterialFile[] files = await classeviva.GetFilesAsync();
foreach (MaterialFile file in files)
{
    Console.WriteLine(file.Author);
    Console.WriteLine(file.Description);
    Console.WriteLine(file.Link);
    Console.WriteLine(file.RootFolder);
    Console.WriteLine(file.Date);
    Console.WriteLine(file.MaterialFileType);
}
```

# Practical Example
```cs
Classeviva cs = await Classeviva.LoginAsync("foo", "bar");
Console.WriteLine(cs.Name);
Console.WriteLine(cs.Surname);
Console.WriteLine(await cs.GetSchoolAsync());
Console.WriteLine(await cs.GetFullNameAsync());
Console.WriteLine((await cs.GetHomeworkAsync(new DateTime(2019, 9, 1), DateTime.Now))[1].Id);
MaterialFile[] files = await cs.GetFilesAsync();
foreach (MaterialFile file in files)
{
    Console.WriteLine(file.RootFolder);
}
```
