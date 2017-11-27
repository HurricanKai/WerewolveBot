using System;
using System.Collections.Generic;
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

	public partial class SignupManager
	{
		public const string file = "./signups";
		public static SignupManager instance { get; set; }
		[J("Players")] public List<Player> Players { get; set; }

		public static void Load(string file = file)
		{
			file = Path.GetFullPath(file);
			if (!File.Exists(file))
				File.Create(file).Close();
			instance = SignupManager.FromJson(File.ReadAllText(file));
		}
		public void Save(string file = file)
		{
			file = Path.GetFullPath(file);
			File.WriteAllText(file, this.ToJson());
		}
	}
}
