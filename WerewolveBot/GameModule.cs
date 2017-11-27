using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Net;
using Discord;
using Discord.Rest;
using Discord.Net.Rest;

namespace WerewolveBot
{
	[Group("session")]
	[Alias("Session", "s")]
	public class GameModule : ModuleBase<SocketCommandContext>
	{
		[Command("start")]
		public async Task Start()
		{
			/*
			 1. Send anyone Role + text
			 2. Send Shoutout that we start
			 3. Start Timer
			 4. -> Timer
			 */

			foreach (var v in RoleManager.instance.Players)
			{
				var v2 = Context.Client.GetUser(v.Name, v.Descriminator);
				var channel = await v2.GetOrCreateDMChannelAsync();
				await channel.SendMessageAsync($"You are an {v.GetMainRole().Name}{Environment.NewLine}{v.GetMainRole().Description}{Environment.NewLine}Your Sub-Role is: {v.GetSubRole().Name}{Environment.NewLine}{v.GetSubRole().Description}");
			}

			await Context.Guild.DefaultChannel.SendMessageAsync("Everyone Signed up & with an Role got that role now!" + Environment.NewLine + "Let The Games BEGIN!");
			await Context.Channel.SendMessageAsync(Context.Message.Author.Mention + " You Shoud Start the Timer!");
			SignupManager.instance.Players.Clear();
		}

		[Command("kill")]
		public async Task Kill([Summary("The User to Kill")] string user)
		{
			var role = RoleManager.instance.Players.Find(new Predicate<PlayerRole>((PlayerRole r) => {return (("@" + r.Name + "#" + r.Descriminator) == Context.Message.MentionedUsers.First().Mention); }));
			RoleManager.instance.Players.Remove(role);
			await Context.Guild.DefaultChannel.SendMessageAsync($"Sadly {Context.Message.MentionedUsers.First().Mention} has Left us.");
		}
	}
}
