using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDentreprise.Entites
{
    internal class Admin : Utilisateur
    {
        public Admin(string? nom, string? motDePasse) : base(nom, motDePasse)
        {
        }
    }
}
