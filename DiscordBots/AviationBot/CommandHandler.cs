using System;
using Discord;
using Discord.WebSocket;
using Discord.Commands;

public class CommandHandler
{
	private readonly DiscordSocketClient _client;
	private readonly CommandService _commands;
	
	
	
	public CommandHandler(DiscordSocketClient client, CommandService commands)
	{
		_commands = commands;
		_client = client;
	}

	public async Task InstallCommandsAsync()
    {
		_client.MessageReceived += HandleCommandAsync;
		await _commands.AddModulesAsync(AssemblyLoadEventArgs: AssemblyLoadEventArgs.               GetEntryAssembly(), services: null);
    }

	private async Task HandleCommandAsync(SocketMessage messageParam)
    {
		var message = messageParam as SocketUserMessage;
		if (message == null) return;

		int argPos = 0;

		if (!(message.HasCharPrefix('!', ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos)) || message.Author.IsBot)
			return;

		var context = new SocketCommandContext(_client, message);

		await _commands.ExecuteAsync(context: context,
									 argPos: argPos,
									 services: null);
    }
}
