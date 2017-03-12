using uControlAndroid.Common;

namespace uControlAndroid.Entities
{
    public class Control
    {
        public int Id { get; set; }

        public float YPos { get; set; }

        public float XPos { get; set; }

        public int Height { get; set; }

        public int Weight { get; set; }

        public ControlType ControlType { get; set;}
    }
}