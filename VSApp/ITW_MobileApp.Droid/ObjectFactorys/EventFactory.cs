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
    class EventFactory
    {
        public EventItem createEvent(string newName, string newEventRecipients, DateTime newEventDate, string newEventTime, string newLocation, string newCategory, string newEventPriority, string newEventDescription, int newEventID, int newEmployeeID)
        {
            //create a recipientListitem for every EventRecipient after parsing Event Recipients
            return new EventItem { Name = newName, EventRecipients = newEventRecipients, EventDate = newEventDate, EventTime = newEventTime, Location = newLocation, Category = newCategory, EventPriority = newEventPriority, EventDescription = newEventDescription, EventID = newEventID, EmployeeID = newEmployeeID };


        }

    }
}