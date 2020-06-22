using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordBotTemplate.Modules
{
	public class SampleModule : ModuleBase<SocketCommandContext>
	{
		#region Echo
		[Command("echo")]
		[Alias("say")]
		[Summary("Says a message.")]
		// "Remainder" makes sure you get the full text (But the previos arguments like "!say ")
		public Task SayAsync([Remainder][Summary("The text to echo")] string echo)
			=> ReplyAsync(echo);
		#endregion

		#region Clear
		// You can check User and Bot permission very easily
		[RequireUserPermission(GuildPermission.ManageMessages)]
		[RequireBotPermission(ChannelPermission.ManageMessages)]
		// Define this a command with following name and alias
		[Command("clear")]
		[Alias("cls", "other alias")]
		[Summary("Clear a channel. No limit :)")]
		public async Task ClearAsync([Summary("Number of messages to clear")] int amount)
		{
			// Avoid deleting too many messages too often to avoid api spam
			IEnumerable<IMessage> messages = await Context.Channel.GetMessagesAsync(amount + 1).FlattenAsync();

			await ((SocketTextChannel)Context.Channel).DeleteMessagesAsync(messages);
		}
		#endregion

	}
}
