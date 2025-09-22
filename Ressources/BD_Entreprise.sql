-- Création de la base de données
CREATE DATABASE GestionTachesEntreprise;
 
-- Utilisation de la base
USE GestionTachesEntreprise;

-- ========================
-- Table Utilisateurs
-- ========================
CREATE TABLE Utilisateurs (
    id_utilisateur INT AUTO_INCREMENT PRIMARY KEY,
    nom VARCHAR(50) NOT NULL,
    prenom VARCHAR(50) NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    mot_de_passe VARCHAR(255) NOT NULL,
    role ENUM('Administrateur','Employe') NOT NULL,
    date_embauche DATE NULL,
    actif BOOLEAN DEFAULT TRUE
);

-- ========================
-- Table Taches
-- ========================
CREATE TABLE Taches (
    id_tache INT AUTO_INCREMENT PRIMARY KEY,
    titre VARCHAR(100) NOT NULL,
    description VARCHAR(255),
    priorite ENUM('Basse','Moyenne','Haute'),
    date_creation DATETIME DEFAULT CURRENT_TIMESTAMP,
    date_limite DATE,
    etat ENUM('Non commencée','En cours','Terminée') DEFAULT 'Non commencée',
    id_utilisateur INT,
    FOREIGN KEY (id_utilisateur) REFERENCES Utilisateurs(id_utilisateur)
);

-- ========================
-- Données de test (optionnel)
-- ========================
INSERT INTO Utilisateurs (nom, prenom, email, mot_de_passe, role, date_embauche)
VALUES 
('Namfaim', 'Ariel Thierry', 'admin@entreprise.com', 'admin123', 'Administrateur', CURDATE()),
('Dupont', 'Jean', 'jean.dupont@entreprise.com', 'password', 'Employe', CURDATE());

INSERT INTO Taches (titre, description, priorite, date_limite, id_utilisateur)
VALUES 
('Développer module authentification', 'Créer la gestion login/logout', 'Haute', '2025-10-01', 2),
('Mettre à jour documentation', 'Compléter le wiki projet', 'Moyenne', '2025-10-05', 2);
