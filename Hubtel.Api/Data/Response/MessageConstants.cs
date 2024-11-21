namespace Hubtel.Api.Data.Response;

public static class MessageConstants
{
    public const string WalletAddedSuccessfully = "Wallet added successfully.";
    public const string WalletRetrievedSuccessfully = "Wallet retrieved successfully.";
    public const string WalletsRetrievedSuccessfully = "Wallets retrieved successfully.";

    public const string InvalidPaginationParameters = "Invalid pagination parameters.";
    public const string AccountInUse = "The account number/name is already in use.";
    public const string MaxWalletsReached = "You can only add five(5) wallets";
    public const string WalletNotFound = "Wallet not found.";
    public const string InvalidAccountScheme = "Invalid account scheme provided.";
    public const string InvalidWalletType = "Invalid wallet type. Only 'momo' or 'card' are accepted.";
    public const string InvalidCardNumber = "Invalid card number provided for the specified Account Scheme.";
    public const string InvalidWalletId = "Invalid wallet ID";
}
