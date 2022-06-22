using System;

public static class GenTypes
{
	public static bool HasGenericDefinition(this Type type, Type definition)
	{
		return type.GetTypeWithGenericDefinition(definition) != null;
	}

	public static Type GetTypeWithGenericDefinition(this Type type, Type definition)
	{
		if (type == null)
		{
			throw new ArgumentNullException("type");
		}
		if (definition == null)
		{
			throw new ArgumentNullException("definition");
		}
		if (!definition.IsGenericTypeDefinition)
		{
			throw new ArgumentException("The definition needs to be a GenericTypeDefinition", "definition");
		}
		if (definition.IsInterface)
		{
			Type[] interfaces = type.GetInterfaces();
			foreach (Type type2 in interfaces)
			{
				if (type2.IsGenericType && type2.GetGenericTypeDefinition() == definition)
				{
					return type2;
				}
			}
		}
		for (Type type3 = type; type3 != null; type3 = type3.BaseType)
		{
			if (type3.IsGenericType && type3.GetGenericTypeDefinition() == definition)
			{
				return type3;
			}
		}
		return null;
	}
}
