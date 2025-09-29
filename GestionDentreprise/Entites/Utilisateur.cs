using MySqlConnector;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace GestionDentreprise.Entites
{
    public abstract class Utilisateur
    {
        private int id; // nouvel attribut privé pour l'id
        private string nom;
        private string prenom;
        private string email;
        private string motDePasse;
        private string role;
        private bool actif;
        private DateTime? dateEmbauche;

        protected Connexion cnx;

        // Accesseurs en lecture/écriture
        public int Id
        {
            get => id;
            set
            {
                if (value < 0) throw new ArgumentException("L'id doit être positif.");
                id = value;
            }
        }

        public string Nom
        {
            get => nom;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Le nom est requis.");
                nom = value.Trim();
            }
        }

        public string Prenom
        {
            get => prenom;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Le prénom est requis.");
                prenom = value.Trim();
            }
        }

        public string Email
        {
            get => email;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || !Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                    throw new ArgumentException("L'email n'est pas valide.");
                email = value.Trim();
            }
        }

        public string MotDePasse
        {
            get => motDePasse;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length < 6 || value.Length > 100 || !Regex.IsMatch(value, @"^(?=.*[A-Za-z])(?=.*\d).+$"))
                    throw new ArgumentException("Mot de passe invalide.");
                motDePasse = value;
            }
        }

        public string Role
        {
            get => role;
            set => role = value;
        }

        public bool Actif
        {
            get => actif;
            set => actif = value;
        }

        public DateTime? DateEmbauche
        {
            get => dateEmbauche;
            set => dateEmbauche = value;
        }

        // Constructeur
        protected Utilisateur(int id, string nom, string prenom, string email, string motDePasse, string role, bool actif = true, DateTime? dateEmbauche = null)
        {
            Id = id;
            Nom = nom;
            Prenom = prenom;
            Email = email;
            MotDePasse = motDePasse;
            Role = role;
            Actif = actif;
            DateEmbauche = dateEmbauche;

            cnx = new Connexion();
        }

        // Méthode statique pour se connecter
        public static Utilisateur? SeConnecter(string email, string motDePasse)
        {
            Connexion cnx = new Connexion();
            try
            {
                cnx.Open();
                string query = @"SELECT id_utilisateur, nom, prenom, email, mot_de_passe, role, date_embauche, actif 
                                 FROM utilisateurs 
                                 WHERE email = @Email AND mot_de_passe = @Mdp;";

                using var cmd = new MySqlCommand(query, cnx.GetConnection());
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Mdp", motDePasse);

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    int id = reader.GetInt32("id_utilisateur");
                    string nom = reader.GetString("nom");
                    string prenom = reader.GetString("prenom");
                    string role = reader.GetString("role");
                    DateTime? dateEmbauche = reader.IsDBNull(reader.GetOrdinal("date_embauche"))
                        ? null
                        : reader.GetDateTime("date_embauche");
                    bool actif = reader.GetBoolean("actif");

                    if (role == "Administrateur")
                        return new Administrateur(id, nom, prenom, email, motDePasse, role, actif, dateEmbauche);
                    else if (role == "Employe")
                        return new Employe(id, nom, prenom, email, motDePasse, role, actif, dateEmbauche);
                    else
                        return null;
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur connexion : " + ex.Message);
                return null;
            }
            finally
            {
                cnx.Close();
            }
        }

        public abstract List<Tache> RecupererTaches();

        public string AfficherInfos()
        {
            return $"Utilisateur: {Nom} {Prenom} ({Role})";
        }
    }
}
