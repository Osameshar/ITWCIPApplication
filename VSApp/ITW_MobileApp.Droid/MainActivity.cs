using System;
using Android.OS;
using Android.App;
using Android.Views;
using Android.Widget;
using System.Net.Http;
using ModernHttpClient;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using System.IO;

namespace ITW_MobileApp.Droid
{
    [Activity(MainLauncher = true, Theme = "@android:style/Theme.Black.NoTitleBar")]
    public class MainActivity : Activity
    {
        //Mobile Service Client reference
        private MobileServiceClient client;

        //Mobile Service sync table used to access data
        private IMobileServiceSyncTable<EmployeeItem> employeeSyncTable;
        private IMobileServiceSyncTable<EventItem> eventSyncTable;
        private IMobileServiceSyncTable<RecipientListItem> recipientListSyncTable;

        //Adapter to map the items list to the view
        private EmployeeItemAdapter employeeItemAdapter;
        private EventItemAdapter eventItemAdapter;
        private RecipientListItemAdapter recipientListItemAdapter;


        const string applicationURL = @"https://itw-mobileapp.azurewebsites.net";
        const string localDbFilename = "test2.db";

        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Login);

            CurrentPlatform.Init();

            // Create the Mobile Service Client instance, using the provided
            // Mobile Service URL 
            client = new MobileServiceClient(applicationURL, new NativeMessageHandler());
            await InitLocalStoreAsync();

            // Get the Mobile Service sync table instance to use
            employeeSyncTable = client.GetSyncTable<EmployeeItem>();
            eventSyncTable = client.GetSyncTable<EventItem>();
            recipientListSyncTable = client.GetSyncTable<RecipientListItem>();

            // Create an adapter to bind the items with the view
            employeeItemAdapter = new EmployeeItemAdapter(this, Resource.Layout.Row_List_To_Do);
            eventItemAdapter = new EventItemAdapter(this, Resource.Layout.Row_List_To_Do);
            recipientListItemAdapter = new RecipientListItemAdapter(this, Resource.Layout.Row_List_To_Do);
            
            //testing 
            //await employeeSyncTable.InsertAsync(makeSampleEmployeeItem());
            
            // Load the items from the Mobile Service
            OnRefreshItemsSelected();
            //await RefreshItemsFromTableAsync();

        }

        private static EmployeeItem makeSampleEmployeeItem()
        {
            return new EmployeeItem
            {
                Name = "Emp 5",
                Email = "Test Email",
                EmployeeID = 321321,
                Department = "IT",
                PrivledgeLevel = "User"
            };
        }


        private async Task InitLocalStoreAsync()
        {
            // new code to initialize the SQLite store
            string path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), localDbFilename);

            if (!File.Exists(path)) {
                File.Create(path).Dispose();
            }

            var store = new MobileServiceSQLiteStore(path);

            store.DefineTable<EmployeeItem>();
            store.DefineTable<EventItem>();
            store.DefineTable<RecipientListItem>();

            // Uses the default conflict handler, which fails on conflict
            await client.SyncContext.InitializeAsync(store);
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

        private async Task SyncAsync(bool pullData = false)
        {
            try
            {
                await client.SyncContext.PushAsync();

                if (pullData)
                {
                    await employeeSyncTable.PullAsync("allEmployeeItems", employeeSyncTable.CreateQuery());
                    await eventSyncTable.PullAsync("allEventItems", eventSyncTable.CreateQuery());
                    await recipientListSyncTable.PullAsync("allRecipientListItems", recipientListSyncTable.CreateQuery());
                }

            }
            catch (Java.Net.MalformedURLException)
            {
                CreateAndShowDialog(new Exception("There was an error creating the Mobile Service. Verify the URL"), "Error");
            }
            catch (MobileServicePushFailedException)
            {
                // Not reporting this exception. Assuming the app is offline for now
            }
            catch (Java.Net.UnknownHostException)
            {
                // Not reporting this exception. Assuming the app is offline for now
            }
            catch (Exception e)
            {
                CreateAndShowDialog(e, "Error");
            }
        }

        // Called when the refresh menu option is selected
        private async void OnRefreshItemsSelected()
        {
            await SyncAsync(pullData: true); // get changes from the mobile service
            await RefreshItemsFromTableAsync(); // refresh view using local database
        }

        //Refresh the list with the items in the local database
        private async Task RefreshItemsFromTableAsync()
        {
            try {
                // Get the items that weren't marked as completed and add them in the adapter
                //var list = await toDoTable.Where(item => item.Complete == false).ToListAsync();
                var employeeList = await employeeSyncTable.ToListAsync();
                var eventList = await eventSyncTable.ToListAsync();
                var recipientList = await recipientListSyncTable.ToListAsync();

                employeeItemAdapter.Clear();
                eventItemAdapter.Clear();
                recipientListItemAdapter.Clear();
                
                foreach (EmployeeItem currentEmployee in employeeList)
                    employeeItemAdapter.Add(currentEmployee);
                foreach (EventItem currentEvent in eventList)
                    eventItemAdapter.Add(currentEvent);
                foreach (RecipientListItem currentRecipientList in recipientList)
                    recipientListItemAdapter.Add(currentRecipientList);

            }
            catch (Exception e) {
                CreateAndShowDialog(e, "Error");
            }
        }
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
        private void CreateAndShowDialog(Exception exception, String title)
        {
            CreateAndShowDialog(exception.Message, title);
        }

        private void CreateAndShowDialog(string message, string title)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetMessage(message);
            builder.SetTitle(title);
            builder.Create().Show();
        }

    }
}