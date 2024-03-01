using System.Data;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Regira.Fleet.Data;

namespace Regira.Fleet.Statistics;

public class StatisticsService(FleetContext dbContext) : IDisposable
{
    private readonly DbConnection _dbConnection = dbContext.Database.GetDbConnection();

    public async Task<IList<IDictionary<string, object?>>> VehicleTypes_Per_Month(int year)
    {
        var list = new List<dynamic>();
        await using (var cmd = _dbConnection.CreateCommand())
        {
            cmd.CommandText = VEHICLETYPES_PER_MONTH;
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
                    var carTypeCode = reader.GetString("cartype_code");
                    var total = reader.GetDecimal("total");
                    list.Add(new { month, carTypeCode, total });
                }
            }
        }

        var carTypes = list.Select(x => (string)x.carTypeCode).Distinct().OrderBy(x => x).ToArray();
        var stats = list
            .GroupBy(x => (int)x.month)
            .OrderBy(x => x.Key)
            .Select(x =>
            {
                var dic = new Dictionary<string, object?>();
                dic.Add("Maand", FormatMonth(x.Key, year));
                dic.Add("JaarTotaal", x.Sum(v => (decimal?)v.total ?? 0));
                foreach (var carType in carTypes)
                {
                    dic.Add(carType, null);
                }
                foreach (var v in x)
                {
                    dic[(string)v.carTypeCode] = v.total;
                }
                return (IDictionary<string, object?>)dic;
            })
            .ToList();

        AddSums(stats);

        return stats;
    }
    public async Task<IList<IDictionary<string, object?>>> Vehicles_Per_VehicleType_Per_Month(string carTypeCode, int year)
    {
        var list = new List<dynamic>();
        await using (var cmd = _dbConnection.CreateCommand())
        {
            cmd.CommandText = VEHICLES_PER_VEHICLETYPES_PER_MONTH;
            var yearParam = cmd.CreateParameter();
            yearParam.ParameterName = "year";
            yearParam.Value = year;
            cmd.Parameters.Add(yearParam);
            var carTypeCodeParam = cmd.CreateParameter();
            carTypeCodeParam.ParameterName = "car_type_code";
            carTypeCodeParam.Value = carTypeCode;
            cmd.Parameters.Add(carTypeCodeParam);
            await _dbConnection.OpenAsync();
            await using (var reader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
            {
                while (await reader.ReadAsync())
                {
                    var month = reader.GetInt32("month");
                    var brand = reader.IsDBNull("brand_code") ? string.Empty : reader.GetString("brand_code");
                    var model = reader.IsDBNull("model") ? string.Empty : reader.GetString("model");
                    var car = $"{reader.GetString("car_code")} {brand} {model}";
                    var total = reader.IsDBNull("total") ? 0m : reader.GetDecimal("total");
                    list.Add(new { month, carTypeCode, car, total });
                }
            }
        }


        var cars = list.Select(x => (string)x.car).Distinct().OrderBy(x => x).ToArray();
        var stats = list
            .GroupBy(x => (int)x.month)
            .OrderBy(x => x.Key)
            .Select(x =>
            {
                var dic = new Dictionary<string, object?>();
                dic.Add("Maand", FormatMonth(x.Key, year));
                dic.Add("Wagentype", carTypeCode);
                dic.Add("JaarTotaal", x.Sum(v => (decimal?)v.total ?? 0));
                foreach (var car in cars)
                {
                    dic.Add(car, null);
                }
                foreach (var v in x)
                {
                    dic[(string)v.car] = v.total;
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
                    var brand = reader.IsDBNull("brand_code") ? string.Empty : reader.GetString("brand_code");
                    var model = reader.IsDBNull("model") ? string.Empty : reader.GetString("model");
                    var car = $"{reader.GetString("car_code")} {brand} {model}";
                    var total = reader.IsDBNull("total") ? 0m : reader.GetDecimal("total");
                    list.Add(new { month, car, total });
                }
            }
        }

        var months = list.Select(x => (int)x.month).Distinct().OrderBy(x => x).ToArray();
        var stats = list
            .OrderBy(x => x.car)
            .GroupBy(x => x.car)
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
                    var intervention_operator = reader.GetString("intervention_operator");
                    var total = reader.GetDecimal("total");
                    list.Add(new { month, intervention_operatorId, intervention_operator, total });
                }
            }
        }

        var months = list.Select(x => (int)x.month).Distinct().OrderBy(x => x).ToArray();
        var intervention_operators = list
            .GroupBy(x => x.intervention_operatorId)
            .ToDictionary(x => x.Key, x => x.First().intervention_operator);
        var stats = list
            .OrderBy(x => x.intervention_operator)
            .GroupBy(x => x.intervention_operatorId)
            .Select(x =>
            {
                var dic = new Dictionary<string, object?>
                {
                    {"Leverancier", intervention_operators[x.Key]},
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
                    var carTypeCode = reader.GetString("car_type");
                    var total = reader.GetDecimal("total");
                    list.Add(new { month, interventionTypeCode, carTypeCode, total });
                }
            }
        }

        var months = list.Select(x => (int)x.month).Distinct().OrderBy(x => x).ToArray();
        var stats = list
            .GroupBy(x => x.interventionTypeCode + "|" + x.carTypeCode)
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
    const string VEHICLETYPES_PER_MONTH = @"SELECT year, month, cartype_code, total
