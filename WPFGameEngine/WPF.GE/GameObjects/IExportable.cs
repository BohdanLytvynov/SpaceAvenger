namespace WPFGameEngine.WPF.GE.GameObjects
{
    public interface IExportable
    {
        /// <summary>
        /// Use for editor only not for games
        /// </summary>
        public bool IsExported { get; set; }
    }
}
