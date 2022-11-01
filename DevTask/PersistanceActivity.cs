using Android.App;
using Android.OS;
using Android.Widget;
using DevTask.Services.Enums;
using DevTask.Services.Implementations;
using DevTask.Services.Interfaces;
using DevTask.Services.ViewModels;
using Google.Android.Material.Button;
using Google.Android.Material.TextField;
using Newtonsoft.Json;
using System;

namespace DevTask
{
    [Activity(Label = "PersistanceActivity", Theme = "@style/AppTheme", MainLauncher = false)]
    public class PersistanceActivity : Activity
    {
        private MaterialButton _saveButton;

        private TextInputEditText _title;
        private TextInputEditText _description;
        private TextInputEditText _estimate;
        private TextInputEditText _completed;

        private TextInputLayout _titleLayout;
        private TextInputLayout _descriptionLayout;
        private TextInputLayout _estimateLayout;

        private TaskViewModel _task;
        private bool _isUpdate;

        private IDevTaskManager _devTaskManager;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            try
            {
                SetContentView(Resource.Layout.devtaskpersistence);
                
                initializeObjects();
                initializeViews();
            }
            catch(Exception ex)
            {
                ShowToast(ex.Message.ToString());
            }

        }

        private void initializeObjects()
        {
            var value = Intent.GetStringExtra("Task");

            if (!string.IsNullOrEmpty(value))
            {
                _isUpdate = true;
                _task = JsonConvert.DeserializeObject<TaskViewModel>(value);
            }

            _devTaskManager = new DevTaskManager();
        }

        private void initializeViews()
        {
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);

            ActionBar.Title = _isUpdate ? "Update Task" : "Add Task";

            _title = FindViewById<TextInputEditText>(Resource.Id.et_title);
            _description = FindViewById<TextInputEditText>(Resource.Id.et_description);
            _estimate = FindViewById<TextInputEditText>(Resource.Id.et_estimate);
            _completed = FindViewById<TextInputEditText>(Resource.Id.et_completed);
            _saveButton = FindViewById<MaterialButton>(Resource.Id.btn_save);

            if (_isUpdate)
            {
                _title.Text = _task.TaskName;
                _description.Text = _task.TaskDescription;
                _estimate.Text = _task.Estimate;
                _completed.Text = _task.Completed;
                
                _estimate.Enabled = false;
                _estimate.Focusable = false;
            }

            _titleLayout = FindViewById<TextInputLayout>(Resource.Id.ly_title);
            _descriptionLayout = FindViewById<TextInputLayout>(Resource.Id.ly_description);
            _estimateLayout = FindViewById<TextInputLayout>(Resource.Id.ly_estimate);


            _saveButton.Click += SaveButtonClick;
        }

        private async void SaveButtonClick(object sender, EventArgs e)
        {
            var fieldsAreValid = ValidateFields();

            if (fieldsAreValid)
            {
                var devTask = new TaskViewModel()
                {
                    
                    TaskName = _title.Text.ToString().Trim(),
                    TaskDescription = _description.Text.ToString().Trim(),
                    Estimate = _estimate.Text.ToString().Trim(),
                    Completed = _completed.Text.ToString().Trim(),
                    State = TaskState.InProgress,
                };

                if (_isUpdate)
                {
                    devTask.TaskId = _task.TaskId;
                    await _devTaskManager.UpdateTask(devTask);

                    ShowToast("Task Updated Successfully");
                }
                else
                {
                    await _devTaskManager.AddTask(devTask);

                    ShowToast("Task Created Successfully");
                }
              
                Finish();
            }
        }
        private void ShowToast(string message)
        {
            Toast.MakeText(this, message, ToastLength.Long).Show();
        }

        private bool ValidateFields()
        {
            var titleIsValid = ValidateField(_titleLayout, _title);
            var descriptionIsValid = ValidateField(_descriptionLayout, _description);
            var estimateIsValid = ValidateField(_estimateLayout, _estimate);
            return titleIsValid && descriptionIsValid && estimateIsValid;
        }

        private bool ValidateField(TextInputLayout layout, TextInputEditText textValue)
        {
            var value = textValue.Text.Trim().ToString();

            if (string.IsNullOrEmpty(value))
            {
                layout.Error = "This Field Is Required";
                layout.ErrorEnabled = true;
                return false;
            }

            layout.Error = string.Empty;
            layout.ErrorEnabled = false;
            return true;
        }
    }
}