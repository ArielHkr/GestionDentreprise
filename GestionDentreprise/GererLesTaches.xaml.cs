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
            for(int i = 0; i < taches.Count; i++)
            {
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

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }


        private void BtnTerminer_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnAttribuerTache_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnCreerTache_Click(object sender, RoutedEventArgs e)
        {
            AjouterTache ajouterTache = new AjouterTache(admin);
            ajouterTache.ShowDialog();
        }
    }
}
