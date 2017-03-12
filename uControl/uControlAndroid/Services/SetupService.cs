using Android.Content;
using uControlAndroid.Entities;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using uControlAndroid.Common;

namespace uControlAndroid.Services
{
    public class GamePadService
    {
        private Dictionary<int, GamePad> CustomGamePads { get; set; }

        private string FilePath { get; set; }

        public GamePadService()
        {
            var fileName = "setupDb.json";
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
            FilePath = Path.Combine(documentsPath, fileName);

            if (File.Exists(FilePath))
            {
                var json = File.ReadAllText(FilePath);
                CustomGamePads = JsonConvert.DeserializeObject<GamePad[]>(json).ToDictionary(x => x.Id);
            }
            else
            {
                CustomGamePads = new Dictionary<int, GamePad>();
            }
        }

        private void SaveChanges()
        {
            var json = JsonConvert.SerializeObject(CustomGamePads.Values.ToArray());
            File.WriteAllText(FilePath, json);
        }

        public ListItem[] GetGamePadList()
        {
            return CustomGamePads.Select(x => new ListItem
            {
                Id = x.Key,
                Name = x.Value.Name
            }).ToArray();
        }

        public int CreateOrUpdateGamePad(ListItem gamePad)
        {
            var gamePadId = gamePad.Id;
            if (gamePadId > 0)
            {
                CustomGamePads[gamePad.Id].Name = gamePad.Name;
            }
            else
            {
                gamePadId = CustomGamePads.Count > 0
                ? CustomGamePads.Max(x => x.Key) + 1
                : 1;
                var newGamePad = new GamePad
                {
                    Id = gamePadId,
                    Name = gamePad.Name,
                    Controls = new Control[0],
                };

                CustomGamePads.Add(gamePadId, newGamePad);
            }
            
            SaveChanges();

            return gamePadId;
        }

        public void DeleteGamePad(int id)
        {
            CustomGamePads.Remove(id);
            SaveChanges();
        }

        public Control[] GetGamePadControls(int gamePadId)
        {
            return CustomGamePads[gamePadId].Controls;
        }

        public int CreateOrUpdateControl(Control control)
        {
            var controls = GetGamePadControls(control.GamePadId).ToDictionary(x => x.Id);

            if (control.Id > 0)
            {
                controls[control.Id] = control;
            }
            else
            {
                var nextId = controls.Values.Count > 0
                    ? controls.Values.Max(x => x.Id) + 1
                    : 1;
                control.Id = nextId;
                controls.Add(nextId, control);
            }

            CustomGamePads[control.GamePadId].Controls = controls.Values.ToArray();

            SaveChanges();

            return control.Id;
        }

        public void DeleteControl(int gamePadId, int id)
        {
            CustomGamePads[gamePadId].Controls = GetGamePadControls(gamePadId).Where(x => x.Id != id).ToArray();

            SaveChanges();
        }

    }
}