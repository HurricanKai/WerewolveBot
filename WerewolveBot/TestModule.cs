using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WerewolveBot
{
	//[Group("My Prefix")]
	public class TestModule : ModuleBase<SocketCommandContext>
	{
		// ~say hello -> hello
		[Command("say")]
		[Summary("Echos a message.")]
		public async Task SayAsync([Remainder] [Summary("The text to echo")] string echo)
		{
			// ReplyAsync is a method on ModuleBase
			await ReplyAsync(echo);
		}

		[Command("exit")]
		[Summary("Exit")]
		[RequireUserPermission(Discord.GuildPermission.Administrator)]
		public async Task Logout()
		{
			await ReplyAsync("Disconnecting");
			await Context.Client.SetStatusAsync(Discord.UserStatus.Offline);
			await Context.Client.StopAsync();


			Environment.Exit(0);
		}
	}
}
