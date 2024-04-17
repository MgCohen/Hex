using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hex.Views
{
    public class StartingNode : NodeView
    {
        private void Start()
        {
            animations.SetHeightUp();
        }

        public override void Pop(int intensity)
        {
            //do nothing on visuals
        }
    }
}
