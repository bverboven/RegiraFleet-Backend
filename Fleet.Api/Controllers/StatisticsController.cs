using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Regira.Fleet.Authentication;
using Regira.Fleet.Statistics;
using Regira.IO.Extensions;
using Regira.IO.Utilities;
using Regira.Office.Excel;
using Regira.Office.Excel.Abstractions;

namespace Regira.Fleet.Api.Controllers;

[ApiController]
[Route("stats")]
[Authorize(FleetConstants.CanReadPolicy)]
public class StatisticsController(StatisticsService statsService, IExcelManager excelManager) : ControllerBase
{

    [HttpGet("per-cartype/{year}")]
    public async Task<IActionResult> CarTypes_Per_Month(int? year = null, bool asTable = true)
    {
        var stats = await statsService.CarTypes_Per_Month(year ?? DateTime.Now.Year);

        if (asTable)
        {
            return Ok(statsService.DicListToTable(stats));
        }

        return Ok(stats);
    }
    [HttpGet("per-cartype/{year}/xlsx")]
    public async Task<IActionResult> CarTypes_Per_Month_Excel(int? year = null)
    {
        year ??= DateTime.Now.Year;
        var stats = await statsService.CarTypes_Per_Month(year.Value);
        return GetExcel(stats, "Wagentypes per maand", $"wagentypes-per-maand-{year}.xlsx");
    }

    [HttpGet("per-car/{carTypeCode}/{year}")]
    public async Task<IActionResult> Cars_Per_CarType_Per_Month(string carTypeCode, int? year = null, bool asTable = true)
    {
        var stats = await statsService.Cars_Per_CarType_Per_Month(carTypeCode, year ?? DateTime.Now.Year);

        if (asTable)
        {
            return Ok(statsService.DicListToTable(stats));
        }

        return Ok(stats);
    }
    [HttpGet("per-car/{carTypeCode}/{year}/xlsx")]
    public async Task<IActionResult> Cars_Per_CarType_Per_Month_Excel(string carTypeCode, int? year = null)
    {
        year ??= DateTime.Now.Year;
        var stats = await statsService.Cars_Per_CarType_Per_Month(carTypeCode, year.Value);
        return GetExcel(stats, "Wagens per type per maand", $"wagens-per-type-per-maand-{year}.xlsx");
    }

    [HttpGet("per-car/{year}")]
    public async Task<IActionResult> Cars_Per_Month(int? year = null, bool asTable = true)
    {
        var stats = await statsService.Cars_Per_Month(year ?? DateTime.Now.Year);

        if (asTable)
        {
            return Ok(statsService.DicListToTable(stats));
        }

        return Ok(stats);
    }
    [HttpGet("per-car/{year}/xlsx")]
    public async Task<IActionResult> Cars_Per_Month_Excel(int? year = null)
    {
        year ??= DateTime.Now.Year;
        var stats = await statsService.Cars_Per_Month(year.Value);
        return GetExcel(stats, "Wagens per maand", $"wagens-per-maand-{year}.xlsx");
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
        return GetExcel(stats, "Interventietypes per maand", $"interventietypes-per-maand-{year}.xlsx");
    }

    [HttpGet("per-supplier/{year}")]
    public async Task<IActionResult> Suppliers_Per_Month(int? year = null, bool asTable = true)
    {
        var stats = await statsService.Suppliers_Per_Month(year ?? DateTime.Now.Year);

        if (asTable)
        {
            return Ok(statsService.DicListToTable(stats));
        }

        return Ok(stats);
    }
    [HttpGet("per-supplier/{year}/xlsx")]
    public async Task<IActionResult> Suppliers_Per_Month_Excel(int? year = null)
    {
        year ??= DateTime.Now.Year;
        var stats = await statsService.Suppliers_Per_Month(year.Value);
        return GetExcel(stats, "Leveranciers per maand", $"leveranciers-per-maand-{year}.xlsx");
    }

    [HttpGet("per-interventiontype-and-cartype/{year}")]
    public async Task<IActionResult> InterventionTypes_And_CarTypes_Per_Month(int? year = null, bool asTable = true)
    {
        var stats = await statsService.InterventionTypes_And_CarTypes_Per_Month(year ?? DateTime.Now.Year);

        if (asTable)
        {
            return Ok(statsService.DicListToTable(stats));
        }

        return Ok(stats);
    }
    [HttpGet("per-interventiontype-and-cartype/{year}/xlsx")]
    public async Task<IActionResult> InterventionTypes_And_CarTypes_Per_Month_Excel(int? year = null)
    {
        year ??= DateTime.Now.Year;
        var stats = await statsService.InterventionTypes_And_CarTypes_Per_Month(year.Value);
        return GetExcel(stats, "Interventietypes+Wagentypes per maand", $"interventietypes-en-wagentypes-per-maand-{year}.xlsx");
    }


    protected FileResult GetExcel(IList<IDictionary<string, object?>> stats, string sheetName, string filename)
    {
        var sheet = new ExcelSheet { Data = stats.Cast<object>().ToList(), Name = sheetName };
        using var excelFile = excelManager.Create(sheet);
        Response.Headers.Append("Access-Control-Expose-Headers", "content-disposition");
        return File(excelFile.GetBytes()!, ContentTypeUtility.GetContentType(filename), filename);
    }
}