using Events.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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
    public sealed partial class UpdateDetailsPage : Page
    {
        public UpdateDetailsPage()
        {
            this.InitializeComponent();
        }

        private async void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            User existingconact = MainVM.Instance.Conn.Query<User>("select * from User where StudentId='" + MainVM.Instance.AuthVM.CurrentUser.StudentId + "'").FirstOrDefault();

            if (!string.IsNullOrEmpty(fname.Text))
            {
                existingconact.FirstName = fname.Text;
            }

            if (!string.IsNullOrEmpty(lname.Text))
            {
                existingconact.LastName = lname.Text;
            }

            if (!string.IsNullOrEmpty(pwd.Password))
            {
                existingconact.Password = pwd.Password;
            }


            try
            {
                MainVM.Instance.Conn.Update(existingconact);
                MainVM.Instance.AuthVM.CurrentUser = existingconact;
                await new MessageDialog("User details updated.").ShowAsync();
            }
            catch (Exception ex)
            {
                await new MessageDialog("Error occured. User details cannot be updated. Please try again later").ShowAsync();
            }
            this.Frame.Navigate(typeof(HomePage));
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }
    }
}
