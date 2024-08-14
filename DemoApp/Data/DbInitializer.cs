using DemoApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using CsvHelper.Expressions;
using System.IO;
using System.Reflection;
using System.Text;
using System.Globalization;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace DemoApp.Data
{
    public class DbInitializer
    {
        public static void Initialize(WindFarmContext context)
        {
            // Look for any windmills.
            if (!context.Windmills.Any())
            {
                // DB has been not been seeded
                var windmills = new Windmill[]
                {
                    new Windmill{Model="Windmaster1090",Manufacturer="SuperWind",DateOfLastMaintenance=DateTime.Parse("2005-09-01")},
                };

                foreach (Windmill w in windmills)
                {
                    context.Windmills.Add(w);
                }
            }

            /* Look for any samples in a CSV file and load them.  This code is a work in progress.
            
            if (!context.TurbineTelemetrySamples.Any())
            {
                // DB has not been seeded
                Assembly assembly = Assembly.GetExecutingAssembly();
                string resourceName = "RawWindmillTelemetryData.csv";

                var embeddedProvider = new EmbeddedFileProvider(Assembly.GetExecutingAssembly());
                using (var stream = embeddedProvider.GetFileInfo("Data\\RawWindmillTelemetryData.csv").CreateReadStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        CsvReader csvReader = new CsvReader((IParser)reader);
                        var samples = csvReader.GetRecords<TurbineTelemetrySample>().ToArray();
                        context.TurbineTelemetrySamples.AddRange(samples);
                    }
                }
            }
            */
            context.SaveChanges();
        }

        public static void Initialize(BookStoreContext context)
        {
            // Look for any books.
            if (!context.Books.Any())
            {
                // DB has been not been seeded, so let's seed some data into the table
                var books = new Book[]
                {
                    new Book{Title="Azure Arc-enabled Data Services",Genre="Technology",Price=Decimal.Parse("35.52"),ReleaseDate=DateTime.Parse("2021-12-14")},
                    new Book{Title="Implementing Hybrid Cloud with Azure Arc: Explore the new-generation hybrid cloud and learn how to build Azure Arc-enabled solutions",Genre="Technology",Price=Decimal.Parse("44.99"),ReleaseDate=DateTime.Parse("2021-07-21")},
                    new Book{Title="Azure Arc-enabled Data Services Revealed",Genre="Technology",Price=Decimal.Parse("39.99"),ReleaseDate=DateTime.Parse("2021-02-03")},
                    new Book{Title="Azure Arc-enabled Kubernetes for Multicloud",Genre="Technology",ReleaseDate=DateTime.Parse("2021-02-01")}
                };

                foreach (Book w in books)
                {
                    context.Books.Add(w);
                }
            }

            context.SaveChanges();
        }

        /* DEMO_CUSTOMIZATION: Add additional Intialize method overloads here for additional DbContexts as needed
        Copy one of the existing Initialize methods and paste it as a peer of the others.

        Rename the context class in the Intialize method parameter signature.
        Example:
        public static void Initialize(BookStoreContext context) --> public static void Initialize(MiningOperationsContext context)

        Rename the DbSet of the context
        Example:
        if (!context.Books.Any()) --> if (!context.MiningCarts.Any())

        Rename the collection variable name and the data model class name
        Example:
        var books = new Book[] --> var miningcarts = new MiningCart[]

        Rename the data model class name on each new object line.
        Example:
        new Book{ --> new MiningCart{ 

        Change the property assignments to match the data model class properties
        Example:
        Title="Azure Arc-enabled Data Services",Genre="Technology" --> CartId="3",Type="Loader" ... 

        Change the data model class name and collection variable name to match the above
        Example:
        foreach (Book w in books) --> foreach (MiningCart w in miningcarts)

        Change the DbSet name
        context.Books.Add(w); --> context.MiningCarts.Add(w);

        Complete example:

        public static void Initialize(MiningOperationsContext context)
        {
            // Look for any mining carts.
            if (!context.MiningCarts.Any())
            {
                // DB has been not been seeded
                var miningcarts = new MiningCart[]
                {
                    new MiningCart{CartId="3",Type="Loader"},
                    new MiningCart{CartId="4",Type="Loader"},
                    new MiningCart{CartId="5",Type="Personnel"},
                    new MiningCart{CartId="6",Type="Carrier"}
                };

                foreach (MiningCart w in miningcarts)
                {
                    context.MiningCarts.Add(w);
                }
            }

            context.SaveChanges();
        }
        */
    }
}



