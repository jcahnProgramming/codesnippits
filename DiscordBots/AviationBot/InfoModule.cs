﻿using System;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

public class InfoModule : ModuleBase<SocketCommandContext>
{
	[Command("say")]
	[Summary("Echoes a message.")]
	public Task SayAsync([Remainder][Summary("The text to echo")] string echo)
		=> ReplyAsync(echo);
}

public class SampleModule: ModuleBase<SocketCommandContext>
{
	[Command("square")]
	[Summary("Squares a number.")]
	public async Task SquareAsync(
		[Summary("The number to square.")]
		int num)
    {
		await Context.Channel.SendMessageAsync($"{num}^2 = {Math.Pow(num, 2)}");
    }

	[Command("userinfo")]
	[Summary("Returns info about the current user, or the user parameter, if one passed.")]
	[Alias("user", "whois")]
	public async Task UserInfoAsync(
		[Summary("The (optional) user to get info from")]
		SocketUser user = null)
    {
		var userInfo = user ?? Context.Client.CurrentUser;
		await ReplyAsync($"{userInfo.Username}#{userInfo.Discriminator}");
    }

	[Command("russianflights")]
	[Summary($"Returns information about current Russian Flights")]
	public async Task RussianFlightsAsync([Summary("Gets information about current russian flights in the air above russian airspace.")]
		TrackedFlight flightData)
    {
		await ReplyAsync($"Information Requested: {SavedData.Instance.flights[SavedData.Instance.flights.Count - 1].ToString()}");
	
    }
}
