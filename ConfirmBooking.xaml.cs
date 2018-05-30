using Events.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
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
    public sealed partial class ConfirmBooking : Page
    {
        public ConfirmBooking()
        {
            this.InitializeComponent();
        }

        private async void BookingButton_Click(object sender, RoutedEventArgs e)
        {
            MainVM.Instance.IsActionInProgress = true;

            if (cardno.Text.Length == 19 && cvv.Text.Length == 3 && doe.Text.Length == 5)
            {
                int returnValue = await (this.DataContext as HomePageVM).AddBooking(MainVM.Instance.EventVM.SelectedEvent, email.Text);

                await new MessageDialog("Your booking is confirmed. Email confirmaion will be sent to " + email.Text + ". Thanks for booking with us. Enjoy the event !").ShowAsync();

                this.Frame.Navigate(typeof(HomePage));
            }
            else
            {
                await new MessageDialog("Enter correct card details..").ShowAsync();
            }
            MainVM.Instance.IsActionInProgress = false;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = (sender as TextBox);
            if (textBox.Text.Length == 4 || textBox.Text.Length == 9 || textBox.Text.Length == 14)
            {
                textBox.Text = textBox.Text + "-";
                textBox.SelectionStart = textBox.Text.Length;
            }
        }

        private void TextBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = (sender as TextBox);
            if (textBox.Text.Length == 2)
            {
                textBox.Text = textBox.Text + "/";
                textBox.SelectionStart = textBox.Text.Length;
            }
        }

        private async void promoValidate_Click(object sender, RoutedEventArgs e)
        {
            if (promo.Text.Equals((MainVM.Instance.EventVM.SelectedEvent.PromoCode)))
            {
                MainVM.Instance.HomeVM.GetDiscount();
            }
            else
            {
                await new MessageDialog("Invalid promocode").ShowAsync();
            }
        }

        private void HandleKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (!((e.Key >= VirtualKey.Number0 && e.Key <= VirtualKey.Number9) || (e.Key >= VirtualKey.NumberPad0 && e.Key <= VirtualKey.NumberPad9)) && (e.Key != VirtualKey.Shift))
                e.Handled = true;
        }
    }
}
