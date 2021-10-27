using System;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string user = "kcskfnsdg.";

            if (user.Substring(user.Length - 1, 1) == ".")
                user = user.Substring(0, user.Length - 1);

            Console.WriteLine("Hello World!");
        }
    }
}
