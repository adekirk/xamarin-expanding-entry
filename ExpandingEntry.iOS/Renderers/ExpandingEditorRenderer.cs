using System;
using System.ComponentModel;
using CoreGraphics;
using ExpandingEntry.Controls;
using ExpandingEntry.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExpandingEditorControl), typeof(ExpandingEditorRenderer))]
namespace ExpandingEntry.iOS.Renderers
{
    public class ExpandingEditorRenderer : EditorRenderer
    {
        private const int MAX_LINES = 5;
        private const int CORNER_RADIUS = 5;


        private double _previousHeight = -1;
        private int _previousLines = 0;



        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                var control = (ExpandingEditorControl)e.NewElement;

                Control.ScrollEnabled = !control.IsExpandable;
                Control.Layer.CornerRadius = control.HasRoundedCorner
                    ? CORNER_RADIUS
                    : 0;

                Control.InputAccessoryView = new UIView(CGRect.Empty);
                Control.ReloadInputViews();
            }

            if (e.OldElement != null)
            {

            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            var control = (ExpandingEditorControl)Element;

            HandleCorners(control, e.PropertyName);
            HandleShrinking(control, e.PropertyName);
            HandleExpanding(control, e.PropertyName);
        }


        private void HandleCorners(ExpandingEditorControl control, string propertyName)
        {
            if (propertyName == ExpandingEditorControl.HasRoundedCornerProperty.PropertyName)
            {
                Control.Layer.CornerRadius = control.HasRoundedCorner
                   ? CORNER_RADIUS
                   : 0;
            }           
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="propertyName"></param>
        private void HandleShrinking(ExpandingEditorControl control, string propertyName)
        {
            if (control.IsExpandable && propertyName == Editor.TextProperty.PropertyName)
            {
                var size = Control.Text.StringSize(Control.Font, Control.Frame.Size, UILineBreakMode.WordWrap);
                var numberOfLines = (int)(size.Height / Control.Font.LineHeight);

                if (_previousLines > numberOfLines || string.IsNullOrEmpty(Control.Text))
                {
                    control.HeightRequest = -1;
                }

                _previousLines = numberOfLines;
            }
        }


        /// <summary>
        /// When the number of lines exceeds the maximum allowed, freeze the control's
        /// height and enable the scrollbar.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="propertyName"></param>
        private void HandleExpanding(ExpandingEditorControl control, string propertyName, int maximumNumberOfLines = MAX_LINES)
        {
            if (control.IsExpandable && propertyName == ExpandingEditorControl.HeightProperty.PropertyName)
            {
                var size = Control.Text.StringSize(Control.Font, Control.Frame.Size, UILineBreakMode.WordWrap);
                var numberOfLines = (int)(size.Height / Control.Font.LineHeight);

                if (numberOfLines >= maximumNumberOfLines)
                {
                    Control.ScrollEnabled = true;
                    control.HeightRequest = _previousHeight;
                }
                else
                {
                    Control.ScrollEnabled = false;
                    _previousHeight = control.Height;
                }
            }
        }
    }
}
