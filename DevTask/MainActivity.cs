using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.RecyclerView.Widget;
using DevTask.Adapters;
using DevTask.Services.Implementations;
using DevTask.Services.Interfaces;
using DevTask.Services.ViewModels;
using Google.Android.Material.FloatingActionButton;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevTask
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private IDevTaskManager _taskManager;
        private DevTaskAdapter _adapter;
        private FloatingActionButton _addButton;
        private List<TaskViewModel> _data;
        private RecyclerView _recyclerView;
        private LinearLayoutManager _layoutManager;
        private bool _loadData = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            try
            {
                SetContentView(Resource.Layout.activity_main);
                InitializeViews();
                InitializeObjects();
            }
            catch (Exception ex)
            {
                RunOnUiThread(() =>
                {
                    ShowToast(ex.Message);
                });
            }
        }

        protected override void OnResume()
        {
            base.OnResume();

            Task.Run(async () =>
            {
                await LoadData();
            });
        }

        private void InitializeViews()
        {
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);
            ActionBar.Title = GetString(Resource.String.app_name);

            _addButton = FindViewById<FloatingActionButton>(Resource.Id.fab_add);

            _addButton.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(PersistanceActivity));
                StartActivity(intent);
            };

            _data = new List<TaskViewModel>();

            _recyclerView = FindViewById<RecyclerView>(Resource.Id.rv_tasks);

            _layoutManager = new LinearLayoutManager(this);
            _recyclerView.SetLayoutManager(_layoutManager);

            _adapter = new DevTaskAdapter(_data);

            _adapter.DeleteItemClick += OnDeleteItemClick;
            _adapter.UpdateItemClick += OnUpdateItemClick;

            _recyclerView.SetAdapter(_adapter);
        }

        private void InitializeObjects()
        {
            _taskManager = new DevTaskManager();

            Task.Run(async () =>
            {
                var tablesResult = await _taskManager.CreateTables();
                _loadData = true;

                // Load the data after data migration
                await LoadData();
            });
        }

        private async Task LoadData()
        {
            if (_loadData)
            {
                var tasks = await _taskManager.GetTasks();

                RunOnUiThread(() =>
                {
                    _data.Clear();
                    _data.AddRange(tasks);
                    _adapter.NotifyDataSetChanged();
                });
            }
        }

        private void ShowToast(string message)
        {
            Toast.MakeText(this, message, ToastLength.Long).Show();
        }

        private void OnUpdateItemClick(object sender, int position)
        {
            var item = _data[position];
            var intent = new Intent(this, typeof(PersistanceActivity));
            intent.PutExtra("Task", JsonConvert.SerializeObject(item));
            StartActivity(intent);
        }


        private void OnDeleteItemClick(object sender, int position)
        {
            var item = _data[position];

            Task.Run(async () =>
            {
                 await _taskManager.DeleteTask(item.TaskId);
                _loadData = true;

                RunOnUiThread(() =>
                {
                    ShowToast("Task Deleted Successfully!");
                });

                // Load the data after deletion
                await LoadData();
            });
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}