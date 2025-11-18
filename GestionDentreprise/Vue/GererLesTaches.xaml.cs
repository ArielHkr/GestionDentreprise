using GestionDentreprise.Entites;
using System.Windows;
using System.Windows.Input;

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
            taches = admin.RecupererTaches();

            for (int i = 0; i < taches.Count; i++)
            {
                if (taches[i].IdUtilisateur is null)
                    lstTaches.Items.Add(taches[i]);
            }
            employes = admin.RecupererTousLesEmployes();
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
                MessageBox.Show("Tâche attribuée avec succès !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de l'attribution : " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            tache.IdUtilisateur = employe.Id;
            lstTaches.Items.Clear();
            for (int i = 0; i < taches.Count; i++)
            {
                if (taches[i].IdUtilisateur is null)
                    lstTaches.Items.Add(taches[i]);
            }

        }


        private void BtnCreerTache_Click(object sender, RoutedEventArgs e)
        {
            AjouterTache ajouterTache = new AjouterTache(admin);
            bool? resultat = ajouterTache.ShowDialog();
            if (resultat != null && resultat == true)
            {
                DialogResult = true;

                this.Close();
            }
        }

        private void AffichageEmploye_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Employe employe = (Employe)lstEmployes.SelectedItem;
            AfficherLesInformations afficherLesInformations = new AfficherLesInformations(employe);
            afficherLesInformations.ShowDialog();
  


        }

        private void AfficherDetailsTache_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int index = lstTaches.SelectedIndex;

            if (index >= 0 && index < taches.Count)
            {
                Tache tache = (Tache)lstTaches.SelectedItem;

                AfficherTache afficherTache = new AfficherTache(tache);
                afficherTache.ShowDialog();
            }
        }
    }
}
