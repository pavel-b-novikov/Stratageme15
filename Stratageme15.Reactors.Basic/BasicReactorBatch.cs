using Roslyn.Compilers.CSharp;
using Stratageme15.Core.Transaltion.Reactors;
using Stratageme15.Reactors.Basic.Declarations;
using Stratageme15.Reactors.Basic.Declarations.Field;
using Stratageme15.Reactors.Basic.Expressions;
using Stratageme15.Reactors.Basic.Statements;
using Stratageme15.Reactors.Basic.Statements.For;
using Stratageme15.Reactors.Basic.Statements.If;
using Stratageme15.Reactors.Basic.Statements.LocalDeclaration;
using Stratageme15.Reactors.Basic.Statements.Switch;
using Stratageme15.Reactors.Basic.Statements.Try;

namespace Stratageme15.Reactors.Basic
{
    public class BasicReactorBatch : ReactorBatchBase
    {
        protected override void Reactors()
        {
            #region Declarations
                RegisterReactor<UsingDirectiveSyntaxReactor,UsingDirectiveSyntax>();
                RegisterReactor<NamespaceDeclarationSyntaxReactor,NamespaceDeclarationSyntax>();
                RegisterReactor<ClassDeclarationSyntaxReactor,ClassDeclarationSyntax>();
                RegisterReactor<ConstructorDeclarationSyntaxReactor, ConstructorDeclarationSyntax>();
                RegisterReactor<ParameterListSyntaxReactor, ParameterListSyntax>();
                RegisterReactor<ArgumentListSyntaxReactor,ArgumentListSyntax>();
                RegisterReactor<BlockSyntaxReactor, BlockSyntax>();
                #region Fields
                RegisterReactor<FieldDeclarationSyntaxReactor, FieldDeclarationSyntax>();
                RegisterReactor<FieldVariableDeclarationSyntaxReactor, VariableDeclarationSyntax>();
                #endregion
            #endregion

                #region Expressions
                RegisterReactor<LiteralExpressionSyntaxReactor, LiteralExpressionSyntax>();
                RegisterReactor<ThisExpressionSyntaxReactor, ThisExpressionSyntax>();
                RegisterReactor<ParenthesizedExpressionSyntaxReactor, ParenthesizedExpressionSyntax>();
                RegisterReactor<BinaryExpressionSyntaxReactor, BinaryExpressionSyntax>();
                RegisterReactor<IdentifierNameSyntaxReactor, IdentifierNameSyntax>();

                RegisterReactor<MemberAccessExpressionSyntaxReactor, MemberAccessExpressionSyntax>();
                RegisterReactor<InvocationExpressionSyntaxReactor, InvocationExpressionSyntax>();
                RegisterReactor<ElementAccessExpressionSyntaxReactor, ElementAccessExpressionSyntax>();
                RegisterReactor<BracketedArgumentListSyntaxReactor, BracketedArgumentListSyntax>();

                RegisterReactor<ObjectCreationExpressionSyntaxReactor, ObjectCreationExpressionSyntax>();

                RegisterReactor<ArrayCreationExpressionSyntaxReactor, ArrayCreationExpressionSyntax>();
                RegisterReactor<ImplicitArrayCreationExpressionSyntaxReactor, ImplicitArrayCreationExpressionSyntax>();

                RegisterReactor<AnonymousObjectCreationExpressionSyntaxReactor, AnonymousObjectCreationExpressionSyntax>();
                RegisterReactor<AnonymousObjectMemberDeclaratorSyntaxReactor, AnonymousObjectMemberDeclaratorSyntax>();

                RegisterReactor<ConditionalExpressionSyntaxReactor, ConditionalExpressionSyntax>();
                RegisterReactor<PrefixUnaryExpressionSyntaxReactor, PrefixUnaryExpressionSyntax>();
                RegisterReactor<PostfixUnaryExpressionSyntaxReactor, PostfixUnaryExpressionSyntax>();
            #endregion

            #region Local variable declaration
                RegisterReactor<VariableDeclarationSyntaxReactor,VariableDeclarationSyntax>();
                RegisterReactor<VariableDeclaratorSyntaxReactor, VariableDeclaratorSyntax>();
            #endregion

            #region Statements
                RegisterReactor<StatementBlockSyntaxReactor, BlockSyntax>();
                RegisterReactor<MethodDeclarationSyntaxReactor, MethodDeclarationSyntax>();
                RegisterReactor<ReturnStatementSyntaxReactor, ReturnStatementSyntax>();
                RegisterReactor<EmptyStatementSyntaxReactor, EmptyStatementSyntax>();
                RegisterReactor<LabeledStatementSyntaxReactor, LabeledStatementSyntax>();
                RegisterReactor<GotoStatementSyntaxReactor, GotoStatementSyntax>();
                RegisterReactor<ThrowStatementSyntaxReactor, ThrowStatementSyntax>();

                #region Cycles
                    #region For statement
                        RegisterReactor<ForInitializerExpressionSyntaxReactor, InitializerExpressionSyntax>();
                        RegisterReactor<ForStatementSyntaxReactor, ForStatementSyntax>();
                    #endregion
                    RegisterReactor<DoStatementSyntaxReactor, DoStatementSyntax>();
                    RegisterReactor<WhileStatementSyntaxReactor, WhileStatementSyntax>();
                    RegisterReactor<BreakStatementSyntaxReactor, BreakStatementSyntax>();
                    RegisterReactor<ContinueStatementSyntaxReactor, ContinueStatementSyntax>();
                #endregion

                #region If statement
                    RegisterReactor<IfStatementSyntaxReactor, IfStatementSyntax>();
                    RegisterReactor<ElseClauseSyntaxReactor, ElseClauseSyntax>();
                #endregion

                #region Try-Catch-Finally
                    RegisterReactor<TryStatementSyntaxReactor, TryStatementSyntax>();
                    RegisterReactor<CatchClauseSyntaxReactor, CatchClauseSyntax>();
                    RegisterReactor<FinallyClauseSyntaxReactor, FinallyClauseSyntax>();
                #endregion
            #region Switch statement
                    RegisterReactor<SwitchLabelSyntaxReactor, SwitchLabelSyntax>();
                    RegisterReactor<SwitchStatementSyntaxReactor, SwitchStatementSyntax>();
                    RegisterReactor<SwitchSectionSyntaxReactor, SwitchSectionSyntax>();
            #endregion
            #endregion


        }
    }
}
