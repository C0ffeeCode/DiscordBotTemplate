using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBotTemplate.Services;
using dotenv.net;
using System;
using System.Threading.Tasks;

namespace DiscordBotTemplate
{
	class Program
	{
		public static void Main() =>
			new Program().MainAsync().GetAwaiter().GetResult();

		private readonly DiscordSocketClient _client = new DiscordSocketClient();

		private async Task MainAsync()
		{
			// Load config from .env file at the projects root path to the enviroment variables
			// In production when using Docker, you define them using Docker instead
			DotEnv.Config(false, ".env");

			Startup y = new Startup(_client);
			CommandHandlerService x = new CommandHandlerService(y.BuildProvider(), new CommandService(), _client);

			// Registers logging
			_client.Log += Log;

			// Starts logging in and registering the commands 
			await x.InstallCommandsAsync();
			await _client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("DiscordToken"));

			await _client.StartAsync();

			// Block this task until the program is closed.
			await Task.Delay(-1);
		}

		private Task Log(LogMessage msg)
		{
			Console.WriteLine(msg.ToString());
			return Task.CompletedTask;
		}
	}
}
