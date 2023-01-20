using Server.Utils.ProceduralGeneration.GenerationAlgorithm;
using Server.Utils.ProceduralGeneration.GenerationAlgorithm.Realisation.Graphes;
using Serveur.Utils.ProceduralGeneration.Carte;
using static Serveur.Utils.ProceduralGeneration.GenerationAlgorithm.IGenerationAlgorithm;

namespace Serveur.Utils.ProceduralGeneration.GenerationAlgorithm.Realisation
{
    public class AlgorithmeEliminationSimple : IGenerationAlgorithm
    {
        public Carte.Carte Generer(int seed)
        {

            Graphe g = new Graphe();
            RandomAlgorithm.Instance.SetSeed(seed);

            List<Coordonnees> coordonnees = new List<Coordonnees>();


            while (coordonnees.Count < 3)
            {
                bool add = true;
                Coordonnees c = RandomAlgorithm.NextCoordonnes();

                if (coordonnees.Count >= 1)
                {
                    for (int i = 0; i < coordonnees.Count; i++)
                    {
                        if (c.Ligne == coordonnees[i].Ligne && c.Colonne == coordonnees[i].Colonne)
                        {
                            add = false;
                        }

                        if (c.Distance(coordonnees[i]) < 6)
                        {
                            add = false;
                        }
                    }

                    if (add)
                    {
                        coordonnees.Add(c);
                    }
                }
                else
                {
                    coordonnees.Add(c);
                }

            }

            g.GetSommet(coordonnees[0].Ligne, coordonnees[0].Colonne).TypeSalle = Carte.Salles.TypeSalle.BOSS;
            g.GetSommet(coordonnees[1].Ligne, coordonnees[1].Colonne).TypeSalle = Carte.Salles.TypeSalle.START;


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
                            if (s.TypeSalle == Carte.Salles.TypeSalle.VIDE)
                            {
                                canDel = false;
                            }
                        }

                        if (canDel)
                        {
                            g.GetSommet(coordonnees[i].Ligne, coordonnees[i].Colonne).Voisins[j].TypeSalle = Carte.Salles.TypeSalle.VIDE;
                        }
                    }
                }
            }


            List<Sommet> listeDesSommets = g.Parcours(g.GetSommet(coordonnees[1].Ligne, coordonnees[1].Colonne), null);
            for (int i = 0; i < 40; i++)
            {
                listeDesSommets = this.SupprimerSommet(g, listeDesSommets, g.GetSommet(coordonnees[0].Ligne, coordonnees[0].Colonne), g.GetSommet(coordonnees[1].Ligne, coordonnees[1].Colonne));
            }
            return g.ToCarte();
        }
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
                                graphe.GetSommet(i, j).TypeSalle = Carte.Salles.TypeSalle.VIDE;
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