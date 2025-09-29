using GestionDentreprise.Entites;
using MySqlConnector;
using System;
using System.Collections.Generic;

namespace GestionDentreprise.Entites
{
    public class Employe : Utilisateur
    {
        public Employe(int id, string nom, string prenom, string email, string motDePasse, string role = "Employe", bool actif = true, DateTime? dateEmbauche = null)
            : base(id, nom, prenom, email, motDePasse, role, actif, dateEmbauche)
        {
        }

        public override List<Tache> RecupererTaches()
        {
            var liste = new List<Tache>();

            try
            {
                cnx.Open();

                string query = @"SELECT id_tache, titre, description, priorite, date_creation, date_limite, etat 
                                 FROM Taches 
                                 WHERE id_utilisateur = @IdUtilisateur;";

                using var cmd = new MySqlCommand(query, cnx.GetConnection());
                cmd.Parameters.AddWithValue("@IdUtilisateur", Id); // Utilise l'id de l'employé

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int idTache = reader.GetInt32("id_tache");
                    string titre = reader.GetString("titre");
                    string description = reader.IsDBNull(reader.GetOrdinal("description")) ? "" : reader.GetString("description");
                    string priorite = reader.IsDBNull(reader.GetOrdinal("priorite")) ? "Moyenne" : reader.GetString("priorite");
                    DateTime dateCreation = reader.GetDateTime("date_creation");
                    DateTime? dateLimite = reader.IsDBNull(reader.GetOrdinal("date_limite")) ? null : reader.GetDateTime("date_limite");
                    string etat = reader.GetString("etat");

                    liste.Add(new Tache(idTache, titre, description, priorite, dateCreation, dateLimite, etat,this.Id));
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
