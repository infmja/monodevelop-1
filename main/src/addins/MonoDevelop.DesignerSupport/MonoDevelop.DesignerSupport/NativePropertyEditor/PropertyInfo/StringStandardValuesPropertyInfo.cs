﻿//
// StringStandardValuesPropertyInfo.cs
//
// Author:
//       jmedrano <josmed@microsoft.com>
//
// Copyright (c) 2018 
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

#if MAC

using Xamarin.PropertyEditing;
using System.ComponentModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using MonoDevelop.Core;
using System.Linq;

namespace MonoDevelop.DesignerSupport
{
	class StringStandardValuesPropertyInfo
	: DescriptorPropertyInfo, IHavePredefinedValues<string>
	{
		public StringStandardValuesPropertyInfo (PropertyDescriptor propertyDescriptor, object propertyProvider, ValueSources valueSources) : base (propertyDescriptor, propertyProvider, valueSources)
		{
			foreach (object stdValue in PropertyDescriptor.Converter.GetStandardValues ()) {
				var value = PropertyDescriptor.Converter.ConvertToString (stdValue);
				predefinedValues.Add (value, value);
			}
		}

		public bool IsConstrainedToPredefined => true;

		public bool IsValueCombinable { get; }

		protected Dictionary<string, string> predefinedValues = new Dictionary<string, string> ();
		public IReadOnlyDictionary<string, string> PredefinedValues => predefinedValues;

		internal override void SetValue<T> (object target, T value)
		{
			var tc = PropertyDescriptor.Converter;
			if (tc.CanConvertFrom (typeof (T))) {
				try {
					PropertyDescriptor.SetValue (PropertyProvider, tc.ConvertTo (value, PropertyDescriptor.ComponentType));
				} catch (Exception ex) {
					LogInternalError ($"Error trying to set and convert a value: {value} T:{typeof (T).FullName}", ex);
				}
			} else {
				base.SetValue<T> (target, value);
			}
		}

		internal async override Task<TValue> GetValueAsync<TValue> (object target)
		{
			TValue result = default;
			result = await base.GetValueAsync<TValue> (target);

			return result;
		}
	}
}

#endif