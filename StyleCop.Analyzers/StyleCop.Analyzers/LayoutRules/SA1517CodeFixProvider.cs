﻿namespace StyleCop.Analyzers.LayoutRules
{
    using System.Collections.Immutable;
    using System.Composition;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CodeActions;
    using Microsoft.CodeAnalysis.CodeFixes;
    using Microsoft.CodeAnalysis.CSharp;

    using StyleCop.Analyzers.Helpers;

    /// <summary>
    /// Implements a code fix for <see cref="SA1517CodeMustNotContainBlankLinesAtStartOfFile"/>.
    /// </summary>
    [ExportCodeFixProvider(nameof(SA1517CodeFixProvider), LanguageNames.CSharp)]
    [Shared]
    public class SA1517CodeFixProvider : CodeFixProvider
    {
        private static readonly ImmutableArray<string> FixableDiagnostics =
            ImmutableArray.Create(SA1517CodeMustNotContainBlankLinesAtStartOfFile.DiagnosticId);

        /// <inheritdoc/>
        public override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return FixableDiagnostics; }
        }

        /// <inheritdoc/>
        public override FixAllProvider GetFixAllProvider()
        {
            return WellKnownFixAllProviders.BatchFixer;
        }

        /// <inheritdoc/>
        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            foreach (Diagnostic diagnostic in context.Diagnostics.Where(d => FixableDiagnostics.Contains(d.Id)))
            {
                var syntaxRoot = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

                var firstToken = syntaxRoot.GetFirstToken(includeZeroWidth: true);
                var leadingTrivia = firstToken.LeadingTrivia;

                var newTriviaList = SyntaxFactory.TriviaList();

                var firstNonBlankLineTriviaIndex = TriviaHelper.IndexOfFirstNonBlankLineTrivia(leadingTrivia);
                if (firstNonBlankLineTriviaIndex != -1)
                {
                    for (var index = firstNonBlankLineTriviaIndex; index < leadingTrivia.Count; index++)
                    {
                        newTriviaList = newTriviaList.Add(leadingTrivia[index]);
                    }
                }

                var newFirstToken = firstToken.WithLeadingTrivia(newTriviaList);
                var newSyntaxRoot = syntaxRoot.ReplaceToken(firstToken, newFirstToken);
                var newDocument = context.Document.WithSyntaxRoot(newSyntaxRoot);

                context.RegisterCodeFix(CodeAction.Create("Remove blank lines at the start of the file", token => Task.FromResult(newDocument)), diagnostic);
            }
        }
    }
}
