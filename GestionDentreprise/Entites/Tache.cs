namespace GestionDentreprise.Entites
{
    public class Tache
    {
        private int idTache;
        private string titre;
        private string description;
        private string priorite;
        private DateTime dateCreation;
        private DateTime dateLimite;
        private string etat = "Non commencée";
        private int? idUtilisateur;

        public int IdTache { get => idTache; set => idTache = value; }
        public string Titre { get => titre; set => titre = value; }
        public string Description { get => description; set => description = value; }
        public string Priorite { get => priorite; set => priorite = value; }
        public DateTime DateCreation { get => dateCreation; set => dateCreation = value; }
        public DateTime DateLimite { get => dateLimite; set => dateLimite = value; }
        public string Etat { get => etat; set => etat = value; }
        public int? IdUtilisateur { get => idUtilisateur; set => idUtilisateur = value; }


        public Tache(string titre, string description, string priorite,
                      DateTime dateLimite)
        {
            this.titre = titre;
            this.description = description;
            this.priorite = priorite;
            this.dateLimite = dateLimite;
        }
        public override string ToString()
        {
            return $"{Titre} (Priorité: {Priorite})";
        }
    }
}
