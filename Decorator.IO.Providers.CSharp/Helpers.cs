﻿using Decorator.IO.Core;

using Humanizer;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Decorator.IO.Providers.CSharp
{
	public static class Helpers
	{
		public static CompilationUnitSyntax AsCompilationUnitSyntax(this string str)
			=> CSharpSyntaxTree.ParseText(str, CSharpParseOptions.Default).GetCompilationUnitRoot();

		public static IEnumerable<MemberDeclarationSyntax> AsMemberDeclarationSyntaxes(this CompilationUnitSyntax compilationUnitSyntax)
			=> compilationUnitSyntax
			.ChildNodes()
			.OfType<MemberDeclarationSyntax>();

		// TODO: make this more versatile by replacing Pascalize calls with
		// a Func<string, string> parameter to do the casing for us
		public static void ApplyCSharpCasing(this DecoratorFile @in)
		{
			@in.Namespace = @in.Namespace.Split('.')
				.Select(x => x.Pascalize())
				.Aggregate((a, b) => $"{a}.{b}");

			foreach (var x in @in.Classes)
			{
				x.Name = x.Name.Pascalize();
			}

			foreach (var i in @in.Classes.SelectMany(x => x.Fields))
			{
				i.Name = i.Name.Pascalize();
			}
		}
	}
}