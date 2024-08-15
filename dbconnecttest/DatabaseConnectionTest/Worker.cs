using Microsoft.Data.SqlClient;

namespace DatabaseConnectionTest
{
    public class Worker : BackgroundService
    {
        int i = 1;
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                
                var options = new SqlRetryLogicOption()
                {
                    // Tries 60 times before throwing an exception
                    NumberOfTries = 60,
                    // Preferred gap time to delay before retry
                    DeltaTime = TimeSpan.FromSeconds(1),
                    // Maximum gap time for each delay time before retry
                    MaxTimeInterval = TimeSpan.FromSeconds(3),
                };


                // Create a retry logic provider
                SqlRetryLogicBaseProvider retryLogicProvider = SqlConfigurableRetryFactory.CreateFixedRetryProvider(options);

                ConfigurationManager configManager = new ConfigurationManager();

                var connectionStringBuilder = new SqlConnectionStringBuilder();

                //Read in the DB connection string secrets injected by Kubernetes.  Reading them in this way enables creating a single username/password secret which can be shared between the SQL MI custom resource and the app.
#if DEBUG
                var username = "";
                var password = "";
#else
                var usernamePath = Path.Combine(Directory.GetCurrentDirectory(), "secrets", "username");
                var passwordPath = Path.Combine(Directory.GetCurrentDirectory(), "secrets", "password");
                var username = System.IO.File.ReadAllText(usernamePath);
                var password = System.IO.File.ReadAllText(passwordPath);
#endif

                connectionStringBuilder.UserID = username;
                connectionStringBuilder.Password = password;
                connectionStringBuilder.IntegratedSecurity = false;
                connectionStringBuilder.DataSource = Environment.GetEnvironmentVariable("DB_DATASOURCE") ?? "k3s-sql-external-svc,11433";
                connectionStringBuilder.InitialCatalog = "master";
                connectionStringBuilder.Encrypt = false; //Demo hack.  Don't do this at home kids!

                // Adjust these values if you like.
                connectionStringBuilder.ConnectRetryCount = 100;
                connectionStringBuilder.ConnectRetryInterval = 1;  // Seconds.

                // Leave these values as they are.
                connectionStringBuilder.ConnectTimeout = 30;

                //connection string builder -> connection string
                var connectionString = connectionStringBuilder.ToString();
                var sqlConnection = new SqlConnection(connectionString);
                sqlConnection.RetryLogicProvider = retryLogicProvider;

                using (sqlConnection)
                {
                    // DEMO_CUSTOMIZATION - Add/Remove/Change the SQL queries that you want to execute on the periodic interval.
                    // DEMO_CUSTOMIZATION - Note: the queries below use three-part names to point to the table - ie. database.schema.table.
                    // If you use a demo database name with a name other than demo or a different schema or table name, adjust acoordingly.
                    // Make sure that your VALUE data set for the INSERT statement match your schema.

                    //Get the current SQL instance name
                    SqlCommand commandSelectServerName = new SqlCommand("SELECT @@SERVERNAME", sqlConnection);
                    commandSelectServerName.Connection = sqlConnection;

                    //Insert some data into a table                    
                    var bookInsertString = String.Format("INSERT INTO Demo.dbo.Book VALUES ('Some title of a book - {0}','2022-01-01','Technology', 39.95)", i);
                    SqlCommand commandInsertBooks = new SqlCommand(bookInsertString, sqlConnection);

                    //Get the count of records in a table
                    var bookCountString = String.Format("SELECT COUNT(*) FROM Demo.dbo.Book");
                    SqlCommand commandCountBooks = new SqlCommand(bookCountString, sqlConnection);
                    
                    try
                    {
                        sqlConnection.Open();

                        //Execute the queries
                        commandInsertBooks.ExecuteScalar();
                        var serverName = commandSelectServerName.ExecuteScalar();
                        var bookCount = commandCountBooks.ExecuteScalar().ToString();

                        //DEMO_CUSTOMIZATION - Change the logging output format as needed for the demo
                        //Log the results of the queries
                        _logger.LogInformation("ServerName: {0} \t ServerVersion: {1} \t Count: {2}", serverName, sqlConnection.ServerVersion, bookCount);                        
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                    }
                }
                i++;

                // DEMO_CUSTOMIZATION - Change the frequency that the connection/query happens here
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}