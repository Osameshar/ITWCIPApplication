using System;
using Android.App;
using Android.Views;
using Android.Widget;
using ModernHttpClient;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using System.IO;


namespace ITW_MobileApp.Droid
{
    public class DatabaseConnection
    {
        //Mobile Service Client reference
        private MobileServiceClient client;

        //Mobile Service sync table used to access data
        private IMobileServiceSyncTable<EmployeeItem> employeeSyncTable;
        private IMobileServiceSyncTable<EventItem> eventSyncTable;
        private IMobileServiceSyncTable<RecipientListItem> recipientListSyncTable;



        const string applicationURL = @"https://itw-mobileapp.azurewebsites.net";
        const string localDbFilename = "test2.db";

        public DatabaseConnection()
        {
            CurrentPlatform.Init();

            // Create the Mobile Service Client instance, using the provided
            // Mobile Service URL 
            client = new MobileServiceClient(applicationURL, new NativeMessageHandler());
        }

        public async Task InitLocalDBSyncTables()
        {
            await InitLocalStoreAsync();

            // Get the Mobile Service sync table instance to use
            employeeSyncTable = client.GetSyncTable<EmployeeItem>();
            eventSyncTable = client.GetSyncTable<EventItem>();
            recipientListSyncTable = client.GetSyncTable<RecipientListItem>();

            // Load the items from the Mobile Service
            await SyncAsync(pullData: true);
        }

        private async Task InitLocalStoreAsync()
        {
            // new code to initialize the SQLite store
            string path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), localDbFilename);

            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
            }

            var store = new MobileServiceSQLiteStore(path);

            store.DefineTable<EmployeeItem>();
            store.DefineTable<EventItem>();
            store.DefineTable<RecipientListItem>();

            // Uses the default conflict handler, which fails on conflict
            await client.SyncContext.InitializeAsync(store);
        }

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
                System.Diagnostics.Debug.WriteLine("There was an error creating the Mobile Service. Verify the URL");
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
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }

        // Called when the refresh menu option is selected
        public async void OnRefreshItemsSelected(BaseAdapter adapter)
        {
            await SyncAsync(pullData: true); // get changes from the mobile service
            await RefreshItemsFromTableAsync(adapter); // refresh view using local database
        }

        //Refresh the list with the items in the local database
        private async Task RefreshItemsFromTableAsync(BaseAdapter adapter)
        {
            try
            {
                // Get the items that weren't marked as completed and add them in the adapter
                //var list = await toDoTable.Where(item => item.Complete == false).ToListAsync();

                if (dataObj.GetType() == typeof(EmployeeItem) )
                {
                    var employeeList = await employeeSyncTable.ToListAsync();
                    ((EmployeeItemAdapter)adapter).Clear();
                    foreach (EmployeeItem currentEmployee in employeeList)
                       ((EmployeeItemAdapter)adapter).Add(currentEmployee);
                }
                else if (dataObj.GetType() == typeof(EventItem))
                {
                    var eventList = await eventSyncTable.ToListAsync();
                    ((EventItemAdapter)adapter).Clear();
                    foreach (EventItem currentEvent in eventList)
                        ((EventItemAdapter)adapter).Add(currentEvent);
                }
                else if (dataObj.GetType() == typeof(EventItem))
                {
                    var recipientList = await recipientListSyncTable.ToListAsync();
                    ((RecipientListItemAdapter)adapter).Clear();
                    foreach (RecipientListItem currentRecipientList in recipientList)
                       ((RecipientListItemAdapter)adapter).Add(currentRecipientList);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }


    }
}