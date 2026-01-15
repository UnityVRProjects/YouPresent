using UnityEngine;

public class PresentButton : MenuButton
{
    public override void Click()
    {
        FindFirstObjectByType<ButtonBehavior>().ChangeColor();
    }
}
