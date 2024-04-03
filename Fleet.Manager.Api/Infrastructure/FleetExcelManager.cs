using OfficeOpenXml;
using Regira.IO.Abstractions;
using Regira.IO.Extensions;
using Regira.Office.Excel;
using Regira.Office.Excel.Abstractions;
using System.Data;

namespace Regira.Fleet.Manager.Api.Infrastructure;

public class FleetExcelManager : IExcelManager
{
    public IEnumerable<ExcelSheet> Read(IBinaryFile input, string[]? headers = null)
    {
        throw new NotImplementedException();
    }

    public IMemoryFile Create(ExcelSheet sheet)
    {
        return Create(new[] { sheet });
    }
    public IMemoryFile Create(IEnumerable<ExcelSheet> sheets)
    {
        var package = new ExcelPackage();
        foreach (var sheet in sheets)
        {
            var excelSheet = package.Workbook.Worksheets.Add(sheet.Name);
            var data = sheet.Data!.Cast<IDictionary<string, object?>>().ToList();
            excelSheet.Cells["A1"].LoadFromArrays(data.Select(d => d.Keys.ToArray()));
            excelSheet.Row(1).Style.Font.Bold = true;

            excelSheet.Cells["A2"].LoadFromArrays(data.Take(sheet.Data!.Count - 1).Select(d => d.Values.Select(v => v ?? 0).ToArray()));
            excelSheet.Cells[$"A{sheet.Data.Count + 1}"].Value = "Totalen";

            var keys = data.First().Keys.ToArray();
            for (var i = 0; i < keys.Length - 1; i++)
            {
                var column = (char)(i + 66);
                excelSheet.Cells[$"{column}{sheet.Data.Count + 1}"].Formula = $"=SUM({column}1:{column}{sheet.Data.Count})";
            }

            excelSheet.Row(sheet.Data.Count + 1).Style.Font.Bold = true;

            excelSheet.Cells[excelSheet.Dimension.Address].AutoFitColumns();
        }

        package.Save();
        return package.Stream.ToMemoryFile();
    }

    public IMemoryFile Create(DataSet dataSet)
    {
        throw new NotImplementedException();
    }
}