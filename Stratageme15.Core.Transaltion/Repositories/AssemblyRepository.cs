using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.CodeAnalysis;

namespace Stratageme15.Core.Translation.Repositories
{
    public class AssemblyRepository
    {
        public enum FrameworkVersion
        {
            v1_0_3705,
            v1_1_4322,
            v2_0_50727,
            v3_0,
            v3_5,
            v4_0_30319
        }
        private static readonly Dictionary<FrameworkVersion,string> _frameworkDirs = new Dictionary<FrameworkVersion,string>()
                                                                                {
                                                                                    {FrameworkVersion.v1_0_3705, "v1.0.3705"},
                                                                                    {FrameworkVersion.v1_1_4322, "v1.1.4322"},
                                                                                    {FrameworkVersion.v2_0_50727, "v2.0.50727"},
                                                                                    {FrameworkVersion.v3_0, "v3.0"},
                                                                                    {FrameworkVersion.v3_5, "v3.5"},
                                                                                    {FrameworkVersion.v4_0_30319, "v4.0.30319"}
                                                                                };
        private static readonly DirectoryInfo _frameworkInstallPath;

        static AssemblyRepository()
        {
            var mscorlibPath = typeof (string).Assembly.Location;
            var systemDllsPath = Path.GetDirectoryName(mscorlibPath);
            DirectoryInfo di = new DirectoryInfo(systemDllsPath);
            _frameworkInstallPath = di.Parent;
        }

        public void AddSystemReference(string dllName,FrameworkVersion version)
        {
            var dir = _frameworkInstallPath.GetDirectories(_frameworkDirs[version])[0];
            string file = Path.Combine(dir.FullName, dllName);
            AddReference(file);
        }

        private readonly List<MetadataReference> _references = new List<MetadataReference>();

        internal IEnumerable<MetadataReference> GetMetadataReferences()
        {
            return _references;
        }

        public void AddReference(string path)
        {
            var libName = Path.GetFileName(path);
            using(var fs = new FileStream(path,FileMode.Open,FileAccess.Read))
            {
                var r = new MetadataImageReference(fs,display:libName);
                _references.Add(r);
            }
        }

        public Type GetType(string fullTypeName)
        {
            Type t = (Assembly.GetCallingAssembly().GetType(fullTypeName) ??
                      Assembly.GetEntryAssembly().GetType(fullTypeName)) ??
                     Assembly.GetExecutingAssembly().GetType(fullTypeName) ??
                     Type.GetType(fullTypeName);
            return t;
        }

        public Type GetType(string typeName, IEnumerable<string> usings, string ns)
        {
            foreach (string usg in usings)
            {
                string tn = string.Format("{0}.{1}", usg, typeName);
                Type t = GetType(tn);
                if (t != null)
                {
                    return t;
                }
            }

            return GetType(string.Format("{0}.{1}", ns, typeName));
        }

        #region References
        public void AddMscorlibReference(FrameworkVersion version)
        {
            AddSystemReference("mscorlib.dll", version);
        }
        public void AddSystemCoreReference(FrameworkVersion version)
        {
            AddSystemReference("System.Core.dll", version);
        }
        public void AddSystemWindowsFormsReference(FrameworkVersion version)
        {
            AddSystemReference("System.Windows.Forms.dll", version);
        }
        public void AddSystemDrawingReference(FrameworkVersion version)
        {
            AddSystemReference("System.Drawing.dll", version);
        }
        public void AddSystemDataReference(FrameworkVersion version)
        {
            AddSystemReference("System.Data.dll", version);
        }
        #endregion
    }
}