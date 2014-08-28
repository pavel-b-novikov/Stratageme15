using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratageme15.ExtensionsPack.Prototyping
{
    /// <summary>
    /// Marks static class as Prototype extensions set
    /// The prototype extensions are constructions like Something.prototype.someFunction = ...
    /// Actually it is designed not to translate maked class into independent structure,
    /// but add some methods to built-in javascript objects
    /// Be careful when writing prototypings - you must not use methods described in prototype
    /// and avoid recursive methods definitions
    /// </summary>
    [AttributeUsage(AttributeTargets.Class,AllowMultiple = false,Inherited = false)]
    public class PrototypeAttribute : Attribute
    {
        public PrototypeAttribute(string prototypeName, bool useCamelCase)
        {
            PrototypeName = prototypeName;
            UseCamelCase = useCamelCase;
        }

        public string PrototypeName { get; private set; }

        public bool UseCamelCase { get; private set; }

        public const string PrototypeAttrKey = "PrototypeAttribute";
    }
}
