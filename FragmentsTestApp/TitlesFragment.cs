using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace FragmentsTestApp
{
    public class TitlesFragment : ListFragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            return base.OnCreateView(inflater, container, savedInstanceState);
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            ArrayAdapter<String> adapter = new ArrayAdapter<String>(Activity, Android.Resource.Layout.SimpleListItemChecked, Shakespeare.Titles);
            ListAdapter = adapter;
            if(savedInstanceState != null)
            {
                _currentPlayId = savedInstanceState.GetInt("current_play_id", 0);
            }
            View detailsFrame = Activity.FindViewById<View>(Resource.Id.details);
            _isDualPane = detailsFrame != null && detailsFrame.Visibility == ViewStates.Visible;
            if (_isDualPane)
            {
                ListView.ChoiceMode = (int)ChoiceMode.Single;
                ShowDetails(_currentPlayId);
            }
        }

        public override void OnListItemClick(ListView l, View v, int position, long id)
        {
            ShowDetails(position);
        }

        private void ShowDetails(int playId)
        {
            _currentPlayId = playId;
            if (_isDualPane)
            {
                ListView.SetItemChecked(playId, true);

                DetailsFragment details = FragmentManager.FindFragmentById(Resource.Id.details) as DetailsFragment;
                if(details == null || details.ShownPlayId != playId)
                {
                    details = DetailsFragment.NewInstance(playId);

                    FragmentTransaction ft = FragmentManager.BeginTransaction();
                    ft.Replace(Resource.Id.details, details);
                    ft.SetTransition(FragmentTransaction.TransitFragmentFade);
                    ft.Commit();
                }
            }
            else
            {
                Intent intent = new Intent();
                intent.SetClass(Activity, typeof(DetailsActivity));
                intent.PutExtra("current_play_id", playId);
                StartActivity(intent);
            }
        }
    }
}