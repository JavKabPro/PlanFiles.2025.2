using PlainFiles.Core;
using System.Globalization;
using System.Text;
string[] lineas = File.ReadAllLines("people.csv", Encoding.UTF8);

string listName = "people";
string path = $"{listName}.csv";
var helper = new NugetCsvHelper();
var people = helper.Read(path).ToList();

bool salir = false;

while (!salir)
{
    Console.WriteLine();
    Console.WriteLine("\n==========================");
    Console.WriteLine("1. Show content.");
    Console.WriteLine("2. Add person.");
    Console.WriteLine("3. Save.");
    Console.WriteLine("4. Delete person.");
    Console.WriteLine("0. Exit.");
    Console.WriteLine("==========================");
    Console.Write("Choose an option: ");
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
            SaveFile(people, listName);
            Console.WriteLine("File saved.");
            break;
        case "4":
            DeletePeople();
            break;
        case "0":
            salir = true;
            break;
        default:
            Console.WriteLine("Invalid option.");
            break;
    }
}
void SaveFile(List<Person> people, string? listName)
{
    if (string.IsNullOrWhiteSpace(listName))
        listName = "people";

    var helper = new NugetCsvHelper();
    helper.Write($"{listName}.csv", people);
}

void DeletePeople()
{
        Console.WriteLine();
        Console.WriteLine("Delete by:");
        Console.WriteLine("1. Id");
        Console.WriteLine("2. Name");
        Console.WriteLine("3. Phone");
        Console.WriteLine("4. City");
        Console.Write("Option: ");
        string option = Console.ReadLine() ?? "";
        List<Person> toRemove = new();

        switch (option)
        {
            case "1":
                Console.Write("Enter Id: ");
                if (!int.TryParse(Console.ReadLine(), out int idToDelete))
                {
                    Console.WriteLine("Invalid Id.");
                    return;
                }
                var byId = people.FirstOrDefault(p => p.Id == idToDelete);
                if (byId == null)
                {
                    Console.WriteLine("No person found with that Id.");
                    return;
                }
                toRemove.Add(byId);
                break;
            case "2":
                Console.Write("Enter Name: ");
                string name = (Console.ReadLine() ?? "").Trim();
                toRemove = people.Where(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).ToList();

                if (!toRemove.Any())
                {
                    Console.WriteLine("No person found with that name.");
                    return;
                }
                break;
            case "3":
                Console.Write("Enter Phone: ");
                string phone = (Console.ReadLine() ?? "").Trim();

                toRemove = people.Where(p => p.Phone.Equals(phone, StringComparison.OrdinalIgnoreCase)).ToList();
                if (!toRemove.Any())
                {
                    Console.WriteLine("No person found with that phone.");
                    return;
                }
                break;
            case "4":
                Console.Write("Enter City: ");
                string city = (Console.ReadLine() ?? "").Trim();

                toRemove = people
                    .Where(p => p.City.Equals(city, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                if (!toRemove.Any())
                {
                    Console.WriteLine("No person found with that city.");
                    return;
                }
                break;
            default:
                Console.WriteLine("Invalid option.");
                return;
        }
    foreach (var p in toRemove)
        people.Remove(p);

    people = people.OrderBy(p => p.Id).ToList();
    helper.Write(path, people);

    Console.WriteLine($"{toRemove.Count} person(s) deleted.");
}

void AddPeople()
{
    int newId;
    if (people.Count == 0)
        newId = 1;
    else
        newId = people.Max(p => p.Id) + 1;

    Console.WriteLine();
    Console.WriteLine("Adding new person:");

    string name;
    do
    {
        Console.Write("Name: ");
        name = Console.ReadLine() ?? "";
        if (string.IsNullOrWhiteSpace(name))
            Console.WriteLine("Name cannot be empty.");
    }
    while (string.IsNullOrWhiteSpace(name));

    string phone;
    do
    {
        Console.Write("Phone: ");
        phone = Console.ReadLine() ?? "";
        if (string.IsNullOrWhiteSpace(phone))
            Console.WriteLine("Phone cannot be empty.");
    }
    while (string.IsNullOrWhiteSpace(phone));

    string city;
    do
    {
        Console.Write("City: ");
        city = Console.ReadLine() ?? "";
        if (string.IsNullOrWhiteSpace(city))
            Console.WriteLine("City cannot be empty.");
    }
    while (string.IsNullOrWhiteSpace(city));

    decimal balance;
    while (true)
    {
        Console.Write("Balance: ");
        if (!decimal.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out balance))
        {
            Console.WriteLine("Invalid balance.");
            continue;
        }
        break;
    }

    people.Add(new Person{Id = newId, Name = name, Phone = phone, City = city, Balance = balance});
    people = people.OrderBy(p => p.Id).ToList();
    helper.Write(path, people);
    Console.WriteLine("Person added.");
}


void ListPeople()
{
    Console.WriteLine();

    if (!people.Any())
    {
        Console.WriteLine("No records found.");
        return;
    }

    foreach (var p in people)
    {
        Console.WriteLine($"{p.Id}");
        Console.WriteLine($"   {p.Name}");
        Console.WriteLine($"   Phone: {p.Phone}");
        Console.WriteLine($"   City: {p.City}");
        Console.WriteLine($"   Balance: {p.Balance.ToString("C", CultureInfo.CurrentCulture)}");
        Console.WriteLine();
    }
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