using MySqlConnector;

namespace Server.Utils
{
    // ExecuteReaderAsync SELECT
    // ExecuteNonQueryAsync
    public abstract class DatabaseMessage
    {
        private string _query;
        private IDictionary<string, object> _params;

        public DatabaseMessage(string query, IDictionary<string, object> parameters)
        {
            this._query = query;
            this._params = parameters;
        }

        protected string Query => this._query;

        protected IDictionary<string, object> Parameters => this._params;

        public abstract void Execute(MySqlConnection connection);
    }
}