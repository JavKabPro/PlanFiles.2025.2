using PlanFiles.Core;
using System.ComponentModel.Design;

var textFile = new SimpleTextFile(".//data//data.txt");
var lines = textFile.ReadAllLines().ToList();
var opc = string.Empty;

do
{
    opc = Menu();
    switch (opc)
    {
        case "1":
            Console.WriteLine("Contenido del archivo: ");
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
            }
            else 
            {
                Console.WriteLine("No se agregó ninguna línea");
            }
            break;
        case "3":
            textFile.WriteAllLines(lines.ToArray());
            Console.WriteLine("Archivo guardado.");
            break;
        case "0":
            Console.WriteLine("Saliendo...");
            break;
        default:
            Console.WriteLine("Opción no válido.");
            break;
    }
} while (opc != "0");

string Menu()
{
    Console.WriteLine("1. Mostrar");
    Console.WriteLine("2. Adicionar");
    Console.WriteLine("3. Guardar");
    Console.WriteLine("0. Salir");
    Console.Write("Su opción es: ");
    return Console.ReadLine() ?? string.Empty;
}