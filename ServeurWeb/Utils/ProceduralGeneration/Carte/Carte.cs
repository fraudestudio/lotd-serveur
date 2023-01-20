using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Serveur.Utils.ProceduralGeneration.Carte.Salles;


namespace Serveur.Utils.ProceduralGeneration.Carte
{
    /// <summary>
    /// Classe représentant une carte de jeu
    /// </summary>
    public class Carte
    {
              
        //Taille des cartes
        public static int Taille => 6;

        /// <summary>
        /// Tableau des salles (0,0) en haut à gauche
        /// </summary>
        public Salle[,] Salles => salles;
        private Salle[,] salles;

        public Carte()
        {
            this.salles = new Salle[Taille, Taille];
        }

        /// <summary>
        /// Ajoute une salle à la position i,j
        /// </summary>
        /// <param name="ligne">Numéro de la ligne</param>
        /// <param name="colonne">Numéro de la colonne</param>
        /// <param name="salle">Salle à ajouter</param>
        public void AjouterSalle(int ligne, int colonne, TypeSalle typeSalle)
        {
            //Création de la salle
            Salle salle = FabriqueSalle.Creer(typeSalle, ligne, colonne);
            //Positionnement de la salle
            for (int i = ligne; i < ligne + salle.Hauteur; i++)
            {
                for (int j = colonne; j < colonne + salle.Largeur; j++)
                {
                    this.salles[i, j] = salle;
                }
            }
        }

    }
}

