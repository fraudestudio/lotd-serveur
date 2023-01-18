using Server.Utils.ProceduralGeneration.GenerationAlgorithm;
using Server.Utils.ProceduralGeneration.GenerationAlgorithm.Realisation.Graphes;
using Serveur.Utils.ProceduralGeneration.Carte;
using Serveur.Utils.ProceduralGeneration.Carte.Salles;
using Serveur.Utils.ProceduralGeneration.Carte.Salles.Realisation;
using System.Numerics;

namespace Serveur.Utils.ProceduralGeneration.GenerationAlgorithm.Realisation
{
    public class RoomGenerationAlgorithm : IAlgorithmeGeneration
    {
        // Size of the room 
        private static int roomSize = 23;
        public static int RoomSize { get => roomSize; set => roomSize = value; }

        // List of the validate figure present on the graphe 
        private List<Coordonnees> valideFigure = new List<Coordonnees>();


        public Carte.Carte Generer(int seed)
        {
            // we modify the map length for the room
            Carte.Carte.Taille = roomSize;
            Graphe g = new Graphe();
            // we insert the local seed
            RandomAlgorithm.Instance.SetSeedLocale(seed);


            // We set all the tiles to normal
            for (int ligne = 0; ligne < Carte.Carte.Taille; ligne++)
            {
                for (int colonne = 0; colonne < Carte.Carte.Taille; colonne++)
                {
                    g.GetSommet(ligne, colonne).TypeSalle = TypeSalle.TILENORMALE;
                }

            }
            

            // We create some squares
            for (int i = 0; i < RandomAlgorithm.Instance.Next(12) + 6; i++)
            {

                // We take a random coordinates and length
                Coordonnees c = RandomAlgorithm.Instance.NextCoordonnees();
                int taille = RandomAlgorithm.Instance.Next(4) + 2;

                // We make sure that we can create the square
                // If we can't, we redo it until we can
                while (!CanCreateSquare(g, c.Colonne, c.Ligne, taille))
                {
                    c = RandomAlgorithm.Instance.NextCoordonnees();
                    taille = RandomAlgorithm.Instance.Next(4) + 2;
                    
                }
                g = CreateSquare(g, c.Colonne, c.Ligne, taille);
                
            }
            TorchGeneration(g.ToCarte());
            return g.ToCarte();
        }


        /// <summary>
        /// Verify if we can create the square with parameters that we want on the graph
        /// </summary>
        /// <param name="g">graphe that we want</param>
        /// <param name="colonne">column that is set</param>
        /// <param name="ligne">line that is set</param>
        /// <param name="taille">length of the square</param>
        /// <returns>Boolean of if the square can be created</returns>
        private bool CanCreateSquare(Graphe g, int colonne, int ligne, int taille)
        {

            bool result = true;


            // Line and column with the length need to be lower than the maximum map range
            if (ligne + taille > Carte.Carte.Taille)
            {
                result = false;
            }

            if (colonne + taille > Carte.Carte.Taille)
            {
                result = false;
            }

            // Line and column has to be superior or equal to 0
            if (ligne < 0)
            {
                result = false;
            }
            if (colonne < 0)
            {
                result = false;
            }

            // If it pass the first tested
            if (result)
            {

                // Foreach tiles of the square, we make sure that it is at least 2 blocks away of already 
                // created square
                for (int i = 0; i < taille; i++)
                {
                    for (int j = 0; j < taille; j++)
                    {
                        Coordonnees coordinate = new Coordonnees(ligne + i, colonne + j);
                        foreach (Coordonnees valideCoordinate in valideFigure)
                        {
                            if (coordinate.Distance(valideCoordinate) < 3)
                            {
                                result = false;
                            }
                        }
                    }
                }
            }
            // We send back the result
            return result;
        }

        /// <summary>
        /// Create a square with parameters that we want on the graph
        /// </summary>
        /// <param name="g"></param>
        /// <param name="colonne"></param>
        /// <param name="ligne"></param>
        /// <param name="taille"></param>
        /// <returns></returns>
        private Graphe CreateSquare(Graphe g, int colonne, int ligne, int taille)
        {
            Graphe graphe = g;

            // We create the square
            for (int i = 0; i < taille; i++)
            {
                for (int j = 0; j < taille; j++)
                {
                    graphe.GetSommet(ligne + i, colonne + j).TypeSalle = TypeSalle.TILEFULL;
                    // We add it to the valide figure of the graph
                    valideFigure.Add(new Coordonnees(ligne + i, colonne + j));
                }
            }
            // we return the new graph
            return graphe;
        }

        /// <summary>
        /// Generate Torchs for RandomAlgorithm synchronisation with clients 
        /// </summary>
        /// <param name="map"></param>
        public void TorchGeneration(Carte.Carte map)
        {
            List<Salle> salles = new List<Salle>();

            foreach (Salle salle in map.Salles)
            {
                if (salle.Type == TypeSalle.TILEFULL)
                {
                    salles.Add(salle);
                }
            }
            
            foreach(Salle salle in salles)
            {
                string Larger = "BIG";
                string PositionColonne = "MIDDLE";
                string PositionLigne = "MIDDLE";
                int currColonne = salle.Colonne;
                int currLigne = salle.Ligne;
               

                if (currLigne + 1 <= Carte.Carte.Taille - 1 && currLigne - 1 >= 0)
                {
                    if (map.Salles[currLigne + 1, currColonne].Type == TypeSalle.TILEFULL && map.Salles[currLigne - 1, currColonne].Type != TypeSalle.TILEFULL)
                    {
                        PositionColonne = "DOWN";
                    }
                    else if (map.Salles[currLigne - 1, currColonne].Type == TypeSalle.TILEFULL && map.Salles[currLigne + 1, currColonne].Type != TypeSalle.TILEFULL)
                    {
                        PositionColonne = "UP";
                    }
                    else
                    {
                        PositionColonne = "MIDDLE";
                    }
                }



                if (currColonne + 1 <= Carte.Carte.Taille - 1 && currColonne - 1 >= 0)
                {
                    if (map.Salles[currLigne, currColonne + 1].Type == TypeSalle.TILEFULL && map.Salles[currLigne, currColonne - 1].Type != TypeSalle.TILEFULL)
                    {
                        PositionLigne = "LEFT";
                    }
                    else if (map.Salles[currLigne, currColonne - 1].Type == TypeSalle.TILEFULL && map.Salles[currLigne, currColonne + 1].Type != TypeSalle.TILEFULL)
                    {
                        PositionLigne = "RIGHT";
                    }
                    else
                    {
                        PositionLigne = "MIDDLE";
                    }
                }

                if (Larger + "_" + PositionLigne + "_" + PositionColonne == "BIG_MIDDLE_DOWN")
                {
                    if (RandomAlgorithm.Instance.Next(100) + 1 > 70)
                    {
                    }
                }
            }
        }
        public void Afficher()
        {
            AlgorithmeEliminationSimple map = new AlgorithmeEliminationSimple();

            Carte.Carte carte = new AlgorithmeEliminationSimple().Generer(123);
            
            SalleStart salleStart = null;

            foreach (Salle salle in carte.Salles)
            {
                if(salle is SalleStart)
                {
                    salleStart = salle as SalleStart;
                }
            }

            RoomGenerationAlgorithm room = new RoomGenerationAlgorithm();
            Carte.Carte test = room.Generer(salleStart.SeedLocal);

            foreach (Salle salle in test.Salles)
            {
                if (salle.Type == TypeSalle.TILEFULL)
                {
                    Console.Write("X");
                }
                else
                {
                    Console.Write(" ");
                }

                if (salle.Colonne == Carte.Carte.Taille - 1)
                {
                    Console.WriteLine();
                }
            }
            
        }
    }
}
