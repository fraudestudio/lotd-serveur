using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Serveur.Utils.ProceduralGeneration.Carte.Salles;
using Server.Model;
using Serveur.Model;

namespace Serveur.Utils.ProceduralGeneration.Carte
{
    public class Carte
    {
        //Taille des cartes
        private static int taille;
        public static int Taille { get => taille; set => taille = value; }

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
        /// Property tha contains the list of playable characters
        /// </summary>
        public List<Perso> CharactersJ1 { get => charactersJ1; set => charactersJ1 = value; }
        private List<Perso> charactersJ1;


        /// <summary>
        /// Property tha contains the list of playable characters
        /// </summary>
        public List<Perso> CharactersJ2 { get => charactersJ2; set => charactersJ2 = value; }
        private List<Perso> charactersJ2;
        
        /// <summary>
        /// Property that contains the list of ennemies on the map
        /// </summary>
        public List<Enemies> Enemies { get => enemies; set => enemies = value; }
        private List<Enemies> enemies;

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
            for (int i = ligne; i < ligne + 1; i++)
            {
                for (int j = colonne; j < colonne + 1; j++)
                {
                    this.salles[i, j] = salle;
                }
            }
        }
    }
}

