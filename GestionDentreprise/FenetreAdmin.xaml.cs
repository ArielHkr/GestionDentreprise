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
        public FenetreAdmin(Administrateur admin)
        {
            InitializeComponent();
        }
    }
}
