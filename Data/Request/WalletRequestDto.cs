using System.ComponentModel.DataAnnotations;
using Hubtel.Api.Utils;
using Hubtel.Api.Utils.Binders;
using Microsoft.AspNetCore.Mvc;

namespace Hubtel.Api.Data.Request;

public record WalletDto(
    [ModelBinder(BinderType = typeof(TrimModelBinder))]
    [Required]
    [Length(3,30)] string Name,
    [ModelBinder(BinderType = typeof(TrimModelBinder))]
    [Required]
    [AccountNumber("Type","AccountScheme")]
    string AccountNumber,
    [ModelBinder(BinderType = typeof(TrimModelBinder))]
    [Required]
    string AccountScheme,
    [ModelBinder(BinderType = typeof(TrimModelBinder))]
    [Required]
    string Type,
    [ModelBinder(BinderType = typeof(TrimModelBinder))]
    [Required]
    [PhoneNumber]
    string Owner);