using Discord.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WerewolveBot
{
	[Group("signup")]
	[Alias("Signup")]
	public class SignupModule : ModuleBase<SocketCommandContext>
	{

		[Command]
		[Summary("Register for next Game")]
		public Task Signup()
		{
			var v = new Player(Context.User.Discriminator, this.Context.User.Username);
			if (!SignupManager.instance.Players.Contains(v))
				SignupManager.instance.Players.Add(v);
			else
				return Context.Channel.SendMessageAsync("You are already signed up!");
			return Task.Delay(0);
		}

		[Command("Clear")]
		[Alias("clear")]
		[Summary("Deletes all Players")]
		[RequireUserPermission(Discord.GuildPermission.Administrator)]
		public Task Clear()
		{
			SignupManager.instance.Players.Clear();
			SignupManager.instance.Save(SignupManager.file);
			return Context.Channel.SendMessageAsync("Cleared.");
		}

		[Command("list")]
		[Alias("List")]
		[Summary("List All Signed up Players")]
		public Task List()
		{
			string n = Environment.NewLine;
			string s = "";
			foreach (var v in SignupManager.instance.Players)
				s += v.Name + "#" + v.Descriminator + n;
			if (s == "")
				s = "No Players Registered";
			return Context.Channel.SendMessageAsync(s);
		}

		[Command("GetFormated")]
		[Alias("gf")]
		[Summary("Gets an Formated List of Players")]
		public Task GetFormated()
		{
			string file = SignupManager.file + "formated";
			file = Path.GetFullPath(file);
			File.Create(file).Close();
			foreach (var v in SignupManager.instance.Players)
				File.AppendAllText(file, v.Name + "#" + v.Descriminator);

			Context.Channel.SendFileAsync(file);
			File.Delete(file);

			return Task.Delay(0);
		}

		[Command("GetRaw")]
		[Alias("gr")]
		[Summary("Get Raw Save File")]
		public Task GetRaw()
		{
			return Context.Channel.SendFileAsync(Path.GetFullPath(SignupManager.file));
		}

		[Command("Save")]
		[Alias("s")]
		[Summary("Save Current List")]
		public Task Save()
		{
			SignupManager.instance.Save(SignupManager.file);
			return Context.Channel.SendMessageAsync("Saved.");
		}
		[Command("Load")]
		[Alias("l")]
		[Summary("Load list")]
		public Task Load()
		{
			SignupManager.Load(SignupManager.file);
			return Context.Channel.SendMessageAsync("Loaded.");
		}
	}
}
