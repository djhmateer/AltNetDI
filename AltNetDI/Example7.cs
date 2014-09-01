using System;
using System.IO;
using Xunit;

// 7.Manager wants you to write business logic that looks for [Funny] and
// replace it with [FunnyNewDb] in Sql script input file
namespace AltNetDI7 {
    class CompositionRoot {
        public static void EMain() {
            IReader reader = new TextFileReader();
            IWriter writer = new ConsoleWriter();
            // The new object we're going to use to transform
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
            var sql = reader.Read();
            var transformedSql = sqlTransformer.ReplaceOldDbNamesWithNewDbNames(sql);
            writer.Write(transformedSql);
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

    public class SQLTransformerTests {
        [Fact]
        public void ReplaceOldDbNamesWithNewDbNames_WhenFunny_ShouldReplaceWithFunnyNewDb() {
            var transformer = new SQLTransformer();
            var result = transformer.ReplaceOldDbNamesWithNewDbNames("[Funny]");
            Assert.Equal("[FunnyNewDb]", result);
        }
    }
}
