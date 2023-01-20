namespace Server.Model
{
    public class Perso
    {
        /// <summary>
        /// the id of the characters 
        /// </summary>
        public int Id { get => id; set => id = value; }
        private int id;

        /// <summary>
        /// the level of the characters 
        /// </summary>
        public int Level { get => level; set => level = value; }
        private int level;

        /// <summary>
        /// the health points of the characters 
        /// </summary>
        public int PV { get => pv; set => pv = value; }
        private int pv;
        public bool Possede { get => possede; set => possede = value; }
        private bool possede;

        /// <summary>
        /// the max health points of the characters
        /// </summary>
        public int PV_MAX { get => pv_max; set => pv_max = value; }
        private int pv_max;

        /// <summary>
        /// the name of the characters
        /// </summary>
        public string Name { get => name; set => name = value; } 
        private string name = "";

        /// <summary>
        /// move points max  
        /// </summary>
        public int PA_MAX { get => pa_max; set => pa_max = value; }
        private int pa_max;

        /// <summary>
        /// magic points max
        /// </summary>
        public int PM_MAX { get => pm_max; set => pm_max = value;}
        private int pm_max;

        /// <summary>
        /// sprite of the characters
        /// </summary>
        public int IMG { get => img; set => img = value; }
        private int img;

        /// <summary>
        /// id du village du joueurs 
        /// </summary>
        public int ID_VILLAGE { get => id_village; set => id_village = value; }
        private int id_village;

        /// <summary>
        /// the class of the characters
        /// </summary>
        public string CLASSE { get => classe; set => classe = value; }
        private string classe = "";

        /// <summary>
        /// race of the characters
        /// </summary>
        public string RACE { get => race; set => race = value ; }
        private string race = "";

        /// <summary>
        /// id of the equipement 
        /// </summary>
        public int ID_EQUIPEMENT { get => id_equipement; set => id_equipement = value; }
        private int id_equipement;
    }
}
