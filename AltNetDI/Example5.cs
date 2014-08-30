using System;
using Xunit;

//5. Testing - want to make sure the datetime of the writerlogger is correct
namespace AltNetDI5 {
    class CompositionRoot {
        public static void EMain() {
            IReader reader = new TextFileReader();

            // Decorating Writer with a WriterLogger
            IWriter writer = new WriterLogger(new Writer(), new ConsoleWriter(), () => DateTime.Now);

            // Pass objects to our Appliction via Dependency Injection
            IApplication application = new Application(reader, writer);
            application.Run();
        }
    }

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

    public interface IWriter { void Write(string text);}
    public class Writer : IWriter {
        public void Write(string text) {
            Console.WriteLine("Writer says: {0}", text);
        }
    }

    //5. Testing - want to make sure that the datetime outputted of the writerlogger is correct
    public class WriterLogger : IWriter {
        private readonly IWriter writer;
        private readonly IConsoleWriter consoleWriter;
        private readonly Func<DateTime> now;

        public WriterLogger(IWriter writer, IConsoleWriter consoleWriter, Func<DateTime> now)
        {
            this.writer = writer;
            this.consoleWriter = consoleWriter;
            this.now = now;
        }

        public void Write(string text) {
            consoleWriter.WriteLine("WriterLogger says: {0} {1}", now(), text);
            writer.Write(text);
            consoleWriter.WriteLine("WriterLogger says: Finished logging");
        }
    }

    // 
    public interface IConsoleWriter {void WriteLine(string format, params object[] args);}
    public class ConsoleWriter : IConsoleWriter {
        public void WriteLine(string format, params object[] args) {
            Console.WriteLine(format, args);
        }
    }

    public class WriterLoggerTests {
        public void ShouldReturnDateTimeWhichIsNow() {
            var fakeWriter = new FakeWriter();
            //var writerLogger = new WriterLogger(fakeWriter);
            //writerLogger.Write("test message");

            //Assert.Equal("WriterLogger says: test message");
        }
    }

    public class FakeWriter : IWriter {
        public void Write(string text) { }
    }

    public class TextFileReaderTest {
        [Fact]
        public void Read_ShouldReturnBlah() {
            var reader = new TextFileReader();
            var result = reader.Read();
            Assert.Equal("blah", result);
        }
    }
}
