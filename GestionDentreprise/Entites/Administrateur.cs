using GestionDentreprise.Entites;
using MySqlConnector;
using System;
using System.Collections.Generic;

namespace GestionDentreprise.Entites
{
    public class Administrateur : Utilisateur
    {
        public Administrateur(int id, string nom, string prenom, string email, string motDePasse, string role = "Administrateur", bool actif = true, DateTime? dateEmbauche = null)
            : base(id, nom, prenom, email, motDePasse, role, actif, dateEmbauche)
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
                    DateTime? dateEmbauche = reader.IsDBNull(reader.GetOrdinal("date_embauche"))
                        ? null
                        : reader.GetDateTime("date_embauche");

                    liste.Add(new Employe(
                        id: id,
                        nom: nom,
                        prenom: prenom,
                        email: email,
                        motDePasse: motDePasse,
                        role: "Employe",
                        actif: actif,
                        dateEmbauche: dateEmbauche
                    ));
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
            List<Tache> taches = new List<Tache>();
            return taches;
        }
    }
}
