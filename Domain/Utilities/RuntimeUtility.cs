namespace Domain.Utilities
{
    public static class RuntimeUtility
    {
        public static string GetPathToExe()
        { 
            return AppDomain.CurrentDomain.BaseDirectory;
        }
    }
}
