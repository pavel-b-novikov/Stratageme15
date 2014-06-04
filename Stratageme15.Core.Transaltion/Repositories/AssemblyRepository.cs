using System;
using System.Collections.Generic;
using System.Reflection;

namespace Stratageme15.Core.Transaltion.Repositories
{
    public class AssemblyRepository
    {
        public Type GetType(string fullTypeName)
        {
            var t = (Assembly.GetCallingAssembly().GetType(fullTypeName) ??
                     Assembly.GetEntryAssembly().GetType(fullTypeName)) ??
                    Assembly.GetExecutingAssembly().GetType(fullTypeName);
            return t;
        }

        public Type GetType(string typeName,IEnumerable<string> usings, string ns)
        {
            foreach (var usg in usings)
            {
                string tn = string.Format("{0}.{1}", usg, typeName);
                var t = GetType(tn);
                if (t!=null)
                {
                    return t;
                }
            }

            return GetType(string.Format("{0}.{1}",ns, typeName));
        }
    }
}
