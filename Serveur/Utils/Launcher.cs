namespace Server.Utils
{
	public class Launcher
	{
		static private String EXECUTABLE = Environment.GetEnvironmentVariable("GAME_EXECUTABLE") ?? "";

		public static int? LaunchInstance()
		{
			Process process = Process.Start(
				new ProcessStartInfo
				{
					FileName = Launcher.EXECUTABLE,
					RedirectStandardOutput = true,
					UseShellExecute = false
				}
			);
			
			String? maybeOutput = process.StandardOutput.ReadLine();

			if (maybeOutput is String output) {
				return Int32.Parse(output);
			}
			else {
				return null;
			}
		}
	}
}