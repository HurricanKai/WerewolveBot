using DSharpPlus.Entities;

namespace GoodBot.Commands.Werewolve
{
	internal class Player
	{
		public DiscordEmoji avatar;
		public DiscordUser user;

		public Player(DiscordEmoji avatar, DiscordUser user)
		{
			this.avatar = avatar;
			this.user = user;
		}
	}
}