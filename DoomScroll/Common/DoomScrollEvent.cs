using System;
using System.Collections.Generic;
using System.Text;

namespace DoomScroll.Common
{
    public class DoomScrollEvent
    {
        public event Action MyAction;
        public void InvokeAction() 
        {
            MyAction?.Invoke();
        }
    }
}
