namespace GestionDentreprise.Entites
{
    /// <summary>
    /// Représente un administrateur héritant de la classe Utilisateur.
    /// </summary>
    public class Administrateur : Utilisateur
    {
        /// <summary>
        /// Initialise une nouvelle instance d'Administrateur.
        /// </summary>
        /// <param name="id">Identifiant unique de l'utilisateur.</param>
        /// <param name="nom">Nom de l'administrateur.</param>
        /// <param name="prenom">Prénom de l'administrateur.</param>
        /// <param name="email">Adresse courriel de l'administrateur.</param>
        /// <param name="motDePasse">Mot de passe de l'administrateur.</param>
        /// <param name="role">Rôle de l'utilisateur (par défaut "Administrateur").</param>
        /// <param name="actif">Statut actif ou non.</param>
        /// <param name="dateEmbauche">Date d'embauche, optionnelle.</param>
        public Administrateur(int id, string nom, string prenom, string email, string motDePasse, string role = "Administrateur", bool actif = true, DateTime? dateEmbauche = null)
            : base(id, nom, prenom, email, motDePasse, role, actif)
        {

        }

        /// <summary>
        /// Récupère tous les employés de l'entreprise.
        /// </summary>
        /// <returns>Liste complète des employés.</returns>
        public List<Employe> RecupererTousLesEmployes()
        {
            return GestionDesDonnees.ObtenirLesEmployes();
        }

        /// <summary>
        /// Récupère toutes les tâches disponibles pour l'administrateur.
        /// </summary>
        /// <returns>Liste des tâches.</returns>
        public override List<Tache> RecupererTaches()
        {
            return GestionDesDonnees.ObtenirLesTaches(0);
        }

        /// <summary>
        /// Ajoute une tâche dans le système.
        /// </summary>
        /// <param name="tache">Objet tâche à ajouter.</param>
        public void AjouterUneTache(Tache tache)
        {
            GestionDesDonnees.AjouterUneTache(tache);
        }

        /// <summary>
        /// Attribue une tâche à un employé.
        /// </summary>
        /// <param name="tache">La tâche à attribuer.</param>
        /// <param name="employe">L'employé recevant la tâche.</param>
        public void AttribuerUneTache(Tache tache, Employe employe)
        {
            GestionDesDonnees.AttribuerUneTache(tache, employe);
        }

        /// <summary>
        /// Embauche un nouvel employé.
        /// </summary>
        /// <param name="employe">L'employé à embaucher.</param>
        public void EmbaucherEmploye(Employe employe)
        {
            GestionDesDonnees.AjouterUnEmploye(employe);
        }

        /// <summary>
        /// Rend un employé inactif (le vire).
        /// </summary>
        /// <param name="employe">Identifiant de l'employé.</param>
        public void VirerEmploye(int employe)
        {
            GestionDesDonnees.RendreEmployeInactif(employe);
        }

        /// <summary>
        /// Recherche des utilisateurs ou employés selon une chaîne.
        /// </summary>
        /// <param name="Chaine">Texte à rechercher.</param>
        /// <returns>Liste des utilisateurs trouvés.</returns>
        public List<Utilisateur> Rechercher(string Chaine)
        {
            return GestionDesDonnees.RechercherEmployw(Chaine);
        }
    }
}
