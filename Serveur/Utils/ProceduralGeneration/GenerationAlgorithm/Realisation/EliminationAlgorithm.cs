using Server.Utils.ProceduralGeneration.GenerationAlgorithm;
using Server.Utils.ProceduralGeneration.GenerationAlgorithm.Realisation.Graphes;
using Serveur.Utils.ProceduralGeneration.Carte;
using Serveur.Utils.ProceduralGeneration.Carte.Salles;

namespace Serveur.Utils.ProceduralGeneration.GenerationAlgorithm.Realisation
{
    public class AlgorithmeEliminationSimple : IAlgorithmeGeneration
    {

        public Carte.Carte Generer(int seed)
        {
            // We initiale the map length 
            Carte.Carte.Taille = 6;
            Graphe g = new Graphe();
            // We set the global seed
            RandomAlgorithm.Instance.SetSeedGlobal(seed);



            // coordinates that will be used to generate special room 
            List<Coordonnees> coordonnees = new List<Coordonnees>();


            // We generate two coordinates
            while (coordonnees.Count < 2)
            {
                bool add = true;
                Coordonnees c = RandomAlgorithm.Instance.NextCoordonnees();

                // if there is already existing coordinates
                if (coordonnees.Count >= 1)
                {
                    // We make sure that the new coordinates is at the right distances of all the already 
                    // Created coordinates
                    for (int i = 0; i < coordonnees.Count; i++)
                    {
                        if (c.Ligne == coordonnees[i].Ligne && c.Colonne == coordonnees[i].Colonne)
                        {
                            add = false;
                        }

                        // We make sure that there is at least a distance of 6
                        if (c.Distance(coordonnees[i]) < 6)
                        {
                            add = false;
                        }
                    }

                    // if all the verficiation are good, we add the coordinates
                    if (add)
                    {
                        coordonnees.Add(c);
                    }
                }
                else
                {
                    // We simply add the first coordinates if it doesn't exists
                    coordonnees.Add(c);
                }

            }

            // the first selected coordinates is choosed to be the boss room 
            g.GetSommet(coordonnees[0].Ligne, coordonnees[0].Colonne).TypeSalle = TypeSalle.BOSS;
            // the second is choosed for being the start room 
            g.GetSommet(coordonnees[1].Ligne, coordonnees[1].Colonne).TypeSalle = TypeSalle.START;


            // We delete 3 cases around the selected coordinates 
            for (int i = 0; i < 2; i++)
            {
                int startint = RandomAlgorithm.Instance.Next(g.GetSommet(coordonnees[i].Ligne, coordonnees[i].Colonne).Voisins.Count);
                for (int j = 0; j < g.GetSommet(coordonnees[i].Ligne, coordonnees[i].Colonne).Voisins.Count; j++)
                {
                    if (j != startint)
                    {
                        bool canDel = true;

                        foreach (Sommet s in g.GetSommet(coordonnees[i].Ligne, coordonnees[i].Colonne).Voisins[j].Voisins)
                        {
                            if (s.TypeSalle == TypeSalle.VIDE)
                            {
                                canDel = false;
                            }
                        }

                        if (canDel)
                        {
                            g.GetSommet(coordonnees[i].Ligne, coordonnees[i].Colonne).Voisins[j].TypeSalle = TypeSalle.VIDE;
                        }
                    }
                }
            }

            List<Sommet> listeDesSommets = g.Parcours(g.GetSommet(coordonnees[1].Ligne, coordonnees[1].Colonne), null);

            // We delete 40% of the sommet
            for (int i = 0; i < 40; i++)
            {
                listeDesSommets = this.SupprimerSommet(g, listeDesSommets, g.GetSommet(coordonnees[0].Ligne, coordonnees[0].Colonne), g.GetSommet(coordonnees[1].Ligne, coordonnees[1].Colonne));
            }
            return g.ToCarte();
        }

        /// <summary>
        /// Verify if we can delete a sommet and delete it if we can
        /// </summary>
        /// <param name="graphe"></param>
        /// <param name="listeDesSommets"></param>
        /// <param name="SalleDepart"></param>
        /// <param name="SalleBoss"></param>
        /// <returns>The new list of sommet</returns>
        private List<Sommet> SupprimerSommet(Graphe graphe, List<Sommet> listeDesSommets, Sommet SalleDepart, Sommet SalleBoss)
        {
            Sommet sommetAEnlever = listeDesSommets[RandomAlgorithm.Instance.Next(listeDesSommets.Count)];
            List<Sommet> resultat = listeDesSommets;
            if (sommetAEnlever != SalleDepart)
            {
                List<Sommet> newListeDesSommets = graphe.Parcours(SalleDepart, sommetAEnlever);

                if (newListeDesSommets.Contains(SalleBoss) && newListeDesSommets.Count > 15)
                {
                    resultat = newListeDesSommets;
                    for (int i = 0; i < Carte.Carte.Taille; i++)
                    {
                        for (int j = 0; j < Carte.Carte.Taille; j++)
                        {
                            if (!resultat.Contains(graphe.GetSommet(i, j)))
                            {
                                graphe.GetSommet(i, j).TypeSalle = TypeSalle.VIDE;
                            }
                        }
                    }
                }
            }
            return resultat;
        }

        public void Afficher()
        {
            Carte.Carte carte = new AlgorithmeEliminationSimple().Generer(123);
            for (int i = 0; i < Carte.Carte.Taille; i++)
            {
                for (int j = 0; j < Carte.Carte.Taille; j++)
                {
                    if (carte.Salles[i, j].Type == Carte.Salles.TypeSalle.VIDE)
                    {
                        Console.Write(" ");
                    }
                    else if (carte.Salles[i, j].Type == Carte.Salles.TypeSalle.START)
                    {
                        Console.Write("S");
                    }
                    else if (carte.Salles[i, j].Type == Carte.Salles.TypeSalle.BOSS)
                    {
                        Console.Write("B");
                    }
                    else
                    {
                        Console.Write("X");
                    }
                }
                Console.WriteLine();
            }
        }
    }  
}