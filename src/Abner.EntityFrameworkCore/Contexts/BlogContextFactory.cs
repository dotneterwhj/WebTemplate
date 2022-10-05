using Abner.EntityFrameworkCore.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Abner.EntityFrameworkCore;
public class BlogContextFactory : IDesignTimeDbContextFactory<BlogContext>
{
    public BlogContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();

        var connStr = configuration.GetConnectionString("Default");

        var builder = new DbContextOptionsBuilder<BlogContext>()
            .UseMySql(connStr, MySqlServerVersion.AutoDetect(connStr));

        return new BlogContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Abner.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}