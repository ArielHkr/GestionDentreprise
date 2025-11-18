using GestionDentreprise.Entites;
using System.Windows;

namespace GestionDentreprise
{
    /// <summary>
    /// Logique d'interaction pour AjouterTache.xaml
    /// </summary>
    public partial class AjouterTache : Window
    {
        Administrateur administrateur;
        public AjouterTache(Administrateur admin)
        {
            InitializeComponent();
            administrateur = admin;
        }

        private void BtnAjout_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitre.Text))
            {
                MessageBox.Show("Le titre de la tâche est requis.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtTitre.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(cmbPriorite.Text))
            {
                MessageBox.Show("Veuillez sélectionner une priorité.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                cmbPriorite.Focus();
                return;
            }

            if (!dpDateLimite.SelectedDate.HasValue)
            {

                MessageBox.Show("Veuillez choisir une date limite.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                dpDateLimite.Focus();
                return;
            }
            if (dpDateLimite.SelectedDate.Value.Date <= DateTime.Today)
            {
                MessageBox.Show("La date limite doit être supérieure à aujourd'hui.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                dpDateLimite.Focus();
                return;
            }

            Tache tache = new Tache(txtTitre.Text, txtDescription.Text, cmbPriorite.Text, dpDateLimite.SelectedDate.Value);


            administrateur.AjouterUneTache(tache);
            MessageBox.Show("Tâche ajoutée avec succès !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);

            DialogResult = true;
            this.Close();
        }


        private void BtnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
