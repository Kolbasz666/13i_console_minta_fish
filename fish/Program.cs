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
            DoStg(number);
        }
    }
    static void DoStg(int number)
    {
        if (!loggedIn)
        {
            switch (number)
            {
                case 1:
                    ListFish();
                    break;
                case 2:
                    FilterFish();
                    break;
                case 3:
                    SearchFish();
                    break;
                case 4:
                    GroupFish();
                    break;
                case 5:
                    Login();
                    break;

            }
        }
        else
        {

        }
    }
    static async void ListFish()
    {
        List<Fish> allFish = await connection.GetFish();
        allFish.ForEach(fish => Console.WriteLine($"Név: {fish.name}, súly: {fish.weight * 100} dkg"));
        Console.ReadKey();
    }
    static async void FilterFish()
    {
        List<Fish> allFish = await connection.GetFish();
        int number = GetNumber(0, int.MaxValue);
        allFish.Where(fish => fish.weight * 100 > number).ToList()
            .ForEach(fish => Console.WriteLine($"Név: {fish.name}, súly: {fish.weight * 100} dkg"));
        Console.ReadKey();
    }
    static void SearchFish()
    {

    }
    static void GroupFish()
    {

    }
    static void Login()
    {

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
            Console.WriteLine("");
            Console.WriteLine("Kérlek válassz egy opciót!");
            return GetNumber(1, 5);
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
        Console.Write($"Add meg a számot {min} és {max} között! ");
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
