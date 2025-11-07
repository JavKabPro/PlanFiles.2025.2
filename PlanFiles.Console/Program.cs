using PlanFiles.Core;

Console.WriteLine("Digite el nombre de la lista: ");
var listName = Console.ReadLine();
var manualCsv = new ManualCsvHelper();
var people = manualCsv.ReadCsv($"{listName}.csv");
var option = string.Empty;
do
{
    option = MyMenu();
    switch (option)
    {
        case "1":
            Console.Write("Digite el nombre: ");
            var name = Console.ReadLine();
            Console.Write("Digite el apellido: ");
            var lastName = Console.ReadLine();
            Console.Write("Digite la edad: ");
            var age = Console.ReadLine();
            people.Add([name ?? string.Empty, lastName ?? string.Empty, age ?? string.Empty]);
            break;

        case "2":
            Console.WriteLine("Lista de personas:");
            Console.WriteLine($"Nombres|Apellidos|Edad");
            foreach (var person in people)
            {
                Console.WriteLine($"{person[0]}|{person[1]}|{person[2]}");
            }
            break;

        case "3":
            SaveFile(people, listName);
            Console.WriteLine("Archivo guardado.");
            break;

        case "0":
            Console.WriteLine("Saliendo...");
            break;

        default:
            Console.WriteLine("Opción no válida.");
            break;
    }
} while (option != "0");
string MyMenu()
{
    Console.WriteLine("1. Adicionar.");
    Console.WriteLine("2. Mostrar.");
    Console.WriteLine("3. Grabar.");
    Console.WriteLine("4. Eliminar.");
    Console.WriteLine("5. Ordenar.");
    Console.WriteLine("0. Salir.");
    Console.Write("Seleccione una opción: ");
    return Console.ReadLine() ?? string.Empty;
}
SaveFile(people, listName);
void SaveFile(List<string[]> people, string? listName)
{
    manualCsv.WriteCsv($"{listName}.csv", people);
}









/*using PlanFiles.Core;

var textFile = new SimpleTextFile(".\\data.txt");
var lines = textFile.ReadAllLines().ToList();
var opc = string.Empty;


using var logger = new LogWriter(".\\app.log");
logger.WriteLog("INFO", "Apliación iniciada.");
    do
    {
        opc = Menu();
        switch (opc)
        {
            case "1":
                Console.WriteLine("Contenido del archivo: ");
                logger.WriteLog("INFO", "Se mostró el archivo.");
                foreach (var line in lines)
                {
                    Console.WriteLine(line);
                }
                break;
            case "2":
                Console.WriteLine("Ingrese una nueva linea de texto: ");
                var newLine = Console.ReadLine();
                if (!string.IsNullOrEmpty(newLine))
                {
                    lines.Add(newLine);
                    Console.WriteLine("Linea agregada.");
                    logger.WriteLog("INFO", $"Se agregó: {newLine}.");
                }
                else
                {
                    Console.WriteLine("No se agregó ninguna línea");
                    logger.WriteLog("INFO", $"No se agregó nada.");
                }
                break;
            case "3":
                textFile.WriteAllLines(lines.ToArray());
                Console.WriteLine("Archivo guardado.");
                logger.WriteLog("INFO", $"Se guardó el archivo.");
            break;
            case "0":
                Console.WriteLine("Saliendo...");
                break;
            default:
                Console.WriteLine("Opción no válido.");
                break;
        }
    } while (opc != "0");
    textFile.WriteAllLines(lines.ToArray());
    Console.WriteLine("Archivo guardado.");
    logger.WriteLog("INFO", $"Se guardó el archivo.");


string Menu()
{
    Console.WriteLine("1. Mostrar");
    Console.WriteLine("2. Adicionar");
    Console.WriteLine("3. Guardar");
    Console.WriteLine("0. Salir");
    Console.Write("Su opción es: ");
    return Console.ReadLine() ?? string.Empty;
}*/