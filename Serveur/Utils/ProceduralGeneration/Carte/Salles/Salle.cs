using Server.Model;
using Server.Utils.ProceduralGeneration.GenerationAlgorithm.Realisation.Graphes;
using Serveur.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serveur.Utils.ProceduralGeneration.Carte.Salles
{
    public abstract class Salle
    {
        //Numéro de ligne où se trouve la salle
        public int Ligne { get => ligne; set => ligne = value; }
        private int ligne;

        //Numéro de colonne où se trouve la salle
        public int Colonne { get => colonne; set => colonne = value; }
        private int colonne;


        public int SeedLocal { get => seedLocal; set => seedLocal = value; }
        private int seedLocal;

        protected Salle(int ligne, int colonne)
        {
            this.ligne = ligne;
            this.colonne = colonne;
            seedLocal = int.Parse(ligne.ToString() + colonne.ToString());
        }

        public abstract TypeSalle Type { get; }


        public override bool Equals(object? obj)
        {
            return obj is Salle salle &&
                   ligne == salle.ligne &&
                   colonne == salle.colonne;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ligne, colonne);
        }

       
        
    }
}