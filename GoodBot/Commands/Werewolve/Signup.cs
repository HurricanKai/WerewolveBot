using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using GoodBot.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodBot.Commands.Werewolve
{
	[Group("signup", CanInvokeWithoutSubcommand = true), Aliases("Signup")]
	internal class Signup
	{
		private const string emoji_ok = ":white_check_mark:";
		private const string emoji_not = ":negative_squared_cross_mark:";
		public static bool IsOpen;
		public Dictionary<DiscordMessage, Player> Requests = new Dictionary<DiscordMessage, Player>();
		public List<Player> signups = new List<Player>();
		private Task RequestChecker;
		private bool CheckingEnabled;
		private InteractivityModule interactivity;

		private async void CheckRequests()
		{
			while (CheckingEnabled)
			{
				foreach (var v in Requests)
				{
					var res = await interactivity.CollectReactionsAsync(v.Key);
					int oks = 0, nos = 0;
					var r = v.Key.Reactions;
					foreach (var e in res.Reactions)
						if (e.Key.Name == emoji_ok)
							oks = e.Value;
						else if (e.Key.Name == emoji_not)
							nos = e.Value;

					if (oks > 1)
					{
						signups.Add(v.Value);
						Requests.Remove(v.Key);
					}
					else if (nos > 0)
					{
						Requests.Remove(v.Key);
					}
				}

				Task.Delay(300).Wait();
			}
		}

		[Command("open"), RequireRolesAttribute("GameMaster")]
		public async Task open(CommandContext ctx)
		{
			interactivity = ctx.Dependencies.GetDependency<InteractivityModule>();
			await ctx.RespondAsync(ctx.Guild.EveryoneRole.Mention + " Signup is open Now!");
			IsOpen = true;
			RequestChecker = new Task(CheckRequests);
			CheckingEnabled = true;
			RequestChecker.Start();
		}

		[Command("close"), RequireRolesAttribute("GameMaster")]
		public async Task close(CommandContext ctx)
		{
			await ctx.RespondAsync(ctx.Guild.EveryoneRole.Mention + " Signup is closed Now!");
			IsOpen = false;
			CheckingEnabled = false;
			RequestChecker.Wait();
		}

		//No need for "Command", CanInvokeWithoutSubCommand is hardcoded to this:
		public async Task ExecuteGroupAsync(CommandContext ctx, [Description("Your Avatar")] DiscordEmoji avatar)
		{
			bool isAvatarUsed = false;
			foreach (var v in Requests)
				if (v.Value.avatar == avatar)
					isAvatarUsed = true;
			foreach (var v in signups)
				if (v.avatar == avatar)
					isAvatarUsed = true;

			if (isAvatarUsed)
			{
				await ctx.RespondAsync("Sorry, this Avatar is taken! " + ctx.User.Mention);
			}

			await ctx.RespondAsync("Your request has been send to the Game Masters!");

			//TODO: NO thats so bad
			var channels = await ctx.Guild.GetChannelsAsync();
			DiscordChannel c = null;
			foreach (var v in channels)
			{
				if (v.Name == "game_masters")
				{
					c = v;
					break;
				}
			}
			if (c == null)
				c = await ctx.Guild.CreateChannelAsync("game_masters", DSharpPlus.ChannelType.Text);

			var m = await c.SendMessageAsync(ctx.User.Mention + " wants to Join.");
			Requests.Add(m, new Player(avatar, ctx.User));
			await m.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, emoji_ok));
			await m.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, emoji_not));
		}
	}
}
