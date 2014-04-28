/* Favorites Tab[s] for Unity
 * version 1.2.6, October 2013
 * Copyright © 2012-2013, by Flipbook Games
 * 
 * Your personalized list of favorite assets and scene objects
 * 
 * Version 1.2.6:
 * - Fixed (again) performance issue with no FG_GamoObjectGUIDs (re-introduced with v1.2.5).
 * 
 * Version 1.2.5:
 * - Fixed lost references to scene objects after entering game mode on modified scenes (thanks to Jimww).
 * - Fixed stars blinking in Hierarchy view when entering game mode with a game object selected (thanks to Jimww again).
 * 
 * Version 1.2.4:
 * - Fixed positioning of Antares Universe icons (thanks to Nezabyte).
 * 
 * Version 1.2.3:
 * - Shows the content of bookmarked folders in the second column of Project view in Unity 4.
 * 
 * Version 1.2.2:
 * - Fixed performance issues in scenes with no FG_GameObjectGUIDs (thanks to Jim Young).
 * 
 * Version 1.2.1:
 * - Fixed title initialization on Favorites tabs hidden behind another tab.
 * 
 * New Features in 1.2:
 * - Support for showing multiple favorites tabs, each with different filtering to show diffent sets of favorites.
 * - Filtering and search setting for each favorites tab are persistent between Unity sessions.
 * - Many new filtering options added to filter by asset type.
 * - Favorites tabs filtered by type show the selected type in the title.
 * - Star icons for each favorite item can optionally be set to colors other than the default yellow star, independently for each user.
 * - FG_GameObjectGUIDs game object gets created only when its needed and can optionally be deleted if user wants that.
 * - A custom inspector appears on FG_GameObjectGUIDs, explaining the function of this game object.
 * - Added context menu items on favorite assets to reimport them and to show them in Explorer (reveal in Finder on Mac).
 * - Editor/Resources folder renamed to Editor/Textures to avoid inclusion of those in final builds.
 * - Unity 4 support.
 * 
 * New in version 1.1:
 * - Multiple selected favorite items can be removed from the Favorites Tab at once.
 * - FG_GameObjectGUIDs game object is not hidden anymore.
 *
 * Features:
 * - Native look and feel, very similar to Project and Hierarchy views!
 * - No learning required! Just use your common Unity Editor knowledge and see it working as you would expect.
 * - Easy to mark or unmark favorite assets and scene object with just a single mouse-click.
 * - Easy to spot your favorite assets or scene objects in the Project and Hierarchy window, even when the Favorites Tab is closed!
 * - Favorites Tab displays all favorites sorted by name or type.
 * - Search by name functionality.
 * - Filters to show only assets or scene objects.
 * - Keyboard and mouse are fully supported.
 * - Selection synchronization. Select an item in the Favorites Tab to easily find it in the Hierarchy or Project views.
 * - Multiple favorite items selections.
 * - Dragging items from the Favorites Tab to any other Unity view is fully supported.
 * - Double click or press F key (or use context menu) to Frame the selected scene object in the Scene View, same as from the Hierarchy view.
 * - Double click or press Enter key (or use context menu) to open the selected asset, same as from the Project view.
 * - Works with teams! All team members have their own list of favorites even if they share the same project!
 * - GUID based asset references, so that assets exported and imported into another project remain in your list of favorites.
 * - Full source code provided for your reference or modification! :-)
 * 
 * Follow me on http://twitter.com/FlipbookGames
 * Like Flipbook Games on Facebook http://facebook.com/FlipbookGames
 * Join Unity forum discusion http://forum.unity3d.com/threads/149856
 * Contact info@flipbookgames.com for feedback, bug reports, or suggestions.
 * Visit http://flipbookgames.com/ for more info.
 */

using UnityEngine;
using UnityEditor;
using System.Text;
using System.Reflection;
using System;
using System.Collections.Generic;


[InitializeOnLoad]
public class FavoritesTab : EditorWindow
{
	public static string GetVersionString()
	{
		return "1.2.6, October 2013";
	}


	private static Favorites favorites;

	[SerializeField]
	private Vector2 scrollPosition = new Vector2();
	private Rect scrollViewRect;
	private float contentHeight;

	private ListViewItem[] listViewItems = new ListViewItem[0];
	private bool recreateListViewItems = true;
	[SerializeField]
	private int focusedItemIndex = -1;
	[SerializeField]
	private int anchorItemIndex = -1;
	private int itemToSelect = -1;
	private int itemMouseDown = -1;
	private int draggedItem = -1;
	private GenericMenu itemPopupMenu = null;
	private static bool paintingFavoritesTab = false;

	[SerializeField]
	private string searchString = "";
	[SerializeField]
	private int searchMode = 0;
	private bool sortByType = true;
	private static GUIContent[] searchModesMenuItems = null;

	private bool focusSearchBox = false;
	private bool hasSearchBoxFocus = false;
	private bool focusListView = true;

	private static bool initialized = false;

	private static Texture2D gameObjectIcon = null;
	private static Texture2D windowIcon = null;
	private static Texture2D emptyStar = null;
	private static Texture2D[] filledStars = null;
	private static GUIContent[] starColorNames = new GUIContent[]
	{
		new GUIContent("Red"),
		new GUIContent("Orange"),
		new GUIContent("Yellow"),
		new GUIContent("Green"),
		new GUIContent("Cyan"),
		new GUIContent("Blue"),
		new GUIContent("Magenta"),
	};
	[NonSerialized]
	public static Texture2D createdByFlipbookGames = null;

	private static GUIStyle scrollViewStyle = null;
	private static GUIStyle labelStyle = null;
	private static GUIStyle toolbarSearchField = null;
	private static GUIStyle toolbarSearchFieldCancelButton = null;
	private static GUIStyle toolbarSearchFieldCancelButtonEmpty = null;

	private static string[] searchModes = new string[] { "All", "Scene Objects", "Assets", "",
		"Project Folders",
		"Animations",
		"Audio Assets",
		"Custom Assets",
		"Fonts",
		"Materials",
		"Models",
		"Prefabs",
		"Preference Assets",
		"Scenes",
		"Scripts",
		"Shaders",
		"SQL Assets",
		"GUI Skins",
		"Text Assets",
		"Textures",
		"Video Assets",
		"XML Assets"
	};

	private static int[] filterTypeIds = { 100, 100, 100, 100,
		-1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

	private static string[] windowTitles = new string[] { "Favorites", "Fav. Objects", "Fav. Assets", "",
		"Fav. Folders",
		"Fav. Anims",
		"Fav. Audio",
		"Custom Favs",
		"Fav. Fonts",
		"Fav. Mat.",
		"Fav. Meshes",
		"Fav. Prefabs",
		"Fav. Prefs",
		"Fav. Scenes",
		"Fav. Scripts",
		"Fav. Shaders",
		"Fav. SQLs",
		"Fav. Skins",
		"Fav. Texts",
		"Fav.Textures",
		"Fav. Videos",
		"Fav. XMLs"
	};

	private static Color[] lightSkinColors = new Color[] { Color.black, new Color(0f, 0.15f, 0.51f, 1f), new Color(0.25f, 0.05f, 0.05f, 1f), Color.black, Color.white, new Color(0.67f, 0.76f, 1f), new Color(1f, 0.71f, 0.71f, 1f), Color.white };
	private static Color[] darkSkinColors = new Color[] { new Color(0.705f, 0.705f, 0.705f, 1f), new Color(0.3f, 0.5f, 0.85f, 1f), new Color(0.7f, 0.4f, 0.4f, 1f), new Color(0.705f, 0.705f, 0.705f, 1f), Color.white, new Color(0.67f, 0.76f, 1f), new Color(1f, 0.71f, 0.71f, 1f), Color.white };


	// A custom asset postprocessor to track deleted object so they can get removed from the list of favorites
	private class FavoritesTabAssetPostprocessor : AssetPostprocessor
	{
		static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
		{
			foreach (string path in deletedAssets)
				favorites.OnAssetDeleted(path);
		}
	}


	private class Favorites
	{
		public class Entry<T> : IComparable
		{
			public int instanceID = 0;
			public string guid;
			public int starColor = 2;
			//public string path;

			public Entry() {}
			public Entry(string guid) { this.guid = guid; }
			public Entry(int instanceID) { this.instanceID = instanceID; }

			public int CompareTo(object obj)
			{
				Entry<T> other = (Entry<T>) obj;
				if (guid == null)
					return other.guid == null ? instanceID.CompareTo(other.instanceID) : 1;
				else
					return other.guid == null ? -1 : guid.CompareTo(other.guid);
			}

			public override string ToString()
			{
				return starColor != 2 ? guid + ':' + starColor.ToString() : guid; // (path != null ? path + "/" : string.Empty) + guid;
			}

			static public Entry<T> FromString(string registryRecord)
			{
				throw new System.NotImplementedException();
			}
		}

		private Entry<string>[] favoriteObjects;
		private Entry<string>[] favoriteAssets;

		private FG_GameObjectGUIDs goGUIDs = null;
		private int[] instanceIDs = null;

		public Favorites()
		{
			FG_GameObjectGUIDs._dirty = true;
			Load();
		}

		private void Load()
		{
			string allFavoriteAssets = EditorPrefs.GetString("FlipbookGames.FavoriteAssets", string.Empty);
			string allFavoriteObjects = EditorPrefs.GetString("FlipbookGames.FavoriteObjects", string.Empty);

			string[] entries = allFavoriteAssets.Split('|');
			int numFavorites = entries.Length;
			favoriteAssets = new Entry<string>[numFavorites];
			while (numFavorites-- > 0)
			{
				Entry<string> current = favoriteAssets[numFavorites] = new Entry<string>();
				string entry = entries[numFavorites];
				
				int lastColon = entry.LastIndexOf(':');
				if (lastColon >= 0)
				{
					current.starColor = int.Parse(entry.Substring(lastColon + 1));
					entry = entry.Substring(0, lastColon);
				}

				current.guid = entry;
			}
			Array.Sort(favoriteAssets);

			entries = allFavoriteObjects.Split('|');
			numFavorites = entries.Length;
			favoriteObjects = new Entry<string>[numFavorites];
			while (numFavorites-- > 0)
			{
				Entry<string> current = favoriteObjects[numFavorites] = new Entry<string>();
				string entry = entries[numFavorites];

				int lastColon = entry.LastIndexOf(':');
				if (lastColon >= 0)
				{
					current.starColor = int.Parse(entry.Substring(lastColon + 1));
					entry = entry.Substring(0, lastColon);
				}

				current.guid = entry;
			}
			Array.Sort(favoriteObjects);
		}

		private void SaveFavoriteAssets()
		{
			StringBuilder sb = new StringBuilder();
			if (favoriteAssets.Length > 0)
				sb.Append(favoriteAssets[0].ToString());
			for (int i = 1; i < favoriteAssets.Length; ++i)
			{
				sb.Append('|');
				sb.Append(favoriteAssets[i].ToString());
			}
			EditorPrefs.SetString("FlipbookGames.FavoriteAssets", sb.ToString());
		}

		private void SaveFavoriteGameObjects()
		{
			StringBuilder sb = new StringBuilder();
			if (favoriteObjects.Length > 0)
				sb.Append(favoriteObjects[0]);
			for (int i = 1; i < favoriteObjects.Length; ++i)
			{
				sb.Append('|');
				sb.Append(favoriteObjects[i]);
			}
			EditorPrefs.SetString("FlipbookGames.FavoriteObjects", sb.ToString());
		}

		private void UpdateGoGUIDs(bool create = false)
		{
			if (!create && !FG_GameObjectGUIDs._dirty)
				return;
			
			UpdateGoGUIDs_Internal(create);
			RefreshAllTabs();
		}
		
		private void UpdateGoGUIDs_Internal(bool create = false)
		{
			FG_GameObjectGUIDs._dirty = false;

			GameObject old = GameObject.Find("/FG_GameObjectGUIDs");
			if (old != null)
				old.gameObject.hideFlags = 0;

			FG_GameObjectGUIDs[] sceneObjects = FindObjectsOfType(typeof(FG_GameObjectGUIDs)) as FG_GameObjectGUIDs[];
			if (sceneObjects.Length == 0)
			{
				if (create)
				{
					GameObject go = new GameObject("FG_GameObjectGUIDs", typeof(FG_GameObjectGUIDs));
					goGUIDs = go.GetComponent<FG_GameObjectGUIDs>();
				}
				else
				{
					if (instanceIDs == null)
					{
						instanceIDs = new int[0];
					}
					return;
				}
			}
			else
			{
				if (sceneObjects.Length > 1)
					Debug.LogWarning("Multiple FG_GameObjectGUIDs found!!! " + sceneObjects.Length);
				goGUIDs = sceneObjects[0];
				goGUIDs.gameObject.hideFlags = 0;
			}

			foreach (FG_GameObjectGUIDs go in sceneObjects)
				go.gameObject.hideFlags = 0;

			// Cleaning up deleted scene objects
			int size = goGUIDs.objects.Length;
			for (int i = size; --i >= 0; )
			{
				if (goGUIDs.objects[i] == null)
				{
					goGUIDs.objects[i] = goGUIDs.objects[--size];
					goGUIDs.guids[i] = goGUIDs.guids[size];
				}
			}
			if (size < goGUIDs.objects.Length)
			{
				Array.Resize<UnityEngine.Object>(ref goGUIDs.objects, size);
				Array.Resize<string>(ref goGUIDs.guids, size);
				EditorUtility.SetDirty(goGUIDs);
			}

			instanceIDs = new int[size];
			for (int i = size; --i >= 0; )
			{
				instanceIDs[i] = goGUIDs.objects[i].GetInstanceID();
			}
			Array.Sort<int>(instanceIDs);
		}

		public string ObjectToGUID(UnityEngine.Object obj)
		{
			if (goGUIDs == null)
				UpdateGoGUIDs(true);
			
			int index = Array.IndexOf<UnityEngine.Object>(goGUIDs.objects, obj);
			if (index >= 0)
				return goGUIDs.guids[index];

			index = goGUIDs.objects.Length;
			Array.Resize<UnityEngine.Object>(ref goGUIDs.objects, goGUIDs.objects.Length + 1);
			Array.Resize<string>(ref goGUIDs.guids, goGUIDs.guids.Length + 1);
			for (int i = goGUIDs.objects.Length - 1; i > index; --i)
			{
				goGUIDs.objects[i - 1] = goGUIDs.objects[i];
				goGUIDs.guids[i - 1] = goGUIDs.guids[i];
			}
			goGUIDs.objects[index] = obj;
			goGUIDs.guids[index] = System.Guid.NewGuid().ToString();

			int indexIDs = Array.BinarySearch<int>(instanceIDs, obj.GetInstanceID());
			ArrayUtility.Insert(ref instanceIDs, ~indexIDs, obj.GetInstanceID());

			EditorUtility.SetDirty(goGUIDs);

			return goGUIDs.guids[index];
		}

		public string FindGUID(UnityEngine.Object obj)
		{
			if (goGUIDs == null)
			{
				UpdateGoGUIDs();
				if (goGUIDs == null)
					return string.Empty;
			}

			int index = Array.IndexOf<UnityEngine.Object>(goGUIDs.objects, obj);
			if (index >= 0)
				return goGUIDs.guids[index];
			else
				return string.Empty;
		}

		public UnityEngine.Object FindGameObject(string guid)
		{
			if (goGUIDs == null)
			{
				UpdateGoGUIDs();
				if (goGUIDs == null)
					return null;
			}

			int index = Array.IndexOf<string>(goGUIDs.guids, guid);
			if (index >= 0)
				return goGUIDs.objects[index];
			else
				return null;
		}

		private bool CheckFilter(string name, ref string[] words)
		{
			foreach (string word in words)
				if (name.IndexOf(word, StringComparison.InvariantCultureIgnoreCase) < 0)
					return false;
			return true;
		}

		// Known file types in Unity. Feel free to add support for non-native file types.
		private static Dictionary<string, int> typeIndexMap = new Dictionary<string, int>()
		{
			// animation
			{"anim", 0},

			// audio
			{"aac", 1}, {"aif", 1}, {"au", 1}, {"mid", 1}, {"midi", 1}, {"mp3", 1}, {"mpa", 1},
			{"ra", 1}, {"ram", 1}, {"wma", 1}, {"wav", 1}, {"wave", 1}, {"ogg", 1},

			// custom
			{"asset", 2}, {"bytes", 2},

			// fonts
			{"ttf", 3}, {"otf", 3}, {"fon", 3}, {"fnt", 3},
			
			// material
			{"mat", 4},

			// meshes
			{"3df", 5}, {"3dm", 5}, {"3dmf", 5}, {"3ds", 5}, {"3dv", 5}, {"3dx", 5}, {"blend", 5},
			{"c5d", 5}, {"lwo", 5}, {"lws", 5}, {"ma", 5}, {"max", 5}, {"mb", 5}, {"mesh", 5},
			{"obj", 5}, {"vrl", 5}, {"wrl", 5}, {"wrz", 5}, {"fbx", 5},

			// prefab
			{"prefab", 6},

			// preferences
			{"prefs", 7},

			// scene
			{"unity", 8},

			// scripts
			{"boo", 9}, {"cs", 9}, {"js", 9},

			// shader
			{"shader", 10}, {"cginc", 10},

			// skin
			{"guiskin", 11},

			// sql
			{"sql", 12},

			// text
			{"txt", 13}, {"doc", 13}, {"docx", 13}, {"pdf", 13}, {"htm", 13}, {"html", 13},

			// textures
			{"ai", 14}, {"apng", 14}, {"png", 14}, {"bmp", 14}, {"cdr", 14}, {"dib", 14}, {"eps", 14},
			{"exif", 14}, {"gif", 14}, {"ico", 14}, {"icon", 14}, {"j", 14}, {"j2c", 14}, {"j2k", 14},
			{"jas", 14}, {"jiff", 14}, {"jng", 14}, {"jp2", 14}, {"jpc", 14}, {"jpe", 14}, {"jpeg", 14},
			{"jpf", 14}, {"jpg", 14}, {"jpw", 14}, {"jpx", 14}, {"jtf", 14}, {"mac", 14}, {"omf", 14},
			{"qif", 14}, {"qti", 14}, {"qtif", 14}, {"tex", 14}, {"tfw", 14}, {"tga", 14}, {"tif", 14},
			{"tiff", 14}, {"wmf", 14}, {"psd", 14}, {"exr", 14},

			// video
			{"asf", 15}, {"asx", 15}, {"avi", 15}, {"dat", 15}, {"divx", 15}, {"dvx", 15}, {"mlv", 15},
			{"m2l", 15}, {"m2t", 15}, {"m2ts", 15}, {"m2v", 15}, {"m4e", 15}, {"m4v", 15}, {"mjp", 15},
			{"mov", 15}, {"movie", 15}, {"mp21", 15}, {"mp4", 15}, {"mpe", 15}, {"mpeg", 15}, {"mpg", 15},
			{"mpv2", 15}, {"ogm", 15}, {"qt", 15}, {"rm", 15}, {"rmvb", 15}, {"wmw", 15}, {"xvid", 15},

			// xml
			{"xml", 16}
		};

		public static int GuessAssetType(ListViewItem item)
		{
#if UNITY_3_5
			return item.guiContent.image != null && item.guiContent.image.name == "_Folder" ? -1 : GetTypeIndexForFile(item.assetPath);
#else
			return item.guiContent.image != null && item.guiContent.image.name == "Folder Icon" ? -1 : GetTypeIndexForFile(item.assetPath);
#endif
		}

		private static int GetTypeIndexForFile(string fileName)
        {
			int typeIndex;
            string extension = System.IO.Path.GetExtension(fileName).ToLowerInvariant();
			if (extension.StartsWith("."))
				extension = extension.Remove(0, 1);
            if (typeIndexMap.TryGetValue(extension, out typeIndex))
				return typeIndex;
			else
				return 100; // unknown types go at the end
        }

		public ListViewItem[] CreateListViewItems(string filter, int searchMode, bool sortByType)
		{
			if (goGUIDs == null)
				UpdateGoGUIDs();

			string[] words = filter.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

			List<ListViewItem> items = new List<ListViewItem>();

			int numGameObjects = 0;
			if (searchMode < 2)
			{
				for (int i = 0; i < favoriteObjects.Length; ++i)
				{
					ListViewItem item = ListViewItem.FromGameObjectGUID(favoriteObjects[i].guid);
					if (item != null && CheckFilter(item.guiContent.text, ref words))
						items.Add(item);
				}
				numGameObjects = items.Count;
			}

			if (searchMode != 1)
			{
				int typeFilter = filterTypeIds[searchMode];
				for (int i = 0; i < favoriteAssets.Length; ++i)
				{
					ListViewItem item = ListViewItem.FromAssetGUID(favoriteAssets[i].guid);
					if (item != null && (typeFilter == 100 || typeFilter == GuessAssetType(item)))
					{
						if (CheckFilter(item.guiContent.text, ref words))
							items.Add(item);
					}
				}
			}

			ListViewItem[] all = items.ToArray();
			if (!sortByType)
			{
				Array.Sort(all);
			}
			else
			{
				if (searchMode < 2)
					Array.Sort(all, 0, numGameObjects);
				if (searchMode != 1)
					Array.Sort(all, numGameObjects, all.Length - numGameObjects, new CompareAssetsByType());
			}
			return all;
		}

		public class CompareAssetsByType : IComparer<ListViewItem>
		{
			public int Compare(ListViewItem a, ListViewItem b)
			{
				int typeOfA = GuessAssetType(a);
				int typeOfB = GuessAssetType(b);

				int r = typeOfA.CompareTo(typeOfB);
				if (r == 0 && (typeOfA == 100 || typeOfA == 2) && a.guiContent.image != null && b.guiContent.image != null)
					r = a.guiContent.image.name.CompareTo(b.guiContent.image.name);
				if (r == 0)
					r = a.guiContent.text.CompareTo(b.guiContent.text);
				return r;
			}
		}

		public int Contains(string guid)
		{
			int index = Array.BinarySearch<Entry<string>>(favoriteAssets, new Entry<string>(guid));
			if (index >= 0)
				return favoriteAssets[index].starColor;
			else
				return -1;
		}

		public int Contains(int instanceID)
		{
			UpdateGoGUIDs();
			int index = Array.BinarySearch<int>(instanceIDs, instanceID);
			if (index < 0)
				return -1;
			//Debug.Log("Found in instanceIDs");
			string guid = FindGUID(EditorUtility.InstanceIDToObject(instanceID));
			if (guid == string.Empty)
				return -1;
			//Debug.Log("  Found its guid");
			index = Array.BinarySearch<Entry<string>>(favoriteObjects, new Entry<string>(guid));
			if (index >= 0)
				return favoriteObjects[index].starColor;
			else
				return -1;
		}

		public void ToggleGameObject(int instanceID)
		{
			UnityEngine.Object obj = EditorUtility.InstanceIDToObject(instanceID);
			string guid = ObjectToGUID(obj);

			ToggleGameObject(guid);
		}

		public void ToggleAsset(string guid)
		{
			ToggleAsset(new Entry<string>(guid));
		}

		public void ToggleAsset(Entry<string> guid)
		{
			int index = Array.BinarySearch<Entry<string>>(favoriteAssets, guid);
			if (index >= 0)
			{
				//Entry<string> oldEntry = favoriteAssets[index];
				ArrayUtility.RemoveAt(ref favoriteAssets, index);
			}
			else
			{
				Entry<string> newEntry = guid;
				ArrayUtility.Insert(ref favoriteAssets, ~index, newEntry);
			}

			EditorApplication.RepaintProjectWindow();
			if (EditorApplication.projectWindowChanged != null)
				EditorApplication.projectWindowChanged();
			SaveFavoriteAssets();
		}

		public void ToggleGameObject(string guid)
		{
			int index = Array.BinarySearch<Entry<string>>(favoriteObjects, new Entry<string>(guid));
			if (index >= 0)
			{
				//Entry<int> oldEntry = favoriteObjects[index];
				ArrayUtility.RemoveAt(ref favoriteObjects, index);
			}
			else
			{
				//Entry<int> newEntry = guid;
				ArrayUtility.Insert(ref favoriteObjects, ~index, new Entry<string>(guid));
			}

			EditorApplication.RepaintHierarchyWindow();
			if (EditorApplication.hierarchyWindowChanged != null)
				EditorApplication.hierarchyWindowChanged();
			SaveFavoriteGameObjects();
		}

		public void OnAssetDeleted(string path)
		{
			string guid = AssetDatabase.AssetPathToGUID(path);
			int index = Array.BinarySearch<Entry<string>>(favoriteAssets, new Entry<string>(guid));
			if (index >= 0)
			{
				ArrayUtility.RemoveAt<Entry<string>>(ref favoriteAssets, index);
				SaveFavoriteAssets();
			}
		}

		public void SetStarColor(object userData, string[] options, int colorIndex)
		{
			if (userData is int)
			{
				int instanceID = (int) userData;
				int index = Array.BinarySearch<int>(instanceIDs, instanceID);
				if (index >= 0)
				{
					string guid = FindGUID(EditorUtility.InstanceIDToObject(instanceID));
					index = Array.BinarySearch<Entry<string>>(favoriteObjects, new Entry<string>(guid));
					favoriteObjects[index].starColor = colorIndex;
					SaveFavoriteGameObjects();
				}
			}
			else
			{
				int index = Array.BinarySearch<Entry<string>>(favoriteAssets, new Entry<string>(userData as string));
				if (index >= 0)
				{
					favoriteAssets[index].starColor = colorIndex;
					SaveFavoriteAssets();
				}
			}
		}
	}


	[UnityEditor.MenuItem("Window/Favorites")]
	public static void OpenWindow()
	{
		FavoritesTab wnd;
		if (EditorWindow.focusedWindow is FavoritesTab)
		{
			wnd = ScriptableObject.CreateInstance<FavoritesTab>();
			wnd.Show();
		}
		else
		{
			wnd = EditorWindow.GetWindow<FavoritesTab>();
		}
		wnd.minSize = new Vector2(130f, 64f);
	}

	FavoritesTab()
	{
		sortByType = EditorPrefs.GetBool("FlipbookGames.FavoritesTab.SortByType", true);
	}

	private void OnDestroy()
	{
	}

	public void OnEnable()
	{
		EditorApplication.hierarchyWindowChanged -= OnHierarchyOrProjectWindowChanged;
		EditorApplication.hierarchyWindowChanged += OnHierarchyOrProjectWindowChanged;
		EditorApplication.projectWindowChanged -= OnHierarchyOrProjectWindowChanged;
		EditorApplication.projectWindowChanged += OnHierarchyOrProjectWindowChanged;

		focusSearchBox = false;
		focusListView = true;
		recreateListViewItems = true;

		if (cachedTitleContent == null)
		{
			cachedTitleContent = base.GetType().GetProperty("cachedTitleContent", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField);
		}
		if (cachedTitleContent != null)
		{
			GUIContent titleContent = cachedTitleContent.GetValue(this, null) as GUIContent;
			if (titleContent != null)
			{
				if (windowIcon == null)
					windowIcon = LoadEditorTexture2D("fgFavoritesTabIcon");
				titleContent.image = windowIcon;
				titleContent.text = windowTitles[searchMode];
			}
		}
	}

	public void OnDisable()
	{
		EditorApplication.hierarchyWindowChanged -= OnHierarchyOrProjectWindowChanged;
		EditorApplication.projectWindowChanged -= OnHierarchyOrProjectWindowChanged;
	}

	private void OnHierarchyOrProjectWindowChanged()
	{
		recreateListViewItems = true;
		//Repaint();
	}

	private static string GetObjectName(UnityEngine.Object assetObject)
	{
		if (assetObject.name != string.Empty)
			return assetObject.name;
		else
			return System.IO.Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(assetObject));
	}

