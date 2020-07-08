using System;
using System.IO;
using System.Runtime.Serialization;

namespace FleckTest.Models
{
    public abstract class SerializedObject
    {
        ReadOnlyMemory<byte> _serialized;

        public void Serialize(Action<BinaryWriter> onSerialize)
        { 
            var buffer = new BinaryWriter(new MemoryStream());
            {
                onSerialize?.Invoke(buffer);

                this._serialized = new ReadOnlyMemory<byte>((buffer.BaseStream as MemoryStream).GetBuffer());
            }
        }

        public void Deserialize(ReadOnlyMemory<byte> data, Action<BinaryReader> onDeserialize)
        {
            try
            {
                var buffer = new BinaryReader(new MemoryStream(data.ToArray()));
                {
                    onDeserialize?.Invoke(buffer);
                }
            }
            catch (Exception ex)
            {
                throw new SerializationException($"Error deserializing input data. {ex}");
            }

            this._serialized = data;
        }
        public ReadOnlyMemory<byte> GetSerializedData()
        {
            return this._serialized;
        }
    }
}
