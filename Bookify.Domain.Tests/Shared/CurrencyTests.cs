using Bookify.Domain.Shared;
using FluentAssertions;

namespace Bookify.Domain.Tests.Shared
{
    public class CurrencyTests
    {
        [Fact]
        public void All_ContainsAllCurrencies()
        {
            var all = Currency.All;
            all.Should().Contain(Currency.Usd);
            all.Should().Contain(Currency.Eur);
            all.Should().Contain(Currency.Cad);
            all.Should().HaveCount(3);
        }

        [Fact]
        public void Cad_HasCorrectCode()
        {
            Currency.Cad.Code.Should().Be("CAD");
        }

        [Fact]
        public void Eur_HasCorrectCode()
        {
            Currency.Eur.Code.Should().Be("EUR");
        }

        [Fact]
        public void FromCode_InvalidCode_ThrowsArgumentException()
        {
            FluentActions
                .Invoking(() => Currency.FromCode("GBP"))
                .Should()
                .Throw<ArgumentException>();
            FluentActions.Invoking(() => Currency.FromCode("")).Should().Throw<ArgumentException>();
            FluentActions
                .Invoking(() => Currency.FromCode(null!))
                .Should()
                .Throw<ArgumentException>();
        }

        [Theory]
        [InlineData("USD", "USD")]
        [InlineData("EUR", "EUR")]
        [InlineData("CAD", "CAD")]
        public void FromCode_ReturnsCorrectCurrency(string code, string expected)
        {
            var currency = Currency.FromCode(code);
            currency.Code.Should().Be(expected);
        }

        [Fact]
        public void None_HasEmptyCode()
        {
            var none =
                typeof(Currency)
                    .GetField(
                        "None",
                        System.Reflection.BindingFlags.NonPublic
                            | System.Reflection.BindingFlags.Static
                    )!
                    .GetValue(null) as Currency;
            none.Should().NotBeNull();
            none!.Code.Should().Be(string.Empty);
        }

        [Fact]
        public void Usd_HasCorrectCode()
        {
            Currency.Usd.Code.Should().Be("USD");
        }
    }
}