FROM (
	SELECT Year(b.invoice_date) year, Month(b.invoice_date) month, ct.code cartype_code, Sum(b.price_incl) total
	FROM bookings b
	INNER JOIN cars c ON b.car_id = c.id
	LEFT JOIN car_types ct ON c.car_type_id = ct.id
	GROUP BY 1, 2, 3
) q
WHERE year = @year
ORDER BY 1, 3, 2;";
    const string VEHICLES_PER_VEHICLETYPES_PER_MONTH = @"SELECT year, month, car_code, brand_code, model, total
FROM (
	SELECT Year(b.invoice_date) year, Month(b.invoice_date) month, ct.code car_type_code, c.code car_code, cb.code brand_code, c.model, Sum(b.price_incl) total
	FROM bookings b
	INNER JOIN cars c ON b.car_id = c.id
	LEFT JOIN car_types ct ON c.car_type_id = ct.id
    LEFT JOIN brands cb ON c.brand_id = cb.id
	GROUP BY 1, 2, 3, 4, 5, 6
) q
WHERE year = @year
AND car_type_code = @car_type_code
ORDER BY 1, 2, 3;";
    const string VEHICLES_PER_MONTH = @"SELECT year, month, car_code, brand_code, model, total
FROM (
	SELECT Year(b.invoice_date) year, Month(b.invoice_date) month, c.code car_code, cb.code brand_code, c.model, Sum(b.price_incl) total
	FROM bookings b
	INNER JOIN cars c ON b.car_id = c.id
    LEFT JOIN brands cb ON c.brand_id = cb.id
	GROUP BY 1, 2, 3, 4, 5
) q
WHERE year = @year
ORDER BY 1, 2, 3";
    const string INTERVENTION_TYPES_PER_MONTH = @"SELECT year, month, interventiontype_code, total
FROM (
	SELECT Year(b.invoice_date) year, Month(b.invoice_date) month, it.code interventiontype_code, Sum(b.price_incl) total
	FROM bookings b
    INNER JOIN intervention_types it ON b.intervention_type_id = it.id
	GROUP BY 1, 2, 3
) q
WHERE year = @year
ORDER BY 1, 3, 2;";
    const string INTERVENTION_OPERATORS_PER_MONTH = @"SELECT year, month, intervention_operator_id, intervention_operator, total
FROM (
	SELECT Year(b.invoice_date) year, Month(b.invoice_date) month, s.id intervention_operator_id, s.name intervention_operator, Coalesce(Sum(b.price_incl),0) total
	FROM bookings b
    INNER JOIN intervention_operators s ON b.intervention_operator_id = s.id
	GROUP BY 1, 2, 3, 4
) q
WHERE year = @year
ORDER BY 1, 3;";
    const string INTERVENTIONTYPES_AND_VEHICLETYPES_PER_MONTH = @"SELECT year, month, interventiontype_code, car_type, total
FROM (
	SELECT Year(b.invoice_date) year, Month(b.invoice_date) month, it.code interventiontype_code, ct.code car_type, Sum(b.price_incl) total
	FROM bookings b
    INNER JOIN intervention_types it ON b.intervention_type_id = it.id
	INNER JOIN cars c ON b.car_id = c.id
	LEFT JOIN car_types ct ON c.car_type_id = ct.id
	GROUP BY 1, 2, 3, 4
) q
WHERE year = @year
ORDER BY 1, 3, 2, 4;";
    #endregion

    public void Dispose()
    {
        _dbConnection.Dispose();
    }
}