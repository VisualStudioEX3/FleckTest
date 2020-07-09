using System;

namespace FleckTest.Models
{
    /// <summary>
    /// Server message data.
    /// </summary>
    public class ServerMessage : SerializedObject
    {
        #region Properties
        /// <summary>
        /// User data that sends the message.
        /// </summary>
        public UserData User { get; private set; }

        /// <summary>
        /// Date and time when the message was sended.
        /// </summary>
        public DateTime TimeStamp { get; private set; }

        /// <summary>
        /// Message sended.
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Is a server announcement instead of user message?
        /// </summary>
        public bool IsAServerAnouncement { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructors.
        /// </summary>
        /// <param name="user">User data that sends the message.</param>
        /// <param name="message">Message sended by user.</param>
        /// <param name="isAServerAnouncement">Is a server announcement instead of user message?</param>
        public ServerMessage(UserData user, string message, bool isAServerAnouncement)
        {
            this.User = user;
            this.TimeStamp = DateTime.Now;
            this.Message = message;
            this.IsAServerAnouncement = isAServerAnouncement;

            this.Serialize(buffer =>
            {
                byte[] userData = this.User.GetSerializedData().ToArray();

                buffer.Write(userData.Length);
                buffer.Write(userData);
                buffer.Write(this.TimeStamp.Ticks);
                buffer.Write(this.Message);
                buffer.Write(this.IsAServerAnouncement);
            });
        }

        /// <summary>
        /// Constructors.
        /// </summary>
        /// <param name="data"><see cref="ReadOnlyMemory{byte}"/> that contains a <see cref="ServerMessage"/> in binary format.</param>
        public ServerMessage(ReadOnlyMemory<byte> data)
        {
            this.Deserialize(data, buffer =>
            {
                this.User = new UserData(buffer.ReadBytes(buffer.ReadInt32()).AsMemory());
                this.TimeStamp = new DateTime(buffer.ReadInt64());
                this.Message = buffer.ReadString();
                this.IsAServerAnouncement = buffer.ReadBoolean();
            });
        } 
        #endregion
    }
}
