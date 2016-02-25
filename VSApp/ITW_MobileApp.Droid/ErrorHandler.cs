using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ITW_MobileApp.Droid
{
    class ErrorHandler
    {
        AlertDialog.Builder builder;

        public ErrorHandler(Context context)
        {
            builder = new AlertDialog.Builder(context);
        }
        private void CreateAndShowDialog(Exception exception, String title)
        {
            CreateAndShowDialog(exception.Message, title);
        }

        private void CreateAndShowDialog(string message, string title)
        {
            builder.SetMessage(message);
            builder.SetTitle(title);
            builder.Create().Show();
        }
    }

}