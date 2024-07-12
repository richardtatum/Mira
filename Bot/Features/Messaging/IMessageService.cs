using Discord;
using Mira.Features.Shared;

namespace Mira.Features.Messaging;

public interface IMessageService
{
    Task<ulong?> SendAsync(ulong channelId, StreamStatus status, string url, int viewers, TimeSpan duration);
    Task<ulong?> ModifyAsync(ulong messageId, ulong channelId, StreamStatus status, string url, int viewers, TimeSpan duration);
}