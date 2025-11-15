using System.IO;
using System.Text.Json;
using WPFGameEngine.WPF.GE.Dto.GameObjects;
using WPFGameEngine.WPF.GE.GameObjects;
using WPFGameEngine.WPF.GE.Serialization.Converters;

namespace WPFGameEngine.WPF.GE.Serialization.GameObjects
{
    public class GameObjectExporter : IGameObjectExporter
    {
        public void Export(IGameObject inpObj, string path, Exception exception)
        {
            try
            {
                GameObjectDto dto = inpObj.ToDto();

                var options = new JsonSerializerOptions()
                { WriteIndented = true };
                options.Converters.Add(new JsonVector2Converter());
                options.Converters.Add(new JsonSizeConverter());

                string str = JsonSerializer.Serialize(dto, options);

                string pathToFile = path + Path.DirectorySeparatorChar + dto.ObjectName + ".json";
                if (!File.Exists(pathToFile)) 
                {
                    using (var fs = File.Create(pathToFile))
                    { }
                }

                File.WriteAllText(pathToFile, str);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            
        }
    }
}
