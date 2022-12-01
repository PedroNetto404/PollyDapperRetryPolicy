using System.Data;
using System.Data.SqlClient;

namespace PollyDapperRetryPolicy.Data;

public abstract class DbSession
{
    protected readonly IDbConnection Connection; 
    protected DbSession()
    {
        Connection = new SqlConnection("Server=DESKTOP-7EUD7S1\\SQLEXPRESS;Database=AdventureWorks2012;Trusted_Connection=True;"); 
    }
}