using Events.ViewModel;
using SQLite.Net;
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
    public sealed partial class LoginPage : Page
    {
        private TableQuery<User> query;

        public LoginPage()
        {
            this.InitializeComponent();

            studId.Text = "5867098";
            password.Password = "111";

            query = MainVM.Instance.Conn.Table<User>();

            foreach (var item in query)
            {
                MainVM.Instance.AuthVM.Users.Add(new User()
                {
                    EmailId = item.EmailId,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Password = item.Password,
                    StudentId = item.StudentId,
                    MyBookedEventIds = item.MyBookedEventIds,
                    SeatsReserved = item.SeatsReserved,
                    BookedSession = item.BookedSession
                });
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainVM.Instance.IsActionInProgress = true;
            User currentUser = MainVM.Instance.AuthVM.Users.Where(s => s.StudentId.Equals(studId.Text)).FirstOrDefault();

            if (currentUser != null)
            {
                if (currentUser.Password.Equals(password.Password))
                {
                    MainVM.Instance.AuthVM.CurrentUser = currentUser;
                    this.Frame.Navigate(typeof(HomePage));
                }
                else
                {
                    new MessageDialog("Sorry. Invalid user credentials - Login failed ! create a new account (or) check username/password combination").ShowAsync();
                    MainVM.Instance.IsActionInProgress = false;
                }
            }
            else
            {
                new MessageDialog("Sorry. Invalid user credentials - Login failed ! create a new account (or) check username/password combination").ShowAsync();
                MainVM.Instance.IsActionInProgress = false;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();

        }
    }
}
