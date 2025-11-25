namespace Domain.Utilities
{
    public class IOUtility
    {
        public static bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        public static bool FileExists(string path)
        { 
            return File.Exists(path);
        }

        public static void CreateDirectoryIfNotExists(string path)
        {
            if (DirectoryExists(path)) return;

            Directory.CreateDirectory(path);
        }

        public static void CreateFileIfNotExists(string path)
        {
            if (FileExists(path)) return;

            using (var file = File.Create(path))
            {}
        }
    }
}
