using Server.Utils.ProceduralGeneration.GenerationAlgorithm.Realisation.Graphes;
using System.Security.Cryptography;
using System.Text;
using Server.Utils.ProceduralGeneration;
using Serveur.Utils.ProceduralGeneration.Carte;
using Random = System.Random;

namespace Server.Utils.ProceduralGeneration.GenerationAlgorithm
{
    public class RandomAlgorithm
    {
        private static RandomAlgorithm instance;
        
        public static RandomAlgorithm Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RandomAlgorithm();
                }
                return instance;
            }
        }

        private Random random;
        private int seedGlobale;

        private RandomAlgorithm() { }

        public void SetSeed(int seed)
        {
            Instance.seedGlobale = seed;
            Instance.random = new Random(Instance.seedGlobale);
            Console.WriteLine("Seed: " + Instance.seedGlobale);
            Console.WriteLine("random: " + Instance.random);
        }

        public int Next()
        {
            return Instance.random.Next();
        }

        public int Next(int borneMax)
        {
            return Instance.random.Next(borneMax);
        }

        public static Coordonnees NextCoordonnes()
        {
            return new Coordonnees(Instance.Next(Carte.Taille), Instance.Next(Carte.Taille));
        }
    }
}
