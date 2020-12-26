using System;
using SAR.Core;
using SAR.Core.Commands;

namespace SAR
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 20; i++)
            {
                RuntimeManager.SetCommand(new BuyCommand(i.ToString()));
            }

            Console.ReadLine();
        }
    }
}