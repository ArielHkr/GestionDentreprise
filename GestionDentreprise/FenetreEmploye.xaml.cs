using GestionDentreprise.Entites;
using MySqlConnector;
using System.Text.RegularExpressions;
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
            MettreAJourCompteurs();

            _employe = emp;
            taches = _employe.RecupererTaches();
            ChargerTaches();

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

            if (targetList.Name == "TodoList")
                tache.Etat = "Non commencée";
            else if (targetList.Name == "DoingList")
                tache.Etat = "En cours";
            else if (targetList.Name == "DoneList")
                tache.Etat = "Terminée";

            TodoList.Items.Remove(tache);
            DoingList.Items.Remove(tache);
            DoneList.Items.Remove(tache);
            targetList.Items.Add(tache);

            try
            {
                 GestionBD cnx = new GestionBD();
                cnx.Open();
                var cmd = new MySqlCommand("UPDATE Taches SET etat=@etat WHERE id_tache=@id", cnx.GetConnection());
                cmd.Parameters.AddWithValue("@etat", tache.Etat);
                cmd.Parameters.AddWithValue("@id", tache.IdTache);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur mise à jour BD : " + ex.Message);
            }

            MettreAJourCompteurs();
        }

        private void MettreAJourCompteurs()
        {
            nbAFaire.Text = TodoList.Items.Count.ToString();
            nbEncours.Text = DoingList.Items.Count.ToString();
            nbFini.Text = DoneList.Items.Count.ToString();
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

        private void ListViewItem_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void TodoList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int index = TodoList.SelectedIndex;

            if (index >= 0 && index < taches.Count)
            {
                Tache tache = (Tache)TodoList.SelectedItem;

                MessageBox.Show(
                    $"Titre : {tache.Titre}\n\n" +
                    $"Description : {tache.Description}\n\n" +
                    $"Priorité : {tache.Priorite}\n\n" +
                    $"Date de création : {tache.DateCreation:dd/MM/yyyy}\n" +
                    $"Date limite : {tache.DateLimite:dd/MM/yyyy}\n\n" +
                    $"État : {tache.Etat}",
                    "Détails de la tâche",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
            }
        }

        private void DoingList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int index = DoingList.SelectedIndex;

            if (index >= 0 && index < taches.Count)
            {
                Tache tache = (Tache)DoingList.SelectedItem;

                MessageBox.Show(
                    $"Titre : {tache.Titre}\n\n" +
                    $"Description : {tache.Description}\n\n" +
                    $"Priorité : {tache.Priorite}\n\n" +
                    $"Date de création : {tache.DateCreation:dd/MM/yyyy}\n" +
                    $"Date limite : {tache.DateLimite:dd/MM/yyyy}\n\n" +
                    $"État : {tache.Etat}",
                    "Détails de la tâche",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
            }
        }

        private void DoneList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int index = DoneList.SelectedIndex;

            if (index >= 0 && index < taches.Count)
            {
                Tache tache = (Tache)DoneList.SelectedItem;


                MessageBox.Show(
                    $"Titre : {tache.Titre}\n\n" +
                    $"Description : {tache.Description}\n\n" +
                    $"Priorité : {tache.Priorite}\n\n" +
                    $"Date de création : {tache.DateCreation:dd/MM/yyyy}\n" +
                    $"Date limite : {tache.DateLimite:dd/MM/yyyy}\n\n" +
                    $"État : {tache.Etat}",
                    "Détails de la tâche",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
            }

        }

      
    }
}
