using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Stratageme15.Core.Translation.Reactors;
using Stratageme15.ExtensionsPack.Prototyping;
using Stratageme15.ExtensionsPack.Tactics;

namespace Stratageme15.ExtensionsPack
{
    class ExtensionsPackReactorsBatch : ReactorBatchBase
    {
        private TacticsRepository _tactics;

        protected override void Reactors()
        {
            #region Prototypes
            RegisterReactor<PrototypeClassDeclarationSyntaxReactor, ClassDeclarationSyntax>();
            RegisterReactor<PrototypeMethodDeclarationSyntaxReactor,MethodDeclarationSyntax>();
            #endregion
        }

        public ExtensionsPackReactorsBatch(TacticsRepository tactics)
        {
            _tactics = tactics;
        }

        public override object ReactorBatchData
        {
            get { return _tactics; }
        }
    }
}
