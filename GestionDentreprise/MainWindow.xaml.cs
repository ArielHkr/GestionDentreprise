using GestionDentreprise.Entites;
using System.Windows;
using System.Windows.Controls;

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
            string username = txtUser.Text.Trim();
            string password = txtPassword.Password.Trim();

            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Veuillez saisir votre nom d'utilisateur.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtUser.Focus();
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Veuillez saisir votre mot de passe.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPassword.Focus();
                return;
            }

            if (cbAdmin.IsChecked == true)
            {
                Admin user = new Admin(username, password);
                if (user.SeConnecter("Administrateur"))
                {
                    MessageBox.Show(
                         $"Connexion réussie en tant qu'administrateur !\nBienvenue {user.Nom}",
                        "Information",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );
                    FenetreAdmin adminWindow = new FenetreAdmin();
                    adminWindow.Show();
                    this.Close();
                }
                else
                {
                    AfficherErreurConnexion();
                }
            }
            else
            {
                Employe user = new Employe(username, password);
                if (user.SeConnecter("Employe"))
                {
                    MessageBox.Show($"Connexion réussie en tant qu'employé !\nBienvenue {user.Nom}", "Information",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    FenetreEmploye employeWindow = new FenetreEmploye();
                    employeWindow.Show();
                    this.Close();
                }
                else
                {
                    AfficherErreurConnexion();
                }
            }
        }

        private void AfficherErreurConnexion()
        {
            MessageBox.Show("Nom d'utilisateur ou mot de passe incorrect", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            txtUser.Text = "";
            txtPassword.Password = "";
            txtUser.Focus();
        }
    }
}
