using System.ComponentModel.DataAnnotations;
using Hubtel.Api.Data.Response;
using Hubtel.Api.Utils.Validators;

namespace Hubtel.Api.Data.Request;



public record WalletRequestDto(
    [Required(AllowEmptyStrings = false, ErrorMessage = MessageConstants.WalletNameRequired)]
    [StringLength(30, MinimumLength = 3, ErrorMessage = MessageConstants.WalletNameLength)]
    string Name,

    [Required(AllowEmptyStrings = false, ErrorMessage = MessageConstants.AccountNumberRequired)]
    [AccountNumber("Type", "AccountScheme")]
    string AccountNumber,

    [Required(AllowEmptyStrings = false, ErrorMessage = MessageConstants.AccountSchemeRequired)]
    string AccountScheme,

    [Required(AllowEmptyStrings = false, ErrorMessage = MessageConstants.WalletTypeRequired)]
    string Type,

    [Required(AllowEmptyStrings = false, ErrorMessage = MessageConstants.OwnerRequired)]
    [Phone(ErrorMessage = MessageConstants.InvalidPhoneNumberFormat)]
    string Owner);
