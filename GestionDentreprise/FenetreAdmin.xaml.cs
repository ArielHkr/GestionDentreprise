using GestionDentreprise.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GestionDentreprise
{
    /// <summary>
    /// Logique d'interaction pour FenetreAdmin.xaml
    /// </summary>
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
            if (emp != null) {
                txtNom_EMois.Text = emp.ToString();
                txtPoints.Text = emp.Points.ToString();
            }
            MettreAJourCompteurs();
        }
        private void ChargerTaches()
        {
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
                mainWindow.ShowDialog();
                this.Close();
            }
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

        private void DoingList_DoubleClick(object sender, MouseButtonEventArgs e)
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

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            GererLesTaches fenetreTaches = new GererLesTaches(administrateur);
            bool? resultat =fenetreTaches.ShowDialog();
            if (resultat != null && resultat == true)
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
    }
}
