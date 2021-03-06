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
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
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

        public GamePad SaveGamePad(GamePad gamePad)
        {
            var gamePadId = gamePad.Id;
            if (gamePadId > 0)
            {
                CustomGamePads[gamePad.Id] = gamePad;
            }
            else
            {
                gamePad.Id = CustomGamePads.Count > 0
                ? CustomGamePads.Max(x => x.Key) + 1
                : 1;

                CustomGamePads.Add(gamePadId, gamePad);
            }

            SaveChanges();

            return gamePad;
        }

        public GamePad[] GetGamePadList()
        {
            return CustomGamePads.Values.ToArray();
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

        public int CreateOrUpdateControl(int gamePadId, Control control)
        {
            var controls = GetGamePadControls(gamePadId).ToDictionary(x => x.Id);

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

            CustomGamePads[gamePadId].Controls = controls.Values.ToArray();

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