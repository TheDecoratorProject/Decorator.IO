﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Decorator.IO.Core.Tokens;

namespace Decorator.IO.Providers.CSharp
{
	public class ModelFlattener
	{
		private readonly Model _model;

		public ModelFlattener(Model model)
		{
			_model = model;
		}

		public Field[] FlattenToFields()
		{
			var fields = new List<Field>();

			foreach (var parent in _model.Parents)
			{
				fields.AddRange(new ModelFlattener(parent.Model).FlattenToFields());
			}

			// if we've redefined fields in the model we'll replace the parent's fields
			// this is so we can update a property or something with a different position in inheriting classes
			foreach (var field in _model.Fields)
			{
				bool Selector(Field f) => f.Identifier == field.Identifier;

				if (!fields.Any(Selector))
				{
					fields.Add(field);
					continue;
				}

				var index = fields.IndexOf(fields.First(Selector));
				fields[index] = field;
			}

			return fields.ToArray();
		}
	}
}
