namespace Server.Model
{
    /// <summary>
    /// Classe représentant une personne
    /// </summary>
    public class Personne
    {
        private string nom;
        private string prenom;

        /// <summary>
        /// Nom de la personne
        /// </summary>
        public String Nom { get => nom;set => nom = value; }
        /// <summary>
        /// Prénom de la personne
        /// </summary>
        public String Prenom { get => prenom; set => nom = value; }

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        /// <param name="prenom">Prénom de la personne</param>
        /// <param name="nom">Nom de la personne</param>
        public Personne(string prenom,string nom)
        {
            this.nom = nom;
            this.prenom = prenom;
        }
    }
}
