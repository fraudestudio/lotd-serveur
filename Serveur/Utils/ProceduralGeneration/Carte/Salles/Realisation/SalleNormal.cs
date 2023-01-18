namespace Serveur.Utils.ProceduralGeneration.Carte.Salles.Realisation
{
    /// <summary>
    /// Salle normale
    /// </summary>
    public class SalleNormale : Salle
    {
        public SalleNormale(int ligne, int colonne) : base(ligne, colonne)
        {
        }


        public override TypeSalle Type => TypeSalle.NORMALE;
    }
}
