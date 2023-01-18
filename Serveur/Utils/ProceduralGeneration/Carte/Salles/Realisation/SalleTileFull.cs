namespace Serveur.Utils.ProceduralGeneration.Carte.Salles.Realisation
{
    public class SalleTileFull : Salle
    {
        public SalleTileFull(int ligne, int colonne) : base(ligne, colonne)
        {
        }
        public override TypeSalle Type => TypeSalle.TILEFULL;
    }
}
