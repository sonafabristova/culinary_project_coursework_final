using System.Windows;
using System.Windows.Controls;

namespace culinary_project_coursework.Controls
{
    public partial class RecipeControl : UserControl
    {
        public RecipeControl()
        {
            InitializeComponent();
        }

        // Свойство для привязки рецепта (опционально, можно использовать прямо DataContext)
        public object RecipeData
        {
            get { return (object)GetValue(RecipeDataProperty); }
            set { SetValue(RecipeDataProperty, value); }
        }

        public static readonly DependencyProperty RecipeDataProperty =
            DependencyProperty.Register("RecipeData", typeof(object), typeof(RecipeControl),
                new PropertyMetadata(null));
    }
}