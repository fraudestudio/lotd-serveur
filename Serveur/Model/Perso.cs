namespace Server.Model
{
    public class Perso
    {
        public int? Id { get; set; }
        public String Name { get; set; } = "";
        public int Race { get; set; }
        public String Classe { get; set; }
        public int PV { get; set; }
        public int PV_MAX { get; set; }
        public int PA_MAX { get; set; }
    }
}
