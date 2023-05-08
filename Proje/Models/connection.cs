using Microsoft.Data.SqlClient;

namespace Proje.Models
{
    public class connection
    {
        public static SqlConnection ServerConnect
        {
            get 
            { 
                SqlConnection sqlConnection = new SqlConnection("Server=94.73.170.33;Database=u1238636_CoreDB;User Id=u1238636_MCAN;Password=13675484020MCKlbs;TrustServerCertificate=True;");
                return sqlConnection;
            }
        }
    }
}
