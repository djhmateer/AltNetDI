using System;
using System.Collections.Generic;
using Xunit;

//6. Manager wants to output to a file and database (and all logging still to console)
namespace AltNetDI6 {
    class CompositionRoot {
        public static void EMain() {
            IReader reader = new TextFileReader();
            // Pass objects to other objects via Dependency Injection

            // Decorating ConsoleWriter with a WriterLogger.  DateTime.Now is a struct
            //IWriter writer = new WriterLogger(new ConsoleWriter(), new consoleWriterLogger(), DateTime.Now);

            var aggregateWriter = new AggregateWriter(new IWriter[]
            {
                new ConsoleWriter(),
                new FileWriter()
            });

            IWriter writer = new WriterLogger(aggregateWriter, new ConsoleWriterLogger(), DateTime.Now);

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
    public class ConsoleWriter : IWriter {
        public void Write(string text) {
            Console.WriteLine("ConsoleWriter says: {0}", text);
        }
    }
    public class FileWriter : IWriter {
        public void Write(string text) {
            Console.WriteLine("FileWriter says: {0}", text);
        }
    }
    
    public class WriterLogger : IWriter {
        private readonly IWriter writer;
        private readonly IConsoleWriterLogger consoleWriterLogger;
        private readonly DateTime now;

        public WriterLogger(IWriter writer, IConsoleWriterLogger consoleWriterLogger, DateTime now) {
            this.writer = writer;
            this.consoleWriterLogger = consoleWriterLogger;
            this.now = now;
        }

        public void Write(string text) {
            // Abstracted out writing to console so tests only test the WriteLogger
            consoleWriterLogger.WriteLine("WriterLogger says: {0} {1}", now, text);
            writer.Write(text);
            consoleWriterLogger.WriteLine("WriterLogger says: Finished logging");
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


    //5. Testing - want to make sure that the datetime outputted of the writerlogger is correct
    public class WriterLoggerTests {
        [Fact]
        public void ShouldLogMessagesWithAGivenDateTime() {
            var fakeWriter = new FakeWriter();
            var fakeConsoleWriter = new FakeConsoleWriterLogger();
            var dateTime = new DateTime(2014, 1, 24, 01, 02, 03);
            // Passing in fake dependencies so can test WriterLogger in isolation
            var writerLogger = new WriterLogger(fakeWriter, fakeConsoleWriter, dateTime);

            writerLogger.Write("test");

            Assert.Equal("WriterLogger says: 24/01/2014 01:02:03 test", fakeConsoleWriter.Messages[0]);
        }
    }

    // FakeConsoleWriterLogger using a collection to store all messages
    public class FakeConsoleWriterLogger : IConsoleWriterLogger
    {
        public List<string> Messages;
        public FakeConsoleWriterLogger() { Messages = new List<string>(); }
        public void WriteLine(string format, params object[] args) {
            Messages.Add(String.Format(format, args));
        }
    }

    // format is the text with {} in there
    public interface IConsoleWriterLogger { void WriteLine(string format, params object[] args);}
    public class ConsoleWriterLogger : IConsoleWriterLogger {
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
