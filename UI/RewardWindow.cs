using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardWindow : BaseWindow 
{
    public Button cancelBtn;
    // Use this for initialization
    protected override bool CanOpen()
    {
        return true;
    }
}
