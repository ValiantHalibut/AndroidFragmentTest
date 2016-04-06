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

namespace FragmentsTestApp
{
    [Activity(Label = "DetailsActivity")]
    public class DetailsActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            int index = Intent.Extras.GetInt("current_play_id", 0);

            DetailsFragment details = DetailsFragment.NewInstance(index);
            FragmentTransaction ft = FragmentManager.BeginTransaction();
            ft.Add(Android.Resource.Id.Content, details);
            ft.Commit();
        }
    }
}