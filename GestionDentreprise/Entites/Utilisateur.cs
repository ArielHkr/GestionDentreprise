using System.Text.RegularExpressions;

namespace GestionDentreprise.Entites
{
    /// <summary>
    /// Représente un utilisateur général de l'application.
    /// Classe abstraite servant de base pour les employés et administrateurs.
    /// </summary>
    public abstract class Utilisateur
    {
        private int id;
        private string nom = default!;
        private string prenom = default!;
        private string email = default!;
        private string motDePasse = default!;
        private string role = default!;
        private bool actif;

        /// <summary>
        /// Identifiant unique de l'utilisateur.
        /// </summary>
        public int Id
        {
            get => id;
            set
            {
                if (value < 0) throw new ArgumentException("L'id doit être positif.");
                id = value;
            }
        }

        /// <summary>
        /// Nom de l'utilisateur.
        /// </summary>
        public string Nom
        {
            get => nom;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Le nom est requis.");
                nom = value.Trim();
            }
        }

        /// <summary>
        /// Prénom de l'utilisateur.
        /// </summary>
        public string Prenom
        {
            get => prenom;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Le prénom est requis.");
                prenom = value.Trim();
            }
        }

        /// <summary>
        /// Adresse courriel de l'utilisateur.
        /// </summary>
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

        /// <summary>
        /// Mot de passe de l'utilisateur.
        /// </summary>
        public string MotDePasse
        {
            get => motDePasse;
            set => motDePasse = value;
        }

        /// <summary>
        /// Rôle de l'utilisateur (Employé, Administrateur, etc.).
        /// </summary>
        public string Role
        {
            get => role;
            set => role = value;
        }

        /// <summary>
        /// Indique si l'utilisateur est actif.
        /// </summary>
        public bool Actif
        {
            get => actif;
            set => actif = value;
        }

        /// <summary>
        /// Date d'embauche de l'utilisateur.
        /// </summary>
        public DateTime DateEmbauche { get; set; }

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="Utilisateur"/>.
        /// </summary>
        public Utilisateur(int id, string nom, string prenom, string email, string motDePasse, string role = "Employe", bool actif = true)
        {
            Id = id;
            Nom = nom;
            Prenom = prenom;
            Email = email;
            MotDePasse = motDePasse;
            Role = role;
            Actif = actif;
        }

        /// <summary>
        /// Vérifie les informations de connexion d'un utilisateur.
        /// </summary>
        /// <param name="email">Adresse email.</param>
        /// <param name="motDePasse">Mot de passe.</param>
        /// <returns>L'utilisateur correspondant ou <c>null</c>.</returns>
        public static Utilisateur? SeConnecter(string email, string motDePasse)
        {
            Utilisateur? utilisateur = GestionDesDonnees.ObtenirUtilisateur(email, motDePasse);
            return utilisateur;
        }

        /// <summary>
        /// Récupère la liste des tâches associées à l'utilisateur.
        /// </summary>
        public abstract List<Tache> RecupererTaches();

        /// <summary>
        /// Met à jour le profil de l'utilisateur dans la base de données.
        /// </summary>
        public void MettreAJourMonProfil()
        {
            GestionDesDonnees.MettreAJourUtilisateur(Nom, Prenom, Email, Id);
        }

        /// <summary>
        /// Retourne une chaîne contenant les informations essentielles de l'utilisateur.
        /// </summary>
        public string AfficherInfos()
        {
            return $"{Prenom} {Nom} ({Role})";
        }

        /// <summary>
        /// Représente l'utilisateur sous forme textuelle.
        /// </summary>
        public override string ToString()
        {
            return $"{Prenom} {Nom}";
        }
    }
}