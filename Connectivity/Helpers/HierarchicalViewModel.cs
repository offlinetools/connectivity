using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OfflineTools.Connectivity.Helpers
{
    public sealed class HierarchicalViewModel : BindableBase
    {
        public HierarchicalViewModel()
        {
            IsEnabled = true;
            Children = new List<HierarchicalViewModel>();
        }

        public object CommandParameter { get; set; }

        public string Text { get; set; }

        public object Data { get; set; }

        public bool IsEnabled { get; set; }

        bool isSelected;
        public bool IsSelected { get { return isSelected; } set { SetProperty(ref isSelected, value); } }

        public ICommand Command { get; set; }

        public List<HierarchicalViewModel> Children { get; set; }
    }
}
