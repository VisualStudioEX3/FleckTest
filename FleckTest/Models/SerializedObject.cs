using System;
using System.IO;
using System.Runtime.Serialization;

namespace FleckTest.Models
{
    /// <summary>
    /// Implements a base for a serializable objects.
    /// </summary>
    public abstract class SerializedObject
    {
        #region Internal vars
        ReadOnlyMemory<byte> _serialized;
        #endregion

        #region Operators
        public static bool operator ==(SerializedObject a, SerializedObject b)
        {
            return a._serialized.GetHashCode() == b._serialized.GetHashCode();
        }

        public static bool operator !=(SerializedObject a, SerializedObject b)
        {
            return !(a == b);
        }
        #endregion

        #region Methods & Functions
        /// <summary>
        /// Serialize data.
        /// </summary>
        /// <param name="onSerialize">Event where implements the serialization logic.</param>
        public void Serialize(Action<BinaryWriter> onSerialize)
        {
            var buffer = new BinaryWriter(new MemoryStream());
            {
                onSerialize?.Invoke(buffer);

                this._serialized = new ReadOnlyMemory<byte>((buffer.BaseStream as MemoryStream).GetBuffer());
            }
        }

        /// <summary>
        /// Deserialize data.
        /// </summary>
        /// <param name="data">Data deserialize.</param>
        /// <param name="onDeserialize">Event where implements the deserialization logic.</param>
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

        public override bool Equals(object obj)
        {
            if (!(obj is SerializedObject)) return false;

            return this == (SerializedObject)obj;
        }

        public override int GetHashCode()
        {
            return this._serialized.GetHashCode();
        }
        #endregion
    }
}
