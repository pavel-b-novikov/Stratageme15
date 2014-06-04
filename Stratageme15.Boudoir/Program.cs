using System;
using System.CodeDom.Compiler;
using System.IO;
using Roslyn.Compilers.CSharp;
using Stratageme15.Core.Compiler;
using Stratageme15.Core.Compiler.Exceptions;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.Tools.JavascriptParser;
using Stratageme15.Core.Transaltion;
using Stratageme15.Core.Transaltion.Repositories;
using Stratageme15.Reactors.Basic;

namespace Stratageme15.Boudoir
{
    class Program
    {
        public static void Test()
        {
            ReactorRepository rep = new ReactorRepository();
            rep.RegisterBatch(new BasicReactorBatch());
            AssemblyRepository arep = new AssemblyRepository();
            Translator tr = new Translator(rep, arep);
            SyntaxTree tree = SyntaxTree.ParseFile(@".\Test.cs");
            JsProgram program = null;
            program = tr.Translate(tree);
            try
            {
                
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                throw;
            }

            JavascriptCompilation.Defaults();
            using (var fs = new FileStream(@"..\..\Test.js", FileMode.Create, FileAccess.Write))
            {
                using (TextWriter s = new StreamWriter(fs))
                {
                    using (IndentedTextWriter itw = new IndentedTextWriter(s))
                    {
                        JavascriptCompilation.Compile(itw, program);
                        itw.Flush();
                    }
                    s.Flush();
                }
            }
        }
        public static void Main()
        {
            Test();
            return;//*/

            Scanner js = new Scanner(@".\CcTest.js");
            Parser p = new Parser(js);
            JsProgram program = null;
            using (var fs = new FileStream(@"J:\parseDebug.txt", FileMode.Create, FileAccess.Write))
            {
                using (TextWriter tw = new StreamWriter(fs))
                {
                    NodeInfoTree._debugWriter = tw;
                    try
                    {
                        program = p.Parse();
                        Console.WriteLine("successfully parsed!");
                    }
                    catch (ParseringException e)
                    {
                        Console.WriteLine(e.Message);
                        Console.ReadKey();
                    }
                }
            }

            if (program != null)
            {
                try
                {
                    JavascriptCompilation.Defaults();
                    using (var fs = new FileStream(@"..\..\Test.js", FileMode.Create, FileAccess.Write))
                    {
                        using (TextWriter s = new StreamWriter(fs))
                        {
                            using (IndentedTextWriter itw = new IndentedTextWriter(s))
                            {
                                JavascriptCompilation.Compile(itw, program);    
                                itw.Flush();
                            }
                            s.Flush();
                        }
                    }
                }
                catch (CompilerException ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.ReadKey();
                }
            }

        }
    }
}
