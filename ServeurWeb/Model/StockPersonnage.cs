namespace Server.Model
{
    /// <summary>
    /// Class that represent the StockPersonnage
    /// </summary>
    public class StockPersonnage
    {
        /// <summary>
        /// the id of the characters
        /// </summary>
        public int Id { get => id; set => id = value; }
        private int id;

        /// <summary>
        /// the date of the entry 
        /// </summary>
        public DateTime Entree { get => entree; set => entree = value; }
        private DateTime entree;

        /// <summary>
        /// the solt 
        /// </summary>
        public int Slot { get => slot; set => slot = value; }
        private int slot;
    }
}
