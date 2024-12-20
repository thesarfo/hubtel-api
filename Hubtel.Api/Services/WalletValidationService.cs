using Hubtel.Api.Contracts;
using Hubtel.Api.Data;
using Hubtel.Api.Data.Enums;
using Hubtel.Api.Data.Response;
using Hubtel.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hubtel.Api.Services;

public class WalletValidationService(WalletContext context) : IWalletValidationService
{
    public async Task<bool> CanAddMoreWalletsAsync(string phoneNumber)
    {
        var walletExists = await context.Wallets
            .Where(w => w.Owner == phoneNumber)
            .Take(5) 
            .CountAsync(); 
        return walletExists < 5;
    }


    public async Task<bool> IsAccountNumberUniqueAsync(string accountNumber, string owner)
    {
        return !await context.Wallets.AnyAsync(w => w.AccountNumber == accountNumber);
    }

    public void ValidateWallet(Wallet wallet)
    {
        if (!Enum.IsDefined(typeof(AccountScheme), wallet.AccountScheme))
        {
            throw new ArgumentException(MessageConstants.InvalidAccountScheme);
        }

        ValidateWalletType(wallet);

        if (wallet.Type == WalletType.card)
        {
            ValidateCardNumber(wallet);
        }
    }

    private void ValidateWalletType(Wallet wallet)
    {
        if (wallet.Type != WalletType.momo && wallet.Type != WalletType.card)
        {
            throw new ArgumentException(MessageConstants.InvalidWalletType);
        }
    }

    private void ValidateCardNumber(Wallet wallet)
    {
        if (!IsValidCardNumber(wallet.AccountNumber, wallet.AccountScheme))
        {
            throw new ArgumentException($"{MessageConstants.InvalidCardNumber} Account Scheme: {wallet.AccountScheme}");
        }

        wallet.AccountNumber = wallet.AccountNumber.Substring(0, 6);
    }

    

    public bool IsValidCardNumber(string cardNumber, AccountScheme accountScheme)
    {
        cardNumber = new string(cardNumber.Where(char.IsDigit).ToArray());

        if (cardNumber.Length != 16)
        {
            return false;
        }

        return accountScheme switch
        {
            AccountScheme.visa => cardNumber.StartsWith("4"),
            AccountScheme.mastercard => cardNumber.StartsWith("5") || 
                                        (cardNumber.CompareTo("2221000000000000") >= 0 &&
                                         cardNumber.CompareTo("2720999999999999") <= 0),
            _ => false
        };
    }

}