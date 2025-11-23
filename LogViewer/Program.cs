namespace LogViewer;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Iniciando Log Viewer...");
        var subscriber = new Subscriber();
        Console.WriteLine("Presione [enter] para salir.");
        Console.ReadLine();
    }
}
