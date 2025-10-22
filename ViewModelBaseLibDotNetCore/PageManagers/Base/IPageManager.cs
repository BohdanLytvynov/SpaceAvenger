using System.Windows.Controls;
using ViewModelBaseLibDotNetCore.PageManagers;

namespace ViewModelBaseLibDotNetCore.PageManager.Base
{
    public interface IPageManagerService<TFrameType>
        where TFrameType : struct, Enum
    {
        public EventHandler<PageManagerEventArgs<TFrameType>>? OnSwitchScreenMethodInvoked { get; set; }

        public void AddPages(IEnumerable<KeyValuePair<string, Page?>> pages);

        public void AddPage(string key, Page? page);

        public void RemovePage(string pageKey);

        public Page? GetPage(string pageKey);

        public IEnumerable<string> GetAllKeys();

        public void SwitchPage(string pageKey, TFrameType frame = default);
    }
}
