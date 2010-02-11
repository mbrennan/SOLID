using System;
using System.Collections.Generic;
using System.Reflection;

namespace Tools.AttributedObjectFactory
{
	internal static class AttributesRegistrar
	{
		private static readonly IDictionary<Type, IList<AttributeInfo>> registeredAttributes = new Dictionary<Type, IList<AttributeInfo>>();

		public static void RegisterAttributesWith<AttributeType>() where AttributeType : Attribute
		{
			var attributeType = typeof(AttributeType);
			if (!registeredAttributes.ContainsKey(attributeType))
				registeredAttributes[attributeType] = new List<AttributeInfo>();

			var types = Assembly.GetExecutingAssembly().GetTypes();
			foreach (var type in types)
			{
				if (!type.IsClass || type.IsAbstract)
					continue;

				var customAttributes = type.GetCustomAttributes(typeof(AttributeType), true);
				if (customAttributes.Length != 1)
					continue;

				RegisterAttribute(type, (AttributeType) customAttributes[0]);
			}
		}

		public static bool HasSearchedForAttributesWith<AttributeType>() where AttributeType : Attribute
		{
			return registeredAttributes.ContainsKey(typeof(AttributeType));
		}

		public static IEnumerable<AttributeInfo> FindAttributeInfo<AttributeType>() where AttributeType : Attribute
		{
			if (!HasSearchedForAttributesWith<AttributeType>())
				RegisterAttributesWith<AttributeType>();

			var attributeType = typeof(AttributeType);
			if (registeredAttributes.ContainsKey(attributeType))
			{
				var attributeInfos = registeredAttributes[attributeType];
				foreach (var attributeInfo in attributeInfos)
				{
					yield return attributeInfo;
				}
			}
		}

		private static void RegisterAttribute<AttributeType>(Type type, AttributeType attribute) where AttributeType : Attribute
		{
			registeredAttributes[attribute.GetType()].Add(new AttributeInfo(type, attribute));
		}
	}
}