using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Hubtel.Api.Data;
using Hubtel.Api.Data.Enums;
using Hubtel.Api.Data.Response;
using Hubtel.Api.Entities;
using Hubtel.Api.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Hubtel.Api.Tests.Services
{
    public class WalletValidationServiceTests
    {
        private readonly WalletContext _context;
        private readonly WalletValidationService _sut;

        public WalletValidationServiceTests()
        {
            var options = new DbContextOptionsBuilder<WalletContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new WalletContext(options);
            _sut = new WalletValidationService(_context);
        }

        #region CanAddMoreWalletsAsync Tests

        [Fact]
        public async Task CanAddMoreWalletsAsync_ShouldReturnTrue_WhenUserHasLessThanFiveWallets()
        {
            var phoneNumber = "0244123456";
            var wallets = new List<Wallet>
            {
                new() { Owner = phoneNumber },
                new() { Owner = phoneNumber },
                new() { Owner = phoneNumber }
            };
            await _context.Wallets.AddRangeAsync(wallets);
            await _context.SaveChangesAsync();

            var result = await _sut.CanAddMoreWalletsAsync(phoneNumber);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task CanAddMoreWalletsAsync_ShouldReturnFalse_WhenUserHasFiveWallets()
        {
            var phoneNumber = "0244123456";
            var wallets = Enumerable.Range(1, 5).Select(_ => new Wallet { Owner = phoneNumber });
            await _context.Wallets.AddRangeAsync(wallets);
            await _context.SaveChangesAsync();

            var result = await _sut.CanAddMoreWalletsAsync(phoneNumber);

            result.Should().BeFalse();
        }

        #endregion

        #region IsAccountNumberUniqueAsync Tests

        [Fact]
        public async Task IsAccountNumberUniqueAsync_ShouldReturnTrue_WhenAccountNumberDoesNotExist()
        {
            var accountNumber = "0244123456";
            var owner = "0244123456";

            var result = await _sut.IsAccountNumberUniqueAsync(accountNumber, owner);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task IsAccountNumberUniqueAsync_ShouldReturnFalse_WhenAccountNumberExists()
        {
            var accountNumber = "0244123456";
            var owner = "0244123456";
            var wallet = new Wallet { AccountNumber = accountNumber, Owner = owner };
            await _context.Wallets.AddAsync(wallet);
            await _context.SaveChangesAsync();

            var result = await _sut.IsAccountNumberUniqueAsync(accountNumber, owner);

            result.Should().BeFalse();
        }

        #endregion

        #region ValidateWallet Tests

        [Fact]
        public void ValidateWallet_ShouldSucceed_WhenWalletIsValid()
        {
            var wallet = new Wallet
            {
                AccountScheme = AccountScheme.visa,
                Type = WalletType.card,
                AccountNumber = "4111111111111111"
            };

            _sut.Invoking(x => x.ValidateWallet(wallet))
                .Should().NotThrow();
        }

        [Fact]
        public void ValidateWallet_ShouldThrow_WhenAccountSchemeIsInvalid()
        {
            var wallet = new Wallet
            {
                AccountScheme = (AccountScheme)999,
                Type = WalletType.card
            };

            _sut.Invoking(x => x.ValidateWallet(wallet))
                .Should().Throw<ArgumentException>()
                .WithMessage(MessageConstants.InvalidAccountScheme);
        }

        [Fact]
        public void ValidateWallet_ShouldThrow_WhenWalletTypeIsInvalid()
        {
            var wallet = new Wallet
            {
                AccountScheme = AccountScheme.visa,
                Type = (WalletType)999
            };

            _sut.Invoking(x => x.ValidateWallet(wallet))
                .Should().Throw<ArgumentException>()
                .WithMessage(MessageConstants.InvalidWalletType);
        }

        [Theory]
        [InlineData("4111111111111111", AccountScheme.visa, true)] // Valid Visa
        [InlineData("5555555555554444", AccountScheme.mastercard, true)] // Valid Mastercard
        [InlineData("2221000000000009", AccountScheme.mastercard, true)] // Valid Mastercard (2-series)
        [InlineData("1111111111111111", AccountScheme.visa, false)] // Invalid Visa
        [InlineData("1111111111111111", AccountScheme.mastercard, false)] // Invalid Mastercard
        [InlineData("411111111111", AccountScheme.visa, false)] // Wrong length
        public void IsValidCardNumberShouldReturnExpectedResult(string cardNumber, AccountScheme scheme, bool expected)
        {
            var result = _sut.IsValidCardNumber(cardNumber, scheme);

            result.Should().Be(expected);
        }

        [Fact]
        public void ValidateWallet_ShouldTruncateCardNumber_WhenTypeIsCard()
        {
            var wallet = new Wallet
            {
                AccountScheme = AccountScheme.visa,
                Type = WalletType.card,
                AccountNumber = "4111111111111111"
            };

            _sut.ValidateWallet(wallet);

            wallet.AccountNumber.Should().Be("411111");
        }

        [Fact]
        public void ValidateWallet_ShouldNotModifyCardNumber_WhenTypeIsMomo()
        {
            var wallet = new Wallet
            {
                AccountScheme = AccountScheme.mtn,
                Type = WalletType.momo,
                AccountNumber = "0244123456"
            };

            _sut.ValidateWallet(wallet);

            wallet.AccountNumber.Should().Be("0244123456");
        }

        #endregion
    }
}