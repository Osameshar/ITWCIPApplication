using System;

using Android.App;
using Android.Content;
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

        public void CreateAndShowDialog(string message, string title)
        {
            builder.SetMessage(message);
            builder.SetTitle(title);
            builder.SetCancelable(true);
            builder.SetNeutralButton("Ok", (senderAlert, args) => {

            }); 
           
            builder.Create().Show();
        }
    }

}