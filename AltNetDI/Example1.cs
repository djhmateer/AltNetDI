using System;

namespace AltNetDI {
    class Example1 {
        public static void EMain() {
            IReader reader = new Reader();
            IWriter writer = new Writer();

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

    public class Reader : IReader {
        public string Read() {
            // read the file
            return "blah";
        }
    }

    public interface IWriter {
        void Write(string text);
    }

    public class Writer : IWriter {
        public void Write(string text) {
            Console.WriteLine(text);
        }
    }
}
