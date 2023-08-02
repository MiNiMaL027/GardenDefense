using Godot;
using System;

public abstract partial class BaseTooltip : Control
{
    public abstract void ShowTooltip(Node n);
    public abstract void HideTooltip();

}
