using System;
using NUnit.Framework;


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

            // Assert, 
            Assert.That(250m, Is.EqualTo(destination.Balance));
            Assert.AreEqual(100m, source.Balance);
        }

        [Test]
        public void A_deposit_must_be_a_positive_ammount()
        {
            // Arrange
            Random rnd = new Random();
            Account source = new Account();

            var exp = Assert.Throws<ArgumentException>(() =>
            {
                // Act
                source.Deposit(0m - rnd.Next(0, 100));
            });
        }


        [Test]
        public void When_transfer_ammount_is_greater_than_balance_throw()
        {
            // Arrange
            Account source = new Account();
            source.Deposit(10m);

            Account destination = new Account();


            Assert.Throws<InvalidOperationException>(() =>
            {
                // Act
                source.TransferFunds(destination, 100m);
            });
        }

        [Test]
        public void When_transfer_to_a_null_account_throw()
        {
            // Arrange
            Account source = new Account();
            source.Deposit(1000m);

            // Act
            Account destination = null;

            Assert.Throws<ArgumentNullException>(() =>
            {
                // Act
                source.TransferFunds(destination, 100m);
            });
        }

    }

    internal class Account
    {
        public decimal Balance { get; internal set; }

        internal void Deposit(decimal v)
        {
            if (v <= 0)
                throw new ArgumentException(nameof(v));
            Balance += v;
        }

        internal void TransferFunds(Account destination, decimal v)
        {
            if (destination == null)
                throw new ArgumentNullException(nameof(destination));

            if (Balance < v)
                throw new InvalidOperationException("The Balance must be greater that the amount to deposit");

            Balance -= v;
            destination.Deposit(v);
        }
    }
}