using GestionDentreprise.Entites;
using System.Text.RegularExpressions;
using System.Windows;

namespace GestionDentreprise
{
    public partial class AfficherLesInformations : Window
    {
        private Utilisateur utilisateur;

        public AfficherLesInformations(Utilisateur user)
        {
            InitializeComponent();
            utilisateur = user;

            TextBoxNom.Text = utilisateur.Nom;
            TextBoxPrenom.Text = utilisateur.Prenom;
            TextBoxEmail.Text = utilisateur.Email;
            TextBoxDateEmbauche.Text = (utilisateur.DateEmbauche).ToString("dd/MM/yyyy");
        }

        private void BtnModifier_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(TextBoxNom.Text))
                    utilisateur.Nom = TextBoxNom.Text;

                if (!string.IsNullOrWhiteSpace(TextBoxPrenom.Text))
                    utilisateur.Prenom = TextBoxPrenom.Text;

                if (!string.IsNullOrWhiteSpace(TextBoxEmail.Text) &&
                   Regex.IsMatch(TextBoxEmail.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    utilisateur.Email = TextBoxEmail.Text;
                }

                utilisateur.MettreAJourMonProfil();

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
