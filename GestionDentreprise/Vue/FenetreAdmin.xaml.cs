using GestionDentreprise.Entites;
using GestionDentreprise.Vue;
using MaterialDesignThemes.Wpf;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Threading.Tasks;

namespace GestionDentreprise
{
    public partial class FenetreAdmin : Window
    {
        Administrateur administrateur;
        List<Tache> taches = new List<Tache>();

        public FenetreAdmin(Administrateur admin)
        {
            InitializeComponent();
            administrateur = admin;
            ChargerTaches();
            Employe? emp = GestionDesDonnees.ObtenirEmployeDuMoisBD();
            if (emp != null && emp.Points >0)
            {
                txtNom_EMois.Text = emp.ToString();
                txtPoints.Text = emp.Points.ToString() + " pts";
            }
            MettreAJourCompteurs();
        }

        private void ChargerTaches()
        {
            TodoList.Items.Clear();
            DoingList.Items.Clear();
            DoneList.Items.Clear();

            taches = administrateur.RecupererTaches();

            foreach (Tache tache in taches)
            {
                switch (tache.Etat)
                {
                    case "Non commencée":
                        TodoList.Items.Add(tache);
                        break;

                    case "En cours":
                        DoingList.Items.Add(tache);
                        break;

                    case "Terminée":
                        DoneList.Items.Add(tache);
                        break;
                }
            }
        }


        private void MettreAJourCompteurs()
        {
            nbAFaire.Text = TodoList.Items.Count.ToString();
            nbEncours.Text = DoingList.Items.Count.ToString();
            nbFini.Text = DoneList.Items.Count.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AfficherLesInformations fenetreInfo = new AfficherLesInformations(administrateur);
            fenetreInfo.ShowDialog();
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            bool? resultat = await ShowConfirmationDialogAsync("Déconnexion", "Voulez-vous vraiment vous déconnecter ?");
            if (resultat == true)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.ShowDialog();
                this.Close();
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            GererLesTaches fenetreTaches = new GererLesTaches(administrateur);
            bool? resultat = fenetreTaches.ShowDialog();
            if (resultat == true)
            {
                Button_Click_2(sender, e);
            }
            else
            {
                ChargerTaches();
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            GestionRH gestionRH = new GestionRH(administrateur);
            gestionRH.ShowDialog();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Tutoriel tutoriel = new Tutoriel();
            tutoriel.ShowDialog();
        }

        private void TodoList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (TodoList.SelectedIndex >= 0)
            {
                Tache tache = (Tache)TodoList.SelectedItem;
                AfficherTache afficher_Tache = new AfficherTache(tache);
                afficher_Tache.ShowDialog();
            }
        }

        private void DoingList_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DoingList.SelectedIndex >= 0)
            {
                Tache tache = (Tache)DoingList.SelectedItem;
                AfficherTache afficher_Tache = new AfficherTache(tache);
                afficher_Tache.ShowDialog();
            }
        }

        private void DoneList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DoneList.SelectedIndex >= 0)
            {
                Tache tache = (Tache)DoneList.SelectedItem;
                AfficherTache afficher_Tache = new AfficherTache(tache);
                afficher_Tache.ShowDialog();
            }
        }

        private async Task<bool?> ShowConfirmationDialogAsync(string title, string message)
        {
            var stack = new StackPanel { Margin = new Thickness(20) };

            stack.Children.Add(new TextBlock
            {
                Text = title,
                FontWeight = FontWeights.Bold,
                FontSize = 22,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 0, 0, 10)
            });

            stack.Children.Add(new TextBlock
            {
                Text = message,
                FontSize = 16,
                TextAlignment = TextAlignment.Center,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 0, 0, 25)
            });

            var panel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var btnYes = new Button { Content = "Oui", Width = 90, Margin = new Thickness(0, 0, 15, 0) };
            var btnNo = new Button { Content = "Non", Width = 90 };

            panel.Children.Add(btnYes);
            panel.Children.Add(btnNo);
            stack.Children.Add(panel);

            var tcs = new TaskCompletionSource<bool?>();

            btnYes.Click += (s, e) =>
            {
                tcs.SetResult(true);
                DialogHost.CloseDialogCommand.Execute(null, null);
            };

            btnNo.Click += (s, e) =>
            {
                tcs.SetResult(false);
                DialogHost.CloseDialogCommand.Execute(null, null);
            };

            await DialogHost.Show(stack, "AppDialog");

            return await tcs.Task;
        }
    }
}
