using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using uControlAndroid.Common;

namespace uControlAndroid.Entities
{
    public class Control
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int YPos { get; set; }

        public int XPos { get; set; }

        public int Height { get; set; }

        public int Weight { get; set; }

        public ControlType ControlType { get; set;}

        [ForeignKey(typeof(GamePad))] 
        public int GamePadId { get; set; }
    }
}