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

	public partial class Player
	{
		[J("id")] public string Descriminator { get; set; }
		[J("name")] public string Name { get; set; }

		public Player()
		{ }

		public Player(string id, string name)
		{
			this.Descriminator = id;
			this.Name = name;
		}
	}

	public partial class Player
	{
		public static Player FromJson(string json) => JsonConvert.DeserializeObject<Player>(json, Converter.Settings);
	}

	public partial class PlayerRole
	{
		public PlayerRole(string id, string name, Role r) : base(id, name)
		{
			this.SetMainRole(r);
			this.Name = name;
			this.Descriminator = id;
		}

		public static PlayerRole FromJson(string json) => JsonConvert.DeserializeObject<PlayerRole>(json, Converter.Settings);
	}

	public static class Serialize
	{
		public static string ToJson(this Player self) => JsonConvert.SerializeObject(self, Converter.Settings);
		public static string ToJson(this SignupManager self) => JsonConvert.SerializeObject(self, Converter.Settings);
		public static string ToJson(this RoleManager self) => JsonConvert.SerializeObject(self, Converter.Settings);
	}

	public class Converter
	{
		public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
		{
			MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
			DateParseHandling = DateParseHandling.None,
		};
	}

	public partial class SignupManager
	{
		public static SignupManager FromJson(string json) => JsonConvert.DeserializeObject<SignupManager>(json, Converter.Settings);
	}
	public partial class RoleManager
	{
		public static RoleManager FromJson(string json) => JsonConvert.DeserializeObject<RoleManager>(json, Converter.Settings);
	}
}
