/**
 * Code-base configuration for the Northwind connection.
 * See http://www.asp.net/mvc/tutorials/getting-started-with-ef-using-mvc/connection-resiliency-and-command-interception-with-the-entity-framework-in-an-asp-net-mvc-application
 */

using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace AspMvc5Northwind.Models
{
    public class NorthwindConfiguration : DbConfiguration
    {
        public NorthwindConfiguration()
        {
            // SqlAzureExecutionStrategy is a child of DbExecutionStrategy, with some
            // preset retry rules for certain SQL errors.
            SetExecutionStrategy("System.Data.SqlClient", () => new SqlAzureExecutionStrategy());
        }
    }
}