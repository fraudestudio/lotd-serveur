namespace Server.Model
{
    /// <summary>
    /// Class that represent the Equipements
    /// </summary>
    public class Equipement
    {
        /// <summary>
        /// The id of the equipement
        /// </summary>
        public int Id { get => id; set => id = value; }
        private int id;
        
        /// <summary>
        /// The level of the wapeon
        /// </summary>
        public int LevelWeapon { get => levelWeapon; set => levelWeapon = value; }
        private int levelWeapon;

        /// <summary>
        /// The level of the Armor
        /// </summary>
        public int LevelArmor { get => levelArmor; set => levelArmor = value; }
        private int levelArmor;

        /// <summary>
        /// The sprite of the weapon
        /// </summary>
        public int ImgWeapon { get => imgWeapon; set => imgWeapon = value; }
        private int imgWeapon;

        /// <summary>
        /// The sprite of the Armor
        /// </summary>
        public int ImgArmor { get => imgArmor; set => imgArmor = value; }
        private int imgArmor;

        /// <summary>
        /// Bonus of the Weapon 
        /// </summary>
        public int BonusWeapon { get => bonusWeapon; set => bonusWeapon = value; }
        private int bonusWeapon;

        /// <summary>
        /// Bonus of the armor 
        /// </summary>
        public int BonusArmor { get => bonusArmor; set => bonusArmor = value; }
        private int bonusArmor;
    }
}
