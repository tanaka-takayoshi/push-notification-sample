using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.PushNotifications;
using Windows.UI.Core;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 空白ページのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace Push_Sample
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private MobileServiceCollectionView<DetailMessage> messages;

        private IMobileServiceTable<DetailMessage> messageTable;

        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// このページがフレームに表示されるときに呼び出されます。
        /// </summary>
        /// <param name="e">このページにどのように到達したかを説明するイベント データ。Parameter 
        /// プロパティは、通常、ページを構成するために使用します。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            messageTable = ((App)App.Current).MobileService.GetTable<DetailMessage>();
            ((App)App.Current).CurrentChannel.PushNotificationReceived += CurrentChannel_PushNotificationReceived;
            RefreshMessages();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ((App)App.Current).CurrentChannel.PushNotificationReceived -= CurrentChannel_PushNotificationReceived;
        }
       
        async void CurrentChannel_PushNotificationReceived(Windows.Networking.PushNotifications.PushNotificationChannel sender, Windows.Networking.PushNotifications.PushNotificationReceivedEventArgs args)
        {
            if (args.NotificationType == PushNotificationType.Raw)
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    var dlg = new MessageDialog(args.RawNotification.Content, "直接通知");
                    await dlg.ShowAsync();
                });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonSend_Click(object sender, RoutedEventArgs e)
        {
            var message = new DetailMessage
            {
                Text = TextInput.Text,
                Detail = DetailInput.Text,
                Channel = ((App)App.Current).CurrentChannel.Uri
            };
            SendMessage(message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        private async void SendMessage(DetailMessage message)
        {
            await messageTable.InsertAsync(message);
            messages.Add(message);
        }

        /// <summary>
        /// 
        /// </summary>
        private void RefreshMessages()
        {
            messages = messageTable.ToCollectionView();
            ListItems.ItemsSource = messages;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            foreach (var msg in messages)
            {
                await messageTable.DeleteAsync(msg);
            }
            messages.Clear();
        }
    }

    public class DetailMessage
    {
        public int Id { get; set; }

        [DataMember(Name = "text")]
        public string Text { get; set; }

        [DataMember(Name = "detail")]
        public string Detail { get; set; }

        [DataMember(Name = "channel")]
        public string Channel { get; set; }

    }
}
