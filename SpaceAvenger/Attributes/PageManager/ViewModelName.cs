using System;

namespace SpaceAvenger.Attributes.PageManager
{
    /// <summary>
    /// Holds name of the ViewModel, used during the auto-mapping of the View and ViewModel.
    /// Mapper will use this name during mapping.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    internal class ViewModelName : Attribute
    {
        /// <summary>
        /// Name of the ViewModel that will be used for mapping
        /// </summary>
        public string Name { get; }

        public ViewModelName(string name)
        {
            Name = name;
        }
    }
}
