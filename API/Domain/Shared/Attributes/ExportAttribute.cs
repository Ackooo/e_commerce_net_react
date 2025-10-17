namespace Domain.Shared.Attributes;

using Domain.Shared.Enums;

[AttributeUsage(AttributeTargets.Property)]
public class ExportAttribute(ExportAllowed export, string? name = null) : Attribute
{
    public ExportAllowed ExportAllowed { get; set; } = export;
    public string? Name { get; set; } = name;
}
