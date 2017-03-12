using uControlAndroid.Common;

namespace uControlAndroid.Entities
{
    public class Control
    {
        public int Id { get; set; }

        public int YPos { get; set; }

        public int XPos { get; set; }

        public int Height { get; set; }

        public int Weight { get; set; }

        public ControlType ControlType { get; set;}

        public int GamePadId { get; set; }
    }
}