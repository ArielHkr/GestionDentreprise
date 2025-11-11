using GestionDentreprise.Entites;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace GestionDentreprise
{
    public partial class GestionRH : Window
    {
        Administrateur Admin;
        List<Employe> utilisateurs;
        Employe empAffiche;
        public GestionRH(Administrateur admin)
        {
            InitializeComponent();
            Admin = admin;
            utilisateurs = Admin.RecupererTousLesEmployes(); 
            foreach (var emp in utilisateurs)
                cbEmployes.Items.Add(emp);
        }

        private void RechercherEmploye_Click(object sender, RoutedEventArgs e)
        {
            lstEmployeRecherche.Items.Clear();
            if (!string.IsNullOrWhiteSpace(txtRecherche.Text))
                foreach (Utilisateur u in Admin.Rechercher(txtRecherche.Text))
                    if (u is Employe)
                        lstEmployeRecherche.Items.Add(u);
        }

        private void lstEmployeRecherche_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstEmployeRecherche.SelectedItem == null) return;
            AfficherEmploye((Employe)lstEmployeRecherche.SelectedItem);
        }

        private void cbEmployes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbEmployes.SelectedItem == null) return;
            AfficherEmploye((Employe)cbEmployes.SelectedItem);
        }

        private void AfficherEmploye(Employe emp)
        {
            empAffiche = emp;
            lstTaches.Items.Clear();
            txtNom.Text = emp.Nom;
            txtPrenom.Text = emp.Prenom;
            txtEmail.Text = emp.Email;
            dpEmbauche.Text = emp.DateEmbauche.ToString("yyyy-MM-dd");
            foreach (var t in emp.RecupererTaches())
                lstTaches.Items.Add(t);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (empAffiche == null)
            {
                MessageBox.Show("Aucun employé sélectionné.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBoxResult result = MessageBox.Show(
                $"Voulez-vous vraiment rendre inactif l’employé {empAffiche.Prenom} {empAffiche.Nom} ?",
                "Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result == MessageBoxResult.Yes)
            {
                Admin.VirerEmploye(empAffiche.Id);
                MessageBox.Show("L’employé a été rendu inactif.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Embauche embauche = new Embauche(Admin);
            bool? resultat = embauche.ShowDialog();
            if (resultat == true)
            {
                this.Close();
            }
        }
    }
}
