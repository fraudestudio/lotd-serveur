namespace Server.Model
{
    /// <summary>
    /// Class that represent the Ressources
    /// </summary>
    public class Ressources
    {
        /// <summary>
        /// the wood 
        /// </summary>
        public int Bois { get => bois; set => bois = value; }
        private int bois;

        /// <summary>
        /// the stone
        /// </summary>
        public int Pierre { get => stone; set => stone = value; }
        private int stone;

        /// <summary>
        /// the gold 
        /// </summary>
        public int Or { get => gold; set => gold = value; }
        private int gold;
    }
}
