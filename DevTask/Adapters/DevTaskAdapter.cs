using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.RecyclerView.Widget;
using DevTask.Services.ViewModels;
using Google.Android.Material.TextView;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DevTask.Adapters
{
    public class DevTaskAdapter : RecyclerView.Adapter
    {
        public event EventHandler<int> UpdateItemClick;
        public event EventHandler<int> DeleteItemClick;
        public List<TaskViewModel> _data;

        public DevTaskAdapter(List<TaskViewModel> data)
        {
            _data = data;
        }

        public override int ItemCount
        {
            get { return _data.Count(); }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var item = _data[position];
            var vh = holder as DevTaskViewHolder;

            if (vh != null)
            {
                vh.Title.Text = item.TaskName;
                vh.Description.Text = item.TaskDescription;
                vh.Estimate.Text = item.Estimate;
                vh.Completed.Text = string.IsNullOrEmpty(item.Completed) ? "0" : item.Completed;
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.task_item, parent, false);
            return new DevTaskViewHolder(itemView, OnUpdateClick, OnDeleteItemClick);
        }

        void OnUpdateClick(int position)
        {
            if (UpdateItemClick != null)
                UpdateItemClick(this, position);
        }

        void OnDeleteItemClick(int position)
        {
            if (DeleteItemClick != null)
                DeleteItemClick(this, position);
        }
    }

    public class DevTaskViewHolder : RecyclerView.ViewHolder
    {
        public MaterialTextView Title { get; set; }
        public MaterialTextView Description { get; set; }
        public MaterialTextView Estimate { get; set; }
        public MaterialTextView Completed { get; set; }
        public AppCompatImageView DeleteIcon  { get; set; }

        public DevTaskViewHolder(View itemview, Action<int> updateListener, Action<int> deleteListener) :
            base(itemview)
        {
            Title = itemview.FindViewById<MaterialTextView>(Resource.Id.tv_title);
            Description = itemview.FindViewById<MaterialTextView>(Resource.Id.tv_description);
            Estimate = itemview.FindViewById<MaterialTextView>(Resource.Id.tv_estimate_hrs);
            Completed = itemview.FindViewById<MaterialTextView>(Resource.Id.tv_completed_hrs);
            DeleteIcon = itemview.FindViewById<AppCompatImageView>(Resource.Id.img_delete_icon);

            Title.Click += (sender, e) => updateListener(base.LayoutPosition);
            Description.Click += (sender, e) => updateListener(base.LayoutPosition);

            DeleteIcon.Click += (sender, e) => deleteListener(base.LayoutPosition);
        }
    }
}