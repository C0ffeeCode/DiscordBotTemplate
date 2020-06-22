using Discord.Commands;
using Discord.WebSocket;
using DiscordBotTemplate.Services;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DiscordBotTemplate
{
	public class Startup
	{
		private readonly DiscordSocketClient _client;
		private readonly CommandService _commands;

		public Startup(
			DiscordSocketClient client = null,
			CommandService commands = null)
		{
			_commands = commands ?? new CommandService();
			_client = client ?? new DiscordSocketClient();
		}

		public IServiceProvider BuildProvider() => new ServiceCollection()
			//// Register DbContext (Database access)
			//.AddDbContext<BeeContext>(o => o.UseCosmos(
			//	Environment.GetEnvironmentVariable("DatabaseEndpoint"),
			//	Environment.GetEnvironmentVariable("DatabaseKey"),
			//	Environment.GetEnvironmentVariable("DatabaseName")))
			// Register services
			.AddSingleton<CommandHandlerService>()
			.AddSingleton(_client)
			.AddSingleton(_commands)
			.BuildServiceProvider();
			// Register custom service
			//.AddSingleton<MyService>()
	}
}
