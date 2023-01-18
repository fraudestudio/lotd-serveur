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
        // Singleton
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

        // the global seed
        private int seedGlobale;
        // the local seed
        private int seedLocale;

        private RandomAlgorithm()
        {

        }

        /// <summary>
        /// Set the global seed of the generator
        /// </summary>
        /// <param name="seed">the seed</param>
        public void SetSeedGlobal(int seed)
        {
            Instance.seedGlobale = seed;
            Instance.random = new Random(Instance.seedGlobale);
            Instance.seedLocale = 0;
        }

        /// <summary>
        /// Set the local seed of the generator
        /// </summary>
        /// <param name="seed">the local seed</param>
        public void SetSeedLocale(int seed)
        {
            Instance.seedLocale = seed;
            Instance.random = new Random(Instance.seedGlobale + Instance.seedLocale);
        }

        /// <summary>
        /// Generate a number
        /// </summary>
        /// <returns>the number generated</returns>
        public int Next()
        {
            return Instance.random.Next();

        }

        /// <summary>
        /// Generate a number with max 
        /// </summary>
        /// <param name="borneMax">the max number</param>
        /// <returns>the generated number</returns>
        public int Next(int borneMax)
        {
            return Instance.random.Next(borneMax);
        }

        /// <summary>
        /// Generate a new coordinates base 
        /// </summary>
        /// <returns></returns>
        public Coordonnees NextCoordonnees()
        {
            return new Coordonnees(Next(Carte.Taille), Next(Carte.Taille));
        }
    }
}
