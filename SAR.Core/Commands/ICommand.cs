using System;

namespace SAR.Core.Commands
{
    public interface ICommand
    {
        public string CommandName { get; set; }
        public bool IsRepeatable { get; set; }
        public TimeSpan Delay { get; set; }
        
        
        public void Execute();
    }
}