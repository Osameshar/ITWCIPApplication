using System;
using Android.OS;
using Android.App;
using Android.Widget;
using Android.Content;

namespace ITW_MobileApp.Droid
{
    [Activity(Theme = "@android:style/Theme.Black.NoTitleBar")]
    public class LoginActivity : Activity
    {
        //Adapter to map the items list to the view
        private EmployeeItemAdapter employeeItemAdapter;
        private EventItemAdapter eventItemAdapter;
        private RecipientListItemAdapter recipientListItemAdapter;

        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Login);

            // Create an adapter to bind the items with the view
            employeeItemAdapter = new EmployeeItemAdapter(this, Resource.Layout.Row_List_To_Do);
            eventItemAdapter = new EventItemAdapter(this, Resource.Layout.Row_List_To_Do);
            recipientListItemAdapter = new RecipientListItemAdapter(this, Resource.Layout.Row_List_To_Do);

            IoC.Dbconnect.OnRefreshItemsSelected(employeeItemAdapter);

            Button loginButton = FindViewById<Button>(Resource.Id.loginBtn);

            //Login Button sends us to the Main View. THIS WILL NEED TO BE CHANGED FOR AUTHENTICATION.
            loginButton.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(LoginActivity));
                StartActivity(intent);
            };

        }



        /*
                //Initializes the activity menu
                public override bool OnCreateOptionsMenu(IMenu menu)
                {
                    MenuInflater.Inflate(Resource.Menu.activity_main, menu);
                    return true;
                }

                //Select an option from the menu
                public override bool OnOptionsItemSelected(IMenuItem item)
                {
                    if (item.ItemId == Resource.Id.menu_refresh) {
                        item.SetEnabled(false);

                        OnRefreshItemsSelected();

                        item.SetEnabled(true);
                    }
                    return true;
                }*/

        /*
                public async Task CheckItem(ToDoItem item)
                {
                    if (client == null) {
                        return;
                    }

                    // Set the item as completed and update it in the table
                    item.Complete = true;
                    try {
                        await toDoTable.UpdateAsync(item); // update the new item in the local database
                        await SyncAsync(); // send changes to the mobile service

                        if (item.Complete)
                            adapter.Remove(item);

                    }
                    catch (Exception e) {
                        CreateAndShowDialog(e, "Error");
                    }
                }

                [Java.Interop.Export()]
                public async void AddItem(View view)
                {
                    if (client == null || string.IsNullOrWhiteSpace(textNewToDo.Text)) {
                        return;
                    }

                    // Create a new item
                    var item = new ToDoItem {
                        Text = textNewToDo.Text,
                        Complete = false
                    };

                    try {
                        await toDoTable.InsertAsync(item); // insert the new item into the local database
                        await SyncAsync(); // send changes to the mobile service

                        if (!item.Complete) {
                            adapter.Add(item);
                        }
                    }
                    catch (Exception e) {
                        CreateAndShowDialog(e, "Error");
                    }

                    textNewToDo.Text = "";
                }
        */
    }
}