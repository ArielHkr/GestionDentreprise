using GestionDentreprise.Entites;
using MaterialDesignThemes.Wpf;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace GestionDentreprise
{
    public partial class MainWindow : Window
    {
        private readonly SnackbarMessageQueue _messageQueue;
        private readonly List<string> _recentMessages = new();

        public MainWindow()
        {
            InitializeComponent();
            _messageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(3));
            SnackbarOne.MessageQueue = _messageQueue;
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string email = txtUser.Text.Trim();
            string password = txtPassword.Password.Trim();

            if (string.IsNullOrEmpty(email))
            {
                ShowSnackbar("Veuillez saisir votre email.");
                txtUser.Focus();
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                ShowSnackbar("Veuillez saisir votre mot de passe.");
                txtPassword.Focus();
                return;
            }

            var user = Utilisateur.SeConnecter(email, password);

            if (user != null)
            {
                await ShowDialogAsync("Connexion réussie", $"Bienvenue {user.Nom} {user.Prenom} ({user.Role})");

                if (user is Administrateur)
                {
                    new FenetreAdmin((Administrateur)user).Show();
                }
                else if (user is Employe)
                {
                    new FenetreEmploye((Employe)user).Show();
                }

                Close();
            }
            else
            {
                ShowSnackbar("Email ou mot de passe incorrect");
                txtUser.Text = "";
                txtPassword.Password = "";
                txtUser.Focus();
            }
        }

        private void ShowSnackbar(string message)
        {
            if (!_recentMessages.Contains(message))
            {
                _recentMessages.Add(message);
                _messageQueue.Enqueue(message);

                Task.Delay(1000).ContinueWith(_ => _recentMessages.Remove(message), TaskScheduler.FromCurrentSynchronizationContext());
            }
        }
        private async Task<bool?> ShowDialogAsync(string title, string message)
        {
            var content = new StackPanel { Margin = new Thickness(20) };

            content.Children.Add(new TextBlock
            {
                Text = title,
                FontSize = 22,
                FontWeight = FontWeights.Bold,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 0, 0, 10)
            });

            content.Children.Add(new TextBlock
            {
                Text = message,
                FontSize = 16,
                TextAlignment = TextAlignment.Center,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 0, 0, 20)
            });

            var okButton = new Button
            {
                Content = "OK",
                Width = 90,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            okButton.Click += (s, e) => DialogHost.CloseDialogCommand.Execute(true, AppDialog);
            content.Children.Add(okButton);

            return await DialogHost.Show(content, "AppDialog") as bool?;
        }

    }
}
