using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Roslyn.Compilers.CSharp;
using Stratageme15.Core.Compiler;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.Transaltion;
using Stratageme15.Core.Transaltion.Repositories;
using Stratageme15.Reactors.Basic;

namespace Stratageme15.Boudoir
{
    public class TestClass
    {
        public string TestPropS { get; set; }
        public int TestPropInt { get; set; }

        public void Method1()
        {

        }

        public bool Method2()
        {
            return true;
        }
    }

    [TestClass]
    public class Experiments
    {
        public TestContext TestContext { get; set; }
        public static IEnumerable<Type> GetAncestorTypes(Type type,bool abstr)
        {
            while (type.BaseType != null)
            {
                if (abstr)
                {
                    if (type.IsAbstract) yield return type.BaseType;
                }else
                {
                    if (!type.IsAbstract) yield return type.BaseType;
                }
                type = type.BaseType;
            }
        }

        public static IEnumerable<Type> GetAllDerivedTypesInAssembly(Type baseType,bool abstr)
        {
            return baseType.Assembly.GetTypes().Where(t => GetAncestorTypes(t,abstr).Contains(baseType));
        }
        [TestMethod]
        public void AllStatementsList()
        {
            var tp = typeof(TypeSyntax);
            if (tp.IsAbstract) TestContext.WriteLine("Type is ABSTRACT");
            else TestContext.WriteLine("Type is CONCRETE");

            TestContext.WriteLine("Abstract:");
            var st = GetAllDerivedTypesInAssembly(tp, true);// typeof(ExpressionStatementSyntax).Assembly.GetTypes().Where(c => typeof(ExpressionSyntax).IsAssignableFrom(c));
            foreach (var type in st)
            {
                TestContext.WriteLine(type.FullName);
            }

            TestContext.WriteLine("Concrete:");
            st = GetAllDerivedTypesInAssembly(tp, false);// typeof(ExpressionStatementSyntax).Assembly.GetTypes().Where(c => typeof(ExpressionSyntax).IsAssignableFrom(c));
            foreach (var type in st)
            {
                TestContext.WriteLine(type.FullName);
            }
            //Assert.Fail();
        }

        [TestMethod]
        public void TestRoslyn()
        {
            SyntaxTree tree = SyntaxTree.ParseFile(@".\Test.cs");
            var root = tree.GetRoot();
            TraverseRoot(root);
        }
        private void TraverseRoot(SyntaxNode root,Type breakOn = null)
        {
            
            Stack<SyntaxNode> synTraversal = new Stack<SyntaxNode>();
            synTraversal.Push(root);
            while (synTraversal.Count > 0)
            {
                var cnode = synTraversal.Pop();
                TestContext.WriteLine("[{0}] {1}", cnode.GetType().Name, cnode.ToString());
                if (breakOn!=null)
                {
                    if (cnode.GetType() == breakOn) Debugger.Break();
                }
                foreach (var source in cnode.ChildNodes().Reverse())
                {
                    synTraversal.Push(source);
                }
            }
        }
        [TestMethod]
        public void TestTranslator()
        {
            ReactorRepository rep = new ReactorRepository();
            rep.RegisterBatch(new BasicReactorBatch());
            AssemblyRepository arep = new AssemblyRepository();
            Translator tr = new Translator(rep,arep);
            SyntaxTree tree = SyntaxTree.ParseFile(@".\Test.cs");

            JsProgram program = tr.Translate(tree);

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

        [TestMethod]
        public void RegexTest()
        {
            Regex r = new Regex("[a-zA-Z0-9]");
            Console.WriteLine(r.ToString());
        }

        [TestMethod]
        public void RoslynConstruction()
        {
            var allPrimitive = typeof (object).Assembly.GetTypes().Where(c => c.IsPrimitive);
            var tree = SyntaxTree.ParseText("public void Test() { try { 1/0; } catch(Exception) { i++; }");
            var root = tree.GetRoot();
            TraverseRoot(root, typeof(CatchDeclarationSyntax));
        }


        [TestMethod]
        public void TraverseTypesHierarchy()
        {
            TraverseChildrenTypes(typeof(SyntaxNode),0);
        }
        private string Tabulate(string s,int tabLevel)
        {
            string tabs = new string(' ',tabLevel*6);
            return string.Format("{0}{1}", tabs, s);
        }
        private void TraverseChildrenTypes(Type root,int tablevel)
        {
            var children = root.Assembly.GetTypes().Where(c => c.BaseType == root);
            if (!children.Any())
            {
                Console.Write(": '{0}',",root.GUID);
                Console.WriteLine();
                return;
            };
            Console.WriteLine(":");
            Console.WriteLine(Tabulate("{",tablevel));
            foreach (var child in children)
            {
                Console.Write(Tabulate(child.Name,tablevel+1));
                if (child.IsAbstract) Console.Write(" (abstract)");
                TraverseChildrenTypes(child,tablevel+1);
            }
            Console.WriteLine(Tabulate("},",tablevel));
            Console.WriteLine();
        }

    }
}
