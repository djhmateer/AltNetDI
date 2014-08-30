using System;
using System.Collections.Generic;
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

        // Do Messages with the dependencies we have been passed
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
    
    public class WriterLogger : IWriter {
        private readonly IWriter writer;
        private readonly IConsoleWriter consoleWriter;
        private readonly Func<DateTime> now;

        public WriterLogger(IWriter writer, IConsoleWriter consoleWriter, Func<DateTime> now) {
            this.writer = writer;
            this.consoleWriter = consoleWriter;
            this.now = now;
        }

        public void Write(string text) {
            // Abstracted out writing to console so tests only test the WriteLogger
            consoleWriter.WriteLine("WriterLogger says: {0} {1}", now(), text);
            writer.Write(text);
            consoleWriter.WriteLine("WriterLogger says: Finished logging");
        }
    }

    //5. Testing - want to make sure that the datetime outputted of the writerlogger is correct
    public class WriterLoggerTests {
        [Fact]
        public void ShouldLogMessagesWithAGivenDateTime() {
            var fakeWriter = new FakeWriter();
            var fakeConsoleWriter = new FakeConsoleWriter();
            var dateTime = new DateTime(2014, 1, 24, 01, 02, 03);
            // Passing in fake dependencies so can test WriterLogger in isolation
            var writerLogger = new WriterLogger(fakeWriter, fakeConsoleWriter, () => dateTime);

            writerLogger.Write("test");

            Assert.Equal("WriterLogger says: 24/01/2014 01:02:03 test", fakeConsoleWriter.Messages[0]);
        }
    }

    // FakeConsoleWriter using a collection to store all messages
    public class FakeConsoleWriter : IConsoleWriter
    {
        public List<string> Messages;
        public FakeConsoleWriter() { Messages = new List<string>(); }
        public void WriteLine(string format, params object[] args) {
            Messages.Add(String.Format(format, args));
        }
    }

    // format is the text with {} in there
    public interface IConsoleWriter { void WriteLine(string format, params object[] args);}
    public class ConsoleWriter : IConsoleWriter {
        public void WriteLine(string format, params object[] args) {
            Console.WriteLine(format, args);
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
