namespace WPFGameEngine.WPF.GE.Serialization.Base
{
    public interface IObjectImporter<out T>
    {
        string PathToFolder { get; init; }

        IEnumerable<T> ImportObjects();
    }
}
