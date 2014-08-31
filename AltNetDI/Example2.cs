using System;

// 2. Manager comes and says wants to log all the writes to a file - decorator pattern
namespace AltNetDI2 {
    class CompositionRoot {
        public static void EMain() {
            IReader reader = new TextFileReader();

            //IWriter writer = new ConsoleWriter();
            // Decorating ConsoleWriter with a WriterLogger
            IWriter writer = new WriterLogger(new ConsoleWriter());

            // Pass objects to our Appliction via Dependency Injection
            IApplication application = new Application(reader, writer);
            application.Run();
        }
    }

    public interface IApplication { void Run();}
    public class Application : IApplication {
        private readonly IReader reader;
        private readonly IWriter writer;

        public Application(IReader reader, IWriter writer) {
            this.reader = reader;
            this.writer = writer;
        }

        public void Run() {
            var text = reader.Read();
            writer.Write(text);
        }
    }

    public interface IReader { string Read();}
    public class TextFileReader : IReader {
        public string Read() {
            // Read from a textfile
            return "blah";
        }
    }

    public interface IWriter { void Write(string text);}
    public class ConsoleWriter : IWriter {
        public void Write(string text) {
            Console.WriteLine("ConsoleWriter says: {0}", text);
        }
    }

    public class WriterLogger : IWriter {
        private readonly IWriter writer;

        // Being passed in Writer which WriterLogger is decorating (or wrapping)
        public WriterLogger(IWriter writer) {
            this.writer = writer;
        }

        public void Write(string text) {
            Console.WriteLine("WriterLogger says: {0}", text);
            writer.Write(text);
            Console.WriteLine("WriterLogger: Finished logging");
        }
    }
}
