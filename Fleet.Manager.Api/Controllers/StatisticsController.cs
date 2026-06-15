using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Regira.Fleet.Core.Constants;
using Regira.Fleet.Statistics;
using Regira.IO.Extensions;
using Regira.IO.Utilities;
using Regira.Office.Excel.Abstractions;
using Regira.Office.Excel.Models;

namespace Regira.Fleet.Manager.Api.Controllers;

[ApiController]
[Route("stats")]
[Authorize(FleetPolicies.CanReadPolicy)]
public class StatisticsController(StatisticsService statsService, IExcelService excelManager) : ControllerBase
{

    [HttpGet("per-vehicletype/{year}")]
    public async Task<IActionResult> VehicleTypes_Per_Month(int? year = null, bool asTable = true)
    {
        var stats = await statsService.VehicleTypes_Per_Month(year ?? DateTime.Now.Year);

        if (asTable)
        {
            return Ok(statsService.DicListToTable(stats));
        }

        return Ok(stats);
    }
    [HttpGet("per-vehicletype/{year}/xlsx")]
    public async Task<IActionResult> VehicleTypes_Per_Month_Excel(int? year = null)
    {
        year ??= DateTime.Now.Year;
        var stats = await statsService.VehicleTypes_Per_Month(year.Value);
        return await GetExcel(stats, "Wagentypes per maand", $"wagentypes-per-maand-{year}.xlsx");
    }

    [HttpGet("per-vehicle/{vehicleTypeId}/{year}")]
    public async Task<IActionResult> Vehicles_Per_VehicleType_Per_Month(int vehicleTypeId, int? year = null, bool asTable = true)
    {
        var stats = await statsService.Vehicles_Per_VehicleType_Per_Month(vehicleTypeId, year ?? DateTime.Now.Year);

        if (asTable)
        {
            return Ok(statsService.DicListToTable(stats));
        }

        return Ok(stats);
    }
    [HttpGet("per-vehicle/{vehicleTypeId}/{year}/xlsx")]
    public async Task<IActionResult> Vehicles_Per_VehicleType_Per_Month_Excel(int vehicleTypeId, int? year = null)
    {
        year ??= DateTime.Now.Year;
        var stats = await statsService.Vehicles_Per_VehicleType_Per_Month(vehicleTypeId, year.Value);
        return await GetExcel(stats, "Wagens per type per maand", $"wagens-per-type-per-maand-{year}.xlsx");
    }

    [HttpGet("per-vehicle/{year}")]
    public async Task<IActionResult> Vehicles_Per_Month(int? year = null, bool asTable = true)
    {
        var stats = await statsService.Vehicles_Per_Month(year ?? DateTime.Now.Year);

        if (asTable)
        {
            return Ok(statsService.DicListToTable(stats));
        }

        return Ok(stats);
    }
    [HttpGet("per-vehicle/{year}/xlsx")]
    public async Task<IActionResult> Vehicles_Per_Month_Excel(int? year = null)
    {
        year ??= DateTime.Now.Year;
        var stats = await statsService.Vehicles_Per_Month(year.Value);
        return await GetExcel(stats, "Wagens per maand", $"wagens-per-maand-{year}.xlsx");
    }

    [HttpGet("per-interventiontype/{year}")]
    public async Task<IActionResult> InterventionTypes_Per_Month(int? year = null, bool asTable = true)
    {
        var stats = await statsService.InterventionTypes_Per_Month(year ?? DateTime.Now.Year);

        if (asTable)
        {
            return Ok(statsService.DicListToTable(stats));
        }

        return Ok(stats);
    }
    [HttpGet("per-interventiontype/{year}/xlsx")]
    public async Task<IActionResult> InterventionTypes_Per_Month_Excel(int? year = null)
    {
        year ??= DateTime.Now.Year;
        var stats = await statsService.InterventionTypes_Per_Month(year.Value);
        return await GetExcel(stats, "Interventietypes per maand", $"interventietypes-per-maand-{year}.xlsx");
    }

    [HttpGet("per-intervention-operator/{year}")]
    public async Task<IActionResult> InterventionOperators_Per_Month(int? year = null, bool asTable = true)
    {
        var stats = await statsService.InterventionOperators_Per_Month(year ?? DateTime.Now.Year);

        if (asTable)
        {
            return Ok(statsService.DicListToTable(stats));
        }

        return Ok(stats);
    }
    [HttpGet("per-intervention-operator/{year}/xlsx")]
    public async Task<IActionResult> InterventionOperators_Per_Month_Excel(int? year = null)
    {
        year ??= DateTime.Now.Year;
        var stats = await statsService.InterventionOperators_Per_Month(year.Value);
        return await GetExcel(stats, "Leveranciers per maand", $"leveranciers-per-maand-{year}.xlsx");
    }

    [HttpGet("per-interventiontype-and-vehicletype/{year}")]
    public async Task<IActionResult> InterventionTypes_And_VehicleTypes_Per_Month(int? year = null, bool asTable = true)
    {
        var stats = await statsService.InterventionTypes_And_VehicleTypes_Per_Month(year ?? DateTime.Now.Year);

        if (asTable)
        {
            return Ok(statsService.DicListToTable(stats));
        }

        return Ok(stats);
    }
    [HttpGet("per-interventiontype-and-vehicletype/{year}/xlsx")]
    public async Task<IActionResult> InterventionTypes_And_VehicleTypes_Per_Month_Excel(int? year = null)
    {
        year ??= DateTime.Now.Year;
        var stats = await statsService.InterventionTypes_And_VehicleTypes_Per_Month(year.Value);
        return await GetExcel(stats, "Interventietypes+Wagentypes per maand", $"interventietypes-en-wagentypes-per-maand-{year}.xlsx");
    }


    protected async Task<FileResult> GetExcel(IList<IDictionary<string, object?>> stats, string sheetName, string filename)
    {
        var sheet = new ExcelSheet { Data = stats.Cast<object>().ToList(), Name = sheetName };
        using var excelFile = await excelManager.Create([sheet]);
        Response.Headers.Append("Access-Control-Expose-Headers", "content-disposition");
        return File(excelFile.GetBytes()!, ContentTypeUtility.GetContentType(filename), filename);
    }
}