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
                if (string.IsNullOrWhiteSpace(TextBoxNom.Text))
                {
                    MessageBox.Show("Le nom ne peut pas être vide.", "Champ manquant",
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(TextBoxPrenom.Text))
                {
                    MessageBox.Show("Le prénom ne peut pas être vide.", "Champ manquant",
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(TextBoxEmail.Text))
                {
                    MessageBox.Show("L'adresse e-mail ne peut pas être vide.", "Champ manquant",
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!Regex.IsMatch(TextBoxEmail.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    MessageBox.Show("Veuillez entrer une adresse e-mail valide.", "Adresse e-mail invalide",
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                utilisateur.Nom = TextBoxNom.Text.Trim();
                utilisateur.Prenom = TextBoxPrenom.Text.Trim();
                utilisateur.Email = TextBoxEmail.Text.Trim();

                utilisateur.MettreAJourMonProfil();

                MessageBox.Show("Votre profil a été mis à jour avec succès !", "Succès",
                              MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une erreur est survenue lors de la mise à jour de votre profil :\n" + ex.Message,
                                "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void BtnFermer_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
