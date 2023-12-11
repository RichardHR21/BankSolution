using BankConsole;

if (args.Length == 0)
    EmailService.SendMail();
else
    ShowMenu();

void ShowMenu()
{
    Console.Clear();
    Console.WriteLine("Selecciona una opción: ");
    Console.WriteLine("1 - Crear un usuario nuevo. ");
    Console.WriteLine("2 - Eliminar un Usuario existente");
    Console.WriteLine("3 - Salir");
    Console.Write("\n Ingresa la opción: ");

    int option = 0;
    do
    {
        string input = Console.ReadLine();

        if(!int.TryParse(input, out option))
            Console.Write("Debes ingresar un número (1, 2 o 3): ");
        else if (option > 3)
            Console.WriteLine("Debes ingresar un número válido (1, 2 o 3): ");
    }
    while (option == 0 || option > 3);

    switch (option)
    {
        case 1:
            CreateUser();
            break;
        case 2:
            DeleteUser();
            break;
        case 3:
            Environment.Exit(0);
            break;
    }
}

void CreateUser()
{
    Console.Clear();
    Console.WriteLine("Ingresa la información del usuario");
    Console.Write("ID: ");
    int ID = 0;
    do
    {   
        string input = Console.ReadLine();

        if(!int.TryParse(input, out ID))
            Console.Write("ID ingresado no válido, intente nuevamente: ");
        else if (ID <= 0)
            Console.WriteLine("Debes ingresar un número positivo: ");
    }
    while (ID <= 0);

    Console.Write("Nombre: ");
    string name = Console.ReadLine();

    string email = "";
    bool check = false;
    do
    {
        Console.Write("Email: ");
        string correo = Console.ReadLine();
        if(string.IsNullOrWhiteSpace(correo))
            Console.WriteLine("Ingresa un email.");
        else
        { try{
            var addr = new System.Net.Mail.MailAddress(correo);
            if(addr.Address == correo){
                check = true;
                email = correo;
            }
            } catch 
                {
                Console.WriteLine("Email ingresado no válido, intenta nuevamente.");
                }
            }
        }while(check == false);

    Console.Write("Saldo: ");
    decimal Saldo = 0;
    do
    {   
        string input = Console.ReadLine();

        if(!decimal.TryParse(input, out Saldo))
            Console.Write("Saldo ingresado no válido, intente nuevamente: ");
        else if (Saldo <= 0.00m)
            Console.WriteLine("El saldo debe ser un número positivo: ");
    }
    while (Saldo <= 0);

    Console.Write("Escribe 'c' si el usuario es Cliente, 'e' si es empleado: ");
    char userType;
    do
    {   
        string input = Console.ReadLine();

        if(!char.TryParse(input, out userType))
            Console.Write("Letra ingresada no válida, intente nuevamente: ");
        else if (!char.Equals(userType, 'e') && !char.Equals(userType, 'c'))
        Console.Write("Escribe 'c' si el usuario es Cliente, 'e' si es empleado: ");
    }
    while (!char.Equals(userType, 'e') && !char.Equals(userType, 'c'));
   
    User newUser;

    if(userType.Equals('c'))
    {
        Console.Write("Regimen Fiscal: ");
        char taxRegime = char.Parse(Console.ReadLine());

        newUser = new Client(ID, name, email, Saldo, taxRegime);
    }
    else
    {
        Console.Write("Departamento: ");
        string department = Console.ReadLine();

        newUser = new Employee(ID, name, email, Saldo, department);
    }

    bool ins_stat = Storage.AddUser(newUser);
    if(ins_stat)
        Console.WriteLine("Usuario creado.");
    else
        Console.WriteLine("ID de usuario ocupado, eliga otro ID y vuelva a registrar.");
    Thread.Sleep(2000);
    ShowMenu();
}

void DeleteUser()
{
    Console.Clear();

    Console.Write("Ingresa el ID del usuario a eliminar: ");
    int ID = int.Parse(Console.ReadLine());

    bool result = Storage.DeleteUser(ID);

    if(result)
    {
        Console.Write("Usuario eliminado. ");
        Thread.Sleep(2000);
        ShowMenu();
    } else{
        Console.Write("Usuario no eliminado. (No existe o hubo algun problema, intenta nuevamente.)");
        Thread.Sleep(2000);
        ShowMenu();
    }
}