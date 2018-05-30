using Events.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Events
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateAccountPage : Page
    {
        public CreateAccountPage()
        {
            this.InitializeComponent();
            Loaded += CreateAccountPage_Loaded;
        }

        private void CreateAccountPage_Loaded(object sender, RoutedEventArgs e)
        {
            (this.DataContext as AuthenticationVM).ResetFields();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            AuthenticationVM dc = (this.DataContext as AuthenticationVM);

            dc.IsPasswordMismatch = !ConfirmTextBox.Password.Equals(pass.Password);
            MainVM.Instance.IsActionInProgress = true;

            dc.InsertUser();
            if (dc.InvalidFirstName || dc.InvalidLastName || dc.InvalidEmailID || dc.InvalidPassword || dc.IsPasswordMismatch || dc.InvalidStudentID)
            {
                MainVM.Instance.IsActionInProgress = false;
                return;
            }
            this.Frame.Navigate(typeof(HomePage));
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }
    }
}
