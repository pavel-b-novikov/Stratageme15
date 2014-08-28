using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Stratageme15.Core.Compiler;
using Stratageme15.Core.Compiler.Exceptions;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.Tools.JavascriptParser;
using Stratageme15.Core.Tools.ParsingErrors;
using Stratageme15.Core.Transaltion;
using Stratageme15.Core.Transaltion.Repositories;
using Stratageme15.Reactors.Basic;

namespace Stratageme15.Boudoir
{
    public delegate bool MyDel(int i);

    public static class SyntaxTreeExtensions
    {
        public static SyntaxTree ParseToSyntax(this string fileName)
        {
            using (var fs = new FileStream(@".\Test.cs", FileMode.Open, FileAccess.Read))
            {
                var st = SourceText.From(fs);
                return CSharpSyntaxTree.ParseText(st);
            }
        }

        public static SyntaxTree ParseTextToSyntax(this string text)
        {
            return CSharpSyntaxTree.ParseText(text);
        }
    }
    class Program
    {
        public static void Test()
        {
            ReactorRepository rep = new ReactorRepository();
            rep.RegisterBatch(new BasicReactorBatch());
            AssemblyRepository arep = new AssemblyRepository();
            Translator tr = new Translator(rep, arep,new ConsoleTranslationLogger());
            SyntaxTree tree = @".\Test.cs".ParseToSyntax();
            
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
#if DEBUG
            using (var fs = new FileStream(@"J:\parseDebug.txt", FileMode.Create, FileAccess.Write))
            {
                using (TextWriter tw = new StreamWriter(fs))
                {
                    
                    NodeInfoTree._debugWriter = tw;
#endif
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
#if DEBUG
                }
            }
#endif
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
