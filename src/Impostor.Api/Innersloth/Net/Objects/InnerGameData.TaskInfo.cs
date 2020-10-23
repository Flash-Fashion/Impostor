﻿using Impostor.Api.Net.Messages;

namespace Impostor.Api.Innersloth.Net.Objects
{
    public partial class InnerGameData
    {
        public class TaskInfo
        {
            public uint Id { get; internal set; }

            public bool Complete { get; internal set; }

            public void Serialize(IMessageWriter writer)
            {
                writer.WritePacked(Id);
                writer.Write(Complete);
            }

            public void Deserialize(IMessageReader reader)
            {
                this.Id = reader.ReadPackedUInt32();
                this.Complete = reader.ReadBoolean();
            }
        }
    }
}