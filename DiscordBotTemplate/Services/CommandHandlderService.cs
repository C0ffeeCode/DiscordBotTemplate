using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace DiscordBotTemplate.Services
{
	public class CommandHandlderService
	{
		private readonly DiscordSocketClient _client;
		private readonly CommandService _commands;
		private readonly IServiceProvider _services;

		public CommandHandlderService(
			IServiceProvider services,
			CommandService commands,
			DiscordSocketClient client)
		{
			_commands = commands;
			_services = services;
			_client = client;
		}

		public async Task InstallCommandsAsync()
		{
			// Finds all modules and registers them
			await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(),
											services: _services);

			// Hook command handler to message recived event
			_client.MessageReceived += HandleCommandAsync;
		}

		private async Task HandleCommandAsync(SocketMessage messageParam)
		{
			// Don't process the command if it was a system message
			SocketUserMessage message = (SocketUserMessage)messageParam;
			if (message == null)
			{
				return;
			}

			// Create a number to track where the prefix ends and the command begins
			int argPos = 0;

			if (!(  // Determine if the message is a command based on the prefix
					message.HasCharPrefix('!', ref argPos) ||
					// You can also use strings, but keep in mind to include the whitespace char if needed
					message.HasStringPrefix("bot ", ref argPos, StringComparison.OrdinalIgnoreCase) ||
					// You can also check if the bot was mentioned
					message.HasMentionPrefix(_client.CurrentUser, ref argPos)
				) ||
				// make sure no bots trigger commands
				message.Author.IsBot)
			{
				return;
			}

			SocketCommandContext context = new SocketCommandContext(_client, message);

			// Keep in mind that result does not indicate a return value
			// rather an object stating if the command executed successfully.
			await _commands.ExecuteAsync(
				context: context,
				argPos: argPos,
				_services);

			// Optionally, we may inform the user if the command fails
			// to be executed; however, this may not always be desired,
			// as it may clog up the request queue should a user spam a
			// command.
			// if (!result.IsSuccess)
			//	await context.Channel.SendMessageAsync(result.ErrorReason);
		}
	}
}
