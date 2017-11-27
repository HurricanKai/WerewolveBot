namespace WerewolveBot
{
	using System;
	using System.Net;
	using System.Collections.Generic;

	using Newtonsoft.Json;
	using J = Newtonsoft.Json.JsonPropertyAttribute;
	using System.IO;

	public class Role
	{
		[J("Name")] public string Name { get; set; }
		[J("Description")] public string Description { get; set; }
		public static Role FromJson(string json) => JsonConvert.DeserializeObject<Role>(json, Converter.Settings);
	}

	public class SubRoleJSON
	{
		[J("Name")] public string Name { get; set; }
		[J("mainRole")] public string mainRole { get; set; }
		[J("Description")] public string Description { get; set; }
		public static SubRoleJSON FromJson(string json) => JsonConvert.DeserializeObject<SubRoleJSON>(json, Converter.Settings);
	}

	public class SubRole
	{
		public static SubRole FromSubRoleJSON(SubRoleJSON srj)
		{
			foreach (var v in RoleManager.MainRoles)
			{
				if (v.Name == srj.mainRole)
					return new SubRole()
					{
						Name = srj.Name,
						mainRole = v,
						Description = srj.Description
					};
			}

			throw new ArgumentOutOfRangeException();
		}
		public string Name { get; set; }
		public Role mainRole { get; set; }
		public string Description { get; set; }
	}

	public partial class RoleManager
	{
		public static List<Role> MainRoles = new List<Role>();
		public static List<SubRole> SubRoles = new List<SubRole>();

		public static Role defaultRole = new Role() { Name = "Default", Description = "DEFAULT Description" };
		public static SubRole defaultsRole = new SubRole() { Name = "None", mainRole = defaultRole, Description = "DEFAULT" };

		public static void LoadRoles(string file = "./roles/", string subfile = "subroles/")
		{
			file = Path.GetFullPath(file);
			foreach (var v in Directory.EnumerateFiles(file, "*.role", SearchOption.TopDirectoryOnly))
			{
				MainRoles.Add(Role.FromJson(File.ReadAllText(v)));
			}
			foreach (var v in Directory.EnumerateFiles(file + subfile, "*.role", SearchOption.TopDirectoryOnly))
			{
				SubRoles.Add(SubRole.FromSubRoleJSON(SubRoleJSON.FromJson(File.ReadAllText(v))));
			}
		}
	}

	public partial class PlayerRole : Player
	{
		[J("Role")] private string Role { get; set; }

		public Role GetMainRole()
		{
			foreach (var v in RoleManager.MainRoles)
				if (v.Name == Role)
					return v;
			return RoleManager.defaultRole;
		}

		public void SetMainRole(Role role)
		{
			Role = role.Name;
		}

		[J("SubRole")] private string SubRole { get; set; }

		public SubRole GetSubRole()
		{
			foreach (var v in RoleManager.SubRoles)
				if (v.Name == SubRole)
					return v;
			return RoleManager.defaultsRole;
		}

		public void SetSubRole(SubRole role)
		{
			SubRole = role.Name;
		}
	}
}