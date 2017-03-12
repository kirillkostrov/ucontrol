using uControlAndroid.Common;

namespace uControlAndroid.Entities
{
    public class GamePad
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Control[] Controls { get; set; }
    }
}