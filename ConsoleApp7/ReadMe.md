# MovieMetaData Database Tool

### **CodeLouisville C# Project**
John Napier - 502-643-2784

## Intro
This is a Code Louisville project for the C# class.

This program will create a database of movie 'metadata' for your movies using an online API called OMDB.

To search the API, the program derives a movie title by reading a movie folder name character by character looking for a 'clue' about where the title in the folder name ends.

>Example:  To derive a movie title from a folder name "Blade Runner (1981)", the program will read a folder name, see the "(", back up 2 characters and truncate from there.
>  + **Note to Evaluator:**  to demonstrate user editing data: The folder name for Akira in the mock folder set was designed to derive a movie title the API won't recognize.  The user is prompted for a correct title name.  The user should type: Akira

The main menu gives users options to:

    A: Create new MovieMetaDatabase
    B: Create MovieTable from movie folders
    C: View movie list
    D: Search MovieMetDatabase (MovieTable)
    E: Edit your movie comments
    X: Exit

## Project Processes Explained:

**A: Create new MovieMetaDatabase**

The program creates the movie database which will receive informaiton from the OMDB API.  User is propted about erasing the old database and then asked what directory your movie folders are in so it knows where to create the database.

**B: Create MovieTable from movie folders**

The program uses folder names to derive movie titles then, one by one, sends the title to the OMDB API to obtain movie metadata and place it in the database.

 The program uses object parameters and the System.Reflection Class to dynamically write retrieved metadata to the database.  This avoids SQL Injection attacks and keeps the classes flexible.

Process:

    After the user enters the main movie folder location, the "Add movie metadata to the database" Cycle starts:
        1. Derive movie title from foldername
        2. Attempt to fetch the metadata from OMDB API
            - if not found: User is notified and prompted to provide correct movie title
            - If found: User is shown sample data and prompted if correct movie was fetched.
                - if user response "No": then user prompted to provide correct movie name
                - if user response "Yes": then user is prompted to save metadata to database
        3.  Movie metadata pulled from object properties and dynamically saved to database
        4.  User is shown movie metadata added to database
        5.  Repeat for next movie folder

**C: View movie list**

 From the database, the user is presented a table of Movie Titles and IMDB urls to the movies.

**D: Search MovieMetDatabase (MovieTable)**

User is prompted to either:
- Enter movie title to search
- See Movie list

**E: Edit your movie comments**

A user comment column has been built into the Movie Table of the database.  This options fills it.

Process:

    1. User is prompted to:
        - Enter movie title to search
        - See Movie list
    2. User provides Movie Title
    3. User is shown chosen movie metadata
    4. User is prompted to provide User Comment
    5. User Comment is saved under movie title

**X: Exit**

Beep then program ends via System.Environment.Exit(0)


## Set Up

To use this program, the user must have a movie directory which contains a folder for each movie the user owns.  The program uses the folder names to derrive the movie titles to search the OMDB API for metadata retrieval.  



### Mandatory Dependencies:

User has a movie directory containing one movie folder per movie (the mock folder set is in this GitHub project meets this requirement).

Nuget Packages needed:\
  - System.Data.SQLite
  - Microsoft.Net.Http - THIS MAY ALREADY BE IN .NET Standard



>Mock Folder Set Note:\
>A mock folder set of movies is given within the project on GitHub.
>  + The mock folders are empty.
>  + The folder name for each movie provides the folder title.
>  + If using the mock folder set, one should save the set to a known location for the program will ask for that location.
>  + The movie 

## Project requirements checklist

- [x] README file included
- [x] Project is uploaded to GitHub
- [x] Find or create a dataset:
  - OMDB API ( http://www.omdbapi.com/ ) for movie metadata
  - Mock Folder Set for movie titles
  - Converson from JSON to object parameters
  - Conversion from object parameters to SQL table
  - Imports data from API
  - Reads data from data tables
- [x] Use Object Oriented Programming
  - At least one class models data (ResponseStrings)
  - At least two instances of the class - several in BDCommands.cs
  - variables of the object ResponseStrings populated from JSON of OMDBResponse object
  
- [x] Application must persist
    - persists in SQLite database
    - use can add / change User Comments per title
- [x] Code has comments
    - includes self-documenting techniques



