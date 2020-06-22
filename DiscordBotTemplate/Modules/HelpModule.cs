using Discord;
using Discord.Commands;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBotTemplate.Modules
{
	public class HelpModule : ModuleBase<SocketCommandContext>
	{
		private readonly CommandService _service;

		// Gets a service injected using dependency injection defined in Startup.cs
		public HelpModule(CommandService service)
		{
			_service = service;
		}

		[Command("Help")]
		public Task Help()
		{
			// Gets all registered Modules & Commands and orderes by their name
			IOrderedEnumerable<CommandInfo> commands = _service.Commands.ToList().OrderBy(o => o.Name);
			EmbedBuilder embedBuilder = new EmbedBuilder
			{
				Author = new EmbedAuthorBuilder()
				{
					Name = "Bot help",
					IconUrl = "https://cdn.discordapp.com/attachments/304260989451370506/723286131562643476/beebot.png"
				}
			};

			foreach (CommandInfo command in commands)
			{
				// Get the command Summary attribute information
				string embedFieldText = command.Summary ?? "No description available\n";

				// add the field to the message
				embedBuilder.AddField($"{command.Module.Group} {command.Name}", embedFieldText);
			}

			return ReplyAsync("Here's a list of commands and their description: ", embed: embedBuilder.Build());
		}
	}
}
