using GestionDentreprise.Entites;
using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GestionDentreprise
{
    public partial class FenetreEmploye : Window
    {
        private Employe _employe;
        List<Tache> taches;

        public FenetreEmploye(Employe emp)
        {
            InitializeComponent();
            _employe = emp;
            taches = _employe.RecupererTaches();
            ChargerTaches();
            MettreAJourCompteurs();
        }

        private void ChargerTaches()
        {
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

        private void ListView_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var listView = sender as ListView;
                var tache = listView?.SelectedItem as Tache;
                if (tache != null)
                {
                    DragDrop.DoDragDrop(listView, tache, DragDropEffects.Move);
                }
            }
        }

        private void ListView_Drop(object sender, DragEventArgs e)
        {
            Tache? tache = e.Data.GetData(typeof(Tache)) as Tache;
            var listeCible = sender as ListView;
            if (tache == null || listeCible == null) return;

            TodoList.Items.Remove(tache);
            DoingList.Items.Remove(tache);
            DoneList.Items.Remove(tache);

            if (listeCible.Name == "TodoList")
                tache.Etat = "Non commencée";
            else if (listeCible.Name == "DoingList")
                tache.Etat = "En cours";
            else if (listeCible.Name == "DoneList")
                tache.Etat = "Terminée";

            listeCible.Items.Add(tache);
            MettreAJourCompteurs();
        }

        private void MettreAJourCompteurs()
        {
            nbAFaire.Text = TodoList.Items.Count.ToString();
            nbEncours.Text = DoingList.Items.Count.ToString();
            nbFini.Text = DoneList.Items.Count.ToString();

            _employe.Points = DoneList.Items.Count * 100;
            nbPoints.Text = _employe.Points.ToString();
            GestionDesDonnees.MettreAJourPointsEmploye(_employe.Id, _employe.Points);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AfficherLesInformations fenetreInfo = new AfficherLesInformations(_employe);
            fenetreInfo.ShowDialog();
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            bool? resultat = await ShowConfirmationDialogAsync("Déconnexion", "Voulez-vous vraiment vous déconnecter ?");
            if (resultat == true)
            {
                MainWindow mainWindow = new MainWindow();
                this.Close();
                mainWindow.Show();
            }
        }

        private void TodoList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AfficherDetailsTache(TodoList);
        }

        private void DoingList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AfficherDetailsTache(DoingList);
        }

        private void DoneList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AfficherDetailsTache(DoneList);
        }

        private void AfficherDetailsTache(ListView listView)
        {
            if (listView.SelectedItem is Tache tache)
            {
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
