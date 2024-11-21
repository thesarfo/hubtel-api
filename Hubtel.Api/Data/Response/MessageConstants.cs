namespace Hubtel.Api.Data.Response;

public static class MessageConstants
{
    public const string WalletAddedSuccessfully = "Wallet added successfully.";
    public const string WalletRetrievedSuccessfully = "Wallet retrieved successfully.";
    public const string WalletsRetrievedSuccessfully = "Wallets retrieved successfully.";
    
    public const string WalletNameRequired = "The Name field is required and cannot be empty.";
    public const string WalletNameLength = "The Name must be between 3 and 30 characters.";
    public const string AccountNumberRequired = "The Account Number field is required and cannot be empty.";
    public const string AccountSchemeRequired = "The Account Scheme field is required and cannot be empty.";
    public const string WalletTypeRequired = "The Type field is required and cannot be empty.";
    public const string OwnerRequired = "The Owner field is required and cannot be empty.";
    public const string InvalidPhoneNumberFormat = "Invalid phone number format.";

    public const string InvalidPaginationParameters = "Invalid pagination parameters.";
    public const string AccountInUse = "The account number/name is already in use.";
    public const string MaxWalletsReached = "You can only add five(5) wallets";
    public const string WalletNotFound = "Wallet not found.";
    public const string InvalidAccountScheme = "Invalid account scheme provided.";
    public const string InvalidWalletType = "Invalid wallet type. Only 'momo' or 'card' are accepted.";
    public const string InvalidCardNumber = "Invalid card number provided for the specified Account Scheme.";
    public const string InvalidWalletId = "Invalid wallet ID";
    public const string ErrorRetrievingWallets = "An error occurred while retrieving the wallet(s).";
    public const string ErrorRemovingWallets = "An error occurred while removing the wallet.";
    public const string UnexpectedError = "An unexpected error occurred while adding the wallet.";
}
