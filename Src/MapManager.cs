using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GZCampusNavigator
{
    public static class MapManager
    {
        public static Dictionary<string, int> LocationToIdMap = new();
        public static Dictionary<int, string> IdtoLocationMap = new();
        public static Dictionary<int, string> LocationDescriptionMap = new();
        public static Dictionary<int, LocationNode> LocationNodeMap = new();

        public static List<EdgeNode> EdgeNodes = new();
        public static Dictionary<int, Dictionary<int, int>> Edge = new();

        private static DijkstraNavigation _dijkstraNavigation;

        public static int LocationCount => LocationToIdMap.Count;

        public static int GetLocationId(string name) => LocationToIdMap[name];

        public static string GetLocationName(int id) => IdtoLocationMap[id];

        public static string GetLocationDescription(int id) => LocationDescriptionMap[id];

        public static string GetLocationDescription(string name) => GetLocationDescription(GetLocationId(name));

        public static bool CheckLocationNameValid(string name) => LocationToIdMap.ContainsKey(name);

        public static int GetCostById(int fromId, int toId) => Edge[fromId][toId];

        public static int GetCostByName(string fromName, string toName) => GetCostById(GetLocationId(fromName), GetLocationId(toName));

        public static LocationNode GetLocationNode(int id) => LocationNodeMap[id];

        public static LocationNode GetLocationNode(string name) => LocationNodeMap[GetLocationId(name)];


        // 确保 user:// 下的配置存在，不存在则从 res:// 拷贝
        private static void EnsureUserFileExists(string userPath, string resPath)
        {
            if (Godot.FileAccess.FileExists(userPath))
                return;

            // 确保目录存在
            string dir = userPath.GetBaseDir();
            if (!DirAccess.DirExistsAbsolute(dir))
                DirAccess.MakeDirRecursiveAbsolute(dir);

            // 拷贝
            using var src = Godot.FileAccess.Open(resPath, Godot.FileAccess.ModeFlags.Read);
            if (src == null)
            {
                GD.PrintErr($"无法读取内置配置: {resPath}");
                return;
            }
            using var dst = Godot.FileAccess.Open(userPath, Godot.FileAccess.ModeFlags.Write);
            dst.StoreBuffer(src.GetBuffer((long)src.GetLength()));
        }


        public static void MapLoadLocationConfig()
        {
            string userPath = "user://Models/Location.txt";
            string resPath = "res://Models/Location.txt";
            EnsureUserFileExists(userPath, resPath);
            MapLoader.Instance.LoadLocationConfig(userPath);
        }

        public static void MapLoadEdgeConfig()
        {
            string userPath = "user://Models/Edge.txt";
            string resPath = "res://Models/Edge.txt";
            EnsureUserFileExists(userPath, resPath);
            MapLoader.Instance.LoadEdgeConfig(userPath);
        }

        public static void Setup()
        {
            _dijkstraNavigation = new(LocationToIdMap.Count);
        }

        public static Stack<int> Navigation(int start, int end, StringBuilder logger = null)
        {
            _dijkstraNavigation?.Reset();
            return _dijkstraNavigation?.Navigate(start, end, logger);
        }
    }
}
