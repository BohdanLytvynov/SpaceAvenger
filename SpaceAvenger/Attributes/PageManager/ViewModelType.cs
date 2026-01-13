using System;

namespace SpaceAvenger.Attributes.PageManager
{
    internal enum ViewModelUsage : byte
    { 
        Page = 1, Window
    }
    /// <summary>
    ///Sets the View type (Page or Window) for mapping with ViewModel.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    internal class ViewModelType : Attribute
    {
        public ViewModelUsage Usage { get; }

        public ViewModelType(ViewModelUsage usage)
        {
            Usage = usage;
        }
    }
}
