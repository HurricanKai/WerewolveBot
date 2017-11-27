using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WerewolveBot
{
	public class MainModule : ModuleBase<SocketCommandContext>
	{
		[Command("Help")]
		[Alias("help", "?")]
		[Summary("Show some Help")]
		public Task Help()
		{
			var e = new EmbedBuilder();
			string n = Environment.NewLine;
			string s = "";
			foreach (var v in Program._commands.Commands)
			{
				string aliases = "";
				foreach (var v2 in v.Aliases)
					aliases += v2 + ", ";

				aliases.Remove(aliases.LastIndexOf(",") - 1);

				string parameters = "";
				foreach (var v2 in v.Parameters)
					parameters += " - " + v2.Name + "   ["+ v2.Summary +"]" + n;

				e.AddInlineField($"{v.Name}", $"{v.Summary}{n}{parameters}");
				s += $"{v.Name}{n}{v.Summary}{n}{parameters}";
			}
			return Context.Channel.SendMessageAsync(s/*, false, e.Build()*/);
		}
	}
}
