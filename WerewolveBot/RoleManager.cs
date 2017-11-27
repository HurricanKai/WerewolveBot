using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WerewolveBot
{
	using System;
	using System.Net;
	using System.Collections.Generic;

	using Newtonsoft.Json;
	using J = Newtonsoft.Json.JsonPropertyAttribute;
	using System.IO;
	using System.Reflection;

	public partial class RoleManager
	{
		public const string file = "./roles.json";
		public static RoleManager instance { get; set; }
		[J("Players")] public List<PlayerRole> Players { get; set; }

		public static void Load(string file = RoleManager.file)
		{
			file = Path.GetFullPath(file);
			if (!File.Exists(file))
				File.Create(file).Close();
			instance = RoleManager.FromJson(File.ReadAllText(file));
		}
		public void Save(string file = RoleManager.file)
		{
			file = Path.GetFullPath(file);
			File.WriteAllText(file, this.ToJson());
		}

	}
}
