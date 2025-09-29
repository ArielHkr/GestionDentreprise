using MySqlConnector;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace GestionDentreprise.Entites
{
    public abstract class Utilisateur
    {
        private int id; 
        private string nom=default!;
        private string prenom = default!;
        private string email = default!;
        private string motDePasse = default!;
        private string role = default!;
        private bool actif;

        protected Connexion cnx;

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

        public DateTime DateEmbauche { get; set; }


        protected Utilisateur(int id, string nom, string prenom, string email, string motDePasse, string role= "Employe", bool actif = true)
        {
            Id = id;
            Nom = nom;
            Prenom = prenom;
            Email = email;
            MotDePasse = motDePasse;
            Role = role;
            Actif = actif;

            cnx = new Connexion();
        }

        public static Utilisateur? SeConnecter(string email, string motDePasse)
        {
            Connexion cnx = new Connexion();
            try
            {
                cnx.Open();
                string query = @"SELECT id_utilisateur, nom, prenom, email, mot_de_passe, role,date_embauche, actif 
                                 FROM utilisateurs 
                                 WHERE email = @Email AND mot_de_passe = @Mdp;";

                var cmd = new MySqlCommand(query, cnx.GetConnection());
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Mdp", motDePasse);

                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    int id = reader.GetInt32("id_utilisateur");
                    string nom = reader.GetString("nom");
                    string prenom = reader.GetString("prenom");
                    string role = reader.GetString("role");
                    bool actif = reader.GetBoolean("actif");
                    DateTime date_embauche = reader.GetDateTime("date_embauche");
                    if (role == "Administrateur")
                    {
                        Administrateur admin = new Administrateur(id, nom, prenom, email, motDePasse, role, actif);
                        admin.DateEmbauche = date_embauche;
                        return admin;
                    }
                          
                    else if (role == "Employe")
                    {
                        Employe emp = new Employe(id, nom, prenom, email, motDePasse, role, actif);
                        emp.DateEmbauche = date_embauche;
                        return emp;

                    }
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

        public void MettreAJourMonProfil()
        {
            try
            {
                cnx.Open();

                string query = @"UPDATE utilisateurs
                         SET nom = @Nom,
                             prenom = @Prenom,
                             email = @Email
                         WHERE id_utilisateur = @Id;";

                using var cmd = new MySqlCommand(query, cnx.GetConnection());
                cmd.Parameters.AddWithValue("@Nom", Nom);
                cmd.Parameters.AddWithValue("@Prenom", Prenom);
                cmd.Parameters.AddWithValue("@Email", Email);
                cmd.Parameters.AddWithValue("@Id", Id);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de la mise à jour de l'utilisateur : " + ex.Message, ex);
            }
            finally
            {
                cnx.Close();
            }
        }
        public string AfficherInfos()
        {
            return $"{Prenom} {Nom} ({Role})";
        }
    }
}
