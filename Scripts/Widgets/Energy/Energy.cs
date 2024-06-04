using Godot;
public partial class Energy : TextureProgressBar
{
    public bool filled;
    public void StartFilling()
    {
        var tween = CreateTween();

        var duration = this.GetPlayerController().TimeToEnergyRestoration;

        tween.TweenProperty(this, "value", 100, duration);      
        tween.Finished += () => { filled = true; };

        tween.Play();
    }

    public void Init(bool filled = false)
    {
        if(filled)      
            Value = 100;       
        else      
            Value = 0;

        this.filled = filled;
    }
}
