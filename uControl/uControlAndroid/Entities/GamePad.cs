using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace uControlAndroid.Entities
{
    public class GamePad
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(256)]
        public string Name { get; set; }
    }
}