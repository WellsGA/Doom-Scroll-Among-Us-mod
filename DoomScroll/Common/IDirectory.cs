﻿using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace DoomScroll.Common
{
   public interface IDirectory
    {
        public string GetName();
        public void SetPath(string path);
        public CustomButton GetButton();
        public void DisplayContent();

        public string PrintDirectory();
    }
}
