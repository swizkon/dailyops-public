using System;
// using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

/*
namespace UnitTestProject2
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {

        }
    }
}
*/

namespace Bank
{

    [TestFixture]
    public class AccountTest
    {
        [Test]
        public void TransferFunds()
        {
            // Arrange
            Account source = new Account();
            source.Deposit(200m);

            Account destination = new Account();
            destination.Deposit(150m);

            // Act
            source.TransferFunds(destination, 100m);

            // Assert
            Assert.That(250m, Is.EqualTo(destination.Balance));
            Assert.AreEqual(100m, source.Balance);
        }
    }

    internal class Account
    {
        public decimal Balance { get; internal set; }

        internal void Deposit(decimal v)
        {
            Balance += v;
        }

        internal void TransferFunds(Account destination, decimal v)
        {
            Balance -= v;
            destination.Deposit(v);
        }
    }
}