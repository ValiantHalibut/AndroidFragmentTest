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
        private int _currentPlayId;
        private bool _isDualPane;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetHasOptionsMenu(true);
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.TitleMenu, menu);
            base.OnCreateOptionsMenu(menu, inflater);
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            //ArrayAdapter<String> adapter = new ArrayAdapter<String>(Activity, Android.Resource.Layout.SimpleListItemChecked, Shakespeare.Titles);
            String[] optionsString = { "Option 1", "Option 2" };
            ArrayAdapter<String> adapter = new ArrayAdapter<String>(Activity, Android.Resource.Layout.SimpleListItemSingleChoice, optionsString);
            ListAdapter = adapter;

            View detailsFrame = Activity.FindViewById<View>(Resource.Id.details);
            _isDualPane = detailsFrame != null && detailsFrame.Visibility == ViewStates.Visible;

            if (savedInstanceState != null)
            {
                _currentPlayId = savedInstanceState.GetInt("current_play_id", 0);
            }

            if (_isDualPane)
            {
                ListView.ChoiceMode = ChoiceMode.Single;
                ShowDetails(_currentPlayId);
            }
        }

        public override void OnListItemClick(ListView l, View v, int position, long id)
        {
            ShowDetails(position);
        }

        public override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            outState.PutInt("current_play_id", _currentPlayId);
        }

        private void ShowDetails(int playId)
        {
            _currentPlayId = playId;
            if (_isDualPane)
            {
                ListView.SetItemChecked(playId, true);

                VideoFragment videoFragment = FragmentManager.FindFragmentById(Resource.Id.details) as VideoFragment;
                if(videoFragment == null)
                {
                    videoFragment = new VideoFragment();
                    FragmentTransaction ft = FragmentManager.BeginTransaction();
                    ft.Replace(Resource.Id.details, videoFragment);
                    ft.SetTransition(FragmentTransit.FragmentFade);
                    ft.Commit();
                }
            }
            else
            {
                Intent intent = new Intent();
                intent.SetClass(Activity, typeof(VideoActivity));
                intent.PutExtra("current_play_id", playId);
                StartActivity(intent);
            }
            /*
            if (_isDualPane)
            {
                ListView.SetItemChecked(playId, true);

                DetailsFragment details = FragmentManager.FindFragmentById(Resource.Id.details) as DetailsFragment;
                if(details == null || details.ShownPlayId != playId)
                {
                    details = DetailsFragment.NewInstance(playId);

                    FragmentTransaction ft = FragmentManager.BeginTransaction();
                    ft.Replace(Resource.Id.details, details);
                    ft.SetTransition(FragmentTransit.FragmentFade);
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
            */
        }
    }
}