using System;
using System.Collections;
using System.Collections.Generic;

namespace CommandPattern
{

    // UnitOfWork

    //public class CommandsUnitOfWork
    //{
    //    private Queue<ICommand> commands = new Queue<ICommand>();

    //    public void Add(ICommand command)
    //    {
    //        commands.Enqueue(command);
    //    }

    //    public void Print()
    //    {

    //    }
    //}

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Command Pattern!");

            Message message = new Message("555000123", "555888000", "Hello World!");

            Queue<ICommand> commands = new Queue<ICommand>();

            commands.Enqueue(new SendCommand("777777", "645654654", message.Content));
            commands.Enqueue(new PrintCommand(3, "423423432", "645654654", message.Content));

            commands.Enqueue(new SendCommand("777777", "645654654", message.Content));
            commands.Enqueue(new PrintCommand(4, "777777", "645654654", message.Content));
            

            while (commands.Count > 0)
            {
                ICommand command = commands.Dequeue();

                if (command.CanExecute())
                {
                    command.Execute();
                }
            }

            
        }
    }

    #region Models

    public class Message
    {
        public Message(string from, string to, string content)
        {
            From = from;
            To = to;
            Content = content;
        }

        public string From { get; set; }
        public string To { get; set; }
        public string Content { get; set; }

     
        public void Send()
        {
            Console.WriteLine($"Send message from <{From}> to <{To}> {Content}");
        }

        public bool CanSend()
        {
            return !(string.IsNullOrEmpty(From) || string.IsNullOrEmpty(To) || string.IsNullOrEmpty(Content));
        }

        public void Print(byte copies = 1)
        {
            for (int i = 0; i < copies; i++)
            {
                Console.WriteLine($"Print message from <{From}> to <{To}> {Content}");
            }
        }

        public bool CanPrint()
        {
            return string.IsNullOrEmpty(Content);
        }



    }

    #endregion

    // Abstract Command
    public interface ICommand
    {
        void Execute();
        bool CanExecute();
    }

    // Concrete Command
    public class SendCommand : ICommand
    {
        public SendCommand(string from, string to, string content)
        {
            this.from = from;
            this.to = to;
            this.content = content;
        }

        private readonly string from;
        private readonly string to;
        private readonly string content;

        public void Execute()
        {            
            Console.WriteLine($"Send message from <{from}> to <{to}> {content}");
        }

        public bool CanExecute()
        {
            return !(string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to) || string.IsNullOrEmpty(content));
        }
    }

    // Concrete Command
    public class PrintCommand : ICommand
    {
        private readonly int copies;
        private readonly string content;
        private readonly string from;
        private readonly string to;

        public PrintCommand(int copies, string content, string from, string to)
        {
            this.copies = copies;
            this.content = content;
            this.from = from;
            this.to = to;
        }

        public bool CanExecute()
        {
            return !string.IsNullOrEmpty(content);
        }

        public void Execute()
        {
            for (int i = 0; i < copies; i++)
            {
                Console.WriteLine($"Print message from <{from}> to <{to}> {content}");
            }
        }
    }
}
