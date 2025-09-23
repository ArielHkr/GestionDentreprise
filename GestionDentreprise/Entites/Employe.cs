using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDentreprise.Entites
{
    internal class Employe : Utilisateur
    {
        public Employe(string? nom, string? motDePasse) : base(nom, motDePasse)
        {
        }
    }
    
    }

