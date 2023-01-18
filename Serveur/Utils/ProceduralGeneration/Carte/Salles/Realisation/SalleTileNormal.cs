namespace Serveur.Utils.ProceduralGeneration.Carte.Salles.Realisation
{
    public class SalleTileNormale : Salle
    {
        public SalleTileNormale(int ligne, int colonne) : base(ligne, colonne)
        {
        }
        public override TypeSalle Type => TypeSalle.TILENORMALE;
    }
}
