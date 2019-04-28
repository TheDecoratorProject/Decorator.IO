﻿using Decorator.IO.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Decorator.IO.Providers.CSharp
{
	public class AssignGenerator
	{
		private readonly DecoratorFile _context;
		private readonly DecoratorClass _classContext;

		public AssignGenerator(DecoratorFile context, DecoratorClass classContext)
		{
			_classContext = classContext;
			_context = context;
			AssignTable[Modifier.Required] = AssignRequired;
			AssignTable[Modifier.Optional] = AssignTable[Modifier.Required];
		}

		public Dictionary<Modifier, Func<DecoratorField, string, string, string>> AssignTable = new Dictionary<Modifier, Func<DecoratorField, string, string, string>>
		{
		};

		public string AssignRequired(DecoratorField decoratorField, string objectContext, string counterName)
		{
			var index = Array.IndexOf(_classContext.Fields, decoratorField);

			// if there's none after us we don't need to advance current
			if (index + 1 >= _classContext.Fields.Length)
			{
				return AssignCurrent();
			}

			// otherwise increment counter by the amount we need to advance
			int needAdvance = _classContext.Fields[index + 1].Index - _classContext.Fields[index].Index;

			return $@"{AssignCurrent()}
{counterName} += {needAdvance};";

			string AssignCurrent()
			{
				return $"{Config.ArrayName}[{counterName}] = {objectContext}{decoratorField.Name};";
			}
		}

		public string Assign(DecoratorField decoratorField, string objectContext, string counterName)
			=> AssignTable[decoratorField.Modifier](decoratorField, objectContext, counterName);
	}
}