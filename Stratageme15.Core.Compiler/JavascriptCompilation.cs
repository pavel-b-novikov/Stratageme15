using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Stratageme15.Core.JavascriptCodeDom;

namespace Stratageme15.Core.Compiler
{
    /// <summary>
    /// Facade for JsCompiler and repository
    /// </summary>
    public static class JavascriptCompilation
    {
        private static readonly NodeCompilersRepository _repository = new NodeCompilersRepository();

        /// <summary>
        /// Removes all registered node compilers
        /// </summary>
        public static void ClearRepository()
        {
            _repository.Clear();
        }

        /// <summary>
        /// Registers new NodeCompiler in repository
        /// </summary>
        /// <typeparam name="TNodeType">Type of node compiler</typeparam>
        public static void Register<TNodeType>() where TNodeType : INodeCompiler, new()
        {
            _repository.Register<TNodeType>();
        }

        /// <summary>
        /// Registers new NodeCompiler in repository
        /// </summary>
        /// <param name="nodeCompilerType">Type of node compiler</param>
        public static void Register(Type nodeCompilerType)
        {
            _repository.Register(nodeCompilerType);
        }


        /// <summary>
        /// Scans assembly for node compilers and registers them all
        /// </summary>
        /// <param name="a"></param>
        public static void RegisterAssembly(Assembly a)
        {
            var types = a.GetTypes();
            var ncdbType = typeof (INodeCompiler);
            var compilerTypes = types.Where(t => ncdbType.IsAssignableFrom(t) && !t.IsAbstract);
            foreach (var compilerType in compilerTypes)
            {
                Register(compilerType);
            }
        }

        /// <summary>
        /// Register executing assembly nodecompilers
        /// </summary>
        public static void RegisterSelf()
        {
            RegisterAssembly(Assembly.GetExecutingAssembly());
        }

        /// <summary>
        /// Register default node compilers
        /// </summary>
        public static void Defaults()
        {
            RegisterAssembly(typeof(JavascriptCompilation).Assembly);
        }

        public static void Compile(Stream s,JsProgram program)
        {
            JsCompiler jsc = new JsCompiler(program,_repository);
            jsc.Compile(s);
        }

        public static void Compile(TextWriter s, JsProgram program)
        {
            JsCompiler jsc = new JsCompiler(program, _repository);
            jsc.Compile(s);
        }
    }
}
