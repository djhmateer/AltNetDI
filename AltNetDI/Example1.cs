using System;

namespace AltNetDI {
    // Read text from a file, and display on the screen
    class Example1 {
        public static void EMain() {
            // Composition Root 
            IReader reader = new TextFileReader();
            IWriter writer = new ConsoleWriter();

            // Dependency Injection!
            IApp app = new App(reader, writer);
            app.Run();
        }
    }

    public interface IApp {
        void Run();
    }

    public class App : IApp {
        private readonly IReader reader;
        private readonly IWriter writer;

        // The App receiving it's dependencies
        public App(IReader reader, IWriter writer) {
            this.reader = reader;
            this.writer = writer;
        }

        public void Run() {
            var text = reader.Read();
            writer.Write(text);
        }
    }

    public interface IReader {
        string Read();
    }

    public class TextFileReader : IReader {
        public string Read() {
            // read from a textfile
            return "blah";
        }
    }

    public interface IWriter {
        void Write(string text);
    }

    public class ConsoleWriter : IWriter {
        public void Write(string text) {
            Console.WriteLine(text);
        }
    }
}
