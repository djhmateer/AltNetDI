using System;

// Read text from a file, and display it on the screen
namespace AltNetDI1 {
    class CompositionRoot {
        public static void EMain() {
            // Instantiating the objects we will need
            IReader reader = new TextFileReader();
            IWriter writer = new ConsoleWriter();

            // Pass objects to our Appliction via Dependency Injection
            IApplication application = new Application(reader, writer);
            application.Run();
        }
    }

    // Arguably this is where the business logic starts - above is just setup
    public interface IApplication { void Run();}
    public class Application : IApplication {
        private readonly IReader reader;
        private readonly IWriter writer;

        // The Application receiving it's dependencies
        public Application(IReader reader, IWriter writer) {
            this.reader = reader;
            this.writer = writer;
        }

        // Do stuff with the dependencies we have been passed
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

    public interface IWriter {void Write(string text);}
    public class ConsoleWriter : IWriter {
        public void Write(string text) {
            Console.WriteLine(text);
        }
    }
}
