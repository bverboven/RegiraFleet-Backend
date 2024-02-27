using Regira.Entities.Models;

namespace Regira.Fleet.Suppliers;

public class SupplierSearchObject : SearchObject
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? VATNumber { get; set; }
    public string? Phone { get; set; }
}