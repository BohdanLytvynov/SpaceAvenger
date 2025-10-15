namespace ViewModelBaseLibDotNetCore.Helpers
{
    public static class ValidationHelper
    {
        public static bool TextIsEmpty(string value, out string error)
        {
            error = string.Empty;

            if (string.IsNullOrEmpty(value))
            {
                error = "Empty Input!";
                return true; 
            }

            return false;
        }
    }
}
