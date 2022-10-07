namespace Server.Model
{
    /// <summary>
    /// Classe représentant une personne
    /// </summary>
    public class Personne
    {
        private string mail;
        private string pseudo;
        private string mdp;
       

        /// <summary>
        /// Nom de la personne
        /// </summary>
        public String Mail { get => mail;set => mail = value; }
        /// <summary>
        /// Prénom de la personne
        /// </summary>
        public String Pseudo { get => pseudo; set => pseudo = value; }
        public string Mdp { get => mdp; set => mdp = value; }

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        /// <param name="prenom">Prénom de la personne</param>
        /// <param name="nom">Nom de la personne</param>
        public Personne(string pseudo,string mail)
        {
            this.mail = mail;
            this.pseudo = pseudo;
        }
        
        
    }
}
