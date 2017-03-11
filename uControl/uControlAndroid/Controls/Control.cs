using System;

namespace uControlAndroid.Controls
{
    public class Control
    {
        public Guid? Id { get; set; }

        public int YPos { get; set; }

        public int XPos { get; set; }

        public int Height { get; set; }

        public int Weight { get; set; }

        public ControlType ControlType { get; set;}
    }
}