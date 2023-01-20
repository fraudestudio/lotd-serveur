using System.Diagnostics;
using System.Net.Sockets;
using System.Text;

namespace Server.Utils
{
	public class Launcher
	{
		// singleton -----------------------------------------------------------

		private static Launcher? current = null;

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

		private Launcher()
		{
			this.executable = Environment.GetEnvironmentVariable("GAME_EXECUTABLE") ?? "";
			this.games = new Dictionary<int, GameSubprocess>();
		}

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

		public void EndGame(int port)
		{
			lock (this.games)
			{
				this.games.Remove(port);
			}
		}

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