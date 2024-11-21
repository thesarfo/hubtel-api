using Hubtel.Api.Contracts;
using Hubtel.Api.Data.Request;
using Hubtel.Api.Data.Response;
using Microsoft.AspNetCore.Mvc;

namespace Hubtel.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class WalletController(IWalletService walletService, IWalletValidationService walletValidationService)
    : ControllerBase
{
    /// <summary>
    /// Adds a new wallet.
    /// </summary>
    /// <param name="walletRequestDto">The wallet request details</param>
    /// <returns>Created wallet details or error response</returns>
    [HttpPost]
    public async Task<IActionResult> AddWalletAsync([FromBody] WalletRequestDto walletRequestDto)
    {
        if (walletRequestDto is null) 
            return BadRequest(ApiResponse<object>.Failure(MessageConstants.InvalidRequest));
        if (!await walletValidationService.IsAccountNumberUniqueAsync(
                walletRequestDto.AccountNumber, 
                walletRequestDto.Owner))
        {
            return BadRequest(ApiResponse<object>.Failure(MessageConstants.AccountInUse));
        }

        if (!await walletValidationService.CanAddMoreWalletsAsync(walletRequestDto.Owner))
        {
            return BadRequest(ApiResponse<object>.Failure(MessageConstants.MaxWalletsReached));
        }
        var result = await walletService.AddWalletAsync(walletRequestDto);

        return result.Content is not null
            ? CreatedAtAction(
                nameof(GetWalletById), 
                new { id = result.Content.Id }, 
                result)
            : BadRequest(result);
    }
    
    /// <summary>
    /// Retrieves a single wallet by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the wallet</param>
    /// <returns>Wallet details or error response</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetWalletById(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest(ApiResponse<object>.Failure(MessageConstants.InvalidWalletId));
        }
        var result = await walletService.GetWalletAsync(id);

        return result.Content is null 
            ? NotFound(result) 
            : Ok(result);
    }
    
    /// <summary>
    /// Retrieves paginated list of wallets.
    /// </summary>
    /// <param name="pageNumber">Page number for pagination</param>
    /// <param name="pageSize">Number of items per page</param>
    /// <returns>Paginated wallet list or error response</returns>
    [HttpGet]
    public async Task<IActionResult> GetWalletsAsync(
        [FromQuery] int pageNumber = 1, 
        [FromQuery] int pageSize = 10)
    {
        if (pageNumber <= 0 || pageSize <= 0)
        {
            return BadRequest(ApiResponse<object>.Failure(
                MessageConstants.InvalidPaginationParameters));
        }

        var result = await walletService.GetWalletsAsync(pageNumber, pageSize);
        
        return Ok(result);
    }

    /// <summary>
    /// Removes a wallet by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the wallet to remove</param>
    /// <returns>No content or error response</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWallet(Guid id)
    {
        if (id == Guid.Empty) 
            return BadRequest(ApiResponse<object>.Failure(MessageConstants.InvalidWalletId));
        var result = await walletService.RemoveWalletAsync(id);

        return result.Content 
            ? NoContent() 
            : BadRequest(result);
    }
}