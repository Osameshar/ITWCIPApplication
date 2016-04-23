// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace ITW_MobileApp.iOS
{
	[Register ("EventDetailsController")]
	partial class EventDetailsController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextView category { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (category != null) {
				category.Dispose ();
				category = null;
			}
		}
	}
}
