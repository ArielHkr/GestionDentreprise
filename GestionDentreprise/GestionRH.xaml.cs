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
    /// Logique d'interaction pour GestionRH.xaml
    /// </summary>
    public partial class GestionRH : Window
    {
        Administrateur Admin;
        List<Employe> utilisateurs = new List<Employe>();
        public GestionRH(Administrateur admin)
        {
            InitializeComponent();
            Admin = admin;
            utilisateurs = Admin.RecupererTousLesEmployes();
            for (int i = 0; i <utilisateurs.Count; i++) { 
             cbEmployes.Items.Add(utilisateurs[i]);
            }
        }

        private void RechercherEmploye_Click(object sender, RoutedEventArgs e)
        {
            if (txtRecherche.Text.Trim() != "")
            {
             lstEmployeRecherche.ItemsSource=Admin.Rechercher(txtRecherche.Text);
            }
        }

        private void lstEmployeRecherche_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lstEmployeRecherche.Items.Count > 0)
            {

            }
        }

        private void cbEmployes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cbEmployes.SelectedItem  != null)
            {
                Employe emp = utilisateurs[cbEmployes.SelectedIndex];
                txtNom.Text = emp.Nom;
                txtPrenom.Text = emp.Prenom;
                txtEmail.Text = emp.Email;
                dpEmbauche.Text = emp.DateEmbauche.ToString("yyyy-MM-dd");
                foreach(Tache t in emp.RecupererTaches())
                {
                    lstTaches.Items.Add(t);
                }
            }
        }
    }
}
