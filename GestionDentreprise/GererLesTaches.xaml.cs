using GestionDentreprise.Entites;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Logique d'interaction pour GererLesTaches.xaml
    /// </summary>
    public partial class GererLesTaches : Window
    {
        List<Tache> taches = new List<Tache>();
        List<Employe> employes = new List<Employe>();
        Administrateur admin;
        public GererLesTaches(Administrateur admin)
        {
            this.admin = admin;
            InitializeComponent();
            taches=admin.RecupererTaches();
            lstTaches.Items.Clear();

            for (int i = 0; i < taches.Count; i++)
            {
                if(taches[i].IdUtilisateur is null)
                   lstTaches.Items.Add(taches[i]);
            }
            employes= admin.RecupererTousLesEmployes();
            for (int i = 0; i < employes.Count; i++)
            {
                lstEmployes.Items.Add(employes[i]);
            }

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
        private void BtnTerminer_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnAttribuerTache_Click(object sender, RoutedEventArgs e)
        {
            Tache tache = (Tache)lstTaches.SelectedItem;
            if (tache == null)
            {
                MessageBox.Show("Veuillez sélectionner une tâche à attribuer.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                lstTaches.Focus();
                return;
            }

            Employe employe = (Employe)lstEmployes.SelectedItem;
            if (employe == null)
            {
                MessageBox.Show("Veuillez sélectionner un employé.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                lstEmployes.Focus();
                return;
            }

            if (tache.IdUtilisateur.HasValue)
            {
                MessageBox.Show("Cette tâche est déjà attribuée.", "Attention", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                admin.AttribuerUneTache(tache, employe);
                this.Close();
                MessageBox.Show("Tâche attribuée avec succès !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
               
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de l'attribution : " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            

            this.Close();
            
        }


        private void BtnCreerTache_Click(object sender, RoutedEventArgs e)
        {
            AjouterTache ajouterTache = new AjouterTache(admin);
            ajouterTache.ShowDialog();
        }

        private void AffichageEmploye_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Employe employe = (Employe)lstEmployes.SelectedItem;
            MessageBox.Show(
      $"Employé : {employe.Nom} {employe.Prenom}\n" +
      $"Email : {employe.Email}\n" +
      $"Date d'embauche : {employe.DateEmbauche:d}\n\n",
      "Détails de l'employé",
      MessageBoxButton.OK,
      MessageBoxImage.Information
  );


        }

        private void AfficherDetailsTache_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int index = lstTaches.SelectedIndex;

            if (index >= 0 && index < taches.Count)
            {
                Tache tache = (Tache)lstTaches.SelectedItem;

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
