using System.Collections.Generic;

namespace GZCampusNavigator
{
    public static class LineRenderManager
    {
        public static void ResetLine()
        {
            foreach (EdgeNode edge in MapManager.EdgeNodes)
            {
                edge.HideLine();
            }
        }


        public static void RenderLine(Stack<int> path)
        {
            ResetLine();

            if (path == null || path.Count == 0) return;

            while (path.Count > 1)
            {
                var x = path.Pop();
                var y = path.Pop();

                var name = MapManager.GetLocationName(x);
                var name2 = MapManager.GetLocationName(y);

                foreach (EdgeNode edge in MapManager.EdgeNodes)
                {
                    if (edge.FromNode.Name == name && edge.ToNode.Name == name2)
                    {
                        edge.ShowLine();
                        break;
                    }
                    else if (edge.ToNode.Name == name && edge.FromNode.Name == name2)
                    {
                        edge.ShowLine();
                        break;
                    }
                }

                if (path.Count != 0)
                {
                    path.Push(y);
                }
            }
        }

    }
}
