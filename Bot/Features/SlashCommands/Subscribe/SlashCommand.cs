using Discord;
using Discord.WebSocket;
using Mira.Features.SlashCommands.Subscribe.Models;
using Mira.Features.SlashCommands.Subscribe.Repositories;
using Mira.Interfaces;

namespace Mira.Features.SlashCommands.Subscribe;

public class SlashCommand(QueryRepository queryRepository, CommandRepository commandRepository)
    : ISlashCommand, ISelectable
{
    public string Name => "subscribe";
    private const string CustomId = "subscribe";
    private const string StreamKeyOptionName = "streamkey";

    public Task<SlashCommandProperties> BuildCommandAsync() => Task.FromResult(
        new SlashCommandBuilder()
            .WithName(Name)
            .WithDescription("Be notified when a user begins streaming.")
            .AddOptions(
                new SlashCommandOptionBuilder()
                    .WithName(StreamKeyOptionName)
                    .WithDescription("The key the stream uses. This is often the streamers username.")
                    .WithRequired(true)
                    .WithType(ApplicationCommandOptionType.String)
            )
            .Build());

    public async Task RespondAsync(SocketSlashCommand command)
    {
        var streamKey = command.Data.Options.First(x => x.Name == StreamKeyOptionName).Value.ToString();
        if (string.IsNullOrWhiteSpace(streamKey))
        {
            // Return failure message
            return;
        }
        
        var channelId = command.ChannelId;
        var guildId = command.GuildId;
        if (channelId is null || guildId is null)
        {
            // Failure message
            return;
        }

        var subscription = new Subscription
        {
            StreamKey = streamKey,
            Channel = channelId,
            CreatedBy = command.User.Id
        };
        
        var subscriptionId = await commandRepository.AddSubscription(subscription);
        var hosts = await queryRepository.GetHostsAsync(guildId.Value);
        var hostOptions = hosts
            .Select(host => new SelectMenuOptionBuilder(host.Url, host.Id.ToString(), $"Create notification for {host.Url}/{streamKey}"))
            .ToList();

        var component = new ComponentBuilder()
            .WithSelectMenu($"{CustomId}-{subscriptionId}", hostOptions, $"Where will {streamKey} stream?")
            .Build();
        
        await command.RespondAsync("Select a host:", components: component, ephemeral: true);
    }

    public bool HandlesComponent(SocketMessageComponent component) => component.Data.CustomId.Split("-").FirstOrDefault() == CustomId;

    public async Task RespondAsync(SocketMessageComponent component)
    {
        var optionValue = component.Data.CustomId.Split("-").LastOrDefault();
        if (!int.TryParse(optionValue, out var subscriptionId))
        {
            // Log error
            return;
        }

        await component.DeferAsync(ephemeral: true);
        var record = await queryRepository.GetSubscriptionAsync(subscriptionId);
        if (record?.Id is null)
        {
            return;
        }
        
        if (!int.TryParse(component.Data.Values.FirstOrDefault(), out var hostId))
        {
            return;
        }

        var host = await queryRepository.GetHostAsync(hostId);
        var url = $"{host.Url}/{record.StreamKey}";

        await commandRepository.UpdateSubscription(record.Id.Value, hostId);

        await component.ModifyOriginalResponseAsync(message =>
        {
            message.Content = $"Subscription created! Url: {url}";
            message.Components = new ComponentBuilder()
                .WithButton("Watch now!", style: ButtonStyle.Link, url: url)
                .Build();
        });
    }
}