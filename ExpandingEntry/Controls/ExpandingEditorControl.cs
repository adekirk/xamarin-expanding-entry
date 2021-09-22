using System;
using Xamarin.Forms;

namespace ExpandingEntry.Controls
{
    public class ExpandingEditorControl : Editor
    {
        public static BindableProperty HasRoundedCornerProperty
            = BindableProperty.Create(nameof(HasRoundedCorner), typeof(bool), typeof(ExpandingEditorControl));

        public static BindableProperty IsExpandableProperty
          = BindableProperty.Create(nameof(IsExpandable), typeof(bool), typeof(ExpandingEditorControl));


        public bool HasRoundedCorner
        {
            get => (bool)GetValue(HasRoundedCornerProperty);
            set => SetValue(HasRoundedCornerProperty, value);
        }

        public bool IsExpandable
        {
            get => (bool)GetValue(IsExpandableProperty);
            set => SetValue(IsExpandableProperty, value);
        }

        public ExpandingEditorControl()
        {
            TextChanged += OnTextChanged;
        }

        ~ExpandingEditorControl()
        {
            TextChanged -= OnTextChanged;
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsExpandable)
            {
                InvalidateMeasure();
            }
        }
    }
}
