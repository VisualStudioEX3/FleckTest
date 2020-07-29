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

        #region Operators
        public static bool operator ==(UserData a, UserData b)
        {
            if (a == null || b == null) return false;

            return a.Name == b.Name &&
                   a.Color == b.Color &&
                   a.CreationTime == b.CreationTime;
        }

        public static bool operator !=(UserData a, UserData b)
        {
            return !(a == b);
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates new instance.
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
        /// Creates new instance from a previous serialized one.
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

        #region Methods & Functions
        public override bool Equals(object obj)
        {
            if (obj == null && !(obj is UserData)) return false;

            return this == (UserData)obj;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode(); // Keep using the SerializedObject hash code.
        }
        #endregion
    }
}
