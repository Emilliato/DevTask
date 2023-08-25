using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using DevTask.Adapters;
using DevTask.Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DevTask.Adapters
{
    public class TreeAdapter: BaseAdapter
    {
        private Context context;
        private List<TreeNodeViewModel> nodes;
        private List<TreeNodeViewModel> displayNodes;

        public TreeAdapter(Context context, List<TreeNodeViewModel> nodes)
        {
            this.context = context;
            this.nodes = nodes;
            this.displayNodes = new List<TreeNodeViewModel>();
            FlattenTree(nodes);
        }

        // Flatten the tree nodes into a list of display nodes
        private void FlattenTree(List<TreeNodeViewModel> nodes)
        {
            foreach (var node in nodes)
            {
                displayNodes.Add(node);
                if (node.IsExpanded && node.Children != null)
                {
                    FlattenTree(node.Children);
                }
            }
        }

        // Get the number of display nodes
        public override int Count => displayNodes.Count;

        // Get the display node at a given position
        public override Java.Lang.Object GetItem(int position) => null;

        public TreeNodeViewModel this[int index] => nodes[index];

        // Get the display node id at a given position
        public override long GetItemId(int position)
        {
            return position;
        }

        // Get the view for the spinner item
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            return CreateView(position, convertView, parent);
        }

        // Get the view for the spinner dropdown item
        public override View GetDropDownView(int position, View convertView, ViewGroup parent)
        {
            return CreateView(position, convertView, parent);
        }

        // Create a view for the spinner item or dropdown item
        private View CreateView(int position, View convertView, ViewGroup parent)
        {
            // Inflate the custom layout if needed
            if (convertView == null)
            {
                convertView = LayoutInflater.From(context).Inflate(Resource.Layout.spinner_item, parent, false);
            }

            // Get the text view and image view from the layout
            TextView textView = convertView.FindViewById<TextView>(Resource.Id.text);
            ImageView imageView = convertView.FindViewById<ImageView>(Resource.Id.indicator);

            // Get the display node at the position
            TreeNodeViewModel node = displayNodes[position];

            // Set the text view with indentation according to the node level
            int level = GetNodeLevel(node);
            textView.Text = new string(' ', level * 4) + node.Name;

            // Set the image view according to the node expansion state and children presence
            if (node.Children != null && node.Children.Count > 0)
            {
                imageView.Visibility = ViewStates.Visible;
                imageView.SetImageResource(node.IsExpanded ? Resource.Drawable.abc_ic_arrow_drop_right_black_24dp : Resource.Drawable.abc_ic_arrow_drop_right_black_24dp);

                // Set the click listener for the image view to handle the expansion and collapse
                imageView.Click += (sender, e) =>
                {
                    // Toggle the node expansion
                    node.IsExpanded = !node.IsExpanded;

                    // Update the display nodes
                    displayNodes.Clear();
                    FlattenTree(nodes);
                    NotifyDataSetChanged();
                };
            }
            else
            {
                imageView.Visibility = ViewStates.Gone;
            }

            return convertView;
        }

        // Get the level of a node in the tree
        private int GetNodeLevel(TreeNodeViewModel node)
        {
            int level = 0;
            foreach (var n in nodes)
            {
                if (n == node) break;
                if (IsDescendant(n, node)) level++;
            }
            return level;
        }

        // Check if a node is a descendant of another node
        private bool IsDescendant(TreeNodeViewModel parent, TreeNodeViewModel child)
        {
            if (parent.Children != null)
            {
                foreach (var node in parent.Children)
                {
                    if (node == child || IsDescendant(node, child)) return true;
                }
            }
            return false;
        }
    }
}

public class TreeNodeVm
{
    public string Name { get; set; }
    public List<TreeNodeViewModel> Children { get; set; }
    public bool IsExpanded { get; set; }
}

//Spinner spinner = FindViewById<Spinner>(Resource.Id.custom_spinner);

//// Create a tree data source
//var treeData = new List<TreeNodeViewModel>
//            {
//                new TreeNodeViewModel
//                {
//                    Name = "Animals",
//                    Children = new List<TreeNodeViewModel>
//                    {
//                        new TreeNodeViewModel { Name = "Cat" },
//                        new TreeNodeViewModel { Name = "Dog" },
//                        new TreeNodeViewModel { Name = "Bird" }
//                    }
//                },
//                new TreeNodeViewModel
//                {
//                    Name = "Fruits",
//                    Children = new List<TreeNodeViewModel>
//                    {
//                        new TreeNodeViewModel { Name = "Apple" },
//                        new TreeNodeViewModel { Name = "Banana" },
//                        new TreeNodeViewModel { Name = "Orange" }
//                    }
//                },
//                new TreeNodeViewModel
//                {
//                    Name = "Colors",
//                    Children = new List<TreeNodeViewModel>
//                    {
//                        new TreeNodeViewModel { Name = "Red" },
//                        new TreeNodeViewModel { Name = "Green" },
//                        new TreeNodeViewModel { Name = "Blue" }
//                    }
//                }
//            };

//// Create an instance of the custom adapter and set it as the adapter for the spinner
//TreeAdapter adapter = new TreeAdapter(this, treeData);
//spinner.Adapter = adapter;

//// Set the item selected listener for the spinner
//spinner.ItemSelected += (sender, e) =>
//{
//    // Get the selected item position
//    int position = e.Position;

//    // Get the selected item value
//    var node = adapter[position];

//    // Do something with the selected item
//    Toast.MakeText(this, "You selected " + node.Name, ToastLength.Short).Show();
//};