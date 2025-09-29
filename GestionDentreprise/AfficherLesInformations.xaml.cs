using GestionDentreprise.Entites;
using System;
using System.Text.RegularExpressions;
using System.Windows;

namespace GestionDentreprise
{
    public partial class AfficherLesInformations : Window
    {
        private Employe _employe;

        public AfficherLesInformations(Employe emp)
        {
            InitializeComponent();
            _employe = emp;

            TextBoxNom.Text = _employe.Nom;
            TextBoxPrenom.Text = _employe.Prenom;
            TextBoxEmail.Text = _employe.Email;
            TextBoxDateEmbauche.Text = _employe.DateEmbauche.HasValue
                ? _employe.DateEmbauche.Value.ToShortDateString()
                : string.Empty;
        }

        private void BtnModifier_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(TextBoxNom.Text))
                    _employe.Nom = TextBoxNom.Text;

                if (!string.IsNullOrWhiteSpace(TextBoxPrenom.Text))
                    _employe.Prenom = TextBoxPrenom.Text;

                if (!string.IsNullOrWhiteSpace(TextBoxEmail.Text) &&
                    Regex.IsMatch(TextBoxEmail.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    _employe.Email = TextBoxEmail.Text;
                }

                _employe.MettreAJourMonProfil();

                MessageBox.Show("Informations modifiées avec succès !", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
           this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la modification : " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnFermer_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
