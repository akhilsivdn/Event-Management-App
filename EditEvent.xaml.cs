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
    public sealed partial class EditEvent : Page
    {

        public EditEvent()
        {
            this.InitializeComponent();
            this.DataContext = MainVM.Instance.EventVM;
            Loaded += EditEvent_Loaded;
        }

        private void EditEvent_Loaded(object sender, RoutedEventArgs e)
        {
            (this.DataContext as EventVM).IsAllDayEvent = (this.DataContext as EventVM).SelectedEvent.IsAllDayEvent;
            (this.DataContext as EventVM).IsFreeEvent = (this.DataContext as EventVM).SelectedEvent.IsFreeEvent;
            (this.DataContext as EventVM).IsSecondSessionAvailable = (this.DataContext as EventVM).SelectedEvent.IsSecondSessionAvailable;
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            MainVM.Instance.IsActionInProgress = true;

            int retValue = MainVM.Instance.EventVM.UpdateEvent();

            this.Frame.Navigate(typeof(HomePage));
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
