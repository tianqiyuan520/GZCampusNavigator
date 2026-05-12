using Godot;
using System;

namespace GZCampusNavigator
{
    public partial class LocationNode : Control
    {
        private Button _button;

        public Action<string> OnClick;

        public override void _Ready()
        {
            _button = GetNode<Button>("Button");
            _button.Pressed += () => { OnClick?.Invoke(Name); };
        }

    }
}
