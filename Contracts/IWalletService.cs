using Hubtel.Api.Data.Request;
using Hubtel.Api.Data.Response;
using Hubtel.Api.Utils.Pagination;

namespace Hubtel.Api.Contracts;

public interface IWalletService
{
    Task<WalletResponseDto> AddWalletAsync(WalletDto wallet);
    Task<WalletResponseDto> GetWalletAsync(Guid id);
    Task<ApiResponse<PaginationInfo<WalletResponseDto>>> GetWalletsAsync(int pageNumber, int pageSize);
    Task<bool> RemoveWalletAsync(Guid id);


}