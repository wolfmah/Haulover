using System;
using Windows.Data.Xml.Dom;
using Windows.UI.Core;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Haulover
{
    /// <summary>
    /// .
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            // Get a toast XML template
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText04);

            // Fill in the text elements
            XmlNodeList stringElements = toastXml.GetElementsByTagName("text");
            for (int i = 0; i < stringElements.Length; i++)
            {
                stringElements[i].AppendChild(toastXml.CreateTextNode("Line " + i));
            }

            // Specify the absolute path to an image
            String imagePath = "file:///C:\\Users\\maharbec\\source\\repos\\Haulover\\Haulover\\Assets\\Haulover-app-icon.png";
            XmlNodeList imageElements = toastXml.GetElementsByTagName("image");
            imageElements[0].Attributes.GetNamedItem("src").NodeValue = imagePath;

            ToastNotification toast = new ToastNotification(toastXml);
            toast.Activated += ToastActivatedAsync;
            toast.Dismissed += ToastDismissed;
            toast.Failed += ToastFailed;

            // Show the toast. Be sure to specify the AppUserModelId on your application's shortcut!
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        private void ToastActivatedAsync(ToastNotification sender, object e)
        {
            DisplayToastAction("The user activated the toast.");
        }

        private void ToastDismissed(ToastNotification sender, ToastDismissedEventArgs e)
        {
            String outputText = "";

            switch (e.Reason)
            {
                case ToastDismissalReason.ApplicationHidden:
                    outputText = "The app hid the toast using ToastNotifier.Hide";
                    break;

                case ToastDismissalReason.UserCanceled:
                    outputText = "The user dismissed the toast";
                    break;

                case ToastDismissalReason.TimedOut:
                    outputText = "The toast has timed out";
                    break;
            }

            DisplayToastAction(outputText);
        }

        private void ToastFailed(ToastNotification sender, ToastFailedEventArgs e)
        {
            DisplayToastAction("The toast encountered an error.");
        }

        private async void DisplayToastAction(String toastAction)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                txtToastAction.Text = toastAction;
            });
        }
    }
}
