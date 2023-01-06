using Serveur.Utils.ProceduralGeneration.Carte.Salles;

namespace Server.Utils.ProceduralGeneration.GenerationAlgorithm.Realisation.Graphes
{
    public class Sommet
    {
        private TypeSalle typeSalle;
        public TypeSalle TypeSalle { get => typeSalle; set => typeSalle = value; }


        List<Sommet> voisins = new List<Sommet>();

        public List<Sommet> Voisins { get => voisins; }

        public Sommet(TypeSalle typeSalle = TypeSalle.NORMALE)
        {
            TypeSalle = typeSalle;
        }

        public void AjouterVoisin(Sommet s)
        {
            voisins.Add(s);
        }
    }
}
