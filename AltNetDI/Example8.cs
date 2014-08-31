﻿using System;
using System.IO;
using Xunit;

//8. Manager wants you to log to screen all transformations with 
// previous 2 lines and following 2 lines
namespace AltNetDI8 {
    class CompositionRoot {
        public static void EMain() {
            IReader reader = new TextFileReader();
            IWriter writer = new ConsoleWriter();
            ISQLTransformerLogger isqlTransformerLogger = new SQLTransformerLogger();
            ISQLTransformer isqlTransformer = new SQLTransformer(isqlTransformerLogger);

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
            var sql = reader.Read();
            var transformedSql = sqlTransformer.ReplaceOldDbNamesWithNewDbNames(sql);
            writer.Write(transformedSql);
        }
    }

    public interface ISQLTransformer { string ReplaceOldDbNamesWithNewDbNames(string sql);}
    public class SQLTransformer : ISQLTransformer {
        private readonly ISQLTransformerLogger isqlTransformerLogger;

        public SQLTransformer(ISQLTransformerLogger isqlTransformerLogger) {
            this.isqlTransformerLogger = isqlTransformerLogger;
        }

        public string ReplaceOldDbNamesWithNewDbNames(string sql) {
            var positionOfReplace = sql.IndexOf("[Funny]");
            isqlTransformerLogger.Log(sql, positionOfReplace);
            return sql.Replace("[Funny]", "[FunnyNewDb]");
        }
    }

    public interface ISQLTransformerLogger { void Log(string allSQL, int positionOfReplace);}
    public class SQLTransformerLogger : ISQLTransformerLogger {
        public void Log(string allSQL, int positionOfReplace) {
            var message = GetSurroundingTextForReplace(allSQL, positionOfReplace);
            Console.WriteLine("SQLTransformerLogger says: {0}", message);
        }

        public string GetSurroundingTextForReplace(string allSQL, int positionOfReplace) {
            // Could do get preceeding 2 lines and subsequent 2 lines here
            return allSQL.Substring(positionOfReplace - 15, 30);
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

    public class SQLTransformerTests {
        [Fact]
        public void ReplaceOldDbNamesWithNewDbNames_WhenFunny_ShouldReplaceWithFunnyNewDb() {
            var transformer = new SQLTransformer(new FakeSQLTransformerLogger());
            var result = transformer.ReplaceOldDbNamesWithNewDbNames("[Funny]");
            Assert.Equal("[FunnyNewDb]", result);
        }

        [Fact]
        public void ReplaceOldDbNamesWithNewDbNames_WhenMultipleFunny_ShouldReplaceWithFunnyNewDb() {
            var transformer = new SQLTransformer(new FakeSQLTransformerLogger());
            var result = transformer.ReplaceOldDbNamesWithNewDbNames("[Funny] asdf asdf [Funny] asdfasdf");
            Assert.Equal("[FunnyNewDb] asdf asdf [FunnyNewDb] asdfasdf", result);
        }
    }

    public class FakeSQLTransformerLogger : ISQLTransformerLogger {
        public void Log(string allSQL, int positionOfReplace) { }
    }

    //public class FileWriter : IWriter {
    //    public void Write(string text) {
    //        Console.WriteLine("FileWriter says: {0}", text);
    //    }
    //}

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
