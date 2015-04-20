using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stratageme15.Core.Compiler;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.Tools.JavascriptParser;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.Logging;
using Stratageme15.Core.Translation.Reactors;
using Stratageme15.Core.Translation.Repositories;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Stratageme15.Reactors.Basic.Tests.NodesComparing;

namespace Stratageme15.Reactors.Basic.Tests
{
    public class StratagemeTestBase
    {
        protected void Init()
        {
            JavascriptCompilation.Defaults();
        }

        protected string Code(JsProgram program)
        {
            StringBuilder sb = new StringBuilder();
            using(StringWriter sw = new StringWriter(sb))
            {
                using (IndentedTextWriter itw = new IndentedTextWriter(sw))
                {
                    JavascriptCompilation.Compile(itw, program);
                    itw.Flush();
                }
                sw.Flush();
            }
            return sb.ToString();
        }

        public Translator InitializeTranslator(string code, params ReactorBatchBase[] batches)
        {
            ReactorRepository rep = new ReactorRepository();
            foreach (var reactorBatchBase in batches)
            {
                rep.RegisterBatch(reactorBatchBase);
            }

            AssemblyRepository arep = new AssemblyRepository();
            arep.AddMscorlibReference(AssemblyRepository.FrameworkVersion.v4_0_30319);
            Translator tr = new Translator(rep, arep, new ConsoleTranslationLogger());
            return tr;
        }

        public Translator InitializeTranslatorWithContext(string code, out SyntaxTree synTree, params ReactorBatchBase[] batches)
        {
            ReactorRepository rep = new ReactorRepository();
            foreach (var reactorBatchBase in batches)
            {
                rep.RegisterBatch(reactorBatchBase);
            }

            AssemblyRepository arep = new AssemblyRepository();
            arep.AddMscorlibReference(AssemblyRepository.FrameworkVersion.v4_0_30319);
            Translator tr = new Translator(rep, arep, new ConsoleTranslationLogger());
            using (var stream = StringStream(code))
            {
                var st = SourceText.From(stream);
                var tree = CSharpSyntaxTree.ParseText(st);
                synTree = tree;
                tr.CreateTranslationContext(tree);
            }
            return tr;
        }
        public JsProgram Translate(string cSharpProgram,params ReactorBatchBase[] batches)
        {
            Translator tr = InitializeTranslator(cSharpProgram, batches);
            using (var stream = StringStream(cSharpProgram))
            {
                var st = SourceText.From(stream);
                var tree =  CSharpSyntaxTree.ParseText(st);
                JsProgram program = tr.Translate(tree);
                return program;
            }
        }

        protected JsProgram Parse(string jsProgram)
        {
            using (Stream s = StringStream(jsProgram))
            {
                Scanner scn = new Scanner(s);
                Parser p = new Parser(scn);
                return p.Parse();
            }
        }

        public Stream StringStream(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        protected bool Compare(JsProgram actual,JsProgram expected)
        {
            Stack<SyntaxTreeNodeBase> actualStack = new Stack<SyntaxTreeNodeBase>();
            Stack<SyntaxTreeNodeBase> expectedStack = new Stack<SyntaxTreeNodeBase>();
            actualStack.Push(actual);
            expectedStack.Push(expected);
            while (actualStack.Count>0)
            {
                var a = actualStack.Pop();
                var e = expectedStack.Pop();
                if (actualStack.Count != expectedStack.Count) 
                    return false;
                if (a==null&&e==null) continue;
                var childrenA = a.Children.ToList();
                var childrenE = e.Children.ToList();
                if (childrenA.Count != childrenE.Count) 
                    return false;
                if (a.GetType() != e.GetType()) 
                    return false;
                var comparer = NodeComparerFactory.GetComparerFor(a.GetType());
                if (!comparer.EqualsExceptChildren(a, e)) 
                    return false;
                childrenA.ForEach(actualStack.Push);
                childrenE.ForEach(expectedStack.Push);
            }
            return true;
        }


    }
}
