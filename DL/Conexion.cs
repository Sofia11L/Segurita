using System.Configuration;

namespace DL
{
    public class Conexion
    {
        public static string GetStringConnection()
        {
            string conexion = ConfigurationManager.ConnectionStrings["SLerdoSeguritas"].ConnectionString;

            return conexion;
        }
    }
}