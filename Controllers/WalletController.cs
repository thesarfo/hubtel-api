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
    [HttpPost]
    public async Task<IActionResult> AddWalletAsync([FromBody] WalletDto walletDto)
    {
        if (walletDto is null) return BadRequest(ModelState);
        if (!await walletValidationService.IsAccountNumberUniqueAsync(walletDto.AccountNumber))
        {
            
            return BadRequest(ApiResponse<object>.Failure("The account number is already in use"));
        }
        if (!await walletValidationService.CanAddMoreWalletsAsync(walletDto.Owner))
        {
            return BadRequest(ApiResponse<object>.Failure("The owner has reached the maximum number of wallets (5)"));
        }

        try
        {

            var result = await walletService.AddWalletAsync(walletDto);
            return Ok(ApiResponse<WalletResponseDto>.Success(result, "Wallet Added Successfully"));
        }
        catch (ArgumentException ex)
        {

            return BadRequest(ApiResponse<object>.Failure(ex.Message));
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetWalletById(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest(ApiResponse<object>.Failure("Invalid wallet ID provided"));
        }

        var wallet = await walletService.GetWalletAsync(id);

        return wallet switch
        {
            null => NotFound(ApiResponse<object>.Failure("Wallet not found")),
            _ => Ok(ApiResponse<object>.Success(wallet, "Wallet retrieved successfully"))
        };
    }

    
    [HttpGet]
    public async Task<IActionResult> GetWalletsAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        if (pageNumber <= 0 || pageSize <= 0)
        {
            return BadRequest(ApiResponse<object>.Failure("Invalid pagination parameters."));
        }

        var result = await walletService.GetWalletsAsync(pageNumber, pageSize);
        return Ok(result);
    }

    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWallet(Guid id)
    {
        if (id == Guid.Empty) return BadRequest(ApiResponse<object>.Failure("Invalid wallet id provided"));
        
        await walletService.RemoveWalletAsync(id);

        return NoContent();
    }

}