using Server.Utils.ProceduralGeneration.GenerationAlgorithm.Realisation.Graphes;

namespace Server.Model
{
    public class Perso
    {

        /// <summary>
        /// Property for the DAO of the Perso
        /// </summary>
        public int Id { get => id; set => id = value; }
        private int id;
        public int Level { get => level; set => level = value; }
        private int level;
        public int PV { get => pv; set => pv = value; }
        private int pv;
        public bool Possede { get => possede; set => possede = value; }
        private bool possede;
        public int PV_MAX { get => pv_max; set => pv_max = value; }
        private int pv_max;
        public string Name { get => name; set => name = value; }
        private string name = "";
        public int PA_MAX { get => pa_max; set => pa_max = value; }
        private int pa_max;
        public int PM_MAX { get => pm_max; set => pm_max = value;}
        private int pm_max;
        public int IMG { get => img; set => img = value; }
        private int img;
        public int ID_VILLAGE { get => id_village; set => id_village = value; }
        private int id_village;
        public string CLASSE { get => classe; set => classe = value; } 
        private string classe = "";
        public string RACE { get => race; set => race = value; }
        private string race ="";
        public int ID_EQUIPEMENT { get => id_equipement; set => id_equipement = value ; }
        private int id_equipement;       

        /// <summary>
        /// property for the position of the character
        /// </summary>
        public Coordonnees Coordonnees { get => coordonnees; set => coordonnees = value; }
        private Coordonnees coordonnees;
    }
}
