using MySqlConnector;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace GestionDentreprise.Entites
{
    public abstract class Utilisateur
    {
        private int id; 
        private string nom=default!;
        private string prenom = default!;
        private string email = default!;
        private string motDePasse = default!;
        private string role = default!;
        private bool actif;


        public int Id
        {
            get => id;
            set
            {
                if (value < 0) throw new ArgumentException("L'id doit être positif.");
                id = value;
            }
        }

        public string Nom
        {
            get => nom;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Le nom est requis.");
                nom = value.Trim();
            }
        }

        public string Prenom
        {
            get => prenom;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Le prénom est requis.");
                prenom = value.Trim();
            }
        }

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

        public string MotDePasse
        {
            get => motDePasse;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length < 6 || value.Length > 100 || !Regex.IsMatch(value, @"^(?=.*[A-Za-z])(?=.*\d).+$"))
                    throw new ArgumentException("Mot de passe invalide.");
                motDePasse = value;
            }
        }

        public string Role
        {
            get => role;
            set => role = value;
        }

        public bool Actif
        {
            get => actif;
            set => actif = value;
        }

        public DateTime DateEmbauche { get; set; }

        protected Utilisateur(int id, string nom, string prenom, string email, string motDePasse, string role= "Employe", bool actif = true)
        {
            Id = id;
            Nom = nom;
            Prenom = prenom;
            Email = email;
            MotDePasse = motDePasse;
            Role = role;
            Actif = actif;
        }

        public static Utilisateur? SeConnecter(string email, string motDePasse)
        {
           Utilisateur? utilisateur = ChargerDonnees.ObtenirUtilisateur(email, motDePasse);
            return utilisateur;
        }

        public abstract List<Tache> RecupererTaches();

        public void MettreAJourMonProfil()
        {
          ChargerDonnees.MettreAJourUtilisateur(Nom, Prenom, Email, Id);
        }
        public string AfficherInfos()
        {
            return $"{Prenom} {Nom} ({Role})";
        }
    }
}
