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
    public sealed partial class CreateEvent : Page
    {
        public CreateEvent()
        {
            this.InitializeComponent();
            Loaded += CreateEvent_Loaded;
        }

        private void CreateEvent_Loaded(object sender, RoutedEventArgs e)
        {
            (this.DataContext as EventVM).SelectedEvent = new Event()
            {
                IsAllDayEvent = true,
            };

            (this.DataContext as EventVM).Session1 = new Session();

            (this.DataContext as EventVM).Session2 = new Session();
            DatePicker.Date = DateTime.Now;
        }

        private async void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            int retValue = (this.DataContext as EventVM).InsertEvent();

            if (retValue == 0)
            {
                await new MessageDialog("Enter correct event details.").ShowAsync();
            }
            else
            {
                this.Frame.Navigate(typeof(HomePage));
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            (this.DataContext as EventVM).IsSecondSessionAvailable = true;
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            (this.DataContext as EventVM).IsSecondSessionAvailable = false;
        }
    }
}
