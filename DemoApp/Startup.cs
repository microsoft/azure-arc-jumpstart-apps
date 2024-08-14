using DemoApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient; 
using System;
using System.IO;

namespace DemoApp
{
    public class Startup
    {
        private readonly ILogger _logger;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Information);
                builder.AddConsole();
                builder.AddEventSourceLogger();
            });
            _logger = loggerFactory.CreateLogger<Startup>();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            AppContext.SetSwitch("Switch.Microsoft.Data.SqlClient.SuppressInsecureTLSWarning", true);
            AppContext.SetSwitch("Switch.Microsoft.Data.SqlClient.EnableRetryLogic", true);

            var options = new SqlRetryLogicOption()
            {
                NumberOfTries = 60,                         // Tries 60 times before throwing an exception
                DeltaTime = TimeSpan.FromSeconds(1),        // Preferred gap time to delay before retry
                MaxTimeInterval = TimeSpan.FromSeconds(3),  // Maximum gap time for each delay time before retry
            };

            // Create a retry logic provider
            SqlRetryLogicBaseProvider retryLogicProvider = SqlConfigurableRetryFactory.CreateFixedRetryProvider(options);

            var connectionStringBuilder = new SqlConnectionStringBuilder();

            //Read in the DB connection string secrets injected by Kubernetes.  Reading them in this way enables creating a single username/password secret which can be shared between the SQL MI custom resource and the app.
#if DEBUG
            var username = "";
            var password = "";
#else
            var usernamePath = Path.Combine(Directory.GetCurrentDirectory(), "secrets", "username");
            var passwordPath = Path.Combine(Directory.GetCurrentDirectory(), "secrets", "password");
            var username = File.ReadAllText(usernamePath);
            var password = File.ReadAllText(passwordPath);
#endif

            connectionStringBuilder.UserID = username;
            connectionStringBuilder.Password = password;
            connectionStringBuilder.IntegratedSecurity = false;
            connectionStringBuilder.DataSource = "k3s-sql-external-svc,11433";
            connectionStringBuilder.InitialCatalog = "master";
            connectionStringBuilder.Encrypt = false; //Demo hack.  Don't do this at home kids!

            // Adjust these values if you like.
            connectionStringBuilder.ConnectRetryCount = 100;
            connectionStringBuilder.ConnectRetryInterval = 1;  // Seconds.
            connectionStringBuilder.ConnectTimeout = 30;  // Seconds.

            //connection string builder -> connection string
            var connectionString = connectionStringBuilder.ToString();
            var sqlConnection = new SqlConnection(connectionString);
            sqlConnection.RetryLogicProvider = retryLogicProvider;
            
            var databaseName = Configuration.GetSection("DatabaseSettings")["DatabaseName"];

            //For debugging if neeeded:
            //_logger.LogInformation(connectionString);
            
            _logger.LogInformation("Sleeping, waiting for SQL server to be up and running...");

            //Wait for SQL server to come up
            System.Threading.Thread.Sleep(4*60*1000);

            //This is a workaround for the issue that is described here: https://github.com/dotnet/efcore/issues/15644
            //Normally the EnsureCreated method would create the database if it does not exist.
            //In the case of Arc SQL MI and Azure SQL PaaS and maybe other flavors of SQL, the error 18456 is thrown
            //because SQL rejects the login attempt because the database that is in the connection string doesn�t exist yet.
            //If you use 'master' as the InitialCatalog in the connection string and rely on EnsureCreated then the schema will be created in the master D.
            //It may be resolved in the future in response to this issue: https://github.com/dotnet/efcore/issues/27917

            SqlCommand createDatabaseSqlCommand = new SqlCommand(String.Format("CREATE DATABASE {0}", databaseName), sqlConnection);

            try
            {
                sqlConnection.Open();
                createDatabaseSqlCommand.ExecuteNonQuery();
            }
            catch (System.Exception ex)
            {
                _logger.LogDebug(ex, "An error occurred while opening the connection or executing the command.");
                _logger.LogInformation(ex.Message);
            }

            sqlConnection.ChangeDatabase(databaseName);
           
            // DEMO_CUSTOMIZATION - Change the DbContext class here if you want to use a different DbContext
            services.AddDbContext<BookStoreContext>(options =>
                options.UseSqlServer(sqlConnection, options => options.EnableRetryOnFailure()));
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // DEMO_CUSTOMIZATION: Change the DbContext class name here if you want to use a different DbContext
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, BookStoreContext dbContext)
        {
            loggerFactory.AddFile("/var/log/applog");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
