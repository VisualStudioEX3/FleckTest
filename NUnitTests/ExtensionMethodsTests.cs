using System;
using NUnit.Framework;
using FleckTest.Extensions;

namespace FleckTest.Tests
{
    /// <summary>
    /// Tests all extension methods.
    /// </summary>
    public class ExtensionMethodsTests
    {
        #region Constants
        const string TEST_STRING = "Hello, World!";
        const int TEST_INTEGER = 1234567890;
        #endregion

        #region Internal vars
        byte[] _testStringBytes;
        ReadOnlyMemory<byte> _testStringMemory;
        byte[] _testIntBytes;
        ReadOnlyMemory<byte> _testIntMemory;
        #endregion

        #region Setup
        [SetUp]
        public void Setup()
        {
            this._testStringBytes = new byte[] { 72, 101, 108, 108, 111, 44, 32, 87, 111, 114, 108, 100, 33 }; // "Hello, World!" in byte array format (UTF8 encoded).
            this._testStringMemory = this._testStringBytes.AsMemory();
            this._testIntBytes = new byte[] { 210, 2, 150, 73 }; // 1234567890 in byte array format.
            this._testIntMemory = this._testIntBytes.AsMemory();
        }
        #endregion

        #region Tests
        [Test]
        public void TestStringToBytes()
        {
            ReadOnlyMemory<byte> buffer = ExtensionMethodsTests.TEST_STRING.AsMemoryByte();

            Assert.True(this._testStringMemory.Compare(buffer));
        }

        [Test]
        public void TestBytesToString()
        {
            string str = this._testStringBytes.GetString();

            Assert.AreEqual(ExtensionMethodsTests.TEST_STRING, str);
        }

        [Test]
        public void TestMemoryToString()
        {
            string str = this._testStringMemory.GetString();

            Assert.AreEqual(ExtensionMethodsTests.TEST_STRING, str);
        }

        [Test]
        public void TestIntToBytes()
        {
            byte[] buffer = ExtensionMethodsTests.TEST_INTEGER.ToByteArray();

            Assert.True(this._testIntBytes.Compare(buffer));
        }

        [Test]
        public void TestBytesToInt()
        {
            int value = this._testIntBytes.GetInt();

            Assert.AreEqual(ExtensionMethodsTests.TEST_INTEGER, value);
        }

        [Test]
        public void TestMemoryToInt()
        {
            int value = this._testIntMemory.GetInt();

            Assert.AreEqual(ExtensionMethodsTests.TEST_INTEGER, value);
        }

        [Test]
        public void TestCompareByteArrayToByteArray()
        {
            byte[] array = new byte[] { 0, 1, 2 };

            Assert.True(array.Compare(array));
        }

        [Test]
        public void TestCompareByteArrayToByteArrayFail()
        {
            byte[] a = new byte[] { 0, 1, 2 };
            byte[] b = new byte[] { 0, 1, 2, 3 };

            Assert.False(a.Compare(b));
        }

        [Test]
        public void TestCompareByteArrayToMemory()
        {
            byte[] array = new byte[] { 0, 1, 2 };
            ReadOnlyMemory<byte> mem = array.AsMemory();

            Assert.True(array.Compare(mem));
        }

        [Test]
        public void TestCompareByteArrayToMemoryFail()
        {
            byte[] a = new byte[] { 0, 1, 2 };
            ReadOnlyMemory<byte> b = new byte[] { 0, 1, 2, 3 };

            Assert.False(a.Compare(b));
        }

        [Test]
        public void TestCompareMemoryToByteArray()
        {
            byte[] array = new byte[] { 0, 1, 2 };
            ReadOnlyMemory<byte> mem = array.AsMemory();

            Assert.True(mem.Compare(array));
        }

        [Test]
        public void TestCompareMemoryToByteArrayFail()
        {
            byte[] a = new byte[] { 0, 1, 2 };
            ReadOnlyMemory<byte> b = new byte[] { 0, 1, 2, 3 };

            Assert.False(b.Compare(a));
        }

        [Test]
        public void TestCompareMemoryToMemory()
        {
            ReadOnlyMemory<byte> mem = new byte[] { 0, 1, 2 }; ;

            Assert.True(mem.Compare(mem));
        }

        [Test]
        public void TestCompareMemoryToMemoryFail()
        {
            ReadOnlyMemory<byte> a = new byte[] { 0, 1, 2 };
            ReadOnlyMemory<byte> b = new byte[] { 0, 1, 2, 3 };

            Assert.False(a.Compare(b));
        }
        #endregion
    }
}
