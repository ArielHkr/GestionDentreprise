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
            return GestionDesDonnees.ObtenirLesEmployes();
        }
        public override List<Tache> RecupererTaches()
        {
            return GestionDesDonnees.ObtenirLesTaches(0);
        }
        public void AjouterUneTache(Tache tache)
        {
            GestionDesDonnees.AjouterUneTache(tache);
        }
        public void AttribuerUneTache(Tache tache, Employe employe)
        {
            GestionDesDonnees.AttribuerUneTache(tache, employe);
        }
        

    }
}
