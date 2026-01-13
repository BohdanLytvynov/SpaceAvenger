using System;

namespace SpaceAvenger.Attributes.PageManager
{
    /// <summary>
    /// Holds name of the ViewModel, used during the auto-mapping of the View and ViewModel
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    internal class ViewModelName : Attribute
    {
        /// <summary>
        /// Name of the ViewModel
        /// </summary>
        public string Name { get; }

        public ViewModelName(string name)
        {
            Name = name;
        }
    }
}
