using Hubtel.Api.Data.Request;
using Hubtel.Api.Utils.Pagination;
using HubtelWallets.API.DTOs;

namespace Hubtel.Api.Contracts;

public interface IWalletService
{
    Task<WalletResponseDto> AddWalletAsync(WalletDto wallet);
    Task<WalletResponseDto> GetWalletAsync(Guid id);
    Task<PaginationInfo<WalletResponseDto>> GetWalletsAsync();
    Task<bool> RemoveWalletAsync(Guid id);


}