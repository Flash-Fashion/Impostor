﻿using System.Linq;
using System.Threading.Tasks;
using Impostor.Api.Innersloth.Data;
using Impostor.Api.Net;
using Impostor.Api.Net.Messages;
using Impostor.Api.Net.Messages.S2C;
using Impostor.Server.Net.Hazel;

namespace Impostor.Server.Net.State
{
    internal partial class Game
    {
        public ValueTask SendToAllAsync(IMessageWriter writer, LimboStates states = LimboStates.NotLimbo)
        {
            foreach (var connection in GetConnections(x => x.Limbo.HasFlag(states)))
            {
                connection.SendAsync(writer);
            }

            return default;
        }

        public ValueTask SendToAllExceptAsync(IMessageWriter writer, int senderId, LimboStates states = LimboStates.NotLimbo)
        {
            foreach (var connection in GetConnections(x =>
                x.Limbo.HasFlag(states) &&
                x.Client.Id != senderId))
            {
                connection.SendAsync(writer);
            }

            return default;
        }

        public ValueTask SendToAsync(IMessageWriter writer, int id)
        {
            if (TryGetPlayer(id, out var player) && player.Client.Connection is HazelConnection hazelConnection)
            {
                hazelConnection.InnerConnection.SendAsync(writer);
            }

            return default;
        }

        private void WriteRemovePlayerMessage(IMessageWriter message, bool clear, int playerId, DisconnectReason reason)
        {
            Message04RemovePlayerS2C.Serialize(message, clear, Code, playerId, HostId, reason);
        }

        private void WriteJoinedGameMessage(IMessageWriter message, bool clear, IClientPlayer player)
        {
            var playerIds = _players
                .Where(x => x.Value != player)
                .Select(x => x.Key)
                .ToArray();

            Message07JoinedGameS2C.Serialize(message, clear, Code, player.Client.Id, HostId, playerIds);
        }

        private void WriteAlterGameMessage(IMessageWriter message, bool clear, bool isPublic)
        {
            Message10AlterGameS2C.Serialize(message, clear, Code, isPublic);
        }

        private void WriteKickPlayerMessage(IMessageWriter message, bool clear, int playerId, bool isBan)
        {
            Message11KickPlayerS2C.Serialize(message, clear, Code, playerId, isBan);
        }

        private void WriteWaitForHostMessage(IMessageWriter message, bool clear, IClientPlayer player)
        {
            Message12WaitForHostS2C.Serialize(message, clear, Code, player.Client.Id);
        }
    }
}