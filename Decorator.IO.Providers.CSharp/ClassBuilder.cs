﻿using Decorator.IO.Core;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using System.Collections.Generic;

namespace Decorator.IO.Providers.CSharp
{
	public static class ClassBuilder
	{
		public static IEnumerable<MemberDeclarationSyntax> BuildClass(DecoratorClass decoratorClass)
			=> $@"public class {decoratorClass.Name} : {Config.InterfaceName(decoratorClass.Name)}
{{
	{decoratorClass.ConcatenateFieldsOfParents().ToPropertyStrings(true)}
	public object[] {Config.SerializeName}()
	{{
		return this.{Config.SerializeAsName(decoratorClass.Name)}();
	}}

	public static {Config.InterfaceName(decoratorClass.Name)} {Config.DeserializeName}(object[] {Config.ArrayName})
	{{
		return {Config.DecoratorFactory}.{Config.DeserializeAsName(decoratorClass.Name)}({Config.ArrayName});
	}}

	public static bool {Config.TryDeserializeAsName(decoratorClass.Name)}(object[] {Config.ArrayName}, out {Config.InterfaceName(decoratorClass.Name)} {Config.ObjectName})
	{{
		return {Config.DecoratorFactory}.{Config.TryDeserializeAsName(decoratorClass.Name)}({Config.ArrayName}, out {Config.ObjectName});
	}}
}}"
			.AsCompilationUnitSyntax()
			.AsMemberDeclarationSyntaxes();
	}
}