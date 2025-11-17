namespace GestionDentreprise.Entites
{
    /// <summary>
    /// Représente un employé de l'entreprise, héritant de la classe Utilisateur.
    /// </summary>
    public class Employe : Utilisateur
    {
        private int _points;

        /// <summary>
        /// Initialise une nouvelle instance d'Employe.
        /// </summary>
        /// <param name="id">Identifiant unique de l'employé.</param>
        /// <param name="nom">Nom de l'employé.</param>
        /// <param name="prenom">Prénom de l'employé.</param>
        /// <param name="email">Adresse courriel de l'employé.</param>
        /// <param name="motDePasse">Mot de passe de l'employé.</param>
        /// <param name="role">Rôle attribué (ignoré, forcé à "Employe").</param>
        /// <param name="actif">Statut actif ou non.</param>
        /// <param name="points">Points accumulés par l'employé.</param>
        public Employe(int id, string nom, string prenom, string email, string motDePasse, string role, bool actif = true, int points = 0)
            : base(id, nom, prenom, email, motDePasse, "Employe", actif)
        {
            _points = points;
        }

        /// <summary>
        /// Obtient ou définit le nombre de points accumulés par l'employé.
        /// </summary>
        public int Points { get => _points; set => _points = value; }

        /// <summary>
        /// Récupère les tâches assignées à cet employé.
        /// </summary>
        /// <returns>Liste des tâches associées à l'employé.</returns>
        public override List<Tache> RecupererTaches()
        {
            return GestionDesDonnees.ObtenirLesTaches(Id);
        }
    }
}