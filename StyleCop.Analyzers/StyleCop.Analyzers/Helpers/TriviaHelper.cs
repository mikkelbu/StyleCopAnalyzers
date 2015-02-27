﻿namespace StyleCop.Analyzers.Helpers
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    /// <summary>
    /// Provides helper methods to work with trivia (lists).
    /// </summary>
    internal static class TriviaHelper
    {
        /// <summary>
        /// Returns the index of the first non-whitespace trivia in the given trivia list.
        /// </summary>
        /// <param name="triviaList">The trivia list to process.</param>
        /// <returns>The index where the non-whitespace starts, or -1 if there is no non-whitespace trivia.</returns>
        internal static int IndexOfFirstNonWhitespaceTrivia(SyntaxTriviaList triviaList)
        {
            for (var index = 0; index < triviaList.Count; index++)
            {
                var currentTrivia = triviaList[index];
                switch (currentTrivia.Kind())
                {
                    case SyntaxKind.EndOfLineTrivia:
                    case SyntaxKind.WhitespaceTrivia:
                        break;

                    default:
                        // encountered non-whitespace trivia -> the search is done.
                        return index;
                }
            }

            return -1;
        }

        /// <summary>
        /// Returns the index of the first trivia that is not part of a blank line.
        /// </summary>
        /// <param name="triviaList">The trivia list to process.</param>
        /// <returns>The index of the first trivia that is not part of a blank line, or -1 if there is no such trivia.</returns>
        internal static int IndexOfFirstNonBlankLineTrivia(SyntaxTriviaList triviaList)
        {
            var firstNonWhitespaceTriviaIndex = IndexOfFirstNonWhitespaceTrivia(triviaList);
            var startIndex = (firstNonWhitespaceTriviaIndex == -1) ? triviaList.Count : firstNonWhitespaceTriviaIndex;

            for (var index = startIndex - 1; index >= 0; index--)
            {
                // Find an end-of-line trivia, to indicate that there actually are blank lines and not just excess whitespace.
                if (triviaList[index].IsKind(SyntaxKind.EndOfLineTrivia))
                {
                    return index == (triviaList.Count - 1) ? -1 : index + 1;
                }
            }

            return 0;
        }
    }
}