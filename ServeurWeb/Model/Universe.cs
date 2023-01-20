namespace Server.Model
{
    /// <summary>
    /// Class that represent the Universe
    /// </summary>
    public class Universe
    {
        /// <summary>
        /// the id of the universe
        /// </summary>
        public int? Id { get => id; set => id = value; }
        private int? id;

        /// <summary>
        /// the name of the universe
        /// </summary>
        public String Name { get => name; set => name = value; }
        private String name = "";

        /// <summary>
        /// the password if their is one
        /// </summary>
        public String? Password { get => password; set => password = value; }
        private String? password;

        /// <summary>
        /// boolean if their is a password
        /// </summary>
        public bool? HasPassword { get => hasPassword; set => hasPassword = value; }
        private bool? hasPassword;

        /// <summary>
        /// the id of the town in the universe
        /// </summary>
        public int? Town { get => town; set => town = value; }
        private int? town;
    }
}
