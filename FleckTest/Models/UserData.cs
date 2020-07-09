using System;

namespace FleckTest.Models
{
    /// <summary>
    /// User data information.
    /// </summary>
    public class UserData : SerializedObject
    {
        #region Properties
        /// <summary>
        /// User name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// <see cref="ConsoleColorScheme"/> to represent messages in console.
        /// </summary>
        public ConsoleColorScheme Color { get; private set; }

        /// <summary>
        /// Date and time when the user sessuib is created.
        /// </summary>
        public DateTime CreationTime { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">User name.</param>
        /// <param name="color"><see cref="ConsoleColorScheme"/> to use with this user messages.</param>
        public UserData(string name, ConsoleColorScheme color)
        {
            this.Name = name;
            this.Color = color;
            this.CreationTime = DateTime.Now;

            this.Serialize(buffer =>
            {
                buffer.Write(this.Name);
                buffer.Write((byte)this.Color.text);
                buffer.Write((byte)this.Color.background);
                buffer.Write(this.CreationTime.Ticks);
            });
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="data"><see cref="ReadOnlyMemory{byte}"/> that contains a <see cref="UserData"/> in binary format.</param>
        public UserData(ReadOnlyMemory<byte> data)
        {
            this.Deserialize(data, buffer =>
            {
                this.Name = buffer.ReadString();
                this.Color = new ConsoleColorScheme((ConsoleColor)buffer.ReadByte(), (ConsoleColor)buffer.ReadByte());
                this.CreationTime = new DateTime(buffer.ReadInt64());
            });
        } 
        #endregion
    }
}
