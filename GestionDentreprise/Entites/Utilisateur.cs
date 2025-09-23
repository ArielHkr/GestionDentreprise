using MySqlConnector;
using System;

namespace GestionDentreprise
{
    public abstract class Utilisateur
    {
        private string? nom;
        private string? motDePasse;

        protected Connexion cnx;

        protected Utilisateur(string? nom, string? motDePasse)
        {
            cnx = new Connexion();
            this.nom = nom;
            this.motDePasse = motDePasse;
        }

        public string? Nom { get => nom; set => nom = value; }
        public string? MotDePasse { get => motDePasse; set => motDePasse = value; }

        public virtual bool SeConnecter(string role)
        {
            try
            {
                cnx.Open();

                string query = @"SELECT role 
                                 FROM utilisateurs 
                                 WHERE nom = @nom 
                                   AND mot_de_passe = @mdp 
                                   AND role = @role;";

                using var cmd = new MySqlCommand(query, cnx.GetConnection());
                cmd.Parameters.AddWithValue("@nom", Nom);
                cmd.Parameters.AddWithValue("@mdp", MotDePasse);
                cmd.Parameters.AddWithValue("@role", role);

                var result = cmd.ExecuteScalar();

                return result != null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la connexion : {ex.Message}");
                return false;
            }
            finally
            {
                cnx.Close();
            }
        }
    }
}
