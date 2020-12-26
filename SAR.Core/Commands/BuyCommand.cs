using System;
using System.Threading;

namespace SAR.Core.Commands
{
    public class BuyCommand : ICommand
    {
        public string CommandName { get; set; } = nameof(BuyCommand);
        public bool IsRepeatable { get; set; }
        public TimeSpan Delay { get; set; }
        

        public BuyCommand(bool isRepeatable, TimeSpan delay = default)
        {
            IsRepeatable = isRepeatable;
            Delay = delay;
        }
        public BuyCommand(string name, bool isRepeatable = false, TimeSpan delay = default)
        {
            CommandName = name;
            IsRepeatable = isRepeatable;
            Delay = delay;
        }

        public void Execute()
        {
            Console.WriteLine(CommandName +  " Buy Something Start");    
            Thread.Sleep(6000);
            Console.WriteLine(CommandName +  " Buy Something Finish");   
        }
    }
}