using Regira.Entities.Models;

namespace Regira.Fleet.SupplierTypes;

public class SupplierTypeSearchObject : SearchObject
{
    public string? Code { get; set; }
    public string? Title { get; set; }
}