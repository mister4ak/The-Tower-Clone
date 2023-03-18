using System;
using System.Collections.Generic;
using static Common.Enums;

namespace UI.ResourcesView
{
	public static class ResourceHandler
	{
		public static event Action<ResourceType, int> OnValueSet;
		public static event Action<ResourceType, int> OnValueAdded;
		public static event Action<ResourceType, int> OnValueSubtracted;
		public static event Action<ResourceType, int> OnValueChanged;

		private static readonly Dictionary<ResourceType, int> resources = new();
		
		public static void Initialize()
		{
			foreach (ResourceType resourceType in Enum.GetValues(typeof(ResourceType))) 
				InitializeData(resourceType);
		}

		private static void InitializeData(ResourceType type)
		{
			resources.Add(type, 0);
			OnValueSet?.Invoke(type, resources[type]);
		}

		public static bool AddResource(ResourceType type, int addedValue)
		{
			if (addedValue < 0) return TrySubtractResource(type, -addedValue);

			if (!resources.ContainsKey(type)) resources.Add(type, 0);

			OnValueAdded?.Invoke(type, addedValue);
			var value = resources[type] += addedValue;
			OnValueChanged?.Invoke(type, value);
			return true;
		}

		public static bool TrySubtractResource(ResourceType type, int subtractValue)
		{
			if (subtractValue < 0) throw new ArgumentOutOfRangeException(null, "Can't subtract negative value");

			if (!resources.ContainsKey(type)) resources.Add(type, 0);
			var value = resources[type];
			if (value < subtractValue) throw new ArgumentOutOfRangeException(null, $"Not enough resource {type}");

			OnValueSubtracted?.Invoke(type, subtractValue);
			resources[type] = value -= subtractValue;
			OnValueChanged?.Invoke(type, value);
			return true;
		}

		public static int GetResourceCount(ResourceType type)
		{
			if (!resources.ContainsKey(type)) resources.Add(type, 0);
			return resources[type];
		}
	}
}