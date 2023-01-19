using Server.Utils.ProceduralGeneration.GenerationAlgorithm.Realisation.Graphes;

namespace Serveur.Model
{
    public class Enemies
    {

        public Enemies(Coordonnees coordonnees)
        {
            this.Coordonnees = coordonnees;
        }
        public int Damage { get => damage; set => damage = value; }
        private int damage;

        public int Health { get => health; set => health = value; }
        private int health;

        public Coordonnees Coordonnees { get => coordonnees; set => coordonnees = value; }
        private Coordonnees coordonnees;
    }
}
