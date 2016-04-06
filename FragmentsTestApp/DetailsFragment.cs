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
    public class DetailsFragment : Fragment
    {
        public int ShownPlayId { get { return Arguments.GetInt("current_play_id", 0); } }

        public static DetailsFragment NewInstance(int playId)
        {
            DetailsFragment detailsFragment = new DetailsFragment();
            detailsFragment.Arguments = new Bundle();
            detailsFragment.Arguments.PutInt("current_play_id", playId);
            return detailsFragment;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            if(container == null)
            {
                return null;
            }

            ScrollView scroller = new ScrollView(Activity);

            TextView text = new TextView(Activity);
            Int32 padding = Convert.ToInt32(TypedValue.ApplyDimension(ComplexUnitType.Dip, 4, Activity.Resources.DisplayMetrics));
            text.SetPadding(padding, padding, padding, padding);
            text.TextSize = 24;
            text.Text = Shakespeare.Dialogue[ShownPlayId];

            scroller.AddView(text);

            return scroller;
        }
    }
}