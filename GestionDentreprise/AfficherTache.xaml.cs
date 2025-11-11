using GestionDentreprise.Entites;
using System;
using System.Windows;

namespace GestionDentreprise
{
    public partial class AfficherTache : Window
    {
        public AfficherTache(Tache tache)
        {
            InitializeComponent();

            txtTitre.Text = tache.Titre;
            txtDescription.Text = tache.Description;
            txtPriorite.Text = tache.Priorite;
            txtDateCreation.Text = tache.DateCreation.ToString("dd/MM/yyyy");
            txtDateLimite.Text = tache.DateLimite.ToString("dd/MM/yyyy");
            txtEtat.Text = tache.Etat;
        }

        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
