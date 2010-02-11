using System;

namespace Tools.AttributedObjectFactory
{
	internal class AttributeInfo
	{
		public AttributeInfo(Type classType, Attribute attribute)
		{
			ClassType = classType;
			Attribute = attribute;
		}

		public Type ClassType
		{
			get;
			private set;
		}

		public Attribute Attribute
		{
			get;
			private set;
		}
	}
}