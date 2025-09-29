using GestionDentreprise.Entites;
using MySqlConnector;
using System;
using System.Collections.Generic;

namespace GestionDentreprise.Entites
{
    public class Administrateur : Utilisateur
    {
        public Administrateur(int id, string nom, string prenom, string email, string motDePasse, string role = "Administrateur", bool actif = true, DateTime? dateEmbauche = null)
            : base(id, nom, prenom, email, motDePasse, role, actif)
        {
        }

        public List<Employe> RecupererTousLesEmployes()
        {
            List<Employe> liste = new List<Employe>();

            try
            {
                Connexion cnx = new Connexion();
                cnx.Open();

                string query = @"SELECT id_utilisateur, nom, prenom, email, mot_de_passe, role, actif, date_embauche 
                                 FROM utilisateurs 
                                 WHERE role = 'Employe';";

                using MySqlCommand cmd = new MySqlCommand(query, cnx.GetConnection());
                using MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int id = reader.GetInt32("id_utilisateur");
                    string nom = reader.GetString("nom");
                    string prenom = reader.GetString("prenom");
                    string email = reader.GetString("email");
                    string motDePasse = reader.GetString("mot_de_passe");
                    bool actif = reader.GetBoolean("actif");
                    DateTime dateEmbauche =  reader.GetDateTime("date_embauche");
                    Employe emp = new Employe(id, nom, prenom, email, motDePasse,"Employe", actif);
                    emp.DateEmbauche = dateEmbauche ;
                    liste.Add(emp);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de la récupération des employés : " + ex.Message, ex);
            }

           
            return liste;
        }
        public override List<Tache> RecupererTaches()
        {
            var liste = new List<Tache>();
            Connexion cnx = new Connexion();
            try
            {
                cnx.Open();

                string query = @"SELECT id_tache, titre, description, priorite, date_creation, date_limite, etat 
                                 FROM Taches 
                                 ";

                var cmd = new MySqlCommand(query, cnx.GetConnection());

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int idTache = reader.GetInt32("id_tache");
                    string titre = reader.GetString("titre");
                    string description = reader.IsDBNull(reader.GetOrdinal("description")) ? "" : reader.GetString("description");
                    string priorite = reader.IsDBNull(reader.GetOrdinal("priorite")) ? "Moyenne" : reader.GetString("priorite");
                    DateTime dateCreation = reader.GetDateTime("date_creation");
                    DateTime? dateLimite = reader.IsDBNull(reader.GetOrdinal("date_limite")) ? null : reader.GetDateTime("date_limite");
                    string etat = reader.GetString("etat");

                    liste.Add(new Tache(idTache, titre, description, priorite, dateCreation, dateLimite, etat, this.Id));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de la récupération des tâches : " + ex.Message);
            }
            finally
            {
                cnx.Close();
            }

            return liste;
        }

    }
}
