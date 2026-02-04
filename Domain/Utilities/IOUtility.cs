namespace Domain.Utilities
{
    /// <summary>
    /// Helper for the additional IO functions
    /// </summary>
    public static class IOUtility
    {
        /// <summary>
        /// Checks if directory exists
        /// </summary>
        /// <param name="path">Path to the Directory</param>
        /// <returns>True - directory exists, False - directory not exists</returns>
        public static bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }
        /// <summary>
        /// Checks if file exists
        /// </summary>
        /// <param name="path">Path to the file</param>
        /// <returns>True - file exists, False - file not exists</returns>
        public static bool FileExists(string path)
        { 
            return File.Exists(path);
        }
        /// <summary>
        /// Creates Directory if not exists
        /// </summary>
        /// <param name="path">Path where you need to create the Directory</param>
        public static void CreateDirectoryIfNotExists(string path)
        {
            if (DirectoryExists(path)) return;

            Directory.CreateDirectory(path);
        }
        /// <summary>
        /// Creates File if not exists
        /// </summary>
        /// <param name="path">Path where you need to create the File</param>
        public static void CreateFileIfNotExists(string path)
        {
            if (FileExists(path)) return;

            using (var file = File.Create(path))
            {}
        }
    }
}
