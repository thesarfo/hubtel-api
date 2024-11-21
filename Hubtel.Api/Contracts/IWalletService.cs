using Hubtel.Api.Data.Request;
using Hubtel.Api.Data.Response;
using Hubtel.Api.Utils.Pagination;

namespace Hubtel.Api.Contracts;

public interface IWalletService
{
    Task<ApiResponse<WalletResponseDto>> AddWalletAsync(WalletRequestDto walletRequestDto);
    Task<ApiResponse<WalletResponseDto>> GetWalletAsync(Guid id);
    Task<ApiResponse<PaginationInfo<WalletResponseDto>>> GetWalletsAsync(int pageNumber = 1, int pageSize = 10);
    Task<ApiResponse<bool>> RemoveWalletAsync(Guid id);


}