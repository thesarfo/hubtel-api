using Hubtel.Api.Contracts;
using Hubtel.Api.Data.Request;
using Microsoft.AspNetCore.Mvc;

namespace Hubtel.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WalletController(IWalletService walletService, IWalletValidationService walletValidationService)
    : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddWalletAsync([FromBody] WalletDto walletDto)
    {
        if (walletDto is null) return BadRequest(ModelState);
        if (!await walletValidationService.IsAccountNumberUniqueAsync(walletDto.AccountNumber))
        {
            return BadRequest("The account number is already in use by another owner.");
        }

        if (!await walletValidationService.CanAddMoreWalletsAsync(walletDto.Owner))
        {
            return BadRequest("The owner has reached the maximum number of wallets (5).");
        }

        try
        {

            var result = await walletService.AddWalletAsync(walletDto);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {

            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetWalletById(Guid id)
    {
        if (id == Guid.Empty) return BadRequest("Invalid wallet id provided.");

        var wallet = await walletService.GetWalletAsync(id);

        return wallet switch
        {
            null => NotFound(),
            _ => Ok(wallet)
        };
    }

}