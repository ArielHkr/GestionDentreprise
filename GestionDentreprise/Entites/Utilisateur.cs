using MySqlConnector;
using System;
using System.Text.RegularExpressions;

namespace GestionDentreprise
{
    public abstract class Utilisateur
    {
        private readonly string nom;
        private readonly string motDePasse;

        protected Connexion cnx;

        protected Utilisateur(string? nom, string? motDePasse)
        {
            // 🔹 Validation du nom
            if (string.IsNullOrWhiteSpace(nom))
                throw new ArgumentException("Le nom d'utilisateur est requis.", nameof(nom));
            if (nom.Length > 50)
                throw new ArgumentException("Le nom d'utilisateur ne doit pas dépasser 50 caractères.", nameof(nom));

            // 🔹 Validation du mot de passe
            if (string.IsNullOrWhiteSpace(motDePasse))
                throw new ArgumentException("Le mot de passe est requis.", nameof(motDePasse));
            if (motDePasse.Length < 6)
                throw new ArgumentException("Le mot de passe doit contenir au moins 6 caractères.", nameof(motDePasse));
            if (motDePasse.Length > 100)
                throw new ArgumentException("Le mot de passe ne doit pas dépasser 100 caractères.", nameof(motDePasse));
            if (!Regex.IsMatch(motDePasse, @"^(?=.*[A-Za-z])(?=.*\d).+$"))
                throw new ArgumentException("Le mot de passe doit contenir au moins une lettre et un chiffre.", nameof(motDePasse));

            // 🔹 Affectation si tout est valide
            this.nom = nom.Trim();
            this.motDePasse = motDePasse;

            cnx = new Connexion();
        }

        public string Nom => nom;
        public string MotDePasse => motDePasse;

        public virtual bool SeConnecter(string role)
        {
            // Validation du rôle avant requête
            if (string.IsNullOrWhiteSpace(role))
            {
                Console.WriteLine("Le rôle est requis.");
                return false;
            }

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
