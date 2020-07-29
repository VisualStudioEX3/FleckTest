using System;
using NUnit.Framework;
using FleckTest.Models;

namespace FleckTest.Tests
{
    /// <summary>
    /// Test serialization and deserialization behaviour on serialized models.
    /// </summary>
    public class SerializedObjectTest
    {
        #region Constants
        const string USERNAME = "EX3";
        #endregion

        #region Internal vars
        Guid _id;
        ConsoleColorScheme _colorScheme;
        UserData _userData;
        #endregion

        #region Setup
        [SetUp]
        public void Setup()
        {
            this._id = Guid.NewGuid();
            this._colorScheme = new ConsoleColorScheme(ConsoleColor.Black, ConsoleColor.White);
            this._userData = new UserData(SerializedObjectTest.USERNAME, this._colorScheme);
        } 
        #endregion

        #region Tests
        [Test]
        public void TestLoginRequest()
        {
            var req1 = new LoginRequest(this._id, SerializedObjectTest.USERNAME);
            var req2 = new LoginRequest(req1.GetSerializedData());

            Assert.AreEqual(req1, req2);
        }

        [Test]
        public void TestServerMessage()
        {
            var msg1 = new ServerMessage(this._userData, "Hello, world!", true);
            var msg2 = new ServerMessage(msg1.GetSerializedData());

            Assert.AreEqual(msg1, msg2);
        }

        [Test]
        public void TestUserData()
        {
            var usr1 = new UserData(SerializedObjectTest.USERNAME, this._colorScheme); // Force to check this constructor in tests.
            var usr2 = new UserData(usr1.GetSerializedData());

            Assert.AreEqual(usr1, usr2);
        } 
        #endregion
    }
}