	private static string thisAssetPath = "Assets/FlipbookGames/FavoritesTab/Editor";

	static void Initialize()
	{
		if (initialized)
			return;
		initialized = true;

		FavoritesTab tempInstance = ScriptableObject.CreateInstance<FavoritesTab>();
		MonoScript thisScript = MonoScript.FromScriptableObject(tempInstance);
		ScriptableObject.DestroyImmediate(tempInstance);
		tempInstance = null;
		thisAssetPath = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(AssetDatabase.GetAssetPath(thisScript)));

		scrollViewStyle = new GUIStyle();
		scrollViewStyle.overflow.left = 2;
		scrollViewStyle.stretchWidth = true;
		scrollViewStyle.stretchHeight = true;

		labelStyle = new GUIStyle("PR Label");
		labelStyle.padding.left = 2;
		labelStyle.margin.right = 0;

		gameObjectIcon = (Texture2D) EditorGUIUtility.ObjectContent(null, typeof(GameObject)).image;
		windowIcon = LoadEditorTexture2D("fgFavoritesTabIcon");
		emptyStar = LoadEditorTexture2D("fgEmptyStar");
		filledStars = new Texture2D[]
		{
			LoadEditorTexture2D("fgRedStar"),
			LoadEditorTexture2D("fgOrangeStar"),
			LoadEditorTexture2D("fgYellowStar"),
			LoadEditorTexture2D("fgGreenStar"),
			LoadEditorTexture2D("fgCyanStar"),
			LoadEditorTexture2D("fgBlueStar"),
			LoadEditorTexture2D("fgMagentaStar"),
		};
		createdByFlipbookGames = LoadEditorTexture2D("CreatedByFlipbookGames", false);
	}

	private static Texture2D LoadEditorTexture2D(string name, bool indieAndPro = true)
	{
		string skin = indieAndPro ? (EditorGUIUtility.isProSkin ? "Pro.png" : "Indie.png") : ".png";

		string path = System.IO.Path.Combine(thisAssetPath, "Textures");
		path = System.IO.Path.Combine(path, name);
		Texture2D texture = AssetDatabase.LoadMainAssetAtPath(path + skin) as Texture2D;
		if (texture != null)
			return texture;

		string oldPath = System.IO.Path.Combine(thisAssetPath, "Resources");
		oldPath = System.IO.Path.Combine(oldPath, name);
		texture = AssetDatabase.LoadMainAssetAtPath(oldPath + skin) as Texture2D;
		if (texture != null)
		{
			AssetDatabase.MoveAsset(oldPath + skin, path + skin);
			if (indieAndPro)
			{
				skin = !EditorGUIUtility.isProSkin ? "Pro.png" : "Indie.png";
				AssetDatabase.MoveAsset(oldPath + skin, path + skin);
			}
			return texture;
		}

		oldPath = System.IO.Path.Combine("FlipbookGames/FavoritesTab", name);
		texture = EditorGUIUtility.Load(oldPath + skin) as Texture2D;
		if (texture != null)
		{
			oldPath = System.IO.Path.Combine("Assets/Editor Default Resources", oldPath);
			AssetDatabase.MoveAsset(oldPath + skin, path + skin);
			if (indieAndPro)
			{
				skin = !EditorGUIUtility.isProSkin ? "Pro.png" : "Indie.png";
				AssetDatabase.MoveAsset(oldPath + skin, path + skin);
			}
		}
		return texture;
	}

	private void ScrollToItem(int index)
	{
		int count = listViewItems.Length;
		if (count > (int)((scrollViewRect.height) / 16f))
		{
			float maxScrollPos = 16f * index;
			float minScrollPos = maxScrollPos - scrollViewRect.height + 16f;
			if (scrollPosition.y < minScrollPos)
			{
				scrollPosition.y = minScrollPos;
				Repaint();
			}
			else if (scrollPosition.y > maxScrollPos)
			{
				scrollPosition.y = maxScrollPos;
				Repaint();
			}
		}
	}

	private void SelectItem(int index)
	{
		ListViewItem item = listViewItems[index];

		bool ctrl = EditorGUI.actionKey;
		bool shift = (Event.current.modifiers & EventModifiers.Shift) != 0 && focusedItemIndex >= 0;
		if (!shift)
			anchorItemIndex = -1;

		if (shift || ctrl)
		{
			List<UnityEngine.Object> newSelection = new List<UnityEngine.Object>();
			if (ctrl)
			{
				newSelection.AddRange(Selection.objects);

				UnityEngine.Object obj;
				if (item.instanceID != 0)
					obj = EditorUtility.InstanceIDToObject(item.instanceID);
				else
					obj = AssetDatabase.LoadAssetAtPath(item.assetPath, typeof(UnityEngine.Object));

				if (newSelection.Contains(obj))
					newSelection.Remove(obj);
				else
					newSelection.Add(obj);
			}
			else
			{
				if (anchorItemIndex < index)
				{
					for (int i = anchorItemIndex; i <= index; ++i)
					{
						UnityEngine.Object obj;
						if (listViewItems[i].instanceID != 0)
							obj = EditorUtility.InstanceIDToObject(listViewItems[i].instanceID);
						else
							obj = AssetDatabase.LoadAssetAtPath(listViewItems[i].assetPath, typeof(UnityEngine.Object));
						if (!newSelection.Contains(obj))
							newSelection.Add(obj);
					}
				}
				else
				{
					for (int i = index; i <= anchorItemIndex; ++i)
					{
						UnityEngine.Object obj;
						if (listViewItems[i].instanceID != 0)
							obj = EditorUtility.InstanceIDToObject(listViewItems[i].instanceID);
						else
							obj = AssetDatabase.LoadAssetAtPath(listViewItems[i].assetPath, typeof(UnityEngine.Object));
						if (!newSelection.Contains(obj))
							newSelection.Add(obj);
					}
				}
			}
			Selection.objects = newSelection.ToArray();
		}
		else if (item.instanceID != 0)
		{
			Selection.instanceIDs = new int[] { item.instanceID };
			EditorGUIUtility.PingObject(item.instanceID);
		}
		else
		{
			bool ping = true;
			UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(item.assetPath, typeof(UnityEngine.Object));

			if (obj != null)
			{
#if !UNITY_3_5
				if (MissingAPI.lastInteractedObjectBrowser != null)
				{
					bool isFolder = (bool) MissingAPI.isFolderMethod.Invoke(null, new object[] { obj.GetInstanceID() });
					if (isFolder)
					{
						EditorWindow objectBrowser = MissingAPI.lastInteractedObjectBrowser.GetValue(null) as EditorWindow;
						if (objectBrowser != null)
						{
							if (!(bool)MissingAPI.isLocked.GetValue(objectBrowser))
							{
								if (MissingAPI.viewModeField.GetValue(objectBrowser).ToString() == "TwoColumns")
								{
									ping = false;
									MissingAPI.showFolderContentsMethod.Invoke(objectBrowser, new object[] { obj.GetInstanceID(), false });

									MissingAPI.isLocked.SetValue(objectBrowser, true);
									Selection.objects = new UnityEngine.Object[] { obj };
									EditorApplication.update -= UnlockObjectBrowser;
									EditorApplication.update += UnlockObjectBrowser;
								}
							}
						}
					}
				}
#endif
				Selection.objects = new UnityEngine.Object[] { obj };
				if (ping)
					EditorGUIUtility.PingObject(obj);
			}
		}

		if (Selection.objects.Length > 0)
		{
			focusedItemIndex = index;
			if (!shift)
				anchorItemIndex = index;
		}

		ScrollToItem(index);
	}

