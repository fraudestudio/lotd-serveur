namespace Serveur.Utils.ProceduralGeneration.Carte.Salles.Realisation
{
    public class SalleStart : Salle
    {
        public SalleStart(int ligne, int colonne) : base(ligne, colonne)
        {
        }

        public override TypeSalle Type => TypeSalle.START;
    }
}
