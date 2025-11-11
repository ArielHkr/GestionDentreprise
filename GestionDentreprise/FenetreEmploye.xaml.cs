using GestionDentreprise.Entites;
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
            var tache = e.Data.GetData(typeof(Tache)) as Tache;
            var targetList = sender as ListView;
            if (tache == null || targetList == null) return;

            TodoList.Items.Remove(tache);
            DoingList.Items.Remove(tache);
            DoneList.Items.Remove(tache);

            if (targetList.Name == "TodoList")
                tache.Etat = "Non commencée";
            else if (targetList.Name == "DoingList")
                tache.Etat = "En cours";
            else if (targetList.Name == "DoneList")
                tache.Etat = "Terminée";

            targetList.Items.Add(tache);
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBoxResult resultat = MessageBox.Show(
                "Voulez-vous vraiment vous déconnecter ?",
                "Confirmation de déconnexion",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (resultat == MessageBoxResult.Yes)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
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
    }
}
