using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

//7. Manager wants you to write business logic that looks for [OldDbName] and
// replace it with [NewDbName] in Sql script input file
namespace AltNetDI7 {
    class CompositionRoot {
        public static void EMain() {
            IReader reader = new TextFileReader();
            IWriter writer = new ConsoleWriter();
            ISQLTransformer isqlTransformer = new SQLTransformer();
            IApplication application = new Application(reader, writer, isqlTransformer);
            application.Run();
        }
    }

    public interface IApplication { void Run();}
    public class Application : IApplication {
        private readonly IReader reader;
        private readonly IWriter writer;
        private readonly ISQLTransformer sqlTransformer;

        public Application(IReader reader, IWriter writer, ISQLTransformer sqlTransformer) {
            this.reader = reader;
            this.writer = writer;
            this.sqlTransformer = sqlTransformer;
        }

        public void Run() {
            var text = reader.Read();
            var transformedText = sqlTransformer.ReplaceOldDbNamesWithNewDbNames(text);
            writer.Write(transformedText);
        }
    }

    public interface ISQLTransformer { string ReplaceOldDbNamesWithNewDbNames(string sql);}
    public class SQLTransformer : ISQLTransformer {
        public string ReplaceOldDbNamesWithNewDbNames(string sql) {
            sql = sql.Replace("[Funny]", "[FunnyNewDb]");
            return sql;
        }
    }

    public interface IReader { string Read();}
    public class TextFileReader : IReader {
        public string Read() {
            return new StreamReader(@"..\..\dbScript.sql").ReadToEnd();
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

    //public class WriterLogger : IWriter {
    //    private readonly IWriter writer;
    //    private readonly IConsoleWriterLogger consoleWriterLogger;
    //    private readonly DateTime now;

    //    public WriterLogger(IWriter writer, IConsoleWriterLogger consoleWriterLogger, DateTime now) {
    //        this.writer = writer;
    //        this.consoleWriterLogger = consoleWriterLogger;
    //        this.now = now;
    //    }

    //    public void Write(string text) {
    //        // Abstracted out writing to console so tests only test the WriteLogger
    //        consoleWriterLogger.WriteLine("WriterLogger says: {0} {1}", now, text);
    //        writer.Write(text);
    //        consoleWriterLogger.WriteLine("WriterLogger says: Finished logging");
    //    }
    //}

    //public class AggregateWriter : IWriter {
    //    private readonly IWriter[] writers;

    //    public AggregateWriter(IWriter[] writers) {
    //        this.writers = writers;
    //    }

    //    public void Write(string text) {
    //        foreach (var writer in writers) {
    //            writer.Write(text);
    //        }
    //    }
    //}

    //// format is the sql with {} in there
    //public interface IConsoleWriterLogger { void WriteLine(string format, params object[] args);}
    //public class ConsoleWriterLogger : IConsoleWriterLogger {
    //    public void WriteLine(string format, params object[] args) {
    //        Console.WriteLine(format, args);
    //    }
    //}

    //public class WriterLoggerTests {
    //    [Fact]
    //    public void ShouldLogMessagesWithAGivenDateTime() {
    //        var fakeWriter = new FakeWriter();
    //        var fakeConsoleWriter = new FakeConsoleWriterLogger();
    //        var dateTime = new DateTime(2014, 1, 24, 01, 02, 03);
    //        // Passing in fake dependencies so can test WriterLogger in isolation
    //        var writerLogger = new WriterLogger(fakeWriter, fakeConsoleWriter, dateTime);

    //        writerLogger.Write("test");

    //        Assert.Equal("WriterLogger says: 24/01/2014 01:02:03 test", fakeConsoleWriter.Messages[0]);
    //    }
    //}

    //// FakeConsoleWriterLogger using a collection to store all messages
    //public class FakeConsoleWriterLogger : IConsoleWriterLogger
    //{
    //    public List<string> Messages;
    //    public FakeConsoleWriterLogger() { Messages = new List<string>(); }
    //    public void WriteLine(string format, params object[] args) {
    //        Messages.Add(String.Format(format, args));
    //    }
    //}



    //public class FakeWriter : IWriter {
    //    public void Write(string sql) { }
    //}

    //public class TextFileReaderTest {
    //    [Fact]
    //    public void Read_ShouldReturnBlah() {
    //        var reader = new TextFileReader();
    //        var result = reader.Read();
    //        Assert.Equal("blah", result);
    //    }
    //}
}
