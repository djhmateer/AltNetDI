using System;
using System.IO;
using Xunit;

//8. Manager wants you to log to screen all transformations with previous 15 characters and following 15 characters 
//   so can see where the changes have taken place
namespace AltNetDI8 {
    class CompositionRoot {
        public static void EMain() {
            IReader reader = new TextFileReader();
            IWriter writer = new ConsoleWriter();
            // A new logger
            ISQLTransformerLogger isqlTransformerLogger = new SQLTransformerLogger();
            // Passing the logger as a dependency to SQLTransformer
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

    public interface IReader { string Read();}
    public class TextFileReader : IReader {
        public string Read() {
            return new StreamReader(@"..\..\dbScript.sql").ReadToEnd();
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
            Console.WriteLine("SQLTransformerLogger says: {0}" + Environment.NewLine + Environment.NewLine, message);
        }

        public string GetSurroundingTextForReplace(string allSQL, int positionOfReplace) {
            return allSQL.Substring(positionOfReplace - 18, 30);
        }
    }

    public interface IWriter { void Write(string text);}
    public class ConsoleWriter : IWriter {
        public void Write(string text) {
            Console.WriteLine("ConsoleWriter says: {0} " , text);
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
}
