using Godot;

namespace GZCampusNavigator
{
    public class MapLoader
    {
        public static MapLoader _instance;
        public static MapLoader Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new MapLoader();
                return _instance;
            }
        }

        /// <summary>
        /// 加载地点配置
        /// </summary>
        public void LoadLocationConfig(string path)
        {
            using var file = FileAccess.Open(path, FileAccess.ModeFlags.Read);
            if (file == null)
            {
                GD.PrintErr($"MapLoader: 无法打开文件 {path}");
                return;
            }

            int locationIndex = 1;
            while (!file.EofReached())
            {
                string line = file.GetLine();
                // 跳过纯空行（可保留，原逻辑遇到空行也不会出错）
                if (line.Length == 0)
                    continue;

                string[] parts = line.Split(',');

                if (parts.Length == 2)
                {
                    MapManager.IdtoLocationMap[locationIndex] = parts[0];
                    MapManager.LocationToIdMap[parts[0]] = locationIndex;
                    MapManager.LocationDescriptionMap[locationIndex] = parts[1];
                    locationIndex++;
                }
            }
        }

        /// <summary>
        /// 加载边配置
        /// </summary>
        public void LoadEdgeConfig(string path)
        {
            using var file = FileAccess.Open(path, FileAccess.ModeFlags.Read);
            if (file == null)
            {
                GD.PrintErr($"MapLoader: 无法打开文件 {path}");
                return;
            }

            while (!file.EofReached())
            {
                string line = file.GetLine();
                if (line.Length == 0)
                    continue;

                string[] parts = line.Split(',');
                if (parts.Length == 3)
                {
                    string start = parts[0];
                    string end = parts[1];
                    int value = int.Parse(parts[2]);

                    int sId = MapManager.LocationToIdMap[start];
                    int eId = MapManager.LocationToIdMap[end];

                    if (!MapManager.Edge.ContainsKey(sId))
                        MapManager.Edge[sId] = new();
                    if (!MapManager.Edge.ContainsKey(eId))
                        MapManager.Edge[eId] = new();

                    MapManager.Edge[sId][eId] = value;
                    MapManager.Edge[eId][sId] = value;
                }
            }
        }
    }
}
