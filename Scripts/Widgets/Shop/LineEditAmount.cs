using Godot;
using System;

public partial class LineEditAmount : LineEdit
{
	RegEx regex = new RegEx();

	string oldtext = "1";
	public override void _Ready()
	{
		regex.Compile("^[0-9]*$");

        this.TextChanged += LineEditAmount_TextChanged;
	}

    private void LineEditAmount_TextChanged(string newText)
    {
		var results = regex.Search(newText);

        if (results != null)
		{
			Text = newText;
			oldtext = Text;        
        }
		else
		{
			Text = oldtext;
			
		}

        CaretColumn = Text.Length;
    }
}
