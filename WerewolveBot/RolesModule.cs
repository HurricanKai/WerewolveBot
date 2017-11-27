using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WerewolveBot
{
	[Group("roles")]
	public class RolesModule : ModuleBase<SocketCommandContext>
	{
		[Command("GetPlayerInfo")]
		[RequireUserPermission(Discord.GuildPermission.Administrator)]
		public Task GetPInfo([Summary("An User Mention")]string UserMention)
		{
			var p = RoleManager.instance.Players.Find(new Predicate<PlayerRole>(((PlayerRole r) => { return (r.Name == Context.Message.MentionedUsers.First().Username && r.Descriminator == Context.Message.MentionedUsers.First().Discriminator); })));
			string n = Environment.NewLine;
			string s = $"Role: {p.GetMainRole().Name}{n}Sub-Role: {p.GetSubRole().Name}";
			return Context.Channel.SendMessageAsync(s);
		}

		[Command("give")]
		[RequireUserPermission(Discord.GuildPermission.Administrator)]
		public Task GiveRole([Summary("The Name of The Role")]string role, [Summary("An User Mention")]string UserMention)
		{
			foreach (var v in RoleManager.MainRoles)
			{
				if (v.Name == role)
					return Task.Run(() => GiveRole(v));
			}

			throw new Exception("Not Found");
		}

		[Command("givesub")]
		[RequireUserPermission(Discord.GuildPermission.Administrator)]
		public Task GiveSubRole([Summary("The Name of The Sub-Role")]string subrole, [Summary("An User Mention")]string UserMention)
		{
			foreach (var v in RoleManager.SubRoles)
			{
				if (v.Name == subrole)
					return Task.Run(() => GiveSubRole(v));
			}

			throw new Exception("Not Found");
		}

		private void GiveSubRole(SubRole r)
		{
			foreach (var v in RoleManager.instance.Players)
				if (v.Name == Context.Message.MentionedUsers.First().Username && v.Descriminator == Context.Message.MentionedUsers.First().Discriminator)
					v.SetSubRole(r);
		}

		private void GiveRole(Role r)
		{
			RoleManager.instance.Players.Add(new PlayerRole(Context.Message.MentionedUsers.First().Discriminator, Context.Message.MentionedUsers.First().Username, r));
		}
	}
}
