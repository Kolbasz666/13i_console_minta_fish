using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace fish;

internal class Program
{
    static string command = "run";
    static bool loggedIn = false;
    static ServerConnection connection = new ServerConnection("http://127.1.1.1:3000");
    static async Task Main(string[] args)
    {

        while (command == "run")
        {
            int number = WriteMenu();
            await DoStg(number);
        }
    }
    static async Task DoStg(int number)
    {
        if (!loggedIn)
        {
            switch (number)
            {
                case 1:
                    await ListFish();
                    break;
                case 2:
                    await FilterFish();
                    break;
                case 3:
                    await SearchFish();
                    break;
                case 4:
                    await GroupFish();
                    break;
                case 5:
                    await Login();
                    break;
                case 6:
                    command = "exit";
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (number)
            {
                case 1:
                    await ListMyFish();
                    break;
                case 2:
                    await NewFish();
                    break;
                case 3:
                    Logout();
                    break;
                default:
                    break;
            }
        }
    }

    private static void Logout()
    {
        connection.Logout();
        loggedIn = false;
    }

    private static async Task NewFish()
    {
        Console.Write("Add meg a hal nevét: ");
        string name = Console.ReadLine().Trim();
        Console.Write("Add meg a hal súlyát: ");
        string weightString = Console.ReadLine().Trim();
        if (double.TryParse(weightString, out double weight)) {
            await connection.PostFish(name, weight);
        }
        else
        {
            Console.WriteLine("Súlynak számot kell megadni!");
        }
    }

    private static async Task ListMyFish()
    {
        List<Fish> allFish = await connection.GetMyFish();
        allFish.ForEach(fish => Console.WriteLine($"Név: {fish.name}, súly: {fish.weight * 100} dkg"));
        Console.ReadKey();
    }

    static async Task ListFish()
    {
        List<Fish> allFish = await connection.GetFish();
        allFish.ForEach(fish => Console.WriteLine($"Név: {fish.name}, súly: {fish.weight * 100} dkg"));
        Console.ReadKey();
    }
    static async Task FilterFish()
    {
        List<Fish> allFish = await connection.GetFish();
        Console.WriteLine("Add meg a minimum súlyt dkg-ban");
        int number = GetNumber(0, int.MaxValue);
        allFish.Where(fish => fish.weight * 100 > number).ToList()
            .ForEach(fish => Console.WriteLine($"Név: {fish.name}, súly: {fish.weight * 100} dkg"));
        Console.ReadKey();
    }
    static async Task SearchFish()
    {
        List<Fish> allFish = await connection.GetFish();
        Console.WriteLine("Add meg a keresett szórészletet");
        string name = Console.ReadLine().Trim();
        allFish.Where(fish => fish.name.Contains(name)).ToList()
            .ForEach(fish => Console.WriteLine($"Név: {fish.name}, súly: {fish.weight * 100} dkg"));
        Console.ReadKey();
    }
    static async Task GroupFish()
    {
        List<Fish> allFish = await connection.GetFish();
        int under100 = allFish.Where(fish => fish.weight < 100).Count();
        int under200 = allFish.Where(fish => fish.weight >= 100 && fish.weight <= 200).Count();
        int over200 = allFish.Where(fish => fish.weight > 200).Count();
        Console.WriteLine("Statisztika");
        Console.WriteLine("Halak súlya csoportosítva");
        Console.WriteLine($"100kg alatt: {under100}");
        Console.WriteLine($"100kg - 200kg: {under200}");
        Console.WriteLine($"200kg felett: {over200}");
        Console.ReadKey();
    }
    static async Task Login()
    {
        Console.Write("Add meg a felhasználóneved: ");
        string username = Console.ReadLine().Trim();
        Console.Write("Add meg a jelszót: ");
        string password = Console.ReadLine().Trim();
        if (await connection.Login(username, password)) {
            loggedIn = true;
        }
    }
    static int WriteMenu()
    {
        Console.Clear();
        if (!loggedIn)
        {
            Console.WriteLine("1. Halak listázása");
            Console.WriteLine("2. Szűrés megadott tömeg felett");
            Console.WriteLine("3. Név szerinti keresés rész-illesztéssel");
            Console.WriteLine("4. Halak csoportosítása súly alapján");
            Console.WriteLine("5. Bejelentkezés");
            Console.WriteLine("6. Kilépés");
            Console.WriteLine("");
            Console.WriteLine("Kérlek válassz egy opciót!");
            return GetNumber(1, 6);
        }
        else
        {
            Console.WriteLine("1. Saját halak listázása");
            Console.WriteLine("2. Új hal felvétele");
            Console.WriteLine("3. kijelentkezés");
            Console.WriteLine("");
            Console.WriteLine("Kérlek válassz egy opciót!");
            return GetNumber(1, 3);
        }
    }
    static int GetNumber(int min, int max)
    {
        Console.WriteLine($"Add meg a számot {min} és {max} között! ");
        string userinput = Console.ReadLine().Trim();
        if (int.TryParse(userinput, out int result))
        {
            if (result >= min && result <= max)
                return result;
            Console.WriteLine("A határon kívül esett a szám!");
        }
        Console.WriteLine("Számot kell megadni!");

        return GetNumber(min, max);
    }
}
