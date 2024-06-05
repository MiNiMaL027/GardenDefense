using Godot;
public partial class Energy : TextureProgressBar
{
    public bool filled;
    public void StartFilling()
    {
        var tween = CreateTween();

        var duration = this.GetPlayerController().TimeToEnergyRestoration;

        tween.TweenProperty(this, "value", MaxValue, duration - 0.1f);      
        tween.Finished += () => { filled = true; };

        tween.Play();
    }

    public void Init(bool filled = false)
    {
        if(filled)      
            Value = MaxValue;       
        else      
            Value = MinValue;

        this.filled = filled;
    }
}
