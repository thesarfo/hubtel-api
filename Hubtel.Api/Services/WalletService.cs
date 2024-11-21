using Hubtel.Api.Contracts;
using Hubtel.Api.Data;
using Hubtel.Api.Data.Enums;
using Hubtel.Api.Data.Request;
using Hubtel.Api.Data.Response;
using Hubtel.Api.Entities;
using Hubtel.Api.Utils;
using Hubtel.Api.Utils.Pagination;

namespace Hubtel.Api.Services
{
     public class WalletService : IWalletService
    {
        private readonly WalletContext _context;
        private readonly ILogger<WalletService> _logger;
        private readonly IWalletValidationService _walletValidationService;

        public WalletService(WalletContext context, ILogger<WalletService> logger, IWalletValidationService walletValidationService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _walletValidationService = walletValidationService ?? throw new ArgumentNullException(nameof(walletValidationService));
        }

        public async Task<ApiResponse<WalletResponseDto>> AddWalletAsync(WalletRequestDto walletRequestDto)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(walletRequestDto);

                var wallet = new Wallet
                {
                    Id = Guid.NewGuid(),
                    Name = walletRequestDto.Name,
                    AccountNumber = walletRequestDto.AccountNumber,
                    AccountScheme = walletRequestDto.AccountScheme.GetEnumValue<AccountScheme>(),
                    Type = walletRequestDto.Type.GetEnumValue<WalletType>(),
                    Owner = walletRequestDto.Owner,
                    CreatedAt = DateTime.UtcNow
                };

                _walletValidationService.ValidateWallet(wallet);

                await _context.Wallets.AddAsync(wallet);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Wallet added successfully: {WalletId}", wallet.Id);

                return ApiResponse<WalletResponseDto>.Success(
                    WalletResponseDto.ToWalletDto(wallet),
                    MessageConstants.WalletAddedSuccessfully
                );
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "ArgumentNullException while adding wallet.");
                return ApiResponse<WalletResponseDto>.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while adding the wallet.");
                return ApiResponse<WalletResponseDto>.Failure(MessageConstants.UnexpectedError);
            }
        }

        public async Task<ApiResponse<WalletResponseDto>> GetWalletAsync(Guid id)
        {
            try
            {
                var wallet = await _context.Wallets.FindAsync(id);

                if (wallet != null)
                {
                    return ApiResponse<WalletResponseDto>.Success(
                        WalletResponseDto.ToWalletDto(wallet),
                        MessageConstants.WalletRetrievedSuccessfully
                    );
                }

                _logger.LogWarning("Wallet not found: {WalletId}", id);
                return ApiResponse<WalletResponseDto>.Failure(MessageConstants.WalletNotFound);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the wallet with ID: {WalletId}", id);
                return ApiResponse<WalletResponseDto>.Failure(MessageConstants.ErrorRetrievingWallets);
            }
        }

        public async Task<ApiResponse<PaginationInfo<WalletResponseDto>>> GetWalletsAsync(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var query = _context.Wallets
                    .OrderBy(w => w.CreatedAt)
                    .Select(w => WalletResponseDto.ToWalletDto(w));

                var pagedResponse = await PagedList<WalletResponseDto>.ToPageableAsync(query, pageNumber, pageSize);

                return ApiResponse<PaginationInfo<WalletResponseDto>>.Success(
                    new PaginationInfo<WalletResponseDto>
                    {
                        Data = pagedResponse,
                        Meta = pagedResponse.Meta
                    },
                    MessageConstants.WalletsRetrievedSuccessfully
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving wallets.");
                return ApiResponse<PaginationInfo<WalletResponseDto>>.Failure(MessageConstants.ErrorRetrievingWallets);
            }
        }

        public async Task<ApiResponse<bool>> RemoveWalletAsync(Guid id)
        {
            try
            {
                var wallet = await _context.Wallets.FindAsync(id);
                if (wallet is null)
                {
                    _logger.LogWarning("Wallet not found: {WalletId}", id);
                    return ApiResponse<bool>.Failure(MessageConstants.WalletNotFound);
                }

                _context.Wallets.Remove(wallet);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Wallet removed successfully: {WalletId}", id);
                return ApiResponse<bool>.Success(true, MessageConstants.WalletRemovedSuccessfully);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while removing the wallet with ID: {WalletId}", id);
                return ApiResponse<bool>.Failure(MessageConstants.ErrorRemovingWallets);
            }
        }
    }
}
