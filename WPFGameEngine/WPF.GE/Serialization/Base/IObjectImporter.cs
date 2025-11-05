namespace WPFGameEngine.WPF.GE.Serialization.Base
{
    public interface IObjectImporter<out T>
    {
        string PathToFolder { get; set; }

        IEnumerable<T> ImportObjects();
    }
}
