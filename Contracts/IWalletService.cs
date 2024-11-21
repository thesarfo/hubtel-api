using Hubtel.Api.Data.Request;
using HubtelWallets.API.DTOs;

namespace Hubtel.Api.Contracts;

public interface IWalletService
{
    Task<WalletResponseDto> AddWalletAsync(WalletDto wallet);
    Task<WalletResponseDto> GetWalletAsync(Guid id);
}