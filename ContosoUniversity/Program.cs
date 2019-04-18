using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ContosoUniversity
{
	public class Program
	{
		/*
		In Program.cs, we modify the Main method to do the following on application startup:
		-- Get a database context instance from the dependency injection container.
		-- Call the seed method, passing to it the context.
		-- Dispose the context when the seed method is done.

		Add using statements:
		-- using Microsoft.Extensions.DependencyInjection;
		-- using ContosoUniversity.Data;

		In older tutorials, you may see similar code in the Configure method in Startup.cs. We recommend that you use the Configure
		method only to set up the request pipeline. Application startup code belongs in the Main method.

		Now the first time you run the application, the database will be created and seeded with test data. Whenever you change
		your data model, you can delete the database, update your seed method, and start afresh with a new database the same way.
		In later tutorials, you'll see how to modify the database when the data model changes, without deleting and re-creating it.
		
		 */
		public static void Main(string[] args)
		{
			var host = CreateWebHostBuilder(args).Build();

			using (var scope = host.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				try
				{
					var context = services.GetRequiredService<SchoolContext>();
					DbInitializer.Initialize(context);
				}
				catch (Exception ex)
				{
					var logger = services.GetRequiredService<ILogger<Program>>();
					logger.LogError(ex, "An error occurred while seeding the database.");
				}
			}

			host.Run();
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>();
	}
}
