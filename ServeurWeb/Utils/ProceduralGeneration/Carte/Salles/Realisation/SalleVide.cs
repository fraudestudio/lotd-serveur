namespace Serveur.Utils.ProceduralGeneration.Carte.Salles.Realisation
{
    public class SalleVide : Salle
    {
        public SalleVide(int ligne, int colonne) : base(ligne, colonne)
        {
        }

        public override TypeSalle Type => TypeSalle.VIDE;
        public override bool EstVide => true;
    }
}
