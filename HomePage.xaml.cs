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
    public sealed partial class HomePage : Page
    {

        public HomePage()
        {
            this.InitializeComponent();
            Loaded += HomePage_Loaded;
        }

        private void HomePage_Loaded(object sender, RoutedEventArgs e)
        {
            MainVM.Instance.IsActionInProgress = false;

            MainVM.Instance.HomeVM.Initialize();
        }

        private void CreateEventButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CreateEvent));
        }

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MainVM.Instance.EventVM.SelectedEvent = (sender as TextBlock).DataContext as Event;

            if (!MainVM.Instance.EventVM.SelectedEvent.IsAllDayEvent)
            {
                MainVM.Instance.EventVM.Session1 = MainVM.Instance.EventVM.SelectedEvent.Sessions[0];
                if (MainVM.Instance.EventVM.SelectedEvent.IsSecondSessionAvailable)
                    MainVM.Instance.EventVM.Session2 = MainVM.Instance.EventVM.SelectedEvent.Sessions[1];
            }


            this.Frame.Navigate(typeof(EditEvent));
        }

        private void UpcomingEvents_Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MainVM.Instance.EventVM.SelectedEvent = (sender as Grid).DataContext as Event;
            if (MainVM.Instance.AuthVM.CurrentUser.MyBookedEventIds != null && MainVM.Instance.AuthVM.CurrentUser.MyBookedEventIds.Contains(MainVM.Instance.EventVM.SelectedEvent.Id.ToString()))
            {
                this.Frame.Navigate(typeof(EditBooking));
            }
            else
            {
                this.Frame.Navigate(typeof(BookEvent));
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            //clear details and navigate to main page
            MainVM.Instance.AuthVM.CurrentUser = null;
            MainVM.Instance.HomeVM.MyEvents?.Clear();
            MainVM.Instance.HomeVM.OtherEvents?.Clear();
            MainVM.Instance.HomeVM.BookedEvents?.Clear();
            MainVM.Instance.EventVM.SelectedEvent = null;
            MainVM.Instance.AuthVM.Users?.Clear();
            this.Frame.Navigate(typeof(MainPage));
        }

        private void BookedEvents_Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MainVM.Instance.EventVM.SelectedEvent = (sender as Grid).DataContext as Event;
            this.Frame.Navigate(typeof(EditBooking));
        }

        private void MyBookingsButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(BookedEventPage));
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(UpdateDetailsPage));
        }
    }
}
