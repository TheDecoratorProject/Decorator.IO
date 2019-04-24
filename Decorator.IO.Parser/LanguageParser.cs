﻿using Sprache;

using System;
using System.Linq;

namespace Decorator.IO.Parser
{
	public static partial class LanguageParsers
	{
		public static readonly Parser<int> NumberParser =
			from number in Parse.Digit.AtLeastOnce().Text()
			select int.Parse(number);

		public static readonly Parser<string> Namespace =
			from _ in Parse.String("NAMESPACE").Once().Token()
			from name in Parse.AnyChar.Until(Parse.Char(';')).Text()
			select name;

		public static readonly Parser<Type> CSharpInt =
			from _ in Parse.String("int")
				.Or(Parse.String("i"))
			select typeof(int);

		public static readonly Parser<Type> CSharpString =
			from _ in Parse.String("string")
				.Or(Parse.String("str"))
				.Or(Parse.String("s"))
			select typeof(string);

		public static readonly Parser<Type> CSharpType =
			from type in CSharpInt
				.Or(CSharpString)
			select type;

		public static readonly Parser<string> FieldName =
			from chars in Parse.Chars("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_1234567890")
				.AtLeastOnce()
			select new string(chars.ToArray());

		public static readonly Parser<int> DecoratorNumber =
			from _ in Parse.Char('(').Optional().Token()
			from number in NumberParser.Token()
			from __ in Parse.Char(')').Optional().Token()
			select number;

		public static readonly Parser<DecoratorField> DecoratorField =
			from _ in Parse.Char('|').Once().Token()
			from number in DecoratorNumber.Token()
			from fieldType in FieldType.Token()
			from csharpType in CSharpType.Token()
			from name in FieldName.Token()
			select new DecoratorField
			{
				Index = number,
				Type = fieldType,
				CSharpType = csharpType,
				Name = name
			};
	}

	public class DecoratorField
	{
		public int Index { get; set; }
		public FieldType Type { get; set; }
		public Type CSharpType { get; set; }
		public string Name { get; set; }
	}

	public enum FieldType
	{
		Required,
		Optional
	}
}