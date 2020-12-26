using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using SAR.Core.Commands;

namespace SAR.Core
{
    public static class RuntimeManager
    {
        private static Thread Thread;
        private static Queue<ICommand> CommandQueue = new Queue<ICommand>();
        private static List<DelayedItem> DelayedCommands = new List<DelayedItem>();


        static RuntimeManager()
        {
            Thread = new Thread(MainLoop);
            Thread.Start();
        }

        public static void SetCommand(ICommand command)
        {
            lock(CommandQueue)
            {
                CommandQueue.Enqueue(command);
            }
        }
        
        private static void MainLoop()
        {
            while (true)
            {
                ICommand command = null;

                lock (DelayedCommands)
                {
                    if (DelayedCommands.Count != 0)
                    {
                        var item = DelayedCommands.FirstOrDefault(x=> x.ExecutionTime <= DateTime.Now);

                        if (item != null)
                        {
                            DelayedCommands.Remove(item);

                            command = item.Command;
                        }
                    }
                }

                if (command == null)
                {
                    lock (CommandQueue)
                    {
                        if(CommandQueue.Count != 0)
                        {
                            command = CommandQueue.Dequeue();
                        }
                    }
                }
            

                if (command != null)
                {
                    ThreadPool.QueueUserWorkItem(_ =>
                    {
                        command.Execute();
                    });
                
                    if (command.IsRepeatable)
                    {
                        lock (DelayedCommands)
                        {

                            DelayedCommands.Add(new DelayedItem(command, DateTime.Now + command.Delay));
                        }
                    }
                }
                
                Thread.Sleep(100); 
            }
        }
    }

    public class DelayedItem
    {
        public ICommand Command { get; set; }
        public DateTime ExecutionTime { get; set; }


        public DelayedItem(ICommand command, DateTime delay)
        {
            Command = command;
            ExecutionTime = delay;
        }
    }
}