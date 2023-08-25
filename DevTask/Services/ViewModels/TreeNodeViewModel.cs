using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DevTask.Services.ViewModels
{
    public class TreeNodeViewModel
    {
        public string Name { get; set; }
        public List<TreeNodeViewModel> Children { get; set; }
        public bool IsExpanded { get; set; }
    }
}