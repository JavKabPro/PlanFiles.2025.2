using CsvHelper.Configuration.Attributes;

namespace PlainFiles.Core;

public class Person
{
    public int Id { get; set; }
    [Name("Nombre", "Name")]
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public decimal Balance { get; set; }

}
