namespace Regira.Fleet.Core.Abstractions;

public interface ITenantContext
{
    string? TenantId { get; }
}