using System.Reflection;

using DbUp;

using Domain.Shared.Constants;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

var configurationBuilder = new ConfigurationBuilder()
			.SetBasePath($"{Directory.GetCurrentDirectory()}/Config")
			.AddJsonFile($"appsettings.json", false, true);

var configuration = configurationBuilder.Build();
var connectionString = configuration.GetConnectionString("DbConnectionString");

EnsureDatabase.For.SqlDatabase(connectionString);

var sqlConnBuilder = new SqlConnectionStringBuilder(connectionString);
Console.ForegroundColor = ConsoleColor.Yellow;
Console.BackgroundColor = ConsoleColor.Black;
Console.WriteLine("Server: " + sqlConnBuilder.DataSource);
Console.WriteLine("Db:     " + sqlConnBuilder.InitialCatalog);
Console.WriteLine("Check and press Enter to proceed");
Console.ReadLine();

var upgrader =
DeployChanges.To
	.SqlDatabase(connectionString, DbConstants.DbSchemaNameApp)
	.JournalToSqlTable(DbConstants.DbSchemaNameApp, "DbHistory")
	.LogToConsole()
	//.WithScriptsFromFileSystem("Scripts")
	.WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
	.Build();

var result = upgrader.PerformUpgrade();

if (!result.Successful)
{
	Console.ForegroundColor = ConsoleColor.Red;
	Console.WriteLine("Failure!");
	Console.WriteLine(result.Error);	
}
else
{
	Console.ForegroundColor = ConsoleColor.Green;
	Console.WriteLine("Success!");	
}
Console.ResetColor();
Console.ReadLine();

