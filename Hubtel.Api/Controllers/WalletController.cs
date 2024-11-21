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
    /// <param name="walletDto"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> AddWalletAsync([FromBody] WalletDto walletDto)
    {
        if (walletDto is null) return BadRequest(ModelState);
        if (!await walletValidationService.IsAccountNumberUniqueAsync(walletDto.AccountNumber, walletDto.Owner))
        {
            
            return BadRequest(ApiResponse<object>.Failure(MessageConstants.AccountInUse));
        }
        if (!await walletValidationService.CanAddMoreWalletsAsync(walletDto.Owner))
        {
            return BadRequest(ApiResponse<object>.Failure(MessageConstants.MaxWalletsReached));
        }

        try
        {

            var result = await walletService.AddWalletAsync(walletDto);
            return CreatedAtAction(nameof(GetWalletById), new { id = result.Id }, 
                ApiResponse<WalletResponseDto>.Success(result, MessageConstants.WalletAddedSuccessfully));
        }
        catch (ArgumentException ex)
        {

            return BadRequest(ApiResponse<object>.Failure(ex.Message));
        }
    }

    /// <summary>
    /// Gets all wallets.
    /// </summary>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetWalletById(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest(ApiResponse<object>.Failure(MessageConstants.InvalidWalletId));
        }

        var wallet = await walletService.GetWalletAsync(id);

        return wallet switch
        {
            null => NotFound(ApiResponse<object>.Failure(MessageConstants.WalletNotFound)),
            _ => Ok(ApiResponse<object>.Success(wallet, MessageConstants.WalletRetrievedSuccessfully))
        };
    }

    /// <summary>
    /// Gets a single wallet by id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetWalletsAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        if (pageNumber <= 0 || pageSize <= 0)
        {
            return BadRequest(ApiResponse<object>.Failure(MessageConstants.InvalidPaginationParameters));
        }

        var result = await walletService.GetWalletsAsync(pageNumber, pageSize);
        return Ok(result);
    }

    
    /// <summary>
    /// Removes a wallet by id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWallet(Guid id)
    {
        if (id == Guid.Empty) return BadRequest(ApiResponse<object>.Failure(MessageConstants.InvalidWalletId));
        
        await walletService.RemoveWalletAsync(id);

        return NoContent();
    }

}