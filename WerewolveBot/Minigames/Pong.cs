
using Discord;
using Discord.Commands;
using Discord.Rest;
using Discord.Net;
using Discord.Net.Rest;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace WerewolveBot.Minigames
{
	public class Pong : ModuleBase<SocketCommandContext>
	{
		//Emoji Unicode found on: https://emojipedia.org/

		public IEmote EmojiDown1 = new Emoji("⬇");
		//public IEmote EmojiDown1 = new Emoji(":arrow_down:");
		public IEmote EmojiUp1 = new Emoji("⬆");
		//public IEmote EmojiUp1 = new Emoji(":arrow_up:");

		public IEmote EmojiDown2 = new Emoji("🔽");
		//public IEmote EmojiDown2 = new Emoji(":arrow_down_small:");
		public IEmote EmojiUp2 = new Emoji("🔼");
		//public IEmote EmojiUp2 = new Emoji(":arrow_up_small:");


		public IEmote EmojiExit = new Emoji("⏹");
		//public IEmote EmojiExit = new Emoji(":stop_button:");

		public ulong channel;
		public List<Point> player1;
		public List<Point> player2;
		public Point ballposition;
		public Point ballVelocity;
		private RestUserMessage message;
		public Task runner;
		public bool Run;
		[Command("PlayPong")]
		[Summary("Trying to start a game of pong!")]
		public async Task Start()
		{
			foreach (var v in Context.Guild.Channels)
			{
				if (v.Name == "Pong")
				{
					await Context.Channel.SendMessageAsync("Game Already Running.");
					return;
				}
			}

			OverwritePermissions allownothingbutreactions = new OverwritePermissions(
				PermValue.Deny,
				PermValue.Deny,
				PermValue.Allow,
				PermValue.Deny,
				PermValue.Deny,
				PermValue.Deny,
				PermValue.Deny,
				PermValue.Deny,
				PermValue.Deny,
				PermValue.Deny,
				PermValue.Deny,
				PermValue.Deny,
				PermValue.Deny,
				PermValue.Deny,
				PermValue.Deny,
				PermValue.Deny,
				PermValue.Deny,
				PermValue.Deny,
				PermValue.Deny,
				PermValue.Deny
				);
			OverwritePermissions alloweverything = new OverwritePermissions(
					PermValue.Allow, PermValue.Allow, PermValue.Allow, PermValue.Allow, PermValue.Allow, PermValue.Allow, PermValue.Allow, PermValue.Allow, PermValue.Allow, PermValue.Allow, PermValue.Allow, PermValue.Allow, PermValue.Allow, PermValue.Allow, PermValue.Allow, PermValue.Allow, PermValue.Allow, PermValue.Allow, PermValue.Allow
				);

			var channelref = await Context.Guild.CreateTextChannelAsync("Pong");
			channel = channelref.Id;
			await channelref.AddPermissionOverwriteAsync(Context.Client.CurrentUser, alloweverything);
			await channelref.AddPermissionOverwriteAsync(Context.User, allownothingbutreactions);
			ballposition = new Point(5, 5);
			ballVelocity = new Point(-1, 0);

			message = await channelref.SendMessageAsync("Pong Starting Soon...");

			/* Field:
			0, 0 - 0, 11
			  |		   |
			 11, 0 - 11, 11
			*/

			player1 = new List<Point>();
			player1.Add(new Point(1, 4));
			player1.Add(new Point(1, 5));
			player1.Add(new Point(1, 6));

			player2 = new List<Point>();
			player2.Add(new Point(10, 4));
			player2.Add(new Point(10, 5));
			player2.Add(new Point(10, 6));

			runner = new Task(Runner);
			Run = true;
			runner.Start();
		}

		private async void Runner()
		{
			while (Run)
			{
				try
				{
					await Update();
					await Draw();
				}
				catch
				{ /* ignore */}
				Task.Delay(2000).Wait();
			}
		}

		public async Task End()
		{
			Run = false;
			runner.Wait();
			var channelref = Context.Guild.GetTextChannel(channel);
			await channelref.DeleteAsync();
		}

		public async Task Update()
		{
			#region Controlls

			if (message.Reactions.TryGetValue(EmojiUp1, out ReactionMetadata value1))
			{
				if (value1.ReactionCount > 1)
				{
					for (int v = 0; v < player1.Count; v++)
						player1[v] = new Point(player1[v].X, player1[v].Y - 1);
				}
			}
			if (message.Reactions.TryGetValue(EmojiUp2, out ReactionMetadata value2))
			{
				if (value2.ReactionCount > 1)
				{
					for (int v = 0; v < player2.Count; v++)
						player2[v] = new Point(player2[v].X, player2[v].Y - 1);
				}
			}

			if (message.Reactions.TryGetValue(EmojiDown1, out ReactionMetadata value3))
			{
				if (value3.ReactionCount > 1)
				{
					for (int v = 0; v < player1.Count; v++)
						player1[v] = new Point(player1[v].X, player1[v].Y + 1);
				}
			}

			if (message.Reactions.TryGetValue(EmojiDown2, out ReactionMetadata value4))
			{
				if (value4.ReactionCount > 1)
				{
					for (int v = 0; v < player2.Count; v++)
						player2[v] = new Point(player2[v].X, player2[v].Y + 1);
				}
			}

			if (message.Reactions.TryGetValue(EmojiExit, out ReactionMetadata value))
			{
				if (value.ReactionCount > 1)
				{
					await End();
				}
			}
			#endregion

			var nextballposition = new Point(ballVelocity.X + ballposition.X, ballVelocity.Y + ballposition.Y);
			if (player1.Contains(nextballposition) || player2.Contains(nextballposition))
			{
				ballVelocity = new Point(ballVelocity.X * -1, ballVelocity.Y * -1);
				ballposition = new Point(ballVelocity.X + ballposition.X, ballVelocity.Y + ballposition.Y);
			}
			else
				ballposition = nextballposition;

			try
			{
				message.RemoveAllReactionsAsync(RequestOptions.Default).Wait();
			}
			catch (RateLimitedException rle)
			{
				// Ignore
			}
			//TODO: Controlls per Reactions

			await message.AddReactionAsync(EmojiUp1, RequestOptions.Default);
			await message.AddReactionAsync(EmojiDown1, RequestOptions.Default);
			await message.AddReactionAsync(EmojiUp2, RequestOptions.Default);
			await message.AddReactionAsync(EmojiDown2, RequestOptions.Default);
			await message.AddReactionAsync(EmojiExit, RequestOptions.Default);
		}

		public Task Draw()
		{
			/* Field:
			 0, 0 - 0, 11
			  |		   |
			 11, 0 - 11, 11
			 */
			string current = "";
			EmbedBuilder b = new EmbedBuilder();
			List<string> Lines = new List<string>();
			for (int y = 0; y <= 11; y++)
			{
				current += "|";
				for (int x = 0; x <= 11; x++)
				{
					var point = new Point(x, y);
					if (player1.Contains(point) || player2.Contains(point))
						current += "°|°";
					else if (ballposition == point)
						current += "°·°";
					else
						current += "°°°";
				}
				current += "|";
				current += Environment.NewLine;
			}
			b.AddField("--- Pong ---", current);
			return message.ModifyAsync((MessageProperties p) =>
			{
				p.Content = "";
				p.Embed = b.Build();
			});
		}
	}
}
