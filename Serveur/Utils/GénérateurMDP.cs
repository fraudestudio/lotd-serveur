namespace Serveur.Utils
{
    public class GénérateurMDP
    {
        public static string GénérerMDP()
        {

            Random r = new Random();
            string mdp = "";


            for (int i = 0; i < 8; i++)
            {

                switch (r.Next(0, 2))
                {
                    case 0:
                        mdp += (char)r.Next(65, 90);
                        break;
                    case 1:
                        mdp += (char)r.Next(97, 122);
                        break;
                    case 2:
                        mdp += (char)r.Next(48, 57);
                        break;
                }

            }
            return mdp;
        }
    }
}
