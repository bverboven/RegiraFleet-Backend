using Microsoft.EntityFrameworkCore;
using Regira.Fleet.Core.Abstractions;
using Regira.Fleet.Data;
using System.Data;
using System.Data.Common;

namespace Regira.Fleet.Statistics;

public class StatisticsService(FleetContext dbContext, IClientContext clientContext) : IDisposable
{
    private readonly DbConnection _dbConnection = dbContext.Database.GetDbConnection();

    public async Task<IList<IDictionary<string, object?>>> VehicleTypes_Per_Month(int year)
    {
        var list = new List<dynamic>();
        await using (var cmd = _dbConnection.CreateCommand())
        {
            cmd.CommandText = VEHICLETYPES_PER_MONTH;
            var clientParam = cmd.CreateParameter();
            clientParam.ParameterName = "clientId";
            clientParam.Value = clientContext.ClientId;
            cmd.Parameters.Add(clientParam);
            var yearParam = cmd.CreateParameter();
            yearParam.ParameterName = "year";
            yearParam.Value = year;
            cmd.Parameters.Add(yearParam);
            await _dbConnection.OpenAsync();
            await using (var reader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
            {
                while (await reader.ReadAsync())
                {
                    var month = reader.GetInt32("month");
                    var vehicleTypeCode = reader.GetString("vehicle_type_code");
                    var total = reader.GetDecimal("total");
                    list.Add(new { month, vehicleTypeCode, total });
                }
            }
        }

        var vehicleTypes = list.Select(x => (string)x.vehicleTypeCode).Distinct().OrderBy(x => x).ToArray();
        var stats = list
            .GroupBy(x => (int)x.month)
            .OrderBy(x => x.Key)
            .Select(x =>
            {
                var dic = new Dictionary<string, object?>();
                dic.Add("Maand", FormatMonth(x.Key, year));
                dic.Add("JaarTotaal", x.Sum(v => (decimal?)v.total ?? 0));
                foreach (var vehicleType in vehicleTypes)
                {
                    dic.Add(vehicleType, null);
                }
                foreach (var v in x)
                {
                    dic[(string)v.vehicleTypeCode] = v.total;
                }
                return (IDictionary<string, object?>)dic;
            })
            .ToList();

        AddSums(stats);

        return stats;
    }
    public async Task<IList<IDictionary<string, object?>>> Vehicles_Per_VehicleType_Per_Month(int vehicleTypeId, int year)
    {
        var vehicleType = (await dbContext.VehicleTypes.FindAsync(vehicleTypeId))?.Code;
        var list = new List<dynamic>();
        await using (var cmd = _dbConnection.CreateCommand())
        {
            cmd.CommandText = VEHICLES_PER_VEHICLETYPES_PER_MONTH;
            var clientParam = cmd.CreateParameter();
            clientParam.ParameterName = "clientId";
            clientParam.Value = clientContext.ClientId;
            cmd.Parameters.Add(clientParam);
            var yearParam = cmd.CreateParameter();
            yearParam.ParameterName = "year";
            yearParam.Value = year;
            cmd.Parameters.Add(yearParam);
            var vehicleTypeIdParam = cmd.CreateParameter();
            vehicleTypeIdParam.ParameterName = nameof(vehicleTypeId);
            vehicleTypeIdParam.Value = vehicleTypeId;
            cmd.Parameters.Add(vehicleTypeIdParam);
            await _dbConnection.OpenAsync();
            await using (var reader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
            {
                while (await reader.ReadAsync())
                {
                    var month = reader.GetInt32("month");
                    var brand = reader.IsDBNull("vehicle_brand_code") ? string.Empty : reader.GetString("vehicle_brand_code");
                    var model = reader.IsDBNull("model") ? string.Empty : reader.GetString("model");
                    var vehicle = $"{reader.GetString("vehicle_code")} {brand} {model}";
                    var total = reader.IsDBNull("total") ? 0m : reader.GetDecimal("total");
                    list.Add(new { month, vehicleType, vehicle, total });
                }
            }
        }


        var vehicles = list.Select(x => (string)x.vehicle).Distinct().OrderBy(x => x).ToArray();
        var stats = list
            .GroupBy(x => (int)x.month)
            .OrderBy(x => x.Key)
            .Select(x =>
            {
                var dic = new Dictionary<string, object?>();
                dic.Add("Maand", FormatMonth(x.Key, year));
                dic.Add("Wagentype", vehicleType);
                dic.Add("JaarTotaal", x.Sum(v => (decimal?)v.total ?? 0));
                foreach (var vehicle in vehicles)
                {
                    dic.Add(vehicle, null);
                }
                foreach (var v in x)
                {
                    dic[(string)v.vehicle] = v.total;
                }
                return (IDictionary<string, object?>)dic;
            })
            .ToList();

        AddSums(stats);

        return stats;
    }
    public async Task<IList<IDictionary<string, object?>>> Vehicles_Per_Month(int year)
    {
        var list = new List<dynamic>();
        await using (var cmd = _dbConnection.CreateCommand())
        {
            cmd.CommandText = VEHICLES_PER_MONTH;
            var clientParam = cmd.CreateParameter();
            clientParam.ParameterName = "clientId";
            clientParam.Value = clientContext.ClientId;
            cmd.Parameters.Add(clientParam);
            var yearParam = cmd.CreateParameter();
            yearParam.ParameterName = "year";
            yearParam.Value = year;
            cmd.Parameters.Add(yearParam);
            await _dbConnection.OpenAsync();
            await using (var reader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
            {
                while (await reader.ReadAsync())
                {
                    var month = reader.GetInt32("month");
                    var brand = reader.IsDBNull("vehicle_brand_code") ? string.Empty : reader.GetString("vehicle_brand_code");
                    var model = reader.IsDBNull("model") ? string.Empty : reader.GetString("model");
                    var vehicle = $"{reader.GetString("vehicle_code")} {brand} {model}";
                    var total = reader.IsDBNull("total") ? 0m : reader.GetDecimal("total");
                    list.Add(new { month, vehicle, total });
                }
            }
        }

        var months = list.Select(x => (int)x.month).Distinct().OrderBy(x => x).ToArray();
        var stats = list
            .OrderBy(x => x.vehicle)
            .GroupBy(x => x.vehicle)
            .Select(x =>
            {
                var dic = new Dictionary<string, object?>
                {
                    {"Wagen", x.Key},
                    {"JaarTotaal", x.Sum(v => (decimal?) v.total ?? 0)}
                };
                AddMonthTotals(dic, year, months, x);
                return (IDictionary<string, object?>)dic;
            })
            .ToList();

        AddSums(stats);

        return stats;
    }
    public async Task<IList<IDictionary<string, object?>>> InterventionTypes_Per_Month(int year)
    {
        var list = new List<dynamic>();
        await using (var cmd = _dbConnection.CreateCommand())
        {
            cmd.CommandText = INTERVENTION_TYPES_PER_MONTH;
            var clientParam = cmd.CreateParameter();
            clientParam.ParameterName = "clientId";
            clientParam.Value = clientContext.ClientId;
            cmd.Parameters.Add(clientParam);
            var yearParam = cmd.CreateParameter();
            yearParam.ParameterName = "year";
            yearParam.Value = year;
            cmd.Parameters.Add(yearParam);
            await _dbConnection.OpenAsync();
            await using (var reader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
            {
                while (await reader.ReadAsync())
                {
                    var month = reader.GetInt32("month");
                    var interventionTypeCode = reader.GetString("interventiontype_code");
                    var total = reader.GetDecimal("total");
                    list.Add(new { month, interventionTypeCode, total });
                }
            }
        }

        var months = list.Select(x => (int)x.month).Distinct().OrderBy(x => x).ToArray();
        var stats = list
            .GroupBy(x => x.interventionTypeCode)
            .OrderBy(x => x.Key)
            .Select(x =>
            {
                var dic = new Dictionary<string, object?>();
                dic.Add("Interventie", (string)x.Key);
                dic.Add("JaarTotaal", x.Sum(v => (decimal?)v.total ?? 0));
                AddMonthTotals(dic, year, months, x);
                return (IDictionary<string, object?>)dic;
            })
            .ToList();

        AddSums(stats);

        return stats;
    }
    public async Task<IList<IDictionary<string, object?>>> InterventionOperators_Per_Month(int year)
    {
        var list = new List<dynamic>();
        await using (var cmd = _dbConnection.CreateCommand())
        {
            cmd.CommandText = INTERVENTION_OPERATORS_PER_MONTH;
            var clientParam = cmd.CreateParameter();
            clientParam.ParameterName = "clientId";
            clientParam.Value = clientContext.ClientId;
            cmd.Parameters.Add(clientParam);
            var yearParam = cmd.CreateParameter();
            yearParam.ParameterName = "year";
            yearParam.Value = year;
            cmd.Parameters.Add(yearParam);
            await _dbConnection.OpenAsync();
            await using (var reader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
            {
                while (await reader.ReadAsync())
                {
                    var month = reader.GetInt32("month");
                    var intervention_operatorId = reader.GetInt32("intervention_operator_id");
                    var supplier = reader.GetString("supplier");
                    var total = reader.GetDecimal("total");
                    list.Add(new { month, intervention_operatorId, supplier, total });
                }
            }
        }

        var months = list.Select(x => (int)x.month).Distinct().OrderBy(x => x).ToArray();
        var suppliers = list
            .GroupBy(x => x.intervention_operatorId)
            .ToDictionary(x => x.Key, x => x.First().supplier);
        var stats = list
            .OrderBy(x => x.supplier)
            .GroupBy(x => x.intervention_operatorId)
            .Select(x =>
            {
                var dic = new Dictionary<string, object?>
                {
                    {"Leverancier", suppliers[x.Key]},
                    {"JaarTotaal", x.Sum(v => (decimal?) v.total ?? 0)}
                };
                AddMonthTotals(dic, year, months, x);
                return (IDictionary<string, object?>)dic;
            })
            .ToList();

        AddSums(stats);

        return stats;
    }
    public async Task<IList<IDictionary<string, object?>>> InterventionTypes_And_VehicleTypes_Per_Month(int year)
    {
        var list = new List<dynamic>();
        await using (var cmd = _dbConnection.CreateCommand())
        {
            cmd.CommandText = INTERVENTIONTYPES_AND_VEHICLETYPES_PER_MONTH;
            var clientParam = cmd.CreateParameter();
            clientParam.ParameterName = "clientId";
            clientParam.Value = clientContext.ClientId;
            cmd.Parameters.Add(clientParam);
            var yearParam = cmd.CreateParameter();
            yearParam.ParameterName = "year";
            yearParam.Value = year;
            cmd.Parameters.Add(yearParam);
            await _dbConnection.OpenAsync();
            await using (var reader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
            {
                while (await reader.ReadAsync())
                {
                    var month = reader.GetInt32("month");
                    var interventionTypeCode = reader.GetString("interventiontype_code");
                    var vehicleTypeCode = reader.GetString("vehicle_type");
                    var total = reader.GetDecimal("total");
                    list.Add(new { month, interventionTypeCode, vehicleTypeCode, total });
                }
            }
        }

        var months = list.Select(x => (int)x.month).Distinct().OrderBy(x => x).ToArray();
        var stats = list
            .GroupBy(x => x.interventionTypeCode + "|" + x.vehicleTypeCode)
            .OrderBy(x => x.Key)
            .Select(x =>
            {
                var dic = new Dictionary<string, object?>
                {
                    {"Interventie", ((string) x.Key).Split('|').First()},
                    {"Wagentype", ((string) x.Key).Split('|').Last()},
                    {"JaarTotaal", x.Sum(v => (decimal?) v.total ?? 0)}
                };
                AddMonthTotals(dic, year, months, x);
                return (IDictionary<string, object?>)dic;
            })
            .ToList();

        AddSums(stats);

        return stats;
    }


    protected string FormatMonth(int month, int year)
    {
        return $"{year}-{month.ToString().PadLeft(2, '0')}";
    }
    protected void AddMonthTotals(IDictionary<string, object?> dic, int year, IList<int> months, dynamic x)
    {
        foreach (var month in months)
        {
            dic.Add(FormatMonth(month, year), null);
        }
        foreach (var v in x)
        {
            dic[FormatMonth(v.month, year)] = v.total;
        }
    }
    protected void AddSums(IList<IDictionary<string, object?>> stats, int skip = 1)
    {
        if (!stats.Any())
        {
            return;
        }

        var keys = stats.First().Keys;
        var totals = new Dictionary<string, object?> { [keys.First()] = "Totaal" };
        foreach (var key in keys.Skip(skip))
        {
            var sum = stats.Sum(s => s[key] is decimal ? (decimal?)s[key] : null);
            if ((sum ?? 0) > 0)
            {
                totals[key] = sum;
            }
            else
            {
                var distinctValues = stats.Select(s => s[key]).Distinct().ToArray();
                if (distinctValues.Length == 1)
                {
                    totals[key] = distinctValues.First();
                }
                else
                {
                    totals[key] = "***";
                }
            }
        }

        stats.Add(totals);
    }


    public object[,] DicListToTable(IList<IDictionary<string, object?>> dicList)
    {
        var headers = dicList
            .SelectMany(dic => dic.Keys).Distinct().ToArray();

        var table = new object[dicList.Count + 1, headers.Length];
        for (var i = 0; i < headers.Length; i++)
        {
            table[0, i] = headers[i];
        }
        for (var i = 0; i < headers.Length; i++)
        {
            for (var j = 1; j <= dicList.Count; j++)
            {
                var header = headers[i];
                var dic = dicList[j - 1];
                if (!string.IsNullOrWhiteSpace(header) && dic.TryGetValue(header, out var value))
                {
                    table[j, i] = value!;
                }
            }
        }

        return table;
    }

    #region SQL
    const string VEHICLETYPES_PER_MONTH = @"SELECT year, month, vehicle_type_code, total
FROM stats_vehicletypes_per_month
WHERE client_id = @clientId
AND year = @year;";
    const string VEHICLES_PER_VEHICLETYPES_PER_MONTH = @"SELECT year, month, vehicle_code, vehicle_brand_code, model, total
FROM stats_vehicles_per_vehicletypes_per_month
WHERE client_id = @clientId
AND year = @year
AND vehicle_type_id = @vehicleTypeId;";
    const string VEHICLES_PER_MONTH = @"SELECT year, month, vehicle_code, vehicle_brand_code, model, total
FROM stats_vehicles_per_month
WHERE client_id = @clientId
AND year = @year;";
    const string INTERVENTION_TYPES_PER_MONTH = @"SELECT year, month, interventiontype_code, total
FROM stats_interventiontypes_per_month
WHERE client_id = @clientId
AND year = @year;";
    const string INTERVENTION_OPERATORS_PER_MONTH = @"SELECT year, month, intervention_operator_id, supplier, total
FROM stats_interventionoperators_per_month
WHERE client_id = @clientId
AND year = @year;";
    const string INTERVENTIONTYPES_AND_VEHICLETYPES_PER_MONTH = @"SELECT year, month, interventiontype_code, vehicle_type, total
FROM stats_interventiontypes_and_vehicletypes_per_month
WHERE client_id = @clientId
AND year = @year;";
    #endregion

    public void Dispose()
    {
        _dbConnection.Dispose();
    }
}