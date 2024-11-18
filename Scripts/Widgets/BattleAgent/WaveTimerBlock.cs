using Godot;

public partial class WaveTimerBlock : PanelContainer
{
	ProgressBar ProgressBar { get; set; }
    private int stageDuration;
    private Timer timer;
    TextureRect StageTextureRect { get; set; }
	ColorRect StageColorRect { get; set; }

    Color BackgroundColor = new Color(0.6f, 0.6f, 0.6f, 0.239f);
    Color FillColor = new Color(0, 0.494f, 0.537f);

    public bool isFinished = false;

    [Signal]
    public delegate void FinishEventHandler();

    public override void _Ready()
	{
		ProgressBar = GetNode<ProgressBar>("HBoxContainer/Panel2/ProgressBar");
		StageTextureRect = GetNode<TextureRect>("HBoxContainer/Panel/ColorRect/TextureRect");
		StageColorRect = GetNode<ColorRect>("HBoxContainer/Panel/ColorRect");

        StageColorRect.Color = BackgroundColor;
        ProgressBar.Value = 0;
    }

	public void Init(int stageDuration)
	{
        this.stageDuration = stageDuration;
		ProgressBar.MaxValue = stageDuration;       
    }

    public void StartTimer()
    {
        timer = new Timer();
        AddChild(timer);

        timer.WaitTime = 1.0f;
        timer.Timeout += OnTimerTimeout;
        timer.Start();
    }

    public void StartWave(bool instantly = false)
    {       
        timer.Stop();
        timer.QueueFree();

        StageColorRect.Color = FillColor;

        if(instantly)
        {
            ProgressBar.Value = stageDuration;
        }

        isFinished = true;

        EmitSignal(SignalName.Finish);
    }

    private void OnTimerTimeout()
    {
        if (ProgressBar.Value < stageDuration)
        {
            ProgressBar.Value += 1;
        }
    }
}
