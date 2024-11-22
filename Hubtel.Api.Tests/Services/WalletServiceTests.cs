using System;
using System.Threading.Tasks;
using FluentAssertions;
using Hubtel.Api.Contracts;
using Hubtel.Api.Data;
using Hubtel.Api.Data.Enums;
using Hubtel.Api.Data.Request;
using Hubtel.Api.Data.Response;
using Hubtel.Api.Entities;
using Hubtel.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Hubtel.Api.Tests.Services
{
    public class WalletServiceTests
    {
        private readonly WalletContext _context;
        private readonly ILogger<WalletService> _logger;
        private readonly IWalletValidationService _validationService;
        private readonly WalletService _sut;

        public WalletServiceTests()
        {
            var options = new DbContextOptionsBuilder<WalletContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new WalletContext(options);
            _logger = Substitute.For<ILogger<WalletService>>();
            _validationService = Substitute.For<IWalletValidationService>();
            _sut = new WalletService(_context, _logger, _validationService);
        }

        #region Constructor Tests

        [Fact]
        public void ConstructorShouldThrow_WhenContextIsNull()
        {
            FluentActions.Invoking(() => new WalletService(null!, _logger, _validationService))
                .Should().Throw<ArgumentNullException>()
                .WithParameterName("context");
        }

        [Fact]
        public void ConstructorShould_ThrowWhenLoggerIsNull()
        {
            FluentActions.Invoking(() => new WalletService(_context, null!, _validationService))
                .Should().Throw<ArgumentNullException>()
                .WithParameterName("logger");
        }

        [Fact]
        public void ConstructorShouldThrow_WhenValidationServiceIsNull()
        {
            FluentActions.Invoking(() => new WalletService(_context, _logger, null!))
                .Should().Throw<ArgumentNullException>()
                .WithParameterName("walletValidationService");
        }

        #endregion

        #region AddWalletAsync Tests

        [Fact]
        public async Task AddWalletAsync_ShouldSucceed_WhenValidRequestProvided()
        {
            var request = new WalletRequestDto(
                Name: "Test Wallet",
                AccountNumber: "4111111111111111",
                AccountScheme: "visa",
                Type: "card",
                Owner: "0244123456");

            var result = await _sut.AddWalletAsync(request);

            result.Should().NotBeNull();
            result.Content.Should().NotBeNull();
            result.Message.Should().Be(MessageConstants.WalletAddedSuccessfully);
            
            var wallet = result.Content;
            wallet.Should().NotBeNull();
            wallet!.Name.Should().Be(request.Name);
            wallet.AccountNumber.Should().Be(request.AccountNumber);
            
            var savedWallet = await _context.Wallets.FirstOrDefaultAsync();
            savedWallet.Should().NotBeNull();
            savedWallet!.Name.Should().Be(request.Name);
        }
        #endregion
        
        #region GetWalletAsync Tests

        [Fact]
        public async Task GetWalletAsync_ShouldReturnSuccess_WhenWalletExists()
        {
            var walletId = Guid.NewGuid();
            var wallet = new Wallet
            {
                Id = walletId,
                Name = "Test Wallet",
                AccountNumber = "4111111111111111",
                AccountScheme = AccountScheme.visa,
                Type = WalletType.card,
                Owner = "0244123456",
                CreatedAt = DateTime.UtcNow
            };

            await _context.Wallets.AddAsync(wallet);
            await _context.SaveChangesAsync();

            var result = await _sut.GetWalletAsync(walletId);

            result.Should().NotBeNull();
            result.Content.Should().NotBeNull();
            result.Content!.Id.Should().Be(walletId);
            result.Message.Should().Be(MessageConstants.WalletRetrievedSuccessfully);
        }

        [Fact]
        public async Task GetWalletAsync_ShouldReturnFailure_WhenWalletDoesNotExist()
        {
            var walletId = Guid.NewGuid();

            var result = await _sut.GetWalletAsync(walletId);

            result.Should().NotBeNull();
            result.Content.Should().BeNull();
            result.Message.Should().Be(MessageConstants.WalletNotFound);
        }
        #endregion

        #region RemoveWalletAsync Tests

        [Fact]
        public async Task RemoveWalletAsync_ShouldReturnSuccess_WhenWalletExists()
        {
            var walletId = Guid.NewGuid();
            var wallet = new Wallet
            {
                Id = walletId,
                Name = "Test Wallet",
                AccountNumber = "4111111111111111",
                AccountScheme = AccountScheme.visa,
                Type = WalletType.card,
                Owner = "0244123456",
                CreatedAt = DateTime.UtcNow
            };

            await _context.Wallets.AddAsync(wallet);
            await _context.SaveChangesAsync();

            var result = await _sut.RemoveWalletAsync(walletId);

            result.Should().NotBeNull();
            result.Content.Should().BeTrue();
            result.Message.Should().Be(MessageConstants.WalletRemovedSuccessfully);

            var removedWallet = await _context.Wallets.FindAsync(walletId);
            removedWallet.Should().BeNull();
        }

        [Fact]
        public async Task RemoveWalletAsync_ShouldReturnFailure_WhenWalletDoesNotExist()
        {
            var walletId = Guid.NewGuid();

            var result = await _sut.RemoveWalletAsync(walletId);

            result.Should().NotBeNull();
            result.Content.Should().BeFalse();
            result.Message.Should().Be(MessageConstants.WalletNotFound);
        }

        #endregion


    }
}