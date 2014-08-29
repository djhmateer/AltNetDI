using System;

namespace AltNetDI2 {
    class Example6 {
        public static void EMain() {
            IReader reader = new Reader();

            // db - open for extension, closed for modification
            //IReader reader = new DbReader();

            //IWriter writer = new Writer();
            IWriter writer = new WriteLogger(new AggregateWriter(new[]
            {
                new Writer(), 
                new Writer()
            }), new ConsoleWriter(), () => DateTime.Now);

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
            // read your file
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

    public class WriteLogger : IWriter {
        private IWriter writer;
        private IConsoleWriter consoleWriter;
        private Func<DateTime> now;

        public WriteLogger(IWriter writer, IConsoleWriter consoleWriter, Func<DateTime> now) {
            this.writer = writer;
            this.consoleWriter = consoleWriter;
            this.now = now;
        }

        public void Write(string text) {

            consoleWriter.WriteLine("[{0}] {1}", now(), text);
            writer.Write(text);
            consoleWriter.WriteLine("I've done my logging");
        }
    }

    public interface IConsoleWriter {
        void WriteLine(string format, params object[] args);
    }

    public class ConsoleWriter : IConsoleWriter {
        public void WriteLine(string format, params object[] args) {
            Console.WriteLine(format, args);
        }
    }

    public class AggregateWriter : IWriter {
        private readonly IWriter[] writers;

        public AggregateWriter(IWriter[] writers) {
            this.writers = writers;
        }

        public void Write(string text) {
            foreach (var writer in writers) {
                writer.Write(text);
            }
        }
    }
}
