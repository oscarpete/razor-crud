# Title: Razor cuts twigs, handling a CRUD with .NET

- Repository: `razor-crud`
- Type of Challenge: `Learning Challenge`
- Duration: `2 days`
- Deployment strategy : `NA`
- Team challenge : `solo`

## Learning objectives
  - generating templates and understanding them
  - understanding the model and context
  - understanding sql connection


### Starting up
We already understand some actions we can do within Razor to quickly get a project started. Now we are going to use it to the fullest to quickly create a CRUD with a persistent Database!
We will be making a CRUD, the subject can (read should) be of your choosing! I will be using the Microsoft example of Movie.


Create the new project
`` dotnet new webapp -o RazorCrud ``

Trust the certificate if you haven't done so already
`` dotnet dev-certs https --trust ``

Now let's add all the necessary packages for this project. MAKE SURE EVERYTHING IS UP TO DATE !!!!

````

dotnet tool install --global dotnet-ef
dotnet tool install --global dotnet-aspnet-codegenerator
dotnet add package Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.Extensions.Logging.Debug

on windows -> dotnet add package Microsoft.EntityFrameworkCore.SqlServer

on Ubuntu/Mac OS -> dotnet add package Microsoft.EntityFrameworkCore.SQLite


````

- dotnet-ef -> our entity manager
- dotnet-aspnet-codegenerator -> our template generator
- and then a bunch of packages to make everything work

### Creating a Model
Create a folder "Models" -> make ClassName.cs 

We will be using DataAnnotations here.


````
REWORK BELOW EXAMPLE CODE TO YOUR SUBJECT!

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

!! these two usings are necesary !!
Everything between [] are data annotations for later ;)

    public class Movie
    {
        public int ID { get; set; }

        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string Title { get; set; }

        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        [Range(1, 100)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z]*$")]
        [Required]
        [StringLength(30)]
        public string Genre { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s-]*$")]
        [StringLength(5)]
        public string Rating { get; set; }
    }

````

### Generate a CRUD from template .NET stylezzzz
Now it's time for the "magic" to happen ^^

In the following command replace a few words:  (if you have the project running, stop the process. Otherwise you will have errors)

```dotnet-aspnet-codegenerator razorpage -m Movie -dc RazorCRUDMovieContext -udl -outDir Pages/Movies --referenceScriptLibraries ```

 - "Movie" -> "Your class", 
 - "RazorCRUDMovieContext" -> "YourProjectName +YourClassName + Context", 
 - "Pages/Movies" -> "Pages/ + Your classname + s"

some info on this command:
- dotnet-aspnet-codegenerator -> the package we just added to generate the CRUD
- razorpage -> what template we are using
- -m -> the model we are building on
- -dc -> DatabaseContext -> Needed for the sql side, migrations, DB, ...
- -udl -> Use the default layout.
- -outDir -> The relative output folder path to create the views.
- --referenceScriptLibraries 	-> Adds _ValidationScriptsPartial to Edit and Create pages (This will work with our data annotations.) 


Explore everything that is generated for you. (the classname folder in the folder Pages) If you try to run it however it won't work on the route /Classnames just yet. We need to fix our DatabaseContext first ;)

### Generate a database context
So here things get a bit weird, we are going to make an migration to push to our Database (create the table structure etc.) 

If you are on windows you can just continue on to the migration commands, but for Mac / Ubuntu we will be using SQLite. This is because on Windows you can use the built in sql service to create a Local DB, on Mac / Ubuntu we need to create it ourself and link to it! (PS: Windows users could opt for sqlite as well, if desired)

First install the opensource sqlite manager [here](https://sqlitebrowser.org/). then create a new database, remember where you put this file you will need it!

In your startup class look for the function ConfigureServices. In here change this:

````
 options.UseSqlServer to  options.UseSqlite
````

In your appsettings.json change your connection string to ```  "Data Source=/Path/to/your/sqlitefile ```

Now on to the migrations!


#### Migration commands
``dotnet ef migrations add InitialCreate``
Create the initial migration (InitialCreate is the name of the migration)

``dotnet ef database update``
Actually push the migration.

### Be Amazed
Now you can test it! Go to your /Movies route or edit the _Layout to have a nav item.
Be sure to check every route (Create, Read, Update & Delete).
Also, are you amazed with the date field? I sure am! Data Annotations + code generation anyone? 

``<input class="form-control" type="date" data-val="true" data-val-required="The ReleaseDate field is required." id="Movie_ReleaseDate" name="Movie.ReleaseDate" value="">``

More info on the generated pages -> https://docs.microsoft.com/en-us/aspnet/core/tutorials/razor-pages/page?view=aspnetcore-5.0

### Explore the sql and seed it
You might restart your project while doing some edits but don't worry, you won't lose your data! It's in your DB now. :D 
In appsettings.json you can find your connection string, use the built in Database explorer (or ubuntu/macos sqlite) or your favorite database manager and check out the generated Table.

There is an issue tho, you might drop your table, or alter some migrations and then you also want to have some testdata  BUT you don't want to manually create it everytime?
Then it's time for seeders! Their responsibility is, if there is no data in the DB to fill it up.

Now we are going to create our seeder, in the Models folder add a class named SeedData with the following code (alter for your subject):

````
  public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new RazorCRUDMovieContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<RazorCRUDMovieContext>>()))
            {
                // Look for any movies.
                if (context.Movie.Any())
                {
                    return;   // DB has been seeded
                }

                context.Movie.AddRange(
                    new Movie
                    {
                        Title = "When Harry Met Sally",
                        ReleaseDate = DateTime.Parse("1989-2-12"),
                        Genre = "Romantic Comedy",
                        Price = 7.99M
                    },

                    new Movie
                    {
                        Title = "Ghostbusters ",
                        ReleaseDate = DateTime.Parse("1984-3-13"),
                        Genre = "Comedy",
                        Price = 8.99M
                    },

                    new Movie
                    {
                        Title = "Ghostbusters 2",
                        ReleaseDate = DateTime.Parse("1986-2-23"),
                        Genre = "Comedy",
                        Price = 9.99M
                    },

                    new Movie
                    {
                        Title = "Rio Bravo",
                        ReleaseDate = DateTime.Parse("1959-4-15"),
                        Genre = "Western",
                        Price = 3.99M
                    }
                );
                context.SaveChanges();
            }
        }

````

add the seeder in the Main program:

````
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    SeedData.Initialize(services);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }

            host.Run();
        }
````

(if you get lots of errors you might be forgetting some using statements ;) )

now restart your process if you still had it running, make sure the Table has NO records. 

When you test your code you will see the seeded data. 

## you've got the music in you!
Congratz on making it this far! Here are your Must-Have features.

### Now two times more! (Games, Music, Movies, Series, Books, ...)
Add 2 more models migrate them and create a CRUD (repetition is key).

### Glasses look good
Now make it look good! We don't want to present the same solution 20 times so customise it! TIP: check the _Layout ;)




![awwyisss](https://thumbs.gfycat.com/ShinyOrdinaryAnemoneshrimp-max-1mb.gif)
