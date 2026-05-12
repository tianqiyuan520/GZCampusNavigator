using Godot;
using System;
using System.Text;

namespace GZCampusNavigator
{
    public partial class MainScene : Node
    {
        [Export] public LineEdit StartPosLineEdit;
        [Export] public LineEdit EndPosLineEdit;
        [Export] public TextureButton SearchButton;
        [Export] public RichTextLabel InfoText;
        [Export] public TextureRect StartPin;
        [Export] public TextureRect EndPin;

        public override void _Ready()
        {
            SearchButton.Pressed += SearchButtonPress;

            //var fullPath = ProjectSettings.GlobalizePath("res://Models");
            //var fullPath = "res://Models";
            //Console.WriteLine($"加载文件: {fullPath}");
            MapManager.MapLoadLocationConfig();
            MapManager.MapLoadEdgeConfig();

            MapManager.Setup();

            //bool isProgramEnd = false;

            //if (isProgramEnd)
            //{
            //    Console.WriteLine("\n欢迎再次使用~\n");
            //}

            LoadLocationNode();
            LoadEdgeNode();
        }

        /// <summary>
        /// 加载 地点结点
        /// </summary>
        private void LoadLocationNode()
        {
            foreach (LocationNode node in GetNode<Control>("Location").GetChildren())
            {
                Console.WriteLine($"加载地点结点： {node.Name}");
                MapManager.LocationNodeMap[MapManager.LocationToIdMap[node.Name]] =
                    node;
                node.OnClick += LocationButtonPress;
            }
        }

        /// <summary>
        /// 加载 边
        /// </summary>
        private void LoadEdgeNode()
        {
            foreach (EdgeNode node in GetNode<Control>("Edge").GetNode<Control>("1").GetChildren())
            {
                Console.WriteLine($"加载路径： {node}");
                MapManager.EdgeNodes.Add(node);
                node.HideLine();
                node.SetText(MapManager.GetCostByName(node.FromNode.Name, node.ToNode.Name) + "m");
            }

            foreach (EdgeNode node in GetNode<Control>("Edge").GetNode<Control>("2").GetChildren())
            {
                Console.WriteLine($"加载路径： {node}");
                MapManager.EdgeNodes.Add(node);
                node.HideLine();
                node.SetText(MapManager.GetCostByName(node.FromNode.Name, node.ToNode.Name) + "m");
            }

            foreach (EdgeNode node in GetNode<Control>("Edge").GetNode<Control>("3").GetChildren())
            {
                Console.WriteLine($"加载路径： {node}");
                MapManager.EdgeNodes.Add(node);
                node.HideLine();
                node.SetText(MapManager.GetCostByName(node.FromNode.Name, node.ToNode.Name) + "m");
            }
        }


        public void LocationButtonPress(string name)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(name);
            sb.AppendLine(MapManager.GetLocationDescription(name));
            InfoText.Text = sb.ToString();
        }

        /// <summary>
        /// 重置图钉
        /// </summary>
        public void ResetPin()
        {
            StartPin.Position = new Vector2(-100, -100);
            EndPin.Position = new Vector2(-100, -100);
        }

        /// <summary>
        /// 图钉
        /// </summary>
        public void SetPin(LocationNode start, LocationNode end)
        {
            StartPin.GlobalPosition = start.GlobalPosition + Vector2.Right * 11 + Vector2.Up * 10;
            EndPin.GlobalPosition = end.GlobalPosition + Vector2.Right * 11 + Vector2.Up * 10;
        }

        /// <summary>
        /// 搜索按钮按下触发
        /// </summary>
        public void SearchButtonPress()
        {
            ResetPin();

            int start = -1;
            int end = -1;

            string startInput = StartPosLineEdit.Text;
            if (MapManager.CheckLocationNameValid(startInput))
            {
                start = MapManager.GetLocationId(startInput);
                StartPosLineEdit.GetNode<Label>("Label").Hide();
            }
            else
            {
                StartPosLineEdit.GetNode<Label>("Label").Show();
                return;
            }

            string endInput = EndPosLineEdit.Text;
            if (MapManager.CheckLocationNameValid(endInput))
            {
                end = MapManager.GetLocationId(endInput);
                EndPosLineEdit.GetNode<Label>("Label").Hide();
            }
            else
            {
                EndPosLineEdit.GetNode<Label>("Label").Show();
                return;
            }

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"{MapManager.IdtoLocationMap[start]} : {MapManager.LocationDescriptionMap[start]}");
            sb.AppendLine();
            sb.AppendLine($"{MapManager.IdtoLocationMap[end]} : {MapManager.LocationDescriptionMap[end]}");
            sb.AppendLine();

            var path = MapManager.Navigation(start, end, sb);

            InfoText.Text = sb.ToString();

            LineRenderManager.RenderLine(path);

            SetPin(MapManager.GetLocationNode(start), MapManager.GetLocationNode(end));
        }

    }
}

