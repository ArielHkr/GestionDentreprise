using MySqlConnector;

namespace GestionDentreprise
{
    /// <summary>
    /// Gère la connexion à la base de données MySQL pour l'application.
    /// </summary>
    public class GestionBD
    {
        /// <summary>
        /// Chaîne de connexion utilisée pour se connecter à la base de données.
        /// </summary>
        private readonly string connectionString =
            "Server=localhost;Database=gestiontachesentreprise;User ID=root;Password=MariaDB;";

        /// <summary>
        /// Objet représentant la connexion MySQL.
        /// </summary>
        private MySqlConnection connection;

        /// <summary>
        /// Initialise une instance de <see cref="GestionBD"/> avec une connexion MySQL configurée.
        /// </summary>
        public GestionBD()
        {
            connection = new MySqlConnection(connectionString);
        }

        /// <summary>
        /// Ouvre la connexion à la base de données si elle n'est pas déjà ouverte.
        /// </summary>
        /// <exception cref="Exception">Lancée si l'ouverture échoue.</exception>
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

        /// <summary>
        /// Ferme la connexion à la base de données si elle est ouverte.
        /// </summary>
        /// <exception cref="Exception">Lancée si la fermeture échoue.</exception>
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

        /// <summary>
        /// Retourne l'objet MySqlConnection utilisé pour interagir avec la base de données.
        /// </summary>
        /// <returns>Connexion MySQL active ou prête à être ouverte.</returns>
        public MySqlConnection GetConnection()
        {
            return connection;
        }
    }
}