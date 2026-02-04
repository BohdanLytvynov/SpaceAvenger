using System;
using ViewModelBaseLibDotNetCore.Commands;

namespace SpaceAvenger.CommonCommands
{
    /// <summary>
    /// Base command for Exit from the App
    /// </summary>
    public class ExitCommand : Command
    {
        public override bool CanExecute(object? parameter) => true;

        public override void Execute(object? parameter)
        {
            //Exit an app, return 0
            Environment.Exit(0);
        }
    }
}
