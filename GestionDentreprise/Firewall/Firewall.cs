using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Konscious.Security.Cryptography;

namespace GestionDentreprise.Firewall
{
    internal class Firewall
    {
        //Générateur de nombre aléatoire pour le salt
        private static RandomNumberGenerator rng = RandomNumberGenerator.Create();

        /// <summary>
        /// Méthode qui permet de vérifier la correspondance entre un mot de passe,
        /// un salt et un hash.
        /// </summary>
        /// <param name="mdp">Le mot de passe</param>
        /// <param name="salt">Le salt</param>
        /// <param name="hash">Le hash</param>
        /// <returns>Vrai si le mot de passe correspond au hash.</returns>
        public static bool VerifierMotDePasse(string mdp, byte[] salt, byte[] hash)
        {
            return hash.SequenceEqual(HashUnMotDePasse(mdp, salt));
        }

        /// <summary>
        /// Méthode qui permet de hasher un mot de passe. Il est obligatoire
        /// d'utiliser un salt.
        /// </summary>
        /// <param name="mdp">Le mot de passe</param>
        /// <param name="salt">Le salt</param>
        /// <returns>Le hash</returns>
        public static byte[] HashUnMotDePasse(string mdp, byte[] salt)
        {
            Argon2id argon2 = new Argon2id(Encoding.UTF8.GetBytes(mdp))
            {
                Salt = salt,
                DegreeOfParallelism = 8,
                Iterations = 4,
                MemorySize = 1024 * 1024
            };
            return argon2.GetBytes(16);
        }

        /// <summary>
        /// Méthode qui permet de créer un salt pour un nouvel utilisateur.
        /// </summary>
        /// <returns>Un salt aléatoire pour un utilisateur</returns>
        public static byte[] CreerNouveauSalt()
        {
            byte[] buffer = new byte[16];
            rng.GetBytes(buffer);
            return buffer;
        }
    }
}
