using SpaceAvenger.Attributes.PageManager;
using System.Windows.Input;
using ViewModelBaseLibDotNetCore.Commands;

namespace SpaceAvenger.ViewModels.PagesVM
{
    [ViewModelType(ViewModelUsage.Page)]
    internal class ChooseSubFaction_ViewModel
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Commands
        public ICommand OnNextButtonPressed { get; }
        public ICommand OnBackButtonPressed { get; }
        #endregion

        #region Ctor
        public ChooseSubFaction_ViewModel()
        {
            #region Init Commands
            OnNextButtonPressed = new Command(
                OnNextButtonPressedExecute,
                CanOnNextButtonPressedExecute
                );

            OnBackButtonPressed = new Command(
                OnBackButtonPressedExecute,
                CanOnBackButtonPressedExecute
                );
            #endregion
        }
        #endregion

        #region Methods

        #region On Next Button Pressed
        private bool CanOnNextButtonPressedExecute(object p)
        {
            return true;
        }

        private void OnNextButtonPressedExecute(object p)
        { 
            
        }
        #endregion

        #region On Back Button Pressed
        private bool CanOnBackButtonPressedExecute(object p)
            => true;

        private void OnBackButtonPressedExecute(object p)
        { 
            
        }
        #endregion

        #endregion
    }
}
