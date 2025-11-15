using System.IO;
using WPFGameEngine.WPF.GE.Dto.GameObjects;
using System.Text.Json;
using WPFGameEngine.WPF.GE.Serialization.Converters;

namespace WPFGameEngine.WPF.GE.Serialization.GameObjects
{
    public class GameObjectImporter : IGameObjectImporter
    {
        public string PathToFolder { get; set; }

        public GameObjectImporter() : this(null)
        {
            
        }

        public GameObjectImporter(string pathToFolder)
        {
            PathToFolder = pathToFolder;
        }

        public IEnumerable<GameObjectDto> ImportObjects()
        {
            var options = new JsonSerializerOptions() { WriteIndented = true };
            options.Converters.Add(new JsonVector2Converter());
            options.Converters.Add(new JsonSizeConverter());

            var files = Directory.GetFiles(PathToFolder, "*.json");

            foreach (var file in files)
            { 
                var str = File.ReadAllText(file);

                yield return JsonSerializer.Deserialize<GameObjectDto>(str, options);
            }
        }
    }
}
