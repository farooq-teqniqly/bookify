using Bookify.Domain.Shared;
using FluentAssertions;

namespace Bookify.Domain.Tests.Shared
{
    public class MoneyTests
    {
        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenCurrencyIsNull()
        {
            // Arrange
            decimal amount = 10m;
            Currency? currency = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new Money(amount, currency!));
        }

        [Fact]
        public void IsZero_ReturnsFalse_WhenAmountIsNotZero()
        {
            var money = new Money(10m, Currency.Usd);
            money.IsZero().Should().BeFalse();
        }

        [Fact]
        public void IsZero_ReturnsTrue_WhenAmountIsZero()
        {
            var zeroUsd = Money.Zero(Currency.Usd);
            zeroUsd.IsZero().Should().BeTrue();

            var zeroNone = Money.Zero();
            zeroNone.IsZero().Should().BeTrue();
        }

        [Fact]
        public void OperatorPlus_DifferentCurrency_ThrowsInvalidOperationException()
        {
            var money1 = new Money(100m, Currency.Usd);
            var money2 = new Money(50m, Currency.Eur);

            FluentActions
                .Invoking(() =>
                {
                    var _ = money1 + money2;
                })
                .Should()
                .Throw<InvalidOperationException>();
        }

        [Fact]
        public void OperatorPlus_SameCurrency_AddsAmounts()
        {
            var money1 = new Money(100m, Currency.Usd);
            var money2 = new Money(50m, Currency.Usd);

            var result = money1 + money2;

            result.Amount.Should().Be(150m);
            result.Currency.Should().Be(Currency.Usd);
        }

        [Fact]
        public void Zero_ReturnsMoneyWithZeroAmountAndNoneCurrency()
        {
            var zero = Money.Zero();

            zero.Amount.Should().Be(0m);
        }

        [Fact]
        public void Zero_WithCurrency_ReturnsMoneyWithZeroAmountAndSpecifiedCurrency()
        {
            var zeroUsd = Money.Zero(Currency.Usd);
            var zeroEur = Money.Zero(Currency.Eur);

            zeroUsd.Amount.Should().Be(0m);
            zeroUsd.Currency.Should().Be(Currency.Usd);

            zeroEur.Amount.Should().Be(0m);
            zeroEur.Currency.Should().Be(Currency.Eur);
        }
    }
}
