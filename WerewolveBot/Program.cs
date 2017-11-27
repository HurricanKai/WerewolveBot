using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WerewolveBot
{
	public class Program
	{
		static void Main(string[] args)
		{
			try
			{
				new Program().MainAsync().GetAwaiter().GetResult();
			}
			catch (Exception ex)
			{
				Console.WriteLine("Bot CRASHED, Exception: \r\n" + ex.Message);
			}
			Console.Read();
		}

		public DiscordSocketClient _client;
		public static CommandService _commands;
		public IServiceProvider _services;

		private async Task MainAsync()
		{
			_client = new DiscordSocketClient();
			_commands = new CommandService();

			_client.Log += Log;
			//TODO: Add Config
			string token = "Mzg0MDM0NzU2MzY4NzkzNjIw.DPs8Jg.LzShAuHJ70Y85wOZdsHLTE4Mqgg"; // Remember to keep this private!

			_services = new ServiceCollection()
			.AddSingleton(_client)
			.AddSingleton(_commands)
			.BuildServiceProvider();

			await InstallCommandsAsync();

			await _client.LoginAsync(TokenType.Bot, token);
			await _client.StartAsync();

			// Block this task until the program is closed.
			await Task.Delay(-1);
		}

		private Task Log(LogMessage msg)
		{
			Console.WriteLine(msg.ToString());
			return Task.Delay(0);
		}

		public async Task InstallCommandsAsync()
		{
			// Hook the MessageReceived Event into our Command Handler
			_client.MessageReceived += HandleCommandAsync;

			//Load all Managers
			SignupManager.Load(SignupManager.file);
			RoleManager.Load();
			RoleManager.LoadRoles();

			// Discover all of the commands in this assembly and load them.
			await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
		}

		private async Task HandleCommandAsync(SocketMessage messageParam)
		{
			char prefix = '~';
			// Don't process the command if it was a System Message
			var message = messageParam as SocketUserMessage;
			if (message == null) return;
			// Create a number to track where the prefix ends and the command begins
			int argPos = 0;
			// Determine if the message is a command, based on if it starts with the prefix or a mention prefix
			if (!(message.HasCharPrefix(prefix, ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos))) return;
			// Create a Command Context
			var context = new SocketCommandContext(_client, message);
			// Execute the command. (result does not indicate a return value, 
			// rather an object stating if the command executed successfully)
			var result = await _commands.ExecuteAsync(context, argPos, _services);
			if (!result.IsSuccess)
				await context.Channel.SendMessageAsync(result.ErrorReason);
		}
	}
}
