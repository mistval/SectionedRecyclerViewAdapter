using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V7.Widget;

namespace App1
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        #region Methods

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            var recyclerView = this.FindViewById<RecyclerView>(Resource.Id.RecyclerView);
            var adapter = new RecyclerViewAdapter();
            var layoutManager = new LinearLayoutManager(this);
            recyclerView.SetAdapter(adapter);
            recyclerView.SetLayoutManager(layoutManager);
        }

        #endregion
    }
}

