using Events.ViewModel;
using LightBuzz.SMTP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.ApplicationModel.Email;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
    public sealed partial class BookEvent : Page
    {
        private Event currEvent;

        public BookEvent()
        {
            this.InitializeComponent();
            this.Loaded += BookEvent_Loaded;
        }

        private void BookEvent_Loaded(object sender, RoutedEventArgs e)
        {
            (this.DataContext as EventVM).Session1 = null;
            (this.DataContext as EventVM).Session2 = null;
            (this.DataContext as EventVM).IsSecondSessionAvailable = (this.DataContext as EventVM).SelectedEvent.IsSecondSessionAvailable;
            if (!(this.DataContext as EventVM).SelectedEvent.IsAllDayEvent)
            {
                (this.DataContext as EventVM).Session1 = (this.DataContext as EventVM).SelectedEvent.Sessions[0];
                if ((this.DataContext as EventVM).IsSecondSessionAvailable)
                {
                    (this.DataContext as EventVM).Session2 = (this.DataContext as EventVM).SelectedEvent.Sessions[1];
                }
            }

            (this.DataContext as EventVM).PopulateDropDown();
            (this.DataContext as EventVM).IsPromoCodeAvaialble = !string.IsNullOrEmpty((this.DataContext as EventVM).SelectedEvent.PromoCode);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private async void BookingButton_Click(object sender, RoutedEventArgs e)
        {
            MainVM.Instance.IsActionInProgress = true;

            if ((this.DataContext as EventVM).SelectedEvent.IsFreeEvent)
            {

                var regex = new Regex(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?");
                bool isValidEmail = regex.IsMatch(email.Text);

                if (!isValidEmail)
                {
                    await new MessageDialog("Please enter a valid email ID to continue..").ShowAsync();
                    MainVM.Instance.IsActionInProgress = false;
                    return;
                }


                UpdateSeats();


                int returnValue = await MainVM.Instance.HomeVM.AddBooking(currEvent, email.Text);

                await new MessageDialog("Your booking is confirmed. Email confirmaion will be sent to " + email.Text + ". Thanks for booking with us. Enjoy the event !").ShowAsync();

                this.Frame.Navigate(typeof(HomePage));
            }
            else
            {

                UpdateSeats();

                MainVM.Instance.HomeVM.CalculatePrice();

                this.Frame.Navigate(typeof(ConfirmBooking));
            }

            MainVM.Instance.IsActionInProgress = false;
        }

        private void UpdateSeats()
        {
            var eventVM = (this.DataContext as EventVM);

            if (eventVM.SelectedEvent.IsAllDayEvent)
            {
                eventVM.SelectedEvent.AllDayEventRemainingSeats = eventVM.SelectedEvent.AllDayEventRemainingSeats - eventVM.DropDownSelectedValue;
            }

            else
            {
                if (eventVM.IsSession1Selected)
                {
                    eventVM.SelectedEvent.Sessions[0].RemainingSeats = eventVM.SelectedEvent.Sessions[0].RemainingSeats - eventVM.DropDownSelectedValue_Session1;
                }
                else
                {
                    eventVM.SelectedEvent.Sessions[1].RemainingSeats = eventVM.SelectedEvent.Sessions[1].RemainingSeats - eventVM.DropDownSelectedValue_Session2;
                }
            }

            var query = MainVM.Instance.Conn.Table<Event>();

            currEvent = MainVM.Instance.Conn.Query<Event>("select * from Event where Id='" + (this.DataContext as EventVM).SelectedEvent.Id + "'").FirstOrDefault();

            var sess1String = string.Empty;
            var sess2String = string.Empty;

            if (eventVM.SelectedEvent.IsAllDayEvent)
            {
                currEvent.AllDayEventRemainingSeats = eventVM.SelectedEvent.AllDayEventRemainingSeats;
            }
            else
            {
                if (eventVM.IsSession1Selected)
                {
                    eventVM.Session1.RemainingSeats = eventVM.SelectedEvent.Sessions[0].RemainingSeats;
                    sess1String = "start-" + eventVM.Session1.StartTime + ",end-" + eventVM.Session1.EndTime + ",tseats-" + eventVM.Session1.TotalSeats + ",rseats-" + eventVM.Session1.RemainingSeats + ",duration-" + eventVM.Session1.Duration + "#";
                }
                else
                {
                    eventVM.Session2.RemainingSeats = eventVM.SelectedEvent.Sessions[1].RemainingSeats;
                    sess2String = "start-" + eventVM.Session2.StartTime + ",end-" + eventVM.Session2.EndTime + ",tseats-" + eventVM.Session2.TotalSeats + ",rseats-" + eventVM.Session2.RemainingSeats + ",duration-" + eventVM.Session2.Duration + "#";
                }
            }

            if (!eventVM.SelectedEvent.IsAllDayEvent)
            {

                var sessionSplit = eventVM.SelectedEvent.SessionString.Split('#').Where(s => !string.IsNullOrEmpty(s)).ToList();


                if (eventVM.IsSession1Selected)
                {
                    sessionSplit[0] = sess1String;
                }
                else
                {
                    sessionSplit[1] = sess2String;
                }

                if (!string.IsNullOrEmpty(sessionSplit[0]) && !sessionSplit[0].Contains("#"))
                    sessionSplit[0] = sessionSplit[0] + "#";
                if (sessionSplit.Count > 1 && !string.IsNullOrEmpty(sessionSplit[1]) && !sessionSplit[1].Contains("#"))
                    sessionSplit[1] = sessionSplit[1] + "#";


                currEvent.SessionString = currEvent.IsSecondSessionAvailable ? sessionSplit[0] + sessionSplit[1] : sess1String;
            }


            MainVM.Instance.Conn.Update(currEvent);

            query = MainVM.Instance.Conn.Table<Event>();
        }

        private void session1Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        private void session2Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }
    }
}
