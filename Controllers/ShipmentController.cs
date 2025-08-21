using System.Collections.Concurrent;
using LeUs.Application.Dtos.Gps;
using Leus.Application.Features.Catalog.Queries;
using Leus.Application.Features.Data.Commands;
using LeUs.Application.Features.Data.Commands;
using Leus.Application.Features.Data.Queries;
using LeUs.Application.Features.Data.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IHttpResult = Microsoft.AspNetCore.Http.IResult;

namespace LeUs.Controllers;

[Authorize(Roles = RoleConstants.UseApiRole)]
public class ShipmentController(
    IValidator<GetShipmentRequest> getShipmentVal,
    IValidator<CShipmentDto> shipmentVal,
    ILeUsService leUsService)
    : BaseJwtApiController
{
    [HttpPost("get-data")]
    [ProducesResponseType<List<ShipmentDto>>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    [EndpointSummary("Get shipments")]
    public async Task<IHttpResult> GetData(GetShipmentRequest request)
    {
        var valResult = await getShipmentVal.ValidateAsync(request);
        if (valResult.IsValid)
        {
            var results = await _mediator!.Send(new GetAllShipmentByUserQuery()
            {
                DateRange = request.DateRange,
                Status = request.Status,
                UserId = User.GetUserId(),
                IsTimeOut = request.IsTimeOut,
                RefIds = request.ReferenceId.NotIsNullOrEmpty()
                    ? $"{request.ReferenceId}".SplitExt(pattern: ",;\n\t -").ToList()
                    : [],
                Ref2Ids = request.ReferenceId2.NotIsNullOrEmpty()
                    ? $"{request.ReferenceId2}".SplitExt(pattern: ",;\n\t -").ToList()
                    : [],
                TrackIds = request.TrackingId.NotIsNullOrEmpty()
                    ? $"{request.TrackingId}".SplitExt(pattern: ",;\n\t -").ToList()
                    : [],
            });
            var cResults = results.Adapt<List<ShipmentDto>>();
            return Results.Ok(cResults);
        }

        var problemDetails = new HttpValidationProblemDetails(valResult.ToDictionary())
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Validation Failed",
            Detail = "One or more validation errors occurred.",
            Instance = "api/shipment/get-data"
        };
        return Results.Problem(problemDetails);
    }

    [HttpPost("get-page-data")]
    [ProducesResponseType<PagedResponseOffset<ShipmentDto>>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    [EndpointSummary("Get shipments by paging")]
    public async Task<IHttpResult> GetPageData(GetShipmentRequest request)
    {
        if (request.PageNumber <= 0 || request.PageSize <= 0 || request.IsTimeOut)
        {
            var problemPage = new HttpValidationProblemDetails(new ConcurrentDictionary<string, string[]>())
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation Failed",
                Detail = "One or more validation errors occurred.",
                Instance = "api/shipment/generate-label",
            };
            if (request.IsTimeOut)
            {
                problemPage.Errors.Add("Parameter", ["The feature not supported."]);
            }
            if (request.PageNumber <= 0)
            {
                problemPage.Errors.Add("Page number", ["The page number must be greater than zero."]);
            }

            if (request.PageSize <= 0)
            {
                problemPage.Errors.Add("Page size", ["The page size must be greater than zero."]);
            }
            return Results.Problem(problemPage);
        }

        var valResult = await getShipmentVal.ValidateAsync(request);
        if (valResult.IsValid)
        {
            var results = await _mediator!.Send(new GetPageShipmentByUserQuery()
            {
                DateRange = request.DateRange,
                Status = request.Status,
                UserId = User.GetUserId(),
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                RefIds = request.ReferenceId.NotIsNullOrEmpty()
                    ? $"{request.ReferenceId}".SplitExt(pattern: ",;\n\t -").ToList()
                    : [],
            });
            return Results.Ok(results);
        }

        var problemDetails = new HttpValidationProblemDetails(valResult.ToDictionary())
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Validation Failed",
            Detail = "One or more validation errors occurred.",
            Instance = "api/shipment/get-data"
        };
        return Results.Problem(problemDetails);
    }

    [HttpGet("byref/{refId}")]
    [ProducesResponseType<ShipmentDto>(StatusCodes.Status200OK, "application/json")]
    [EndpointSummary("Get Shipment by referenceId")]
    public async Task<IHttpResult> GetRefid(string? refId)
    {
        if (refId is null)
        {
            var problemDetails = new HttpValidationProblemDetails(new ConcurrentDictionary<string, string[]>())
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation Failed",
                Detail = "One or more validation errors occurred.",
                Instance = "api/shipment/generate-label",
            };
            problemDetails.Errors.Add("RefIds", ["Not found the shipment"]);
            return Results.Problem(problemDetails);
        }

        var results = await _mediator!.Send(new GetAllShipmentByUserQuery()
        {
            UserId = User.GetUserId(),
            RefIds = $"{refId}".SplitExt(pattern: ",;\n\t -").ToList(),
        });
        var cResults = results.Adapt<List<ShipmentDto>>();
        return Results.Ok(cResults.FirstOrDefault());
    }

    [HttpPost]
    [ProducesResponseType<Result<AddEditShipmentResponse>>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    [EndpointSummary("1. Create new Shipment")]
    public async Task<IHttpResult> Post(ShipmentInput request)
    {
        var dConvert = new CShipmentDto();
        request.Adapt(dConvert);
        if (dConvert.ShipmentStatus != 0)
        {
            dConvert.ShipmentStatus = 0;
        }

        dConvert.Customs ??= [];
        var valResult = await shipmentVal.ValidateAsync(dConvert);
        if (valResult.IsValid)
        {
            var services = await _mediator!.Send(new GetAllServiceQuery());
            var oFind = services.FirstOrDefault(w => w.ServiceCode == dConvert.ServiceCode);
            if (oFind == null)
            {
                var problemServiceDetails = new HttpValidationProblemDetails(valResult.ToDictionary())
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Validation Failed",
                    Detail = "Not found the service.",
                    Instance = "api/shipment"
                };
                return Results.Problem(problemServiceDetails);
            }

            if ($"{oFind.ApiName}".Equals("gps", StringComparison.CurrentCultureIgnoreCase))
            {
                valResult = await shipmentVal.ValidateAsync(dConvert, options => { options.IncludeAllRuleSets(); });
                if (!valResult.IsValid)
                {
                    var gpsProblemDetails = new HttpValidationProblemDetails(valResult.ToDictionary())
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Title = "Validation Failed",
                        Detail = "One or more validation errors occurred.",
                        Instance = "api/shipment"
                    };
                    return Results.Problem(gpsProblemDetails);
                }
            }
            dConvert.ServiceCode = oFind.ServiceCode;
            dConvert.ApiName = oFind.ApiName;
            dConvert.ServiceId = $"{oFind.Id}";
            dConvert.Weight = dConvert.Boxes?.Sum(s => s.Weight) ?? 0;
            dConvert.CustomerId = await _mediator.Send(new GetAllCustomerByEmailQuery()
            {
                Email = $"{User.GetEmail()}"
            });
            var resultAddShipment = await _mediator!.Send(new AddEditShipmentCommand()
            {
                Request = new AddEditDataRequest<CShipmentDto>()
                {
                    Data = dConvert,
                    Action = ActionCommandType.Add
                }
            });
            resultAddShipment.Data.ServiceCode = request.ServiceCode;
            return Results.Ok(resultAddShipment);
        }

        var problemDetails = new HttpValidationProblemDetails(valResult.ToDictionary())
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Validation Failed",
            Detail = "One or more validation errors occurred.",
            Instance = "api/shipment"
        };
        return Results.Problem(problemDetails);
    }
    
    [HttpPost("create-label")]
    [ProducesResponseType<Result<AddEditShipmentResponse>>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    [EndpointSummary("3. Create new Shipment and generate label")]
    public async Task<IHttpResult> CreateLabelPost(ShipmentInput request)
    {
        var dConvert = new CShipmentDto();
        request.Adapt(dConvert);
        if (dConvert.ShipmentStatus != 0)
        {
            dConvert.ShipmentStatus = 0;
        }
        dConvert.Customs ??= [];
        var valResult = await shipmentVal.ValidateAsync(dConvert);
        if (valResult.IsValid)
        {
            var services = await _mediator!.Send(new GetAllServiceQuery());
            var oFind = services.FirstOrDefault(w => w.ServiceCode == dConvert.ServiceCode);
            if (oFind == null)
            {
                var problemServiceDetails = new HttpValidationProblemDetails(valResult.ToDictionary())
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Validation Failed",
                    Detail = "Not found the service.",
                    Instance = "api/shipment"
                };
                return Results.Problem(problemServiceDetails);
            }
            if ($"{oFind.ApiName}".Equals("gps", StringComparison.CurrentCultureIgnoreCase))
            {
                valResult = await shipmentVal.ValidateAsync(dConvert, options => { options.IncludeAllRuleSets(); });
                if (!valResult.IsValid)
                {
                    var gpsProblemDetails = new HttpValidationProblemDetails(valResult.ToDictionary())
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Title = "Validation Failed",
                        Detail = "One or more validation errors occurred.",
                        Instance = "api/shipment"
                    };
                    return Results.Problem(gpsProblemDetails);
                }
            }
            dConvert.ServiceCode = oFind.ServiceCode;
            dConvert.ApiName = oFind.ApiName;
            dConvert.ServiceId = $"{oFind.Id}";
            dConvert.Weight = dConvert.Boxes?.Sum(s => s.Weight) ?? 0;
            dConvert.CustomerId = await _mediator.Send(new GetAllCustomerByEmailQuery()
            {
                Email = $"{User.GetEmail()}"
            });
            var resultAddShipment = await _mediator!.Send(new AddEditShipmentCommand()
            {
                Request = new AddEditDataRequest<CShipmentDto>()
                {
                    Data = dConvert,
                    Action = ActionCommandType.Add
                }
            });
            if (resultAddShipment.Succeeded == false)
            {
                return Results.Ok(resultAddShipment);
            }

            dConvert.Remote = resultAddShipment.Data.Remote;
            dConvert.Price = resultAddShipment.Data.Price;
            dConvert.ExtraLongFee = resultAddShipment.Data.ExtraLongFee;
            dConvert.OverLimitFee = resultAddShipment.Data.OverLimitFee;
            dConvert.ExcessVolumeFee = resultAddShipment.Data.ExcessVolumeFee;
            dConvert.ServiceCode1 = resultAddShipment.Data.ServiceCode;
            dConvert.ReferenceId = resultAddShipment.Data.ReferenceId;
            dConvert.Id = resultAddShipment.Data.Id;
            resultAddShipment.Data.ServiceCode = request.ServiceCode;
            var userId = User.GetUserId();
            //Generate label
            var resultLabel = await leUsService.CreateShipment(dConvert, services, userId);
            if (resultLabel.Success == false)
            {
                resultAddShipment.Succeeded = false;
                resultAddShipment.Messages.Add($"{resultLabel.ErrorMessage}");
                return Results.Ok(resultAddShipment);
            }
            resultAddShipment.Data.TrackingId = resultLabel.TrackingId;
            resultAddShipment.Data.Label = resultLabel.Data;
            return Results.Ok(resultAddShipment);
        }

        var problemDetails = new HttpValidationProblemDetails(valResult.ToDictionary())
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Validation Failed",
            Detail = "One or more validation errors occurred.",
            Instance = "api/shipment"
        };
        return Results.Problem(problemDetails);
    }

    [HttpPut("{id}")]
    [ProducesResponseType<Result<AddEditShipmentResponse>>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    [EndpointSummary("2. Update Shipment")]
    public async Task<IHttpResult> Put(Guid id, ShipmentInput request)
    {
        var dConvert = new CShipmentDto()
        {
            Id = id
        };
        request.Adapt(dConvert);
        if (dConvert.ShipmentStatus != 0)
        {
            dConvert.ShipmentStatus = 0;
        }
        dConvert.Customs ??= [];
        var valResult = await shipmentVal.ValidateAsync(dConvert);
        if (valResult.IsValid)
        {
            var services = await _mediator!.Send(new GetAllServiceQuery());
            var oFind = services.FirstOrDefault(w => w.ServiceCode == dConvert.ServiceCode);
            if (oFind == null)
            {
                var problemServiceDetails = new HttpValidationProblemDetails(valResult.ToDictionary())
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Validation Failed",
                    Detail = "Not found the service.",
                    Instance = "api/shipment"
                };
                return Results.Problem(problemServiceDetails);
            }
            if ($"{oFind.ApiName}".Equals("gps", StringComparison.CurrentCultureIgnoreCase))
            {
                valResult = await shipmentVal.ValidateAsync(dConvert, options => { options.IncludeAllRuleSets(); });
                if (!valResult.IsValid)
                {
                    var gpsProblemDetails = new HttpValidationProblemDetails(valResult.ToDictionary())
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Title = "Validation Failed",
                        Detail = "One or more validation errors occurred.",
                        Instance = "api/shipment"
                    };
                    return Results.Problem(gpsProblemDetails);
                }
            }
            dConvert.ServiceCode = oFind.ServiceCode;
            dConvert.ApiName = oFind.ApiName;
            dConvert.ServiceId = $"{oFind.Id}";
            dConvert.Weight = dConvert.Boxes?.Sum(s => s.Weight) ?? 0;
            dConvert.CustomerId = await _mediator.Send(new GetAllCustomerByEmailQuery()
            {
                Email = $"{User.GetEmail()}"
            });
            return Results.Ok(await _mediator!.Send(new AddEditShipmentCommand()
            {
                Request = new AddEditDataRequest<CShipmentDto>()
                {
                    Data = dConvert,
                    Action = ActionCommandType.Edit
                }
            }));
        }

        var problemDetails = new HttpValidationProblemDetails(valResult.ToDictionary())
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Validation Failed",
            Detail = "One or more validation errors occurred.",
            Instance = "api/shipment"
        };
        return Results.Problem(problemDetails);
    }

    [HttpDelete("{id}")]
    [EndpointSummary("4. Delete Shipment")]
    [ProducesResponseType<Result<Guid>>(StatusCodes.Status200OK, "application/json")]
    public async Task<IActionResult> Delete(Guid id)
    {
        return Ok(await _mediator!.Send(new DeleteShipmentCommand { Id = id }));
    }

    [HttpPost("generate-label")]
    [ProducesResponseType<List<CResult<string>>>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    [EndpointSummary("5. Generate Labels. Input is list of referenceId that return when you create a shipment")]
    public async Task<IHttpResult> GenerateLabel(List<string> request)
    {
        if (request is { Count: 0 })
        {
            var problemDetails = new HttpValidationProblemDetails(new ConcurrentDictionary<string, string[]>())
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation Failed",
                Detail = "One or more validation errors occurred.",
                Instance = "api/shipment/generate-label",
            };
            problemDetails.Errors.Add("RefIds", ["The list of reference is empty."]);
            return Results.Problem(problemDetails);
        }

        var userId = User.GetUserId();
        // var lstShipments = await _mediator!.Send(new GetAllShipmentByUserQuery()
        // {
        //     RefIds = request,
        //     UserId = userId,
        //     Status = 0
        // });
        // if (lstShipments is { Count: 0 })
        // {
        //     var problemDetails = new HttpValidationProblemDetails(new ConcurrentDictionary<string, string[]>())
        //     {
        //         Status = StatusCodes.Status400BadRequest,
        //         Title = "Validation Failed",
        //         Detail = "One or more validation errors occurred.",
        //         Instance = "api/shipment/generate-label",
        //     };
        //     problemDetails.Errors.Add("RefIds", ["Not found the shipment"]);
        //     return Results.Problem(problemDetails);
        // }
        var result = await leUsService.CreateShipment(request, userId);
        return Results.Ok(result);
    }

    [HttpPost("get-label")]
    [ProducesResponseType<DownloadFileContent>(StatusCodes.Status200OK, "application/json")]
    [EndpointSummary("6. Get Labels. Input is list of referenceId that return when you create a shipment. Result return is pdf or zip file")]
    public async Task<IActionResult> GetLabel(List<string> request)
    {
        return Ok(await leUsService.GetLabel(request, User.GetUserId()));
    }

    [HttpDelete("cancel-label")]
    [ProducesResponseType<List<CResult<string>>>(StatusCodes.Status200OK, "application/json")]
    [EndpointSummary("7. Cancel Labels. Input is list of referenceId that return when you create a shipment")]
    public async Task<IActionResult> CancelLabel([FromBody] List<string> request)
    {
        return Ok(await leUsService.CancelShipment(request, User.GetUserId()));
    }

    [HttpGet("track/{refId}")]
    [ProducesResponseType<CTrackingResponse>(StatusCodes.Status200OK, "application/json")]
    [EndpointSummary("8. Tracking Shipment. The tracking result will be not real time. Input is referenceId that return when you create a shipment")]
    public async Task<IActionResult> GetTrack(string? refId)
    {
        var data = await leUsService.GetTrack([$"{refId}"], User.GetUserId());
        return Ok(data);
    }
    
    [HttpPost("get-rate")]
    [ProducesResponseType<Result<AddEditShipmentResponse>>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    [EndpointSummary("9. Get Price of Shipment")]
    public async Task<IHttpResult> GetRatePost(ShipmentDto request)
    {
        var dConvert = new CShipmentDto();
        request.Adapt(dConvert);
        if (dConvert.ShipmentStatus != 0)
        {
            dConvert.ShipmentStatus = 0;
        }

        var valResult = await shipmentVal.ValidateAsync(dConvert);
        if (valResult.IsValid)
        {
            var services = await _mediator!.Send(new GetAllServiceQuery());
            var oFind = services.FirstOrDefault(w => w.ServiceCode == dConvert.ServiceCode);
            if (oFind == null)
            {
                var problemServiceDetails = new HttpValidationProblemDetails(valResult.ToDictionary())
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Validation Failed",
                    Detail = "Not found the service.",
                    Instance = "api/shipment"
                };
                return Results.Problem(problemServiceDetails);
            }

            dConvert.ServiceCode = oFind.ServiceCode;
            dConvert.ApiName = oFind.ApiName;
            dConvert.ServiceId = $"{oFind.Id}";
            dConvert.Weight = dConvert.Boxes?.Sum(s => s.Weight) ?? 0;
            dConvert.CustomerId = await _mediator.Send(new GetAllCustomerByEmailQuery()
            {
                Email = $"{User.GetEmail()}"
            });
            var resultAddShipment = await _mediator!.Send(new AddEditShipmentCommand()
            {
                Request = new AddEditDataRequest<CShipmentDto>()
                {
                    Data = dConvert,
                    Action = ActionCommandType.View
                }
            });
            resultAddShipment.Data.ServiceCode = request.ServiceCode;
            return Results.Ok(resultAddShipment);
        }

        var problemDetails = new HttpValidationProblemDetails(valResult.ToDictionary())
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Validation Failed",
            Detail = "One or more validation errors occurred.",
            Instance = "api/shipment"
        };
        return Results.Problem(problemDetails);
    }
    [HttpGet("stores")]
    [ProducesResponseType<List<CStoreAddressDto>>(StatusCodes.Status200OK, "application/json")]
    [EndpointSummary("Get Store")]
    public async Task<IActionResult> GetStore()
    {
        var data = await _mediator!.Send(new GetAllStoreAddressQuery());
        var cusId = await _mediator.Send(new GetAllCustomerByEmailQuery()
        {
            Email = User.GetEmail()
        });
        data = cusId.IsNullOrEmpty() ? data.Where(w => w.CustomerId.IsNullOrEmpty()).ToList() : data.Where(w => w.CustomerId.IsNullOrEmpty() || w.CustomerId == cusId).ToList();
        return Ok(data);
    }

    [HttpGet("balance")]
    [ProducesResponseType<UserBalanceDto>(StatusCodes.Status200OK, "application/json")]
    [EndpointSummary("Get Balance")]
    public async Task<IActionResult> Balance()
    {
        var balance = await _mediator!.Send(new GetBalanceByUserQuery()
        {
            UserId = User.GetUserId()
        });
        
        return Ok(balance);
    }
    
    [HttpGet("get-all-balance")]
    [ProducesResponseType<UserBalanceDto>(StatusCodes.Status200OK, "application/json")]
    [EndpointSummary("Get All Balance")]
    [Authorize(Roles = RoleConstants.AccountingRole)]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> GetAllBalance()
    {
        var balances = await _mediator!.Send(new GetBalanceQuery());
        return Ok(balances);
    }
}