using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace PlainFiles.Core;

public class NugetCsvHelper
{
    public void Write(string path, IEnumerable<Person> people)
    {
        using var sw = new StreamWriter(path);
        using var cw = new CsvWriter(sw, CultureInfo.InvariantCulture);
        cw.WriteRecords(people);
    }

    public IEnumerable<Person> Read(string path)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ","
        };
        using var sr = new StreamReader(path);
        using var cr = new CsvReader(sr, config);
        var records = cr.GetRecords<Person>().ToList();
        return records;
    }
}