using System;
using System.Collections.Generic;
using System.Text;

namespace GZCampusNavigator
{
    public class DijkstraNavigation
    {
        private int[] _lastNode;
        private int[] _distance;
        private bool[] _marked;

        public DijkstraNavigation(int n)
        {
            _lastNode = new int[n + 1];
            _distance = new int[n + 1];
            _marked = new bool[n + 1];
        }

        public void Reset()
        {
            for (int i = 0; i < _lastNode.Length; i++) _lastNode[i] = -1;
            for (int i = 0; i < _distance.Length; i++) _distance[i] = int.MaxValue;
            for (int i = 0; i < _marked.Length; i++) _marked[i] = false;
        }

        public Stack<int> Navigate(int start, int end, StringBuilder logger = null)
        {
            _distance[start] = 0;

            while (true)
            {
                // 选出 distance 最小的
                int index = -1;
                for (int i = 1; i <= MapManager.LocationCount; i++)
                {
                    if (!_marked[i])
                    {
                        if (index == -1) index = i;
                        else if (_distance[i] < _distance[index])
                        {
                            index = i;
                        }
                    }
                }
                if (index == -1) break;
                // 如果最短距离已经是 IntMax , 说明不可达
                if (_distance[index] == int.MaxValue) break;

                _marked[index] = true;
                // 该结点的相连的边
                if (MapManager.Edge.ContainsKey(index))
                    foreach (var (id, dis) in MapManager.Edge[index])
                    {
                        int newDist = _distance[index] + dis;
                        if (_distance[id] > newDist)
                        {
                            _distance[id] = newDist;
                            _lastNode[id] = index;
                        }
                    }
            }

            //for (int i = 1; i <= MapManager.LocationCount; ++i)
            //{
            //    Console.WriteLine($"{i}  {_distance[i]} {_lastNode[i]}");
            //}
            //Console.WriteLine();

            Stack<int> res = new();
            Stack<int> ret = new();
            int p = end;

            while (p != -1)
            {
                res.Push(p);
                ret.Push(p);

                p = _lastNode[p];
            }

            if (!res.Contains(start))
            {
                logger?.AppendLine($"{MapManager.IdtoLocationMap[start]} 无法到达 {MapManager.IdtoLocationMap[end]}");
                return null;
            }
            logger?.AppendLine($"{MapManager.IdtoLocationMap[start]} 到 {MapManager.IdtoLocationMap[end]} 的最短距离是: {_distance[end] / 1000.0} (km)");

            logger?.AppendLine();
            while (res.Count > 0)
            {
                if (res.Count > 1)
                    logger?.AppendLine(MapManager.IdtoLocationMap[res.Pop()] + " -> ");
                else
                    logger?.AppendLine(MapManager.IdtoLocationMap[res.Pop()]);
            }
            logger?.AppendLine();

            return ret;
        }


    }
}
