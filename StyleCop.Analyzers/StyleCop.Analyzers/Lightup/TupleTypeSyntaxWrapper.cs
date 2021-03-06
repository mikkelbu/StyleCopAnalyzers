﻿// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Lightup
{
    using System;
    using System.Reflection;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    internal struct TupleTypeSyntaxWrapper : ISyntaxWrapper<TypeSyntax>
    {
        private const string TupleTypeSyntaxTypeName = "Microsoft.CodeAnalysis.CSharp.Syntax.TupleTypeSyntax";
        private static readonly Type TupleTypeSyntaxType;

        private static readonly Func<TypeSyntax, SyntaxToken> OpenParenTokenAccessor;
        private static readonly Func<TypeSyntax, SeparatedSyntaxListWrapper<TupleElementSyntaxWrapper>> ElementsAccessor;
        private static readonly Func<TypeSyntax, SyntaxToken> CloseParenTokenAccessor;
        private static readonly Func<TypeSyntax, SyntaxToken, TypeSyntax> WithOpenParenTokenAccessor;
        private static readonly Func<TypeSyntax, SeparatedSyntaxListWrapper<TupleElementSyntaxWrapper>, TypeSyntax> WithElementsAccessor;
        private static readonly Func<TypeSyntax, SyntaxToken, TypeSyntax> WithCloseParenTokenAccessor;

        private readonly TypeSyntax node;

        static TupleTypeSyntaxWrapper()
        {
            TupleTypeSyntaxType = typeof(CSharpSyntaxNode).GetTypeInfo().Assembly.GetType(TupleTypeSyntaxTypeName);
            OpenParenTokenAccessor = LightupHelpers.CreateSyntaxPropertyAccessor<TypeSyntax, SyntaxToken>(TupleTypeSyntaxType, nameof(OpenParenToken));
            ElementsAccessor = LightupHelpers.CreateSeparatedSyntaxListPropertyAccessor<TypeSyntax, TupleElementSyntaxWrapper>(TupleTypeSyntaxType, nameof(Elements));
            CloseParenTokenAccessor = LightupHelpers.CreateSyntaxPropertyAccessor<TypeSyntax, SyntaxToken>(TupleTypeSyntaxType, nameof(CloseParenToken));
            WithOpenParenTokenAccessor = LightupHelpers.CreateSyntaxWithPropertyAccessor<TypeSyntax, SyntaxToken>(TupleTypeSyntaxType, nameof(OpenParenToken));
            WithElementsAccessor = LightupHelpers.CreateSeparatedSyntaxListWithPropertyAccessor<TypeSyntax, TupleElementSyntaxWrapper>(TupleTypeSyntaxType, nameof(Elements));
            WithCloseParenTokenAccessor = LightupHelpers.CreateSyntaxWithPropertyAccessor<TypeSyntax, SyntaxToken>(TupleTypeSyntaxType, nameof(CloseParenToken));
        }

        private TupleTypeSyntaxWrapper(TypeSyntax node)
        {
            this.node = node;
        }

        public TypeSyntax SyntaxNode => this.node;

        public SyntaxToken OpenParenToken
        {
            get
            {
                return OpenParenTokenAccessor(this.SyntaxNode);
            }
        }

        public SeparatedSyntaxListWrapper<TupleElementSyntaxWrapper> Elements
        {
            get
            {
                return ElementsAccessor(this.SyntaxNode);
            }
        }

        public SyntaxToken CloseParenToken
        {
            get
            {
                return CloseParenTokenAccessor(this.SyntaxNode);
            }
        }

        public static explicit operator TupleTypeSyntaxWrapper(SyntaxNode node)
        {
            if (node == null)
            {
                return default(TupleTypeSyntaxWrapper);
            }

            if (!IsInstance(node))
            {
                throw new InvalidCastException($"Cannot cast '{node.GetType().FullName}' to '{TupleTypeSyntaxTypeName}'");
            }

            return new TupleTypeSyntaxWrapper((TypeSyntax)node);
        }

        public static implicit operator TypeSyntax(TupleTypeSyntaxWrapper wrapper)
        {
            return wrapper.node;
        }

        public static bool IsInstance(SyntaxNode node)
        {
            return node != null && LightupHelpers.CanWrapNode(node, TupleTypeSyntaxType);
        }

        public TupleTypeSyntaxWrapper AddElements(params TupleElementSyntaxWrapper[] items)
        {
            return new TupleTypeSyntaxWrapper(this.WithElements(this.Elements.AddRange(items)));
        }

        public TupleTypeSyntaxWrapper WithOpenParenToken(SyntaxToken openParenToken)
        {
            return new TupleTypeSyntaxWrapper(WithOpenParenTokenAccessor(this.SyntaxNode, openParenToken));
        }

        public TupleTypeSyntaxWrapper WithElements(SeparatedSyntaxListWrapper<TupleElementSyntaxWrapper> elements)
        {
            return new TupleTypeSyntaxWrapper(WithElementsAccessor(this.SyntaxNode, elements));
        }

        public TupleTypeSyntaxWrapper WithCloseParenToken(SyntaxToken closeParenToken)
        {
            return new TupleTypeSyntaxWrapper(WithCloseParenTokenAccessor(this.SyntaxNode, closeParenToken));
        }
    }
}
