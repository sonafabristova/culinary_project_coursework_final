using System.Windows;
using System.Windows.Controls;
using culinary_project_coursework.Models;

namespace culinary_project_coursework.Controls
{
    public partial class RecipeControl : UserControl
    {
        public RecipeControl()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                if (DataContext == null && Parent is FrameworkElement parent)
                {
                    DataContext = parent.DataContext;
                }
            };
        }
    }
}