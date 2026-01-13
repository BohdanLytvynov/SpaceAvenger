using System;

namespace SpaceAvenger.Attributes.PageManager
{
    /// <summary>
    /// Used to Ignore the detection of the class using Reflexion, For exmaple used to Ignore MainWindowViewModel 
    /// auto-mapping with it's View
    /// </summary>
    [AttributeUsage( AttributeTargets.Class)]
    internal class ReflexionDetectionIgnore : Attribute
    {
    }
}
