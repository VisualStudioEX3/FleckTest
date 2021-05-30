using System;

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

        #region Operators
        public static bool operator ==(LoginRequest a, LoginRequest b)
        {
            return a.Id == b.Id &&
                   a.UserName == b.UserName;
        }

        public static bool operator !=(LoginRequest a, LoginRequest b)
        {
            return !(a == b);
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates new instance.
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
        /// Creates new instance from a previous serialized one.
        /// </summary>
        /// <param name="data"><see cref="ReadOnlyMemory{byte}"/> that contains a <see cref="LoginRequest"/> in bianary format.</param>
        public LoginRequest(ReadOnlyMemory<byte> data)
        {
            this.Deserialize(data, buffer =>
            {
                this.Id = new Guid(buffer.ReadBytes(16));
                this.UserName = buffer.ReadString();
            });
        }
        #endregion

        #region Methods & Functions
        public override bool Equals(object obj)
        {
            if (obj == null && !(obj is LoginRequest)) return false;

            return this == (LoginRequest)obj;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode(); // Keep using the SerializedObject hash code.
        }
        #endregion
    }
}
