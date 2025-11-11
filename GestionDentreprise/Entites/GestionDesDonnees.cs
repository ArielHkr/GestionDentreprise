using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDentreprise.Entites
{
    public static class GestionDesDonnees
    {
        public static Utilisateur? ObtenirUtilisateur(string email, string motDePasse)
        {
            GestionBD cnx = new GestionBD();
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

                    else 
                    {
                        Employe emp = new Employe(id, nom, prenom, email, motDePasse, role, actif);
                        emp.DateEmbauche = date_embauche;
                        return emp;

                    }

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


        public static void MettreAJourUtilisateur(string Nom, string Prenom,string Email,int Id)
        {
            GestionBD cnx = new GestionBD();
            try
            {
                cnx.Open();

                string query = @"UPDATE utilisateurs
                         SET nom = @Nom,
                             prenom = @Prenom,
                             email = @Email
                         WHERE id_utilisateur = @Id;";

                var cmd = new MySqlCommand(query, cnx.GetConnection());
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

        public static List<Tache> ObtenirLesTaches( int? Id )
        {
            var liste = new List<Tache>();

            GestionBD cnx = new GestionBD();
            try
            {
                cnx.Open();

                if ( Id != null && Id > 0)
                {
                    string query = @"SELECT id_tache, titre, description, priorite, date_creation, date_limite, etat 
                                 FROM Taches 
                                 WHERE id_utilisateur = @IdUtilisateur;";

                    var cmd = new MySqlCommand(query, cnx.GetConnection());
                    cmd.Parameters.AddWithValue("@IdUtilisateur", Id);

                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        int idTache = reader.GetInt32("id_tache");
                        string titre = reader.GetString("titre");
                        string description = reader.IsDBNull(reader.GetOrdinal("description")) ? "" : reader.GetString("description");
                        string priorite = reader.IsDBNull(reader.GetOrdinal("priorite")) ? "Moyenne" : reader.GetString("priorite");
                        DateTime dateCreation = reader.GetDateTime("date_creation");
                        DateTime dateLimite = reader.GetDateTime("date_limite");
                        string etat = reader.GetString("etat");

                        Tache tache = new Tache(titre, description, priorite, dateLimite);
                        tache.IdTache = idTache;
                        tache.DateCreation = dateCreation;
                        tache.Etat = etat;
                        tache.IdUtilisateur = Id;
                        liste.Add(tache);
                    }
                }
                else
                {
                    string query = @"SELECT id_tache, titre, description, priorite, date_creation, date_limite, etat, id_utilisateur
                                 FROM Taches;";

                    var cmd = new MySqlCommand(query, cnx.GetConnection());

                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        int idTache = reader.GetInt32("id_tache");
                        string titre = reader.GetString("titre");
                        string description = reader.IsDBNull(reader.GetOrdinal("description")) ? "" : reader.GetString("description");
                        string priorite = reader.IsDBNull(reader.GetOrdinal("priorite")) ? "Moyenne" : reader.GetString("priorite");
                        DateTime dateCreation = reader.GetDateTime("date_creation");
                        DateTime dateLimite =  reader.GetDateTime("date_limite");
                        string etat = reader.GetString("etat");
                        int? IdUtilisateur;

                        if (reader["id_utilisateur"] == DBNull.Value)
                        {
                            IdUtilisateur = null;
                        }
                        else
                        {
                            IdUtilisateur = Convert.ToInt32(reader["id_utilisateur"]);
                        }

                        Tache tache = new Tache(titre, description, priorite, dateLimite);
                        tache.IdTache = idTache;
                        tache.DateCreation = dateCreation;
                        tache.Etat = etat;
                        tache.IdUtilisateur = IdUtilisateur;
                        liste.Add(tache);
                    }
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


        public static void AttribuerUneTache(Tache tache, Employe employe)
        {
            GestionBD cnx = new GestionBD();

            try
            {
                string query = @"
            UPDATE Taches
            SET id_utilisateur = @id_u
            WHERE id_tache = @id_tache";

                cnx.Open();

                using (MySqlCommand cmd = new MySqlCommand(query, cnx.GetConnection()))
                {
                    cmd.Parameters.AddWithValue("@id_u", employe.Id);
                    cmd.Parameters.AddWithValue("@id_tache", tache.IdTache);

                    int lignesAffectees = cmd.ExecuteNonQuery();
         
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de l'attribution : " + ex.Message);
            }
            finally
            {
                cnx.Close();
            }
        }

        public static void AjouterUneTache(Tache tache)
        {
            GestionBD cnx = new GestionBD();
            try
            {
                string query = @"
                   INSERT INTO Taches ( titre, description, priorite, date_limite, etat)
                   VALUES ( @titre, @description, @priorite, @date_limite, @etat)";


                cnx.Open();
                MySqlCommand cmd = new MySqlCommand(query, cnx.GetConnection());
                cmd.Parameters.AddWithValue("@titre", tache.Titre);
                cmd.Parameters.AddWithValue("@description", tache.Description);
                cmd.Parameters.AddWithValue("@priorite", tache.Priorite);

                
                cmd.Parameters.AddWithValue("@date_limite", tache.DateLimite.Date);
                cmd.Parameters.AddWithValue("@etat", tache.Etat);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex) {
                throw new Exception("Erreur : " + ex.Message);
            }
            finally
            {
                cnx.Close();
            }
        }
        public static List<Employe> ObtenirLesEmployes()
        {
            List<Employe> liste = new List<Employe>();
            GestionBD cnx = new GestionBD();

            try
            {
                cnx.Open();
                string query = @"SELECT id_utilisateur, nom, prenom, email, mot_de_passe, role, actif, date_embauche, points
                         FROM utilisateurs 
                         WHERE role = 'Employe' AND actif = 1;";

                MySqlCommand cmd = new MySqlCommand(query, cnx.GetConnection());
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int id = reader.GetInt32("id_utilisateur");
                    string nom = reader.GetString("nom");
                    string prenom = reader.GetString("prenom");
                    string email = reader.GetString("email");
                    string mdp = reader.GetString("mot_de_passe");
                    bool actif = reader.GetBoolean("actif");
                    DateTime dateEmbauche = reader.GetDateTime("date_embauche");
                    int points = reader.GetInt32("points"); 

                    Employe emp = new Employe(id, nom, prenom, email, mdp, "Employe", actif, points);
                    emp.DateEmbauche = dateEmbauche;
                    liste.Add(emp);
                }
            }
            finally
            {
                cnx.Close();
            }

            return liste;
        }


        public static void RendreEmployeInactif(int idEmploye)
        {
            GestionBD cnx = new GestionBD();
            try
            {
                cnx.Open();
                string query = @"UPDATE utilisateurs
                         SET actif = 0
                         WHERE id_utilisateur = @Id AND role = 'Employe';";

                MySqlCommand cmd = new MySqlCommand(query, cnx.GetConnection());
                cmd.Parameters.AddWithValue("@Id", idEmploye);
                cmd.ExecuteNonQuery();
            }
            finally
            {
                cnx.Close();
            }
        }


        public static List<Utilisateur> RechercherUtilisateur(string chaine)
        {
            var resultats = new List<Utilisateur>();
            var cnx = new GestionBD();

            try
            {
                cnx.Open();
                var cmd = new MySqlCommand(
                    "SELECT * FROM utilisateurs WHERE nom LIKE CONCAT('%', @Chaine, '%') OR prenom LIKE CONCAT('%', @Chaine, '%');",
                    cnx.GetConnection());
                cmd.Parameters.AddWithValue("@Chaine", chaine);

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int id = reader.GetInt32("id_utilisateur");
                    string nom = reader.GetString("nom");
                    string prenom = reader.GetString("prenom");
                    string email = reader.GetString("email");
                    string mdp = reader.GetString("mot_de_passe");
                    string role = reader.GetString("role");
                    bool actif = reader.GetBoolean("actif");

                    Utilisateur u;
                    if (role == "Administrateur")
                    {
                        u = new Administrateur(id, nom, prenom, email, mdp, role, actif);
                    }
                    else
                    {
                        u = new Employe(id, nom, prenom, email, mdp, role, actif);
                    }

                    resultats.Add(u);
                }
            }
            finally
            {
                cnx.Close();
            }

            return resultats;
        }
        public static void MettreAJourPointsEmploye(int idEmploye, int points)
        {
            GestionBD cnx = new GestionBD();
            try
            {
                cnx.Open();
                string query = @"UPDATE utilisateurs SET points = @Points WHERE id_utilisateur = @Id AND role = 'Employe';";
                using (var cmd = new MySqlCommand(query, cnx.GetConnection()))
                {
                    cmd.Parameters.AddWithValue("@Points", points);
                    cmd.Parameters.AddWithValue("@Id", idEmploye);
                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                cnx.Close();
            }
        }
        public static void EmbaucherEmploye(Employe employe)
        {
            GestionBD cnx = new GestionBD();
            try
            {
                cnx.Open();

                string query = @"
            INSERT INTO utilisateurs (nom, prenom, email, mot_de_passe, role, actif, date_embauche)
            VALUES (@Nom, @Prenom, @Email, @Mdp, @Role, @Actif, @DateEmbauche);";

                using (MySqlCommand cmd = new MySqlCommand(query, cnx.GetConnection()))
                {
                    cmd.Parameters.AddWithValue("@Nom", employe.Nom);
                    cmd.Parameters.AddWithValue("@Prenom", employe.Prenom);
                    cmd.Parameters.AddWithValue("@Email", employe.Email);
                    cmd.Parameters.AddWithValue("@Mdp", employe.MotDePasse);
                    cmd.Parameters.AddWithValue("@Role", "Employe");
                    cmd.Parameters.AddWithValue("@Actif", true);
                    cmd.Parameters.AddWithValue("@DateEmbauche", employe.DateEmbauche);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de l'embauche de l'employé : " + ex.Message);
            }
            finally
            {
                cnx.Close();
            }
        }

    }
}
