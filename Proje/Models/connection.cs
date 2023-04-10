using Microsoft.Data.SqlClient;

namespace Proje.Models
{
    public class connection
    {
        public static SqlConnection ServerConnect
        {
            get 
            { 
                SqlConnection sqlConnection = new SqlConnection("Server=DESKTOP-5EPP4QL;Database=iakademi45Core_proje;Trusted_Connection=True;TrustServerCertificate=True;");
                return sqlConnection;
            }
        }
    }
}
