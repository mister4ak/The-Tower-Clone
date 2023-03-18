namespace Common
{
	public static class Constants
	{
		public static class PlayerPrefsKeyNames
		{
			public const string PLAYER_LEVEL         = "Level";
			public const string CURRENT_LEVEL_NUMBER = "CurrentLevelNumber";
			public const string ALL_LEVELS_COMPLETED = "AllLevelsCompleted";
			public const string PLAYER_MAX_HEAP_SIZE = "PLAYER_MAX_HEAP_SIZE";
			public const string PLAYER_SPEED         = "PlayerSpeed";
			public const string START_MONEY_GET      = "StartMoneyGet";
			public const string TUTORIAL_COMPLETED   = "TutorialCompleted";
		}

		public static class SavePrefix
		{
			public const string LEVEL = "Level_";
			public const string HEAP  = "Heap_";
			public const string AREA  = "Area_";
		}

		public static class AssetPath
		{
			public const string SO_DATA_PATH   = "Game/ScriptableObjects";
			public const string POOL_ITEM_PATH = "Assets/Game/Prefabs|Assets/Game/VFX";
		}

		public static class LayersIds
		{
			public const int NAME_OBJECT_ID = 8;
		}

		public static class SavableValuePrefix
		{
			public const string INT_DATA_VALUE           = "IntDataValue ";
			public const string FLOAT_DATA_VALUE         = "FloatDataValue ";
			public const string STRING_DATA_VALUE        = "StringDataValue ";
			public const string BOOL_DATA_VALUE          = "BoolDataValue ";
			public const string VECTOR2_INT_X_DATA_VALUE = "Vector2IntXDataValue ";
			public const string VECTOR2_INT_Y_DATA_VALUE = "Vector2IntYDataValue ";
		}
	}
}