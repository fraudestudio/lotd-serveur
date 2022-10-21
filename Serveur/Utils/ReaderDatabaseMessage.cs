using MySqlConnector;

namespace Server.Utils
{
    public class ReaderDatabaseMessage<T1> : DatabaseMessage
    {
        private Promise<List<Tuple<T1>>> _result;

        public ReaderDatabaseMessage(string query, IDictionary<string, object> parameters, Promise<List<Tuple<T1>>> result)
        : base(query, parameters)
        {
            this._result = result;
        }

        public override void Execute(MySqlConnection connection)
        {
            /*MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@nomcompte", nom_compte);
            cmd.Parameters.AddWithValue("@mdp", mdp);
            MySqlDataReader dataReader = cmd.ExecuteReader();*/
        }
    }
}