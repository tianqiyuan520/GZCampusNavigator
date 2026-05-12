using Godot;

namespace GZCampusNavigator
{
    public partial class EdgeNode : Control
    {
        [Export] public LocationNode FromNode;
        [Export] public LocationNode ToNode;

        private Label _label;
        private Line2D _line;

        public override void _Ready()
        {
            _label = GetNode<Label>("Label");
            _line = GetNode<Line2D>("Line2D");
        }

        public void ShowLine()
        {
            _line.Show();
        }

        public void HideLine()
        {
            _line.Hide();
        }

        public void SetText(string text)
        {
            _label.Text = text;
        }

        public override string ToString()
        {
            return $"{FromNode.Name} - {ToNode.Name}";
        }
    }
}
