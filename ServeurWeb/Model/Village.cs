namespace Server.Model
{
    /// <summary>
    /// Class that represent the Village
    /// </summary>
    public class Village
    {
        /// <summary>
        /// The id of the Univers
        /// </summary>
        public int IdUnivers { get => idUnivers; set => idUnivers = value; }
        private int idUnivers;
        
        /// <summary>
        /// string that represant the faction of the player 
        /// </summary>
        public String Faction { get => faction; set => faction = value; }
        private String faction = "";

        /// <summary>
        /// The name of the village
        /// </summary>
        public String Name { get => name; set => name = value; }
        private String name = "";

    }
}
