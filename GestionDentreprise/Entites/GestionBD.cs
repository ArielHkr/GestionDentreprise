using MySqlConnector;

namespace GestionDentreprise
{
    public class GestionBD
    {
        private readonly string connectionString =
            "Server=localhost;Database=gestiontachesentreprise;User ID=root;Password=MariaDB;";

        private MySqlConnection connection;

        public GestionBD()
        {
            connection = new MySqlConnection(connectionString);
        }

        public void Open()
        {
            try
            {
                if (connection.State != System.Data.ConnectionState.Open)
                    connection.Open();
            }
            catch (Exception ex)
            {
                throw new Exception("Impossible d'ouvrir la connexion : " + ex.Message);
            }
        }

        public void Close()
        {
            try
            {
                if (connection.State != System.Data.ConnectionState.Closed)
                    connection.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Impossible de fermer la connexion : " + ex.Message);
            }
        }

        public MySqlConnection GetConnection()
        {
            return connection;
        }
    }
}
