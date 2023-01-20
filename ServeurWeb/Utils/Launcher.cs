using System.Diagnostics;
using System.Net.Sockets;
using System.Text;

namespace Server.Utils
{
    /// <summary>
    /// Class that represent the launcher
    /// </summary>
    public class Launcher
	{
        // singleton -----------------------------------------------------------
        
		/// <summary>
        /// The instance of the launcher
        /// </summary>
        private static Launcher? current = null;

        /// <summary>
		/// The Property  of the instance
		/// </summary>
		public static Launcher Current
		{
			get
			{
				if (current == null)
				{
					current = new Launcher();
				}
				return current;
			}
		}

        // ---------------------------------------------------------------------

        /// <summary>
        /// class that represant the sub process
        /// </summary>
        private class GameSubprocess
		{
			private bool started;
			private int universeId;
			private int port;
			private TcpClient processConnection;
			private StreamReader input;
			private StreamWriter output;
			private Process processHandle;

			public bool Started => this.started;

			public int UniverseId => this.universeId;


            /// <summary>
			/// Constructor of the GameSubprocess
			/// </summary>
			/// <param name="port"> the port of the game</param>
			/// <param name="executable"> the executable of the game</param>
			/// <param name="universeId" > the id of the universe</param>
			/// <exception cref="Exception"> if the process is not started </exception>
			public GameSubprocess(int port, String executable, int universeId)
			{
				this.started = false;
				this.universeId = universeId;
				this.port = port;

				String token = Util.RandomPassword(20);

				this.processHandle = Process.Start(
					new ProcessStartInfo
					{
						FileName = executable,
						Arguments = String.Format("{0} {1}", this.port, token),
						// ne pas lancer via l'environement de bureau
						UseShellExecute = false, 
					}
				)!;

				this.processConnection = new TcpClient();

				bool success = false;
				for (int i = 0; i < 100; i++)
				{
					try {
						this.processConnection.Connect("localhost", port);
						success = true;
						break;
					}
					catch (SocketException)
					{
						Thread.Sleep(100);
					}
				}

				if (success)
				{
					this.processConnection.NoDelay = true;
					this.input = new StreamReader(this.processConnection.GetStream(), Encoding.ASCII);
					this.output = new StreamWriter(this.processConnection.GetStream(), Encoding.ASCII);

					String? msg = this.input.ReadLine();
					if (msg != "AUTH") throw new Exception(String.Format("Message inattendu « {0} »", msg));

					this.output.WriteLine("AUTH {0}", token);
					this.output.Flush();

					msg = this.input.ReadLine();
					if (msg != "OK") throw new Exception("L'authentification a échouée");
				}
				else
				{
					throw new Exception("Le serveur n'a pas démarré en 10 secondes");
				}
			}

            /// <summary>
			/// Method that join the game
			/// </summary>
			/// <param name="playerId"> the id of the player</param>
			/// <param name="playerToken"> the token of the player</param>
			/// <exception cref="InvalidOperationException"> if the game is not started </exception>
			public void Join(int playerId, String playerToken)
			{
				if (this.Started)
				{
					throw new InvalidOperationException("This game already started");
				}
				else
				{
					this.output.WriteLine("JOIN {0} {1}", playerId, playerToken);
					this.output.Flush();

					new Thread(() => {
						String? msg;
						do
						{
							msg = this.input.ReadLine();
							if (msg == "STARTED")
							{
								this.started = true;
							}
						} while (msg != "END");

						this.input.Close();
						this.output.Close();
						this.processConnection.Close();

						this.processHandle.WaitForExit();
						this.processHandle.Close();

						Launcher.Current.EndGame(this.port);
					}).Start();
				}
			}
		}

		// ---------------------------------------------------------------------

		private String executable;
		private Dictionary<int, GameSubprocess> games;

		private const int PORT_RANGE_START = 2000;

        /// <summary>
		/// Constructor of the launcher
		/// </summary>
		private Launcher()
		{
			this.executable = Environment.GetEnvironmentVariable("GAME_EXECUTABLE") ?? "";
			this.games = new Dictionary<int, GameSubprocess>();
		}

		/// <summary>
		/// Method that crate or join a game
		/// </summary>
		/// <param name="universeId"></param>
		/// <param name="playerId"></param>
		/// <param name="playerToken"></param>
		/// <returns></returns>
		public int? CreateOrJoinGame(int universeId, int playerId, String playerToken)
		{
			lock (this.games)
			{
				int gameToJoin = this.FindGame(universeId);

				if (gameToJoin == -1)
				{
					gameToJoin = this.CreateGame(universeId);
				}

				this.games[gameToJoin].Join(playerId, playerToken);
				
				return gameToJoin;
			}
		}

        /// <summary>
        /// Method that end the Game
        /// </summary>
        /// <param name="port">port on which the game run</param>
        public void EndGame(int port)
		{
			lock (this.games)
			{
				this.games.Remove(port);
			}
		}

		/// <summary>
		/// find the game
		/// </summary>
		/// <param name="universeId"></param>
		/// <returns></returns>
		private int FindGame(int universeId)
		{
			Console.WriteLine("searching game in universe {0}", universeId);
			foreach (var (port, game) in this.games)
			{
				Console.WriteLine("game on port {0}", port);
				Console.WriteLine("  started: {0}", game.Started);
				Console.WriteLine("  universe ID: {0}", game.UniverseId);
				if (!game.Started && game.UniverseId == universeId)
				{
					return port;
				}
			}

			return -1;
		}

		/// <summary>
		/// Method that create the game
		/// </summary>
		/// <param name="universeId"></param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		private int CreateGame(int universeId)
		{
			for (int port = PORT_RANGE_START; port < PORT_RANGE_START+1000; port++)
			{
				if (!this.games.ContainsKey(port))
				{
					GameSubprocess game = new GameSubprocess(port, this.executable, universeId);
					this.games.Add(port, game);
					return port;
				}
			}

			throw new Exception("No port available"); 
		}
	}
}