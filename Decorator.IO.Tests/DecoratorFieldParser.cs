﻿using Decorator.IO.Core;
using Decorator.IO.Parser;

using FluentAssertions;

using Sprache;

using System.Collections.Generic;

using Xunit;

namespace Decorator.IO.Tests
{
	public class DecoratorFieldParser
	{
		[Theory]
		[MemberData(nameof(A))]
		public void ParseField(string data, DecoratorField expected)
		{
			var result = DecoratorPocoParser.DecoratorField
				.TryParse(data);

			result.WasSuccessful
				.Should().Be(true);

			result.Value
				.Should()
				.BeEquivalentTo(expected);
		}

		public static IEnumerable<object[]> A()
		{
			yield return new object[]
			{
				"| (0) R I some_thing",
				new DecoratorField
				{
					Index = 0,
					Type = typeof(int),
					Modifier = Modifier.Required,
					Name = "some_thing"
				}
			};
		}
	}
}