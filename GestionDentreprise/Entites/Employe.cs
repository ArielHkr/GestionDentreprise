using GestionDentreprise.Entites;
using MySqlConnector;
using System;
using System.Collections.Generic;

namespace GestionDentreprise.Entites
{
    public class Employe : Utilisateur
    {
        public Employe(int id, string nom, string prenom, string email, string motDePasse,string role, bool actif = true)
            : base(id, nom, prenom, email, motDePasse, "Employe", actif)
        {

        }

        public override List<Tache> RecupererTaches()
        {
            var liste = ChargerDonnees.ObtenirLesTaches(Id);
            return liste;
        }
        

    }
}