#if !UNITY_3_5
	private static void UnlockObjectBrowser()
	{
		EditorApplication.update -= UnlockObjectBrowser;
		EditorApplication.update -= UnlockObjectBrowserDelayed;
		EditorApplication.update += UnlockObjectBrowserDelayed;
	}

	private static void UnlockObjectBrowserDelayed()
	{
		EditorApplication.update -= UnlockObjectBrowserDelayed;

		EditorWindow objectBrowser = MissingAPI.lastInteractedObjectBrowser.GetValue(null) as EditorWindow;
		if (objectBrowser != null)
		{
			MissingAPI.isLocked.SetValue(objectBrowser, false);
		}
	}

	private static void ShowFolderContent()
	{
		EditorApplication.update -= ShowFolderContent;
		//EditorApplication.update -= ShowFolderContentDelayed;
		ShowFolderContentDelayed();
		EditorApplication.update += ShowFolderContentDelayed;
	}

	private static void ShowFolderContentDelayed()
	{
		EditorApplication.update -= ShowFolderContentDelayed;

		if (Selection.objects != null && Selection.objects.Length == 1)
		{
			EditorWindow objectBrowser = MissingAPI.lastInteractedObjectBrowser.GetValue(null) as EditorWindow;
			if (objectBrowser != null)
			{
				objectBrowser.Repaint();
				MissingAPI.showFolderContentsMethod.Invoke(objectBrowser, new object[] { Selection.objects[0].GetInstanceID(), false });
			}
		}
	}

	private static class MissingAPI
	{
		public static Type objectBrowserType;
		public static FieldInfo lastInteractedObjectBrowser;
		public static MethodInfo isFolderMethod;
		public static FieldInfo viewModeField;
		public static FieldInfo isLocked;
		public static MethodInfo showFolderContentsMethod;

		static MissingAPI()
		{
			objectBrowserType = typeof(Editor).Assembly.GetType("UnityEditor.ObjectBrowser");
			if (objectBrowserType == null)
				return;

			lastInteractedObjectBrowser = objectBrowserType.GetField("s_LastInteractedObjectBrowser", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			if (lastInteractedObjectBrowser == null)
				return;

			isFolderMethod = objectBrowserType.GetMethod("IsFolder", new Type[] { typeof(int) });
			if (isFolderMethod == null)
			{
				lastInteractedObjectBrowser = null;
				return;
			}

			viewModeField = objectBrowserType.GetField("m_ViewMode", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			isLocked = objectBrowserType.GetField("m_IsLocked", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (viewModeField == null || isLocked == null)
			{
				lastInteractedObjectBrowser = null;
				return;
			}

			showFolderContentsMethod = objectBrowserType.GetMethod("ShowFolderContents",
				BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, null,
				new Type[] { typeof(int), typeof(bool) }, null);
			if (showFolderContentsMethod == null)
			{
				lastInteractedObjectBrowser = null;
				return;
			}
		}
	}
#endif

	PropertyInfo cachedTitleContent = null;

	private static void RefreshAllTabs()
	{
		FavoritesTab[] favsTabs = Resources.FindObjectsOfTypeAll(typeof(FavoritesTab)) as FavoritesTab[];
		foreach (FavoritesTab tab in favsTabs)
		{
			if (tab != null)
			{
				tab.recreateListViewItems = true;
				tab.Repaint();
			}
		}
	}

	public void OnGUI()
	{
		if (!initialized)
		{
			Initialize();
		}

		if (cachedTitleContent != null)
		{
			title = windowTitles[searchMode];

			GUIContent titleContent = cachedTitleContent.GetValue(this, null) as GUIContent;
			if (titleContent != null)
				titleContent.image = windowIcon;
		}

		if (recreateListViewItems && Event.current.type == EventType.layout)
		{
			EditorPrefs.SetBool("FlipbookGames.FavoritesTab.SortByType", sortByType);

			recreateListViewItems = false;
			listViewItems = favorites.CreateListViewItems(searchString, searchMode, sortByType);
		}

		if (Event.current.type == EventType.ValidateCommand && Event.current.commandName == "FrameSelected")
		{
			Event.current.Use();
			return;
		}
		else if (Event.current.type == EventType.ExecuteCommand && Event.current.commandName == "FrameSelected")
		{
			Event.current.Use();
			SceneView.FrameLastActiveSceneView();
			return;
		}
		else if (Event.current.type == EventType.keyDown)
		{
			switch (Event.current.keyCode)
			{
				case KeyCode.UpArrow:
					if (listViewItems.Length > 0)
						itemToSelect = Math.Max(0, focusedItemIndex - 1);
					Event.current.Use();
					break;

				case KeyCode.DownArrow:
					itemToSelect = Math.Min(listViewItems.Length - 1, focusedItemIndex + 1);
					Event.current.Use();
					break;

				case KeyCode.Home:
					if (listViewItems.Length > 0)
						itemToSelect = 0;
					Event.current.Use();
					break;

				case KeyCode.End:
					itemToSelect = listViewItems.Length - 1;
					Event.current.Use();
					break;

				case KeyCode.PageUp:
					if (listViewItems.Length > 0)
						itemToSelect = Math.Max(0, focusedItemIndex - ((int)scrollViewRect.height) / 16);
					Event.current.Use();
					break;

				case KeyCode.PageDown:
					itemToSelect = Math.Min(listViewItems.Length - 1, focusedItemIndex + ((int)scrollViewRect.height) / 16);
					Event.current.Use();
					break;
			}

			if (EditorGUI.actionKey && Event.current.keyCode == KeyCode.T && !Event.current.alt && !Event.current.shift)
			{
				Event.current.Use();
				OpenWindow();
				GUIUtility.ExitGUI();
			}
			else if (Event.current.modifiers == 0 && Event.current.character == '\n')
			{
				Event.current.Use();
				EditorApplication.ExecuteMenuItem("Assets/Open");
				GUIUtility.ExitGUI();
			}
		}

		EditorGUIUtility.LookLikeControls();

		DoToolbar();

		if (Event.current.type == EventType.ValidateCommand && Event.current.commandName == "SelectAll")
		{
			Event.current.Use();
			return;
		}
		else if (Event.current.type == EventType.ExecuteCommand && Event.current.commandName == "SelectAll")
		{
			Event.current.Use();

			UnityEngine.Object[] sel = new UnityEngine.Object[listViewItems.Length];
			for (int i = 0; i < listViewItems.Length; ++i)
			{
				if (listViewItems[i].instanceID != 0)
					sel[i] = EditorUtility.InstanceIDToObject(listViewItems[i].instanceID);
				else
					sel[i] = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(listViewItems[i].guid), typeof(UnityEngine.Object));
			}
			Selection.objects = sel;
			return;
		}
		
		GUI.SetNextControlName("ListView");
		if (focusListView)
		{
			GUI.FocusControl("ListView");
			if (Event.current.type == EventType.Repaint)
			{
				focusListView = false;
			}
		}

		if (Event.current.type == EventType.repaint)
			scrollViewRect = GUILayoutUtility.GetRect(1f, Screen.width, 16f, Screen.height, scrollViewStyle);
		else
			GUILayoutUtility.GetRect(1f, Screen.width, 16f, Screen.height, scrollViewStyle);

		if (Event.current.type != EventType.layout)
		{
			contentHeight = 16f * listViewItems.Length;
			scrollPosition = GUI.BeginScrollView(scrollViewRect, scrollPosition, new Rect(0f, 0f, 1f, contentHeight));

			paintingFavoritesTab = true;
			for (int i = 0; i < listViewItems.Length; ++i)
			{
			    DoListViewItem(listViewItems[i], i, EditorWindow.focusedWindow == this);
			}
			paintingFavoritesTab = false;

			if (itemToSelect >= 0)
			{
				SelectItem(itemToSelect);
				itemToSelect = -1;
			}

			GUI.EndScrollView();
		}

		if (itemPopupMenu != null)
		{
			itemPopupMenu.ShowAsContext();
			itemPopupMenu = null;
		}
	}

	static FavoritesTab()
	{
		favorites = new Favorites();
	}

	[InitializeOnLoad]
	private static class TreeViewTracker
	{
		private static EditorWindow treeViewEditorWindow = null;
		private static Rect highlightedItemRect = new Rect();
		
		private static Delegate[] hierarchyItemCallbacks;
		private static Delegate[] projectItemCallbacks;

		static TreeViewTracker()
		{
			EditorApplication.update += OnFirstUpdate;
			EditorApplication.hierarchyWindowItemOnGUI -= HierarchyItemOnGuiCallback;
			EditorApplication.hierarchyWindowItemOnGUI += HierarchyItemOnGuiCallback;
		}

		private static void OnFirstUpdate()
		{
			EditorApplication.update -= OnFirstUpdate;

			EditorApplication.hierarchyWindowItemOnGUI -= HierarchyItemOnGuiCallback;
			hierarchyItemCallbacks = EditorApplication.hierarchyWindowItemOnGUI != null ? EditorApplication.hierarchyWindowItemOnGUI.GetInvocationList() : null;
			EditorApplication.hierarchyWindowItemOnGUI = HierarchyItemOnGuiCallback;

			EditorApplication.projectWindowItemOnGUI -= ProjectItemOnGuiCallback;
			projectItemCallbacks = EditorApplication.projectWindowItemOnGUI != null ? EditorApplication.projectWindowItemOnGUI.GetInvocationList() : null;
			EditorApplication.projectWindowItemOnGUI = ProjectItemOnGuiCallback;

			EditorApplication.RepaintHierarchyWindow();
			EditorApplication.RepaintProjectWindow();
		}

		static string lastGuid;
		private static void ProjectItemOnGuiCallback(string item, Rect selectionRect)
		{
			if (string.IsNullOrEmpty(item))
				return;

			bool shrink = false;

			if (!paintingFavoritesTab && item != lastGuid)
			{
				lastGuid = item;
				
				if (!initialized)
					Initialize();

				if (treeViewEditorWindow)
					treeViewEditorWindow.wantsMouseMove = true;

				if (IsSameRect())
				{
					Rect rc = new Rect(selectionRect);

					int isFavorite = IsFavorite(item);
					if (isFavorite >= 0)
					{
						shrink = true;
						rc.x = rc.xMax - 18f;
						rc.width = 17f;
						rc.height = 16f;
						if (GUI.Button(rc, filledStars[isFavorite], GUIStyle.none))
						{
							if (Event.current.button == 0)
							{
								favorites.ToggleAsset(item);
								RefreshAllTabs();
							}
							else if (Event.current.button == 1)
							{
								EditorUtility.DisplayCustomMenu(rc, starColorNames, isFavorite, favorites.SetStarColor, item);
							}
						}
					}

					if (mouseOverWindow != null && selectionRect.Contains(Event.current.mousePosition))
					{
						mouseOverWindow.wantsMouseMove = true;
						treeViewEditorWindow = mouseOverWindow;
						highlightedItemRect.Set(Mathf.Max(selectionRect.x, 4f), selectionRect.y, selectionRect.width, selectionRect.height);
						EditorApplication.update -= TrackMouseOnWindow;
						EditorApplication.update += TrackMouseOnWindow;

						if (isFavorite < 0 && (DragAndDrop.visualMode <= 0 || ((int)DragAndDrop.visualMode) > 0xff))
						{
							shrink = true;
							rc.x = rc.xMax - 18f;
							rc.width = 17f;
							rc.height = 16f;
							if (GUI.Button(rc, emptyStar, GUIStyle.none))
							{
								if (Event.current.button == 0)
								{
									favorites.ToggleAsset(item);
									RefreshAllTabs();
								}
							}
						}
					}
				}
			}

			if (projectItemCallbacks != null)
			{
				if (shrink)
					selectionRect.xMax -= 18f;
				foreach (Delegate f in projectItemCallbacks)
					if (f != null)
						f.DynamicInvoke(item, selectionRect);
			}
		}

		private static void HierarchyItemOnGuiCallback(int item, Rect selectionRect)
		{
			bool shrink = false;

			if (!paintingFavoritesTab)
			{
				if (!initialized)
					Initialize();

				if (treeViewEditorWindow)
					treeViewEditorWindow.wantsMouseMove = true;

				if (IsSameRect())
				{
					//selectionRect.xMin = 0;
					Rect rc = new Rect(selectionRect);

					int isFavorite = IsFavorite(item);
					if (isFavorite >= 0)
					{
						shrink = true;
						rc.x = rc.xMax - 18f;
						rc.width = 17f;
						rc.height = 16f;
						if (GUI.Button(rc, filledStars[isFavorite], GUIStyle.none))
						{
							if (Event.current.button == 0)
							{
								favorites.ToggleGameObject(item);
								RefreshAllTabs();
							}
							else if (Event.current.button == 1)
							{
								EditorUtility.DisplayCustomMenu(rc, starColorNames, isFavorite, favorites.SetStarColor, item);
							}
						}
					}

					if (mouseOverWindow != null && selectionRect.Contains(Event.current.mousePosition))
					{
						mouseOverWindow.wantsMouseMove = true;
						treeViewEditorWindow = mouseOverWindow;
						highlightedItemRect = selectionRect;
						EditorApplication.update -= TrackMouseOnWindow;
						EditorApplication.update += TrackMouseOnWindow;

						if (isFavorite < 0 && (DragAndDrop.visualMode <= 0 || ((int) DragAndDrop.visualMode) > 0xff))
						{
							shrink = true;
							rc.x = rc.xMax - 18f;
							rc.width = 17f;
							rc.height = 16f;
							if (GUI.Button(rc, emptyStar, GUIStyle.none))
							{
								if (Event.current.button == 0)
								{
									favorites.ToggleGameObject(item);
									RefreshAllTabs();
								}
							}
						}
					}
				}
			}

			if (hierarchyItemCallbacks != null)
			{
				if (shrink)
					selectionRect.xMax -= 18f;
				foreach (Delegate f in hierarchyItemCallbacks)
					if (f != null)
						f.DynamicInvoke(item, selectionRect);
			}
		}

		static void TrackMouseOnWindow()
		{
			if (treeViewEditorWindow != null && mouseOverWindow != treeViewEditorWindow)
			{
				treeViewEditorWindow.Repaint();
				highlightedItemRect.Set(0, 0, 0, 0);
				EditorApplication.update -= TrackMouseOnWindow;
			}
		}

		static int IsFavorite(string item)
		{
			return favorites.Contains(item);
		}

		static int IsFavorite(int item)
		{
			return favorites.Contains(item);
		}

		static bool IsSameRect()
		{
			if (Event.current.type != EventType.mouseMove ||
				mouseOverWindow == treeViewEditorWindow && highlightedItemRect.Contains(Event.current.mousePosition))
			{
				return true;
			}

			if (highlightedItemRect.width != 0)
			{
				treeViewEditorWindow.Repaint();
				treeViewEditorWindow = null;
				highlightedItemRect.Set(0, 0, 0, 0);
				EditorApplication.update -= TrackMouseOnWindow;
			}
			return false;
		}
	}

	protected class ListViewItem : IComparable<ListViewItem>
	{
		public GUIContent guiContent;
		public string guid;
		public string assetPath;
		public int instanceID;
		public bool selected;
		
		public static ListViewItem FromGameObjectGUID(string guid)
		{
			ListViewItem item = new ListViewItem { guid = guid };
			UnityEngine.Object obj = favorites.FindGameObject(guid);
			if (obj == null)
				return null;

			item.instanceID = obj.GetInstanceID();
			item.guiContent = new GUIContent(EditorGUIUtility.ObjectContent(obj, null));
			if (item.guiContent.image == null && obj is GameObject)
				item.guiContent.image = gameObjectIcon;

			return item;
		}

		public static ListViewItem FromAssetGUID(string guid)
		{
			ListViewItem item = new ListViewItem { guid = guid };
			item.assetPath = AssetDatabase.GUIDToAssetPath(guid);
			if (item.assetPath == string.Empty || item.assetPath.StartsWith("Assets/__DELETED_GUID_Trash/"))
				return null;

			item.guiContent = new GUIContent(System.IO.Path.GetFileNameWithoutExtension(item.assetPath), AssetDatabase.GetCachedIcon(item.assetPath));
			return item;
		}

		public int CompareTo(ListViewItem other)
		{
			return guiContent.text.CompareTo(other.guiContent.text);
		}
	}

	private void OnSelectionChange()
	{
		Repaint();
	}

	private void DoListViewItem(ListViewItem item, int index, bool focused)
	{
		int selectedAssetInstanceID = 0;

		bool on = false;
		if (draggedItem != -1)
		{
			on = draggedItem == index;
		}
		else if (item.instanceID != 0)
		{
			on = Selection.Contains(item.instanceID);
		}
		else
		{
			foreach (int instanceID in Selection.instanceIDs)
			{
				if (AssetDatabase.GetAssetPath(instanceID) == item.assetPath)
				{
					selectedAssetInstanceID = instanceID;
					on = true;
					break;
				}
			}
		}

		if (item == null || Event.current == null)
			return;

        Color[] colorArray = EditorGUIUtility.isProSkin ? darkSkinColors : lightSkinColors;
        int colorCode = 0;

        GameObject go = item.instanceID != 0 ? EditorUtility.InstanceIDToObject(item.instanceID) as GameObject : null;
        if (go)
        {
            PrefabType prefabType = PrefabUtility.GetPrefabType(go);
			switch (prefabType)
			{
				case PrefabType.PrefabInstance:
				case PrefabType.ModelPrefabInstance:
				//case PrefabType.DisconnectedPrefabInstance:
				//case PrefabType.DisconnectedModelPrefabInstance:
					colorCode |= 1;
					break;

				case PrefabType.MissingPrefabInstance:
				//case PrefabType.DisconnectedPrefabInstance:
				//case PrefabType.DisconnectedModelPrefabInstance:
					colorCode |= 2;
					break;

				case PrefabType.Prefab:
				case PrefabType.ModelPrefab:
					go = null;
					break;
			}

#if UNITY_3_5
			if (go != null && !go.active)
#else
			if (go != null && !go.activeSelf)
#endif
				colorCode |= 4;
		}

		Color color = colorArray[colorCode & 3];
		Color onColor = colorArray[(colorCode & 3) + 4];
		color.a = onColor.a = colorCode >= 4 ? 0.6f : 1f;

		labelStyle.normal.textColor = color;
		labelStyle.hover.textColor = color;
		labelStyle.focused.textColor = color;
		labelStyle.active.textColor = color;
		labelStyle.onNormal.textColor = onColor;
		labelStyle.onHover.textColor = onColor;
		labelStyle.onFocused.textColor = onColor;
		labelStyle.onActive.textColor = onColor;


		Rect rcContent = new Rect(0f, 0f, scrollViewRect.width, scrollViewRect.height);
		if (contentHeight > rcContent.height)
		{
			rcContent.height = contentHeight;
			rcContent.width -= 15f;
		}


		Rect rcItem = new Rect(0f, 16f * index, rcContent.width, 16f);

		if (Event.current.type == EventType.repaint)
		{
			EditorGUIUtility.SetIconSize(new Vector2(16f, 16f));
			labelStyle.Draw(rcItem, item.guiContent, focused, on, on, focused);
		}

		EditorGUIUtility.SetIconSize(new Vector2(16f, 16f));
		if (go != null)
		{
			if (EditorApplication.hierarchyWindowItemOnGUI != null)
				EditorApplication.hierarchyWindowItemOnGUI(item.instanceID, new Rect(19f, rcItem.y, rcContent.width - 19f, 16f));
		}
		else
		{
			if (EditorApplication.projectWindowItemOnGUI != null)
				EditorApplication.projectWindowItemOnGUI(item.guid, rcItem);
		}
		EditorGUIUtility.SetIconSize(Vector2.zero);

		if (Event.current.type == EventType.DragExited || Event.current.type == EventType.dragPerform)
		{
			draggedItem = -1;
			Repaint();
		}
		else if (Event.current.isMouse && rcItem.Contains(Event.current.mousePosition))
		{
			if (Event.current.clickCount == 2 && Event.current.button == 0 && Event.current.type == EventType.mouseDown)
			{
				Event.current.Use();
				if (item.instanceID == 0 && selectedAssetInstanceID != 0)
					AssetDatabase.OpenAsset(selectedAssetInstanceID);
				else
					SceneView.FrameLastActiveSceneView();
				GUIUtility.ExitGUI();
				return;
			}

			if (Event.current.type == EventType.mouseDrag)
			{
				if (itemMouseDown != -1)
				{
					DragAndDrop.PrepareStartDrag();
					if (selectedAssetInstanceID != 0)
					{
						DragAndDrop.objectReferences = Selection.objects;
						DragAndDrop.StartDrag(Selection.objects.Length > 1 ? Selection.objects.Length + " objects" : item.guiContent.text);
					}
					else
					{
						draggedItem = itemMouseDown;
						if (item.instanceID != 0)
							DragAndDrop.objectReferences = new UnityEngine.Object[] { EditorUtility.InstanceIDToObject(item.instanceID) };
						else
							DragAndDrop.objectReferences = new UnityEngine.Object[]
								{ AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(item.guid), typeof(UnityEngine.Object)) };
						DragAndDrop.StartDrag(item.guiContent.text);
						Repaint();
					}
					Event.current.Use();
				}
				itemMouseDown = -1;
			}
			else if (Event.current.type == EventType.mouseDown)
			{
				if (Event.current.button == 0)
				{
					itemMouseDown = index;
				}
				else
				{
					selectedOnRightClick = !on;
					if (!on)
						itemToSelect = index;
				}
				focusListView = true;
				Event.current.Use();
			}
			else if (Event.current.type == EventType.mouseUp)
			{
				if (Event.current.button == 0)
				{
					if (itemMouseDown >= 0)
						itemToSelect = index;
					Event.current.Use();
				}
				else if (Event.current.button == 1)
				{
					itemPopupMenu = new GenericMenu();
					if (item.instanceID == 0)
					{
						if (selectedAssetInstanceID != 0)
						{
							if (Application.platform == RuntimePlatform.OSXEditor)
							{
								itemPopupMenu.AddItem(new GUIContent("Open _\n"), false, () => EditorApplication.ExecuteMenuItem("Assets/Open"));
								itemPopupMenu.AddItem(new GUIContent("Reveal in Finder"), false, () => EditorApplication.ExecuteMenuItem("Assets/Reveal in Finder"));
							}
							else
							{
								itemPopupMenu.AddItem(new GUIContent("Open _ENTER"), false, () => EditorApplication.ExecuteMenuItem("Assets/Open"));
								itemPopupMenu.AddItem(new GUIContent("Show in Explorer"), false, () => EditorApplication.ExecuteMenuItem("Assets/Show in Explorer"));
							}
							itemPopupMenu.AddItem(new GUIContent("Reimport"), false, () => EditorApplication.ExecuteMenuItem("Assets/Reimport"));
							itemPopupMenu.AddSeparator(string.Empty);
						}
						itemPopupMenu.AddItem(new GUIContent("Remove from Favorites"), false, () => RemoveFavoritesMenuHandler(item));
					}
					else
					{
						itemPopupMenu.AddItem(new GUIContent("Frame in Scene _f"), false, () => SceneView.FrameLastActiveSceneView());
						itemPopupMenu.AddSeparator(string.Empty);
						itemPopupMenu.AddItem(new GUIContent("Remove from Favorites"), false, () => RemoveFavoritesMenuHandler(item));
					}
					Event.current.Use();
					Focus();
				}
			}
		}
	}

	private bool selectedOnRightClick = true;
	private void RemoveFavoritesMenuHandler(ListViewItem item)
	{
		if (selectedOnRightClick)
		{
			if (item.instanceID != 0)
				favorites.ToggleGameObject(item.instanceID);
			else
				favorites.ToggleAsset(item.guid);
		}
		else
		{
			// Removing all selected favorite items
			ListViewItem[] itemsCopy = (ListViewItem[]) listViewItems.Clone();
			foreach (ListViewItem selectedItem in itemsCopy)
			{
				if (selectedItem.instanceID != 0)
				{
					if (Selection.Contains(selectedItem.instanceID))
						favorites.ToggleGameObject(selectedItem.instanceID);
				}
				else if (Array.Find(Selection.objects, (a) => AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(a)) == selectedItem.guid) != null)
				{
					favorites.ToggleAsset(selectedItem.guid);
				}
			}
		}
		recreateListViewItems = true;
		RefreshAllTabs();
	}

	private void DoToolbar()
	{
		GUI.enabled = true;

		GUILayout.Space(1f);
		GUILayout.BeginHorizontal(EditorStyles.toolbar);

		if (GUILayout.Button("View", EditorStyles.toolbarDropDown))
		{
			GenericMenu menu = new GenericMenu();
			menu.AddItem(new GUIContent("Add Tab %t"), false, OpenWindow);
			menu.AddSeparator(String.Empty);
			menu.AddItem(new GUIContent("Sort by Name"), !sortByType, () => { sortByType = false; recreateListViewItems = true; Repaint(); });
			menu.AddItem(new GUIContent("Sort by Type"), sortByType, () => { sortByType = true; recreateListViewItems = true; Repaint(); });
			menu.AddSeparator(String.Empty);
			menu.AddItem(new GUIContent("About..."), false, ShowAboutWindow);

			Vector2 size = EditorStyles.toolbarDropDown.CalcSize(new GUIContent("View"));
			menu.DropDown(new Rect(5f, -1f, size.x, size.y));
		}

		GUILayout.Space(5f);
		GUILayout.FlexibleSpace();

		if (toolbarSearchField == null)
			toolbarSearchField = "ToolbarSeachTextFieldPopup";
		Rect position = GUILayoutUtility.GetRect(1f, 200f, 16f, 16f, toolbarSearchField);
		DoSearchBox(position);

		GUILayout.EndHorizontal();
	}

	private void ShowAboutWindow()
	{
		EditorWindow.GetWindow<AboutFavoritesTab>();
	}

	private void DoSearchBox(Rect position)
	{
		if (Event.current.type == EventType.MouseDown && position.Contains(Event.current.mousePosition))
		{
			focusSearchBox = true;
		}

		if (Event.current.type == EventType.ValidateCommand && Event.current.commandName == "Find")
		{
			Event.current.Use();
			return;
		}
		else if (Event.current.type == EventType.ExecuteCommand && Event.current.commandName == "Find")
		{
			focusSearchBox = true;
		}

		GUI.SetNextControlName("SearchFilter");
		if (focusSearchBox)
		{
			GUI.FocusControl("SearchFilter");
			if (Event.current.type == EventType.Repaint)
			{
				focusSearchBox = false;
			}
		}
		hasSearchBoxFocus = GUI.GetNameOfFocusedControl() == "SearchFilter";

		if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape)
		{
			if (hasSearchBoxFocus)
			{
				SetSearchFilter(string.Empty, searchModes[searchMode]);
			}
			else
			{
				if (string.IsNullOrEmpty(searchString))
					searchMode = 0;
				SetSearchFilter(string.Empty, searchModes[searchMode]);
			}
			focusListView = true;
			Event.current.Use();
		}

		string text = ToolbarSearchField(position, searchModes, ref searchMode, searchString);
		if (searchString != text)
		{
			SetSearchFilter(text, searchModes[searchMode]);
			Repaint();
		}
	}

	private string ToolbarSearchField(Rect position, string[] searchModes, ref int searchMode, string text)
	{
		if (toolbarSearchField == null || toolbarSearchFieldCancelButton == null || toolbarSearchFieldCancelButtonEmpty == null)
		{
			toolbarSearchField = "ToolbarSeachTextFieldPopup";
			toolbarSearchFieldCancelButton = "ToolbarSeachCancelButton";
			toolbarSearchFieldCancelButtonEmpty = "ToolbarSeachCancelButtonEmpty";
		}

		Rect rcDropDown = position;
		rcDropDown.width = 20f;
		if ((Event.current.type == EventType.MouseDown) && rcDropDown.Contains(Event.current.mousePosition))
		{
			if (searchModesMenuItems == null)
			{
				searchModesMenuItems = new GUIContent[searchModes.Length];
				for (int i = 0; i < searchModes.Length; ++i)
					searchModesMenuItems[i] = new GUIContent(searchModes[i]);
			}

			EditorUtility.DisplayCustomMenu(position, searchModesMenuItems, searchMode, SelectSearchMode, null);
			Event.current.Use();
		}

		Rect rc = position;
		rc.width -= 14f;

		text = EditorGUI.TextField(rc, text, toolbarSearchField);

		bool isEmpty = text == string.Empty;
		if (isEmpty && (!hasSearchBoxFocus || EditorWindow.focusedWindow != this || searchMode != 0) && Event.current.type == EventType.repaint)
		{
			bool enabled = GUI.enabled;
			GUI.enabled = false;
			Color color = GUI.backgroundColor;
			GUI.backgroundColor = Color.clear;
			if (!hasSearchBoxFocus || EditorWindow.focusedWindow != this)
			{
				toolbarSearchField.Draw(rc, searchModes[searchMode], false, false, false, false);
			}
			else if (searchMode > 0)
			{
				toolbarSearchField.alignment = TextAnchor.UpperRight;
				toolbarSearchField.Draw(rc, searchModes[searchMode] + '\xa0', false, false, false, false);
				toolbarSearchField.alignment = TextAnchor.UpperLeft;
			}
			GUI.enabled = enabled;
			GUI.backgroundColor = color;
		}

		rc = position;
		rc.x += position.width - 14f;
		rc.width = 14f;
		if (!isEmpty || searchMode != 0)
		{
			if (GUI.Button(rc, GUIContent.none, toolbarSearchFieldCancelButton))
			{
				recreateListViewItems = true;
				if (isEmpty)
					searchMode = 0;
				else
					text = string.Empty;
				focusListView = true;
				//GUIUtility.keyboardControl = 0;
			}
		}
		else
		{
			if (GUI.Button(rc, GUIContent.none, toolbarSearchFieldCancelButtonEmpty))
				focusSearchBox = true;
		}

		return text;
	}

	private void SelectSearchMode(object userData, string[] options, int selected)
	{
		searchMode = selected;
		recreateListViewItems = true;
		Repaint();
	}

	private void SetSearchFilter(string searchFilter, string searchMode)
	{
		searchString = searchFilter;
		hasSearchBoxFocus = false;
		recreateListViewItems = true;
		Repaint();
	}
}
