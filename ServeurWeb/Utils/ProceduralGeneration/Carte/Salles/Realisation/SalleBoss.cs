namespace Serveur.Utils.ProceduralGeneration.Carte.Salles.Realisation
{
    public class SalleBoss : Salle
    {
        public SalleBoss(int ligne, int colonne) : base(ligne, colonne)
        {
        }

        public override TypeSalle Type => TypeSalle.BOSS;
    }
}
