using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using tenantApp.chat;
using tenantApp.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(MessageViewCell), typeof(MessageRenderer))]
namespace tenantApp.Droid
{
    public class MessageRenderer : ViewCellRenderer
    {
        //protected override View GetCellCore(Cell item, View convertView, ViewGroup parent, Context context)
        //{
        //    var inflatorservice = (LayoutInflater)Forms.Context.GetSystemService(Android.Content.Context.LayoutInflaterService);
        //    var dataContext = item.BindingContext as EventViewModel;

        //    var textMsgVm = dataContext as TextMessageViewModel;
        //    if (textMsgVm != null)
        //    {
                
        //        var template = (LinearLayout)inflatorservice.Inflate(textMsgVm.IsMine ? Resource.Layout.message_item_owner : Resource.Layout.message_item_opponent, null, false);
        //        //template.FindViewById<TextView>(Resource.Id.timestamp).Text = textMsgVm.Timestamp.ToString("HH:mm");
        //        template.FindViewById<TextView>(Resource.Id.nick).Text = textMsgVm.IsMine ? "Me:" : textMsgVm.AuthorName + ":";
        //        template.FindViewById<TextView>(Resource.Id.message).Text = textMsgVm.Text;
        //        return template;
              
        //    }

        //    return base.GetCellCore(item, convertView, parent, context);
        //}

        //private Bitmap GetImageBitmapFromUrl(string url)
        //{
        //    Bitmap imageBitmap = null;
        //    using (var webClient = new WebClient())
        //    {
        //        var imageBytes = webClient.DownloadData(url);
        //        if (imageBytes != null && imageBytes.Length > 0)
        //        {
        //            imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
        //        }
        //    }
        //    return imageBitmap;
        //}


        //protected override void OnCellPropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    base.OnCellPropertyChanged(sender, e);
        //}
    }
}