using System.Globalization;
using Hubtel.Api.Contracts;
using Hubtel.Api.Data;
using Hubtel.Api.Data.Enums;
using Hubtel.Api.Data.Request;
using Hubtel.Api.Entities;
using Hubtel.Api.Utils;
using Hubtel.Api.Utils.Exceptions;
using Hubtel.Api.Utils.Pagination;
using HubtelWallets.API.DTOs;

namespace Hubtel.Api.Services;

public class WalletService(WalletContext context, ILogger<WalletService> logger, IWalletValidationService walletValidationService): IWalletService
{
    private readonly WalletContext _context = context ?? throw new ArgumentNullException(nameof(context));
    private readonly ILogger<WalletService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly IWalletValidationService _walletValidationService = walletValidationService ?? throw new ArgumentNullException(nameof(walletValidationService));


    public async Task<WalletResponseDto> AddWalletAsync(WalletDto walletDto)
    {
        // Debug logging
        Console.WriteLine($"Received AccountScheme: {walletDto.AccountScheme}");
        Console.WriteLine($"Available AccountScheme values: {string.Join(", ", Enum.GetNames(typeof(AccountScheme)))}");

        ArgumentNullException.ThrowIfNull(walletDto);

        var wallet = new Wallet
        {
            Id = Guid.NewGuid(),
            Name = walletDto.Name,
            AccountNumber = walletDto.AccountNumber,
            AccountScheme = walletDto.AccountScheme.GetEnumValue<AccountScheme>(),
            Type = walletDto.Type.GetEnumValue<WalletType>(),
            Owner = walletDto.Owner,
            CreatedAt = DateTime.UtcNow
        };

        _walletValidationService.ValidateWallet(wallet);

        await _context.Wallets.AddAsync(wallet);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Wallet added successfully: {WalletId}", wallet.Id);

        return WalletResponseDto.ToWalletDto(wallet);
    }

    public async Task<WalletResponseDto> GetWalletAsync(Guid id)
    {
        var wallet = await _context.Wallets.FindAsync(id) ??
                     throw new Exception($"Wallet not found");
        _logger.LogWarning("Wallet not found: {WalletId}", id);

        return WalletResponseDto.ToWalletDto(wallet);
        
    }
    
    public async Task<PaginationInfo<WalletResponseDto>> GetWalletsAsync()
    {
        var query = _context.Wallets.Select(w => WalletResponseDto.ToWalletDto(w));

        var pagedResponse = await PagedList<WalletResponseDto>.ToPageableAsync(query, 1, 10);

        return new PaginationInfo<WalletResponseDto>
        {
            Data = pagedResponse,
            Meta = pagedResponse.Meta
        };

    }
    
    public async Task<bool> RemoveWalletAsync(Guid id)
    {
        var wallet = await _context.Wallets.FindAsync(id);
        if (wallet is null)
        {
            _logger.LogWarning("Wallet not found: {WalletId}", id);
            return false;
        }

        _context.Wallets.Remove(wallet);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Wallet removed successfully: {WalletId}", id);
        return true;
    }
}