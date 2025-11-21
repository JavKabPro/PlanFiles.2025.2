using PlainFiles.Core;
using System.Text;
string[] lineas = File.ReadAllLines("people.csv", Encoding.UTF8);



Console.Write("Digite el nombre de la lista (por defecto 'people'): ");
var listName = Console.ReadLine();
if (string.IsNullOrEmpty(listName))
    listName = "people";

var path = $"{listName}.csv";
var helper = new NugetCsvHelper();
var people = helper.Read(path).ToList();

bool salir = false;
while (!salir)
{
    Console.WriteLine();
    Console.WriteLine("\n*** Menú ***");
    Console.WriteLine("1. Ver personas.");
    Console.WriteLine("2. Agregar persona.");
    Console.WriteLine("3. Eliminar persona");
    Console.WriteLine("4. Salir");
    Console.WriteLine("***Seleccione una opción.***");
    Console.WriteLine();

    var opcion = Console.ReadLine();

    switch (opcion)
    {
        case "1":
            ListPeople();
            break;
        case "2":
            AddPeople();
            break;
        case "3":
            DeletePeople();
            break;
        case "4":
            salir = true;
            break;
        default:
            Console.WriteLine("Opción no válida.");
            break;
    }

}

void DeletePeople()
{
    Console.Write("ID de la persona a eliminar: ");
    if (!int.TryParse(Console.ReadLine(), out int deleteId))
    {
        Console.WriteLine("ID inválido.");
        return;
    }

    var personToRemove = people.FirstOrDefault(p => p.Id == deleteId);
    if (personToRemove == null)
    {
        Console.WriteLine("No se encontró la persona.");
        return;
    }

    people.Remove(personToRemove);
    helper.Write(path, people);
    Console.WriteLine("Persona eliminada.");
    return;
}

void AddPeople()
{
    int newId;
    while (true)
    {
        Console.Write("ID: ");
        if (!int.TryParse(Console.ReadLine(), out newId))
        {
            Console.WriteLine("ID inválido. Intente de nuevo.");
            continue;
        }
        if (people.Any(p => p.Id == newId))
        {
            Console.WriteLine("Ya existe una persona con ese ID. Intente con otro.");
            continue;
        }
        break;
    }
    string name;
    do
    {
        Console.Write("Nombre: ");
        name = Console.ReadLine() ?? "";
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("El nombre no puede estar vacío.");
        }
    } while (string.IsNullOrWhiteSpace(name));
    int age;
    while (true)
    {
        Console.Write("Edad: ");
        if (!int.TryParse(Console.ReadLine(), out age))
        {
            Console.WriteLine("Edad inválida. Intente de nuevo.");
            continue;
        }
        if (age < 0 || age > 120)
        {
            Console.WriteLine("Edad fuera de rango razonable. Intente de nuevo.");
            continue;
        }
        break;
    }
    people.Add(new Person { Id = newId, Name = name, Age = age });
    helper.Write(path, people);
    Console.WriteLine("Persona agregada.");
}


void ListPeople()
{
    foreach (var person in people)
    Console.WriteLine($"ID: {person.Id}, Nombre: {person.Name}, Edad: {person.Age}");
}






















//var manualCsv = new ManualCsvHelper();
//var people = manualCsv.ReadCsv($"{listName}.csv");
//var option = string.Empty;

//do
//{
//    option = MyMenu();
//    Console.WriteLine();
//    Console.WriteLine();
//    switch (option)
//    {
//        case "1":
//            AddPerson();
//            break;

//        case "2":
//            ListPeople();
//            break;

//        case "3":
//            SaveFile(people, listName);
//            Console.WriteLine("Archivo guardado.");
//            break;

//        case "4":
//            DeletePerson();
//            break;

//        case "5":
//            SortData();
//            break;

//        case "0":
//            Console.WriteLine("Saliendo...");
//            break;

//        default:
//            Console.WriteLine("Opción no válida.");
//            break;
//    }
//} while (option != "0");

//void SortData()
//{
//    int order;
//    do
//    {
//        Console.Write("Por cual campo desea ordenar 0. Nombre, 1. Apellido, 2. Edad? ");
//        var orderString = Console.ReadLine();
//        int.TryParse(orderString, out order);
//        if (order < 0 || order > 2)
//        {
//            Console.WriteLine("Orden no válido. Intente de nuevo.");
//        }
//    } while (order < 0 || order > 2);

//    int type;
//    do
//    {
//        Console.Write("Desea ordenar 0. Ascendente, 1. Descendente? ");
//        var typeString = Console.ReadLine();
//        int.TryParse(typeString, out type);
//        if (type < 0 || type > 1)
//        {
//            Console.WriteLine("Orden no válido. Intente de nuevo.");
//        }
//    } while (type < 0 || type > 1);

//    people.Sort((a, b) =>
//    {
//        int cmp;
//        if (order == 2) // Edad: comparar como número
//        {
//            bool parsedA = int.TryParse(a[2], out var ageA);
//            bool parsedB = int.TryParse(b[2], out var ageB);

//            // Si no se puede parsear, tratamos como -infinito para que queden al inicio
//            if (!parsedA) ageA = int.MinValue;
//            if (!parsedB) ageB = int.MinValue;

//            cmp = ageA.CompareTo(ageB);
//        }
//        else // Nombre o Apellido: comparación de texto, ignorando mayúsculas/minúsculas
//        {
//            cmp = string.Compare(a[order], b[order], StringComparison.OrdinalIgnoreCase);
//        }

//        return type == 0 ? cmp : -cmp; // 0 = ascendente, 1 = descendente
//    });

//    Console.WriteLine("Datos ordenados.");
//}

//void ListPeople()
//{
//    Console.WriteLine("Lista de personas:");
//    Console.WriteLine($"Nombres|Apellidos|Edad");
//    foreach (var person in people)
//    {
//        Console.WriteLine($"{person[0]}|{person[1]}|{person[2]}");
//    }
//}

//void AddPerson()
//{
//    Console.Write("Digite el nombre: ");
//    var name = Console.ReadLine();
//    Console.Write("Digite el apellido: ");
//    var lastName = Console.ReadLine();
//    Console.Write("Digite la edad: ");
//    var age = Console.ReadLine();
//    people.Add([name ?? string.Empty, lastName ?? string.Empty, age ?? string.Empty]);
//}

//void DeletePerson()
//{
//    Console.Write("Digite el nombre: ");
//    var nameToDelete = Console.ReadLine();
//    var peopleToDelete = people
//        .Where(p => p[0].Equals(nameToDelete, StringComparison.OrdinalIgnoreCase))
//        .ToList();

//    if (peopleToDelete.Count == 0)
//    {
//        Console.WriteLine("No se encontraron personas con ese nombre.");
//        return;
//    }

//    for (int i = 0; i < peopleToDelete.Count; i++)
//    {
//        Console.WriteLine($"ID: {i} - Nombres: {peopleToDelete[i][0]} {peopleToDelete[i][1]}, Edad: {peopleToDelete[i][2]}");
//    }

//    int id;
//    do
//    {
//        Console.Write("Digite el ID del elemento que desea borrar, o -1 para cancelar? ");
//        var idString = Console.ReadLine();
//        int.TryParse(idString, out id);
//        if (id < -1 || id > peopleToDelete.Count)
//        {
//            Console.WriteLine("ID no válido. Intente de nuevo.");
//        }
//    } while (id < -1 || id > peopleToDelete.Count);

//    if (id == -1)
//    {
//        Console.WriteLine("Operación cancelada.");
//        return;
//    }

//    var personToRemove = peopleToDelete[id];
//    people.Remove(personToRemove);
//}

//string MyMenu()
//{
//    Console.WriteLine();
//    Console.WriteLine();
//    Console.WriteLine("1. Adicionar.");
//    Console.WriteLine("2. Mostrar.");
//    Console.WriteLine("3. Grabar.");
//    Console.WriteLine("4. Eliminar.");
//    Console.WriteLine("5. Ordenar.");
//    Console.WriteLine("0. Salir.");
//    Console.Write("Seleccione una opción: ");
//    return Console.ReadLine() ?? string.Empty;
//}
//SaveFile(people, listName);

//void SaveFile(List<string[]> people, string? listName)
//{
//    manualCsv.WriteCsv($"{listName}.csv", people);
//}