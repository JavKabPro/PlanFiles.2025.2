using PlainFiles.Core;
using System.Globalization;
using System.Text;
string[] lineas = File.ReadAllLines("people.csv", Encoding.UTF8);

using var log = new LogWriter("log.txt");
var userService = new UserService("Users.txt", log);

Console.WriteLine("=== LOGIN REQUIRED ===");

int attempts = 0;
bool authenticated = false;
string? currentUser = null;

//Login
while (attempts < 3 && !authenticated)
{
    Console.Write("Username: ");
    string username = Console.ReadLine() ?? "";
    Console.Write("Password: ");
    string password = Console.ReadLine() ?? "";
    var user = userService.ValidateUser(username, password);
    if (user != null)
    {
        authenticated = true;
        currentUser = username;
        log.WriteLog("INFO", $"User '{username}' logged in successfully.");
        Console.WriteLine("\nLogin successful.");
        break;
    }

    attempts++;
    log.WriteLog("WARN", $"Failed login attempt for user '{username}'. Attempt {attempts}/3.");
    if (!userService.UserExists(username))
        Console.WriteLine("User does not exist.");
    else
        Console.WriteLine("Invalid credentials.");

    if (attempts == 3)
    {
        Console.WriteLine("\nToo many attempts. User will be blocked.");
        userService.BlockUser(username);
        return;
    }
}

Console.WriteLine("\nWelcome to the system.");

string listName = "people";
string path = $"{listName}.csv";

var helper = new NugetCsvHelper();
var people = helper.Read(path).ToList();
bool salir = false;

// Principal Menu
while (!salir)
{
    Console.WriteLine();
    Console.WriteLine("\n==========================");
    Console.WriteLine("1. Show content.");
    Console.WriteLine("2. Add person.");
    Console.WriteLine("3. Save.");
    Console.WriteLine("4. Delete person.");
    Console.WriteLine("5. Edit person.");
    Console.WriteLine("6. Delete person (with confirmation).");
    Console.WriteLine("7. Report by City.");
    Console.WriteLine("0. Exit.");
    Console.WriteLine("==========================");
    Console.Write("Choose an option: ");
    Console.WriteLine();

    var opcion = Console.ReadLine();

    switch (opcion)
    {
        case "1":
            log.WriteLog("INFO", $"User '{currentUser}' listed all people.");
            ListPeople();
            break;

        case "2":
            log.WriteLog("INFO", $"User '{currentUser}' is adding a new person.");
            AddPeople();
            break;
        case "3":
            SaveFile(people, listName);
            log.WriteLog("INFO", $"User '{currentUser}' saved the file '{listName}.csv'.");
            Console.WriteLine("File saved.");
            break;
        case "4":
            log.WriteLog("INFO", $"User '{currentUser}' is deleting a person.");
            DeletePeople();
            break;
        case "5":
            log.WriteLog("INFO", $"User '{currentUser}' is editing a person.");
            EditPerson();
            break;
        case "6":
            log.WriteLog("INFO", $"User '{currentUser}' is attempting to delete a person.");
            DeletePersonWithConfirmation();
            break;
        case "7":
            log.WriteLog("INFO", $"User '{currentUser}' generated report by city.");
            ReportByCity();
            break;
        case "0":
            log.WriteLog("INFO", $"User '{currentUser}' exited the system.");
            salir = true;
            break;

        default:
            Console.WriteLine("Invalid option.");
            break;
    }
}

void ReportByCity()
    {
        if (!people.Any())
        {
            Console.WriteLine("No records found.");
            return;
        }

        Console.WriteLine("\n===== REPORT BY CITY =====\n");

        var groups = people.GroupBy(p => p.City).OrderBy(g => g.Key);
        decimal totalGeneral = 0;
        foreach (var group in groups)
        {
            Console.WriteLine($"Ciudad: {group.Key}\n");
            Console.WriteLine("ID\tNombres\t\tApellidos\tSaldo");
            Console.WriteLine("—\t—-------------\t—------------\t—----------");

            decimal subtotal = 0;
            foreach (var p in group)
            {
                var parts = p.Name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                string nombres = parts.Length > 0 ? parts[0] : "";
                string apellidos = parts.Length > 1 ? string.Join(" ", parts.Skip(1)) : "";
                Console.WriteLine($"{p.Id}\t{nombres}\t\t{apellidos}\t\t{p.Balance.ToString("N2", CultureInfo.CurrentCulture)}");
                subtotal += p.Balance;
            }

            Console.WriteLine("\t\t\t\t=======");
            Console.WriteLine($"Total: {group.Key}\t\t\t{subtotal.ToString("N2", CultureInfo.CurrentCulture)}\n");
            totalGeneral += subtotal;
        }

        Console.WriteLine("\t\t\t\t=======");
        Console.WriteLine($"Total General:\t\t\t{totalGeneral.ToString("N2", CultureInfo.CurrentCulture)}\n");
    }




void DeletePersonWithConfirmation()
{
        Console.Write("Enter the ID of the person to delete: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID.");
            return;
        }

        var person = people.FirstOrDefault(p => p.Id == id);
        if (person == null)
        {
            Console.WriteLine("No person found with that ID.");
            return;
        }

        Console.WriteLine("\nPerson found:");
        Console.WriteLine($"ID: {person.Id}");
        Console.WriteLine($"Name: {person.Name}");
        Console.WriteLine($"Phone: {person.Phone}");
        Console.WriteLine($"City: {person.City}");
        Console.WriteLine($"Balance: {person.Balance}");

        Console.Write("\nAre you sure you want to delete this person? (S/N): ");
        string confirm = (Console.ReadLine() ?? "").Trim().ToUpper();

        if (confirm != "S")
        {
            Console.WriteLine("Deletion cancelled.");
            log.WriteLog("INFO", $"User '{currentUser}' cancelled deletion of person with Id {id}.");
            return;
        }

        people.Remove(person);
        people = people.OrderBy(p => p.Id).ToList();
        helper.Write(path, people);

        Console.WriteLine("Person deleted successfully.");
        log.WriteLog("INFO", $"User '{currentUser}' deleted person with Id {id}.");
    }

void EditPerson()
{
    Console.Write("Enter the ID of the person to edit: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("Invalid ID.");
        return;
    }

    var person = people.FirstOrDefault(p => p.Id == id);
    if (person == null)
    {
        Console.WriteLine("No person found with that ID.");
        return;
    }

    Console.WriteLine("\nEditing person:");
    Console.WriteLine($"Current Name: {person.Name}");
    Console.WriteLine($"Current Phone: {person.Phone}");
    Console.WriteLine($"Current City: {person.City}");
    Console.WriteLine($"Current Balance: {person.Balance}");

    Console.Write("New Name (ENTER to keep current): ");
    string newName = (Console.ReadLine() ?? "").Trim();
    if (!string.IsNullOrWhiteSpace(newName))
        person.Name = newName;

    Console.Write("New Phone (ENTER to keep current): ");
    string newPhone = (Console.ReadLine() ?? "").Trim();
    if (!string.IsNullOrWhiteSpace(newPhone))
        person.Phone = newPhone;

    Console.Write("New City (ENTER to keep current): ");
    string newCity = (Console.ReadLine() ?? "").Trim();
    if (!string.IsNullOrWhiteSpace(newCity))
        person.City = newCity;

    Console.Write("New Balance (ENTER to keep current): ");
    string balanceInput = (Console.ReadLine() ?? "").Trim();
    if (!string.IsNullOrWhiteSpace(balanceInput))
    {
        if (decimal.TryParse(balanceInput, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal newBalance))
            person.Balance = newBalance;
    }

    var validator = new PersonValidator();
    var result = validator.Validate(person);

    if (!result.IsValid)
    {
        Console.WriteLine("\nValidation errors:");
        foreach (var error in result.Errors)
            Console.WriteLine($" - {error.ErrorMessage}");

        log.WriteLog("WARN", $"User '{currentUser}' failed to edit person Id {id} due to validation errors.");
        return;
    }

    people = people.OrderBy(p => p.Id).ToList();
    helper.Write(path, people);
    log.WriteLog("INFO", $"User '{currentUser}' edited person with Id {id}.");
    Console.WriteLine("Person updated successfully.");
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
            toRemove = people.Where(p => p.City.Equals(city, StringComparison.OrdinalIgnoreCase)).ToList();
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
    log.WriteLog("INFO", $"User '{currentUser}' deleted {toRemove.Count} person(s).");
    Console.WriteLine($"{toRemove.Count} person(s) deleted.");
}

void AddPeople()
{
    int newId = people.Count == 0 ? 1 : people.Max(p => p.Id) + 1;

    Console.WriteLine();
    Console.WriteLine("Adding new person:");

    Console.Write("Name (first and last name): ");
    string name = (Console.ReadLine() ?? "").Trim();

    Console.Write("Phone: ");
    string phone = (Console.ReadLine() ?? "").Trim();

    Console.Write("City: ");
    string city = (Console.ReadLine() ?? "").Trim();

    Console.Write("Balance: ");
    string balanceInput = (Console.ReadLine() ?? "").Trim();

    decimal balance = 0;
    decimal.TryParse(balanceInput, NumberStyles.Any, CultureInfo.InvariantCulture, out balance);

    var person = new Person
    {
        Id = newId,
        Name = name,
        Phone = phone,
        City = city,
        Balance = balance
    };

    var validator = new PersonValidator();
    var result = validator.Validate(person);

    if (!result.IsValid)
    {
        Console.WriteLine("\nValidation errors:");
        foreach (var error in result.Errors)
            Console.WriteLine($" - {error.ErrorMessage}");

        log.WriteLog("WARN", $"User '{currentUser}' failed to add person due to validation errors.");
        return;
    }

    people.Add(person);
    people = people.OrderBy(p => p.Id).ToList();
    helper.Write(path, people);

    log.WriteLog("INFO", $"User '{currentUser}' added person '{name}' with Id {newId}.");

    Console.WriteLine("Person added successfully.");
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