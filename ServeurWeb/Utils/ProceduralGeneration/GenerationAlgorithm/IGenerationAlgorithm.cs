using Serveur.Utils.ProceduralGeneration.Carte;

namespace Serveur.Utils.ProceduralGeneration.GenerationAlgorithm
{
    public class IGenerationAlgorithm
    {
        public interface IAlgorithmeGeneration
        {
            /// <summary>
            /// Génère une carte aléatoire à partir de la seed donnée
            /// </summary>
            /// <param name="seed">Seed de la carte</param>
            /// <returns>La carte générée</returns>
            Carte.Carte Generer(String seed);
        }
    }
}
