using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingNode : NodeView
{
    private void Start()
    {
        ToggleHeight(true);
    }
    
    public override void Pop()
    {
        //do nothing on visuals
    }

}
