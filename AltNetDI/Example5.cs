using System;
using Xunit;

//5. Testing - want to make sure the datetime of the writerlogger is correct
namespace AltNetDI5 {
    class CompositionRoot {
        public static void EMain() {
            // Instantiating the objects we will need
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
    public class ConsoleWriter : IWriter {
        public void Write(string text) {
            // Write to the console
            Console.WriteLine("ConsoleWriter says: {0}", text);
        }
    }

    //5. Testing - want to make sure that the datetime outputted of the writerlogger is correct
    public class WriterLogger : IWriter {
        private readonly IWriter writer;

        public WriterLogger(IWriter writer) {
            this.writer = writer;
        }

        public void Write(string text) {
            Console.WriteLine("WriterLogger says: {0} {1}", DateTime.Now, text);
            writer.Write(text);
            Console.WriteLine("WriterLogger says: Finished logging");
        }
    }

    public class WriterLoggerTests {
        public void ShouldReturnDateTimeWhichIsNow() {
            var fakeWriter = new FakeWriter();
            var writerLogger = new WriterLogger(fakeWriter);
            writerLogger.Write("test message");

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
