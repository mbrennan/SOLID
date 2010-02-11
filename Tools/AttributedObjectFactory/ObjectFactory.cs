using System;
using System.Collections.Generic;

namespace Tools.AttributedObjectFactory
{
	public static class ObjectFactory
	{
		public static ObjectType CreateObjectMatchingAttribute<ObjectType, AttributeType>(Predicate<AttributeType> attributeMatcher, params object[] constructorParameters) where AttributeType : Attribute
		{
			foreach (var attributeInfo in AttributesRegistrar.FindAttributeInfo<AttributeType>())
			{
				if (!attributeMatcher((AttributeType) attributeInfo.Attribute))
					continue;

				return CreateObject<ObjectType>(attributeInfo.ClassType, constructorParameters);
			}

			throw new ArgumentException("Cannot create object.  Unable to find a class that matches the required attribute");
		}

		private static ObjectType CreateObject<ObjectType>(Type classType, object[] parameters)
		{
			var typeSignature = FindTypeSignature(parameters);
			var constructor = classType.GetConstructor(typeSignature);

			return (ObjectType) constructor.Invoke(parameters);
		}

		private static Type[] FindTypeSignature(IEnumerable<object> parameters)
		{
			var types = new List<Type>();
			foreach (var parameter in parameters)
			{
				types.Add(parameter.GetType());
			}

			return types.ToArray();
		}
	}
}