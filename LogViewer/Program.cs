namespace LogViewer;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Iniciando Log Viewer...");
        
        // Obtener el topic de los argumentos o solicitar por consola
        string bindingKey;
        if (args.Length > 0)
        {
            bindingKey = args[0];
            Console.WriteLine($"Topic especificado por argumentos: {bindingKey}");
        }
        else
        {
            Console.WriteLine("\nOpciones de suscripci\u00f3n:");
            Console.WriteLine("  logs.error       - Solo mensajes de error");
            Console.WriteLine("  logs.information - Solo mensajes informativos");
            Console.WriteLine("  logs.warning     - Solo mensajes de advertencia");
            Console.WriteLine("  logs.debug       - Solo mensajes de depuraci\u00f3n");
            Console.WriteLine("  logs.*           - Todos los mensajes");
            Console.WriteLine("  logs.#           - Todos los mensajes (alternativa)");
            Console.Write("\nIntroduzca el topic al que desea suscribirse: ");
            bindingKey = Console.ReadLine() ?? "logs.*";
        }
        
        var subscriber = new Subscriber(bindingKey);
        Console.WriteLine("\nPresione [enter] para salir.");
        Console.ReadLine();
    }
}
