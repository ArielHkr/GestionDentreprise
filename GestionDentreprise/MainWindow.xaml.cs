using GestionDentreprise.Entites;
using System.Windows;

namespace GestionDentreprise
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string email = txtUser.Text.Trim();
            string password = txtPassword.Password.Trim();

            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Veuillez saisir votre email.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtUser.Focus();
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Veuillez saisir votre mot de passe.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPassword.Focus();
                return;
            }

            Utilisateur? user = Utilisateur.SeConnecter(email, password);

            if (user != null)
            {
                MessageBox.Show(
                    $"Connexion réussie !\nBienvenue {user.Nom} {user.Prenom} ({user.Role})",
                    "Information",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );

                if (user is Administrateur)
                {
                    FenetreAdmin adminWindow = new FenetreAdmin((Administrateur)user);
                    adminWindow.Show();
                }
                else if (user is Employe)
                {
                    FenetreEmploye employeWindow = new FenetreEmploye((Employe)user);
                    employeWindow.Show();
                }

                this.Close();
            }
            else
            {
                AfficherErreurConnexion();
            }
        }

        private void AfficherErreurConnexion()
        {
            MessageBox.Show("Email ou mot de passe incorrect", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            txtUser.Text = "";
            txtPassword.Password = "";
            txtUser.Focus();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Utilisateur? user = Utilisateur.SeConnecter("admin@entreprise.com", "admin123");

            FenetreAdmin adminWindow = new FenetreAdmin((Administrateur)user);
            adminWindow.Show();
            this.Close();
        }
    }
}
