using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WerewolveBot
{
	[Group("dn")]
	[Alias("DN")]
	public class DayNightCircle : ModuleBase<SocketCommandContext>
	{
		public static Task Timer;
		public static DateTime dayStart;
		public static DateTime nightStart;
		public static bool Run;

		public static bool IsDay;

		[Command("current")]
		[Alias("Current")]
		[Summary("Shows Current Time")]
		public Task ShowCurrent()
		{
			return Context.Channel.SendMessageAsync("Current Time: " + DateTime.UtcNow.ToString("HH:mm"));
		}

		[Command("showday")]
		[Alias("dt")]
		[Summary("Shows Day Time")]
		public Task ShowDay()
		{
			return Context.Channel.SendMessageAsync("Day Time: " + dayStart.ToString("HH:mm") + " - " + (DateTime.UtcNow - dayStart).TotalSeconds + " Seconds until Day");
		}

		[Command("shownight")]
		[Alias("nt")]
		[Summary("Shows Night Time")]
		public Task ShowNight()
		{
			return Context.Channel.SendMessageAsync("Night Time: " + nightStart.ToString("HH:mm") + " - " + (DateTime.UtcNow - nightStart).TotalSeconds + " Seconds until Night");
		}



		[Command("StartTimer")]
		[Alias("start")]
		[Summary("Starting The Timer")]
		[RequireUserPermission(Discord.GuildPermission.Administrator)]
		public Task StartT()
		{
			//TODO: Move to Config
#if DEBUG
			dayStart = DateTime.UtcNow + new TimeSpan(0, 0, 10);
			dayStart = DateTime.UtcNow + new TimeSpan(0, 0, 30);
#endif


			Run = true;
			Timer = new Task(() =>
			{
				do
				{
					var now = DateTime.UtcNow;
					if (now - dayStart < TimeSpan.FromSeconds(2) && !IsDay)
						DoDay(Context);
					else if (now - nightStart < TimeSpan.FromSeconds(2) && IsDay)
						DoNight(Context);
					else
						Thread.Sleep(500);
				}
				while (Run);
			});
			Timer.Start();
			return Task.Delay(0);
		}

		[Command("StopTimer")]
		[Alias("stop")]
		[Summary("Stoping The Timer")]
		[RequireUserPermission(Discord.GuildPermission.Administrator)]
		public Task StopT()
		{
			Run = false;
			return Context.Channel.SendMessageAsync("Ended Timer, will end in next 10 seconds");

		}


		private void DoNight(SocketCommandContext context)
		{
			IsDay = false;
			context.Guild.DefaultChannel.SendMessageAsync("Ooouh Shady, Its Night now!");
		}

		private void DoDay(SocketCommandContext context)
		{
			IsDay = true;
			context.Guild.DefaultChannel.SendMessageAsync("Good Morning Left over Friends! Who has left us today?");
		}
	}
}
