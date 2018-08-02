using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;

using Microsoft.AspNet.SignalR;

using NeuralNetworkTrainer.Hubs;

namespace NeuralNetworkTrainer
{
    public class ConsoleWriterEventArgs : EventArgs
    {
        public String Value { get; private set; }
        public ConsoleWriterEventArgs(String value)
        {
            Value = value;
        }
    }

    public class ConsoleWriter : TextWriter
    {
        public override Encoding Encoding { get { return Encoding.UTF8; } }

        public override void Write(string value)
        {
            WriteEvent?.Invoke(this, new ConsoleWriterEventArgs(value));
            base.Write(value);
        }

        public override void WriteLine(string value)
        {
            value += "\n";
            WriteLineEvent?.Invoke(this, new ConsoleWriterEventArgs(value));
            base.WriteLine();
        }

        public event EventHandler<ConsoleWriterEventArgs> WriteEvent;
        public event EventHandler<ConsoleWriterEventArgs> WriteLineEvent;
    }

    public class ConsoleError : TextWriter
    {
        public override Encoding Encoding { get { return Encoding.UTF8; } }

        public override void Write(string value)
        {
            WriteEvent?.Invoke(this, new ConsoleWriterEventArgs("(ex)" + value + "(/ex)"));
            base.Write(value);
        }

        public override void WriteLine(string value)
        {
            WriteEvent?.Invoke(this, new ConsoleWriterEventArgs("(ex)" + value + "(/ex)\n"));
            base.Write(value);
        }

        public event EventHandler<ConsoleWriterEventArgs> WriteEvent;
        public event EventHandler<ConsoleWriterEventArgs> WriteLineEvent;
    }

    public class ConsoleLogger
    {
        public static String log { get; private set; }

        public static void Initialize()
        {
            ConsoleWriter writer = new ConsoleWriter();
            writer.WriteEvent += Writer_WriteEvent;
            writer.WriteLineEvent += Writer_WriteLineEvent;

            Console.SetOut(writer);

            ConsoleError error = new ConsoleError();
            error.WriteEvent += Writer_WriteEvent;
            error.WriteLineEvent += Writer_WriteLineEvent;

            Console.SetError(error);

            Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            DateTime buildDate = new DateTime(2000, 1, 1).AddDays(version.Build).AddSeconds(version.Revision * 2);

            log = "";
            Console.WriteLine("Neural Network Training Program");
            Console.WriteLine("Version: " + version.ToString() + " | Build Date: " + buildDate.ToString());
            Console.WriteLine("=====================================");

        }

        private static void Writer_WriteLineEvent(object sender, ConsoleWriterEventArgs e)
        {
            log += e.Value;
            DataHub.MessageClients(e.Value);
        }

        private static void Writer_WriteEvent(object sender, ConsoleWriterEventArgs e)
        {
            log += e.Value;
            DataHub.MessageClients(e.Value);
        }
        
    }
}