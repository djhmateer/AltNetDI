using System;
using System.IO;
using Xunit;

// 8. Manager wants you to log to screen all transformations with previous 15 characters and following 15 characters 
//    so can see where the changes have taken place
namespace AltNetDI8Decorator {
    class CompositionRoot {
        public static void EMain() {
            IReader reader = new TextFileReader();
            IWriter writer = new ConsoleWriter();

            //ISQLTransformer isqlTransformer = new SQLTransformer(isqlTransformerLogger);

            // Use decorator so that SQLTransformer only does transforming, and not logging we can wrap it 
            ISQLTransformer isqlTransformer = new SQLTransformerLogger(new SQLTransformer());

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

        public string ReplaceOldDbNamesWithNewDbNames(string sql) {
            // class / method should only do 1 thing!!! 
            // ie not call a logger here
            return sql.Replace("[Funny]", "[FunnyNewDb]");
        }
    }

    public class SQLTransformerLogger : ISQLTransformer {
        private readonly ISQLTransformer sqlTransformer;

        public SQLTransformerLogger(ISQLTransformer sqlTransformer) {
            this.sqlTransformer = sqlTransformer;
        }

        public string ReplaceOldDbNamesWithNewDbNames(string sql) {
            Console.WriteLine("SQLTransformerLogger says: {0}", sql);
            return sqlTransformer.ReplaceOldDbNamesWithNewDbNames(sql);
        }
    }

    public interface IWriter { void Write(string text);}
    public class ConsoleWriter : IWriter {
        public void Write(string text) {
            Console.WriteLine("ConsoleWriter says: {0} ", text);
        }
    }

    public class SQLTransformerTests {
        [Fact]
        public void ReplaceOldDbNamesWithNewDbNames_WhenFunny_ShouldReplaceWithFunnyNewDb() {
            var transformer = new SQLTransformer();
            var result = transformer.ReplaceOldDbNamesWithNewDbNames("[Funny]");
            Assert.Equal("[FunnyNewDb]", result);
        }

        [Fact]
        public void ReplaceOldDbNamesWithNewDbNames_WhenMultipleFunny_ShouldReplaceWithFunnyNewDb() {
            var transformer = new SQLTransformer();
            var result = transformer.ReplaceOldDbNamesWithNewDbNames("[Funny] asdf asdf [Funny] asdfasdf");
            Assert.Equal("[FunnyNewDb] asdf asdf [FunnyNewDb] asdfasdf", result);
        }
    }
}
