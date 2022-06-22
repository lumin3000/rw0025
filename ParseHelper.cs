using System;
using UnityEngine;

public static class ParseHelper
{
	public static object FromString(string str, Type itemType)
	{
		//Discarded unreachable code: IL_00c8, IL_0102
		if (itemType == typeof(string))
		{
			str = str.Replace("\\n", "\n");
			return str;
		}
		if (itemType == typeof(int))
		{
			return Convert.ToInt32(str);
		}
		if (itemType == typeof(long))
		{
			return Convert.ToInt64(str);
		}
		if (itemType == typeof(float))
		{
			return Convert.ToSingle(str);
		}
		if (itemType == typeof(double))
		{
			return Convert.ToDouble(str);
		}
		if (itemType == typeof(bool))
		{
			return Convert.ToBoolean(str);
		}
		if (itemType.IsEnum)
		{
			try
			{
				return Enum.Parse(itemType, str);
			}
			catch (ArgumentException)
			{
				Debug.LogError("Tried to load value " + str + " which is not a valid entry in enum " + itemType);
				return 0;
			}
		}
		if (itemType == typeof(Type))
		{
			Type type = Type.GetType(str);
			if (type == null)
			{
				Debug.LogError("Could not find a type named " + str);
			}
			return type;
		}
		if (itemType == typeof(Vector3))
		{
			return FromStringVector3(str);
		}
		if (itemType == typeof(Vector2))
		{
			return FromStringVector2(str);
		}
		if (itemType == typeof(Material))
		{
			return MaterialPool.MatFrom(str);
		}
		if (itemType == typeof(IntVec2))
		{
			return IntVec2.FromString(str);
		}
		if (itemType == typeof(IntVec3))
		{
			return IntVec3.FromString(str);
		}
		if (itemType == typeof(IntRot))
		{
			return IntRot.FromString(str);
		}
		Debug.LogError("Trying to convert from string for undefined data type " + itemType.Name + ". Content is '" + str + "'.");
		return null;
	}

	private static Vector3 FromStringVector3(string Str)
	{
		Str = Str.TrimStart('(');
		Str = Str.TrimEnd(')');
		string[] array = Str.Split(',');
		float x = Convert.ToSingle(array[0]);
		float y = Convert.ToSingle(array[1]);
		float z = Convert.ToSingle(array[2]);
		return new Vector3(x, y, z);
	}

	private static Vector2 FromStringVector2(string Str)
	{
		Str = Str.TrimStart('(');
		Str = Str.TrimEnd(')');
		string[] array = Str.Split(',');
		float x = Convert.ToSingle(array[0]);
		float y = Convert.ToSingle(array[1]);
		return new Vector2(x, y);
	}
}
