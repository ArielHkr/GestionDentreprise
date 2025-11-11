using GestionDentreprise.Entites;
using System;
using System.Windows;

namespace GestionDentreprise
{
    public partial class Embauche : Window
    {
        Administrateur Admin;

        public Embauche(Administrateur admin)
        {
            InitializeComponent();
            Admin = admin;
        }

        private void BtnEmbaucher_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNom.Text) ||
                string.IsNullOrWhiteSpace(txtPrenom.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtMotDePasse.Password) ||
                string.IsNullOrWhiteSpace(txtConfirmation.Password))
            {
                lblMessage.Text = "Veuillez remplir tous les champs.";
                return;
            }

            if (txtMotDePasse.Password != txtConfirmation.Password)
            {
                lblMessage.Text = "Les mots de passe ne correspondent pas.";
                return;
            }
            if (txtMotDePasse.Password.Length < 8 || !System.Text.RegularExpressions.Regex.IsMatch(txtMotDePasse.Password, @"\d"))
            {
                lblMessage.Text = "Le mot de passe doit contenir au moins 8 caractères et un chiffre.";
                return;
            }

            Employe emp = new Employe(
                id: 0,
                nom: txtNom.Text,
                prenom: txtPrenom.Text,
                email: txtEmail.Text,
                motDePasse: txtMotDePasse.Password,
                role: "Employe",
                actif: true
            );

            emp.DateEmbauche = DateTime.Now;

            try
            {
                Admin.EmbaucherEmploye(emp);
                MessageBox.Show("L’employé a été embauché avec succès.", "Succès",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Erreur : " + ex.Message;
            }
        }

        private void BtnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
