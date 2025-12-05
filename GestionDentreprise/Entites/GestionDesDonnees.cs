using MySqlConnector;
using GestionDentreprise.Firewall;
namespace GestionDentreprise.Entites
{
    public static class GestionDesDonnees
    {
        /// <summary>
        /// Obtient un utilisateur actif correspondant à l'email et au mot de passe fournis.
        /// </summary>
        /// <param name="email">Adresse courriel.</param>
        /// <param name="motDePasse">Mot de passe.</param>
        /// <returns>Une instance de <see cref="Utilisateur"/> si trouvée, sinon <c>null</c>.</returns>
        public static Utilisateur? ObtenirUtilisateur(string email, string motDePasseSaisi)
        {
            GestionBD cnx = new GestionBD();
            try
            {
                cnx.Open();
                string query = @"SELECT id_utilisateur, nom, prenom, email, 
                                mot_de_passe_hash, mot_de_passe_salt,
                                role, date_embauche, actif, points
                         FROM utilisateurs 
                         WHERE email = @Email AND actif = 1;";

                var cmd = new MySqlCommand(query, cnx.GetConnection());
                cmd.Parameters.AddWithValue("@Email", email);

                var reader = cmd.ExecuteReader();
                if (!reader.Read())
                    return null;

                int id = reader.GetInt32("id_utilisateur");
                string nom = reader.GetString("nom");
                string prenom = reader.GetString("prenom");
                string role = reader.GetString("role");
                bool actif = reader.GetBoolean("actif");
                DateTime date_embauche = reader.GetDateTime("date_embauche");
                int points = reader.IsDBNull(reader.GetOrdinal("points")) ? 0 : reader.GetInt32("points");

                byte[] hashFromDb = Convert.FromBase64String(reader.GetString("mot_de_passe_hash"));
                byte[] saltFromDb = Convert.FromBase64String(reader.GetString("mot_de_passe_salt"));

                bool _valide = Firewall.Firewall.VerifierMotDePasse(motDePasseSaisi, saltFromDb, hashFromDb);

                if (!_valide)
                    return null;

                if (role == "Administrateur")
                {
                    var admin = new Administrateur(id, nom, prenom, email, "", role, actif);
                    admin.DateEmbauche = date_embauche;
                    return admin;
                }
                else
                {
                    var emp = new Employe(id, nom, prenom, email, "", role, actif, points);
                    emp.DateEmbauche = date_embauche;
                    return emp;
                }
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



        /// <summary>
        /// Met à jour le nom, le prénom et l'email d'un utilisateur.
        /// </summary>
        public static void MettreAJourUtilisateur(string Nom, string Prenom, string Email, int Id)
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

        /// <summary>
        /// Retourne les tâches associées à un utilisateur ou toutes les tâches si l'ID est nul.
        /// </summary>
        public static List<Tache> ObtenirLesTaches(int? Id)
        {
            var liste = new List<Tache>();

            GestionBD cnx = new GestionBD();
            try
            {
                cnx.Open();

                if (Id != null && Id > 0)
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
                        DateTime dateLimite = reader.GetDateTime("date_limite");
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

        /// <summary>
        /// Attribue une tâche existante à un employé.
        /// </summary>
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

        /// <summary>
        /// Permet d'ajouter une tâche.
        /// </summary>
        /// <param name="tache"></param>
        /// <exception cref="Exception"></exception>
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
            catch (Exception ex)
            {
                throw new Exception("Erreur : " + ex.Message);
            }
            finally
            {
                cnx.Close();
            }
        }
       
        /// <summary>
        /// Permet d'obtenir tous les employés.
        /// </summary>
        /// <returns>Retourne un liste d'employés.</returns>
        public static List<Employe> ObtenirLesEmployes()
        {
            List<Employe> liste = new List<Employe>();
            GestionBD cnx = new GestionBD();

            try
            {
                cnx.Open();
                string query = @"SELECT id_utilisateur, nom, prenom, email, mot_de_passe_hash, role, actif, date_embauche, points
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
                    string mdp = reader.GetString("mot_de_passe_hash");
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

        /// <summary>
        /// Désactie un employé.
        /// </summary>
        /// <param name="idEmploye">L'employé à désactiver</param>
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

        /// <summary>
        /// Permet de trouver un employé
        /// </summary>
        /// <param name="chaine">Le nom de l'employé</param>
        /// <returns>Une liste d'employés correspondants.</returns>
        public static List<Utilisateur> RechercherEmployw(string chaine)
        {
            var resultats = new List<Utilisateur>();
            var cnx = new GestionBD();

            try
            {
                cnx.Open();
                var cmd = new MySqlCommand(
                    "SELECT * FROM utilisateurs WHERE actif = 1  AND (nom LIKE CONCAT('%', @Chaine, '%') OR prenom LIKE CONCAT('%', @Chaine, '%'));",
                    cnx.GetConnection());
                cmd.Parameters.AddWithValue("@Chaine", chaine);

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int id = reader.GetInt32("id_utilisateur");
                    string nom = reader.GetString("nom");
                    string prenom = reader.GetString("prenom");
                    string email = reader.GetString("email");
                    string mdp = reader.GetString("mot_de_passe_hash");
                    string role = reader.GetString("role");
                    bool actif = reader.GetBoolean("actif");
                  
                    Utilisateur utilisateur;
                    if (role == "Administrateur")
                    {
                        utilisateur = new Administrateur(id, nom, prenom, email, mdp, role, actif);
                    }
                    else
                    {
                        utilisateur = new Employe(id, nom, prenom, email, mdp, role, actif);
                    }
                    utilisateur.DateEmbauche = reader.GetDateTime("date_embauche");

                    resultats.Add(utilisateur);
                }
            }
            finally
            {
                cnx.Close();
            }

            return resultats;
        }
     

        /// <summary>
        /// Met à jour les points de l'employé.
        /// </summary>
        /// <param name="idEmploye"></param>
        /// <param name="points"></param>
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
            catch(Exception ex) {   
            
                throw new Exception(ex.ToString());
            }
            finally
            {
                cnx.Close();
            }
        }
      
        /// <summary>
        /// Permet d'ajouter un employé.
        /// </summary>
        /// <param name="employe">L'employé à ajouter.</param>
        /// <exception cref="Exception"></exception>
        public static void AjouterUnEmploye(Employe employe)
        {
            GestionBD cnx = new GestionBD();
            byte[] salt = Firewall.Firewall.CreerNouveauSalt();
            byte[] hash = Firewall.Firewall.HashUnMotDePasse(employe.MotDePasse,salt);
            try
            {
                cnx.Open();

                string query = @"
            INSERT INTO utilisateurs (nom, prenom, email, mot_de_passe_hash,mot_de_passe_salt, role, actif, date_embauche)
            VALUES (@Nom, @Prenom, @Email, @Mdp, @Salt, @Role, @Actif, @DateEmbauche);";


                using (MySqlCommand cmd = new MySqlCommand(query, cnx.GetConnection()))
                {
                    cmd.Parameters.AddWithValue("@Nom", employe.Nom);
                    cmd.Parameters.AddWithValue("@Prenom", employe.Prenom);
                    cmd.Parameters.AddWithValue("@Email", employe.Email);
                    cmd.Parameters.AddWithValue("@Mdp", Convert.ToBase64String(hash));
                    cmd.Parameters.AddWithValue("Salt", Convert.ToBase64String(salt));
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

        /// <summary>
        /// Permet d'obtenir l'employé du mois.
        /// </summary>
        /// <returns>L'employé du mois.</returns>
        public static Employe? ObtenirEmployeDuMoisBD()
        {
            GestionBD cnx = new GestionBD();
            try
            {
                cnx.Open();
                string query = @"
            SELECT id_utilisateur, nom, prenom, email, mot_de_passe_hash, role, actif, date_embauche, points
            FROM utilisateurs
            WHERE role = 'Employe' AND actif = 1
            ORDER BY points DESC
            LIMIT 1;";

                MySqlCommand cmd = new MySqlCommand(query, cnx.GetConnection());
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    int id = reader.GetInt32("id_utilisateur");
                    string nom = reader.GetString("nom");
                    string prenom = reader.GetString("prenom");
                    string email = reader.GetString("email");
                    string mdp = reader.GetString("mot_de_passe_hash");
                    bool actif = reader.GetBoolean("actif");
                    DateTime dateEmbauche = reader.GetDateTime("date_embauche");
                    int points = reader.GetInt32("points");

                    Employe emp = new Employe(id, nom, prenom, email, mdp, "Employe", actif, points);
                    emp.DateEmbauche = dateEmbauche;
                    return emp;
                }

                return null;
            }
            finally
            {
                cnx.Close();
            }
        }
        public static void MettreAJourEtatTache(int idTache, string etat)
        {
            GestionBD cnx = new GestionBD();
            try
            {
                cnx.Open();
                string query = @"UPDATE Taches SET etat = @Etat WHERE id_tache = @IdTache;";
                using (var cmd = new MySqlCommand(query, cnx.GetConnection()))
                {
                    cmd.Parameters.AddWithValue("@Etat", etat);
                    cmd.Parameters.AddWithValue("@IdTache", idTache);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de la mise à jour de l'état de la tâche : " + ex.Message);
            }
            finally
            {
                cnx.Close();
            }
        }


    }
}
