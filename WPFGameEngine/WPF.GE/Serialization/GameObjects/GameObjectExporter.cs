using System.IO;
using System.Text.Json;
using WPFGameEngine.WPF.GE.Dto.GameObjects;
using WPFGameEngine.WPF.GE.GameObjects;

namespace WPFGameEngine.WPF.GE.Serialization.GameObjects
{
    public class GameObjectExporter : IGameObjectExporter
    {
        public void Export(IGameObject inpObj, string path, Exception exception)
        {
            try
            {
                GameObjectDto dto = inpObj.ToDto();

                string str = JsonSerializer.Serialize(dto, new JsonSerializerOptions() { WriteIndented = true });

                string pathToFile = path + Path.DirectorySeparatorChar + dto.Name + ".json";

                using (var fs = File.Create(pathToFile))
                {}

                File.WriteAllText(pathToFile, str);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            
        }
    }
}
