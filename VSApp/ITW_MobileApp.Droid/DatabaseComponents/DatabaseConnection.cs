using System;
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
        const string localDbFilename = "itwlocalstore.db";

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

           // if (!File.Exists(path))
           // {
                File.Create(path).Dispose();
           // }

            var store = new MobileServiceSQLiteStore(path);

            store.DefineTable<EmployeeItem>();
            store.DefineTable<EventItem>();
            store.DefineTable<RecipientListItem>();

            // Uses the default conflict handler, which fails on conflict
            await client.SyncContext.InitializeAsync(store);
        }

        public async Task SyncAsync(bool pullData = false)
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

        public IMobileServiceSyncTable<EmployeeItem> getEmployeeSyncTable()
        {
            return employeeSyncTable;
        }
        public IMobileServiceSyncTable<EventItem> getEventSyncTable()
        {
            return eventSyncTable;
        }
        public IMobileServiceSyncTable<RecipientListItem> getRecipientListSyncTable()
        {
            return recipientListSyncTable;
        }
    }
}