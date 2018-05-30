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
    public sealed partial class EditBooking : Page
    {
        public EditBooking()
        {
            this.InitializeComponent();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainVM.Instance.HomeVM.Initialize();
            this.Frame.GoBack();
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            (this.DataContext as EventVM).DeleteBooking();
            await new MessageDialog("Booking for the event deleted.").ShowAsync();
            this.Frame.Navigate(typeof(HomePage));
        }
    }
}
