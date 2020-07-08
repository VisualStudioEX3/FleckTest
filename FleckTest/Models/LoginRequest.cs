using System;
using System.IO;
using System.Runtime.Serialization;

namespace FleckTest.Models
{
    /// <summary>
    /// Login request object.
    /// </summary>
    public class LoginRequest : SerializedObject
    {
        #region Properties
        /// <summary>
        /// <see cref="Guid"/> to former request.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// User name requested by user.
        /// </summary>
        public string UserName { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id"><see cref="Guid"/> to former request.</param>
        /// <param name="userName">User name requested by user.</param>
        public LoginRequest(Guid id, string userName)
        {
            this.Id = id;
            this.UserName = userName;

            this.Serialize(buffer =>
            {
                buffer.Write(this.Id.ToByteArray());
                buffer.Write(this.UserName);
            });
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="data"><see cref="ReadOnlyMemory{byte}"/> that contains a <see cref="LoginRequest"/> in bianary format.</param>
        public LoginRequest(ReadOnlyMemory<byte> data)
        {
            this.Deserialize(data, buffer =>
            {
                this.Id = new Guid(buffer.ReadBytes(16).AsMemory().ToArray());
                this.UserName = buffer.ReadString();
            });
        } 
        #endregion
    }

    //public class LoginRequest2
    //{
    //    ReadOnlyMemory<byte> _serialized;

    //    public readonly Guid id;
    //    public readonly string userName;

    //    public LoginRequest(Guid id, string userName)
    //    {
    //        this.id = id;
    //        this.userName = userName;

    //        var buffer = new BinaryWriter(new MemoryStream());
    //        {
    //            buffer.Write(this.id.ToByteArray());
    //            buffer.Write(this.userName);

    //            this._serialized = new ReadOnlyMemory<byte>((buffer.BaseStream as MemoryStream).GetBuffer());
    //        }
    //    }

    //    public LoginRequest(ReadOnlyMemory<byte> data)
    //    {
    //        try
    //        {
    //            var buffer = new BinaryReader(new MemoryStream(data.ToArray()));
    //            {
    //                this.id = new Guid(buffer.ReadBytes(16).AsMemory().ToArray());
    //                this.userName = buffer.ReadString();
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new SerializationException($"Login request: Error deserializing input data. {ex}");
    //        }

    //        this._serialized = data;
    //    }

    //    public ReadOnlyMemory<byte> GetSerializedData()
    //    {
    //        return this._serialized;
    //    }
    //}
}
