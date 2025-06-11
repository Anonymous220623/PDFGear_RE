// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.TextBoxSelectionAdorner
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Shared;

[DesignTimeVisible(false)]
internal class TextBoxSelectionAdorner : Adorner
{
  private Thumb selectionThumb1;
  private Thumb selectionThumb2;
  private VisualCollection visualCollection;
  private bool isCalledByDragDelta;
  private bool isCalledByMouseUp;
  private double widthChange2;
  private double widthChange1;
  private ThumbDirection thumbDirection;
  private TextBox iTextBox;

  public TextBoxSelectionAdorner(UIElement adornedElement)
    : base(adornedElement)
  {
    this.selectionThumb1 = new Thumb();
    this.selectionThumb2 = new Thumb();
    this.visualCollection = new VisualCollection((Visual) this);
    this.selectionThumb1.Height = this.selectionThumb1.Width = this.selectionThumb2.Height = this.selectionThumb2.Width = 30.0;
    this.selectionThumb1.Margin = this.selectionThumb2.Margin = new Thickness(-15.0, 0.0, 0.0, 0.0);
    this.selectionThumb1.DragDelta += new DragDeltaEventHandler(this.selectionThumb_DragDelta);
    this.selectionThumb2.DragDelta += new DragDeltaEventHandler(this.selectionThumb_DragDelta);
    this.selectionThumb1.Visibility = Visibility.Collapsed;
    this.selectionThumb2.Visibility = Visibility.Collapsed;
    this.visualCollection.Add((Visual) this.selectionThumb1);
    this.visualCollection.Add((Visual) this.selectionThumb2);
    this.iTextBox = adornedElement as TextBox;
    this.selectionThumb1.PreviewMouseDown += new MouseButtonEventHandler(this.selectionThumb_PreviewMouseDown);
    this.selectionThumb2.PreviewMouseDown += new MouseButtonEventHandler(this.selectionThumb_PreviewMouseDown);
    this.selectionThumb1.PreviewMouseUp += new MouseButtonEventHandler(this.selectionThumb_PreviewMouseUp);
    this.selectionThumb2.PreviewMouseUp += new MouseButtonEventHandler(this.selectionThumb_PreviewMouseUp);
    if (this.iTextBox != null)
    {
      this.iTextBox.PreviewMouseDown += new MouseButtonEventHandler(this.iTextBox_PreviewMouseDown);
      this.iTextBox.PreviewMouseUp += new MouseButtonEventHandler(this.iTextBox_PreviewMouseUp);
      this.iTextBox.PreviewKeyDown += new KeyEventHandler(this.iTextBox_PreviewKeyDown);
      this.iTextBox.SelectionChanged += new RoutedEventHandler(this.iTextBox_SelectionChanged);
      this.iTextBox.LostFocus += new RoutedEventHandler(this.iTextBox_LostFocus);
    }
    Style style = new ResourceDictionary()
    {
      Source = new Uri("/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/Generic.xaml", UriKind.RelativeOrAbsolute)
    }[(object) "SelectionThumbStyle"] as Style;
    this.selectionThumb1.Style = style;
    this.selectionThumb2.Style = style;
  }

  private void selectionThumb_PreviewMouseUp(object sender, MouseButtonEventArgs e)
  {
    if (e.StylusDevice != null)
    {
      this.HandleThumbVisiblity();
    }
    else
    {
      if (this.selectionThumb2 != null)
        this.selectionThumb2.Visibility = Visibility.Collapsed;
      if (this.selectionThumb1 == null)
        return;
      this.selectionThumb1.Visibility = Visibility.Collapsed;
    }
  }

  private void iTextBox_LostFocus(object sender, RoutedEventArgs e)
  {
    if (this.selectionThumb1 != null)
      this.selectionThumb1.Visibility = Visibility.Collapsed;
    if (this.selectionThumb2 == null)
      return;
    this.selectionThumb2.Visibility = Visibility.Collapsed;
  }

  private void selectionThumb_PreviewMouseDown(object sender, MouseButtonEventArgs e)
  {
    if (e.StylusDevice != null)
      return;
    if (this.selectionThumb1 != null)
      this.selectionThumb1.Visibility = Visibility.Collapsed;
    if (this.selectionThumb2 == null)
      return;
    this.selectionThumb2.Visibility = Visibility.Collapsed;
  }

  private void iTextBox_SelectionChanged(object sender, RoutedEventArgs e)
  {
    TextBox iTextBox = this.iTextBox;
    if (this.selectionThumb2 == null || this.selectionThumb1 == null || iTextBox.Text == null || iTextBox == null || iTextBox.Text.Length <= 0 || this.iTextBox.SelectionLength != 0 || this.isCalledByDragDelta || this.isCalledByMouseUp || this.selectionThumb1.Visibility != Visibility.Visible || this.selectionThumb2.Visibility != Visibility.Visible)
      return;
    double width1 = new FormattedText(this.iTextBox.Text.Substring(0, this.iTextBox.SelectionStart), Thread.CurrentThread.CurrentUICulture, this.iTextBox.FlowDirection, new Typeface(this.iTextBox.FontFamily, this.iTextBox.FontStyle, this.iTextBox.FontWeight, this.iTextBox.FontStretch), this.iTextBox.FontSize, this.iTextBox.Foreground).Width;
    int length = this.iTextBox.SelectionLength != 0 ? this.iTextBox.SelectionLength + this.iTextBox.SelectionStart : this.iTextBox.SelectionStart;
    double width2 = new FormattedText(this.iTextBox.Text.Substring(0, length), Thread.CurrentThread.CurrentUICulture, this.iTextBox.FlowDirection, new Typeface(this.iTextBox.FontFamily, this.iTextBox.FontStyle, this.iTextBox.FontWeight, this.iTextBox.FontStretch), this.iTextBox.FontSize, this.iTextBox.Foreground).Width;
    this.selectionThumb1.Arrange(new Rect(width1, this.iTextBox.RenderSize.Height - 15.0, 30.0, 30.0));
    this.selectionThumb1.Tag = (object) new ThumbPosition()
    {
      Position = this.iTextBox.SelectionStart,
      StartingPoint = width1
    };
    this.selectionThumb2.Arrange(new Rect(width2, this.iTextBox.RenderSize.Height - 15.0, 30.0, 30.0));
    this.selectionThumb2.Tag = (object) new ThumbPosition()
    {
      Position = length,
      StartingPoint = width2
    };
  }

  public void iTextBox_PreviewMouseUp(object sender, MouseEventArgs e)
  {
    this.isCalledByMouseUp = true;
    if (e.StylusDevice != null)
    {
      this.HandleThumbVisiblity();
    }
    else
    {
      if (this.selectionThumb2 != null)
        this.selectionThumb2.Visibility = Visibility.Collapsed;
      if (this.selectionThumb1 != null)
        this.selectionThumb1.Visibility = Visibility.Collapsed;
    }
    this.isCalledByMouseUp = false;
  }

  private void HandleThumbVisiblity()
  {
    if (this.selectionThumb1 != null && this.selectionThumb2 != null && this.iTextBox != null && this.iTextBox.SelectionLength == 0 && (this.selectionThumb2.Visibility == Visibility.Visible || this.selectionThumb1.Visibility == Visibility.Visible))
    {
      this.selectionThumb2.Visibility = Visibility.Collapsed;
      this.selectionThumb1.Visibility = Visibility.Collapsed;
    }
    else
    {
      TextBox iTextBox = this.iTextBox;
      if (iTextBox != null && this.iTextBox != null && !string.IsNullOrEmpty(iTextBox.Text) && this.iTextBox.SelectionStart <= this.iTextBox.Text.Length)
      {
        if (this.selectionThumb1 != null)
          this.selectionThumb1.Visibility = Visibility.Hidden;
        if (this.selectionThumb2 != null)
          this.selectionThumb2.Visibility = Visibility.Hidden;
        double width1 = new FormattedText(this.iTextBox.Text.Substring(0, this.iTextBox.SelectionStart), Thread.CurrentThread.CurrentUICulture, this.iTextBox.FlowDirection, new Typeface(this.iTextBox.FontFamily, this.iTextBox.FontStyle, this.iTextBox.FontWeight, this.iTextBox.FontStretch), this.iTextBox.FontSize, this.iTextBox.Foreground).Width;
        int length = this.iTextBox.SelectionLength != 0 ? this.iTextBox.SelectionLength + this.iTextBox.SelectionStart : this.iTextBox.SelectionStart;
        double width2 = new FormattedText(this.iTextBox.Text.Substring(0, length), Thread.CurrentThread.CurrentUICulture, this.iTextBox.FlowDirection, new Typeface(this.iTextBox.FontFamily, this.iTextBox.FontStyle, this.iTextBox.FontWeight, this.iTextBox.FontStretch), this.iTextBox.FontSize, this.iTextBox.Foreground).Width;
        if (this.selectionThumb1 != null)
        {
          this.selectionThumb1.Tag = (object) new ThumbPosition()
          {
            Position = this.iTextBox.SelectionStart,
            StartingPoint = width1
          };
          this.selectionThumb1.Arrange(new Rect(width1, this.iTextBox.RenderSize.Height - 15.0, 30.0, 30.0));
        }
        if (this.selectionThumb2 != null)
        {
          this.selectionThumb2.Arrange(new Rect(width2, this.iTextBox.RenderSize.Height - 15.0, 30.0, 30.0));
          this.selectionThumb2.Tag = (object) new ThumbPosition()
          {
            Position = length,
            StartingPoint = width2
          };
        }
        if (this.selectionThumb1 != null)
          this.selectionThumb1.Visibility = Visibility.Visible;
        if (this.selectionThumb2 == null)
          return;
        this.selectionThumb2.Visibility = Visibility.Visible;
      }
      else
      {
        if (this.selectionThumb2 != null)
          this.selectionThumb2.Visibility = Visibility.Collapsed;
        if (this.selectionThumb1 == null)
          return;
        this.selectionThumb1.Visibility = Visibility.Collapsed;
      }
    }
  }

  private void iTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
  {
    if (this.selectionThumb2 != null)
      this.selectionThumb2.Visibility = Visibility.Collapsed;
    if (this.selectionThumb1 == null)
      return;
    this.selectionThumb1.Visibility = Visibility.Collapsed;
  }

  private void iTextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
  {
    if (e.StylusDevice != null)
      return;
    if (this.selectionThumb2 != null)
      this.selectionThumb2.Visibility = Visibility.Collapsed;
    if (this.selectionThumb1 == null)
      return;
    this.selectionThumb1.Visibility = Visibility.Collapsed;
  }

  private void selectionThumb_DragDelta(object sender, DragDeltaEventArgs e)
  {
    this.isCalledByDragDelta = true;
    TextBox iTextBox = this.iTextBox;
    if (iTextBox != null && iTextBox.Text != null)
    {
      FormattedText formattedText1 = new FormattedText(iTextBox.Text.Substring(0, this.iTextBox.Text.Length), Thread.CurrentThread.CurrentUICulture, iTextBox.FlowDirection, new Typeface(this.iTextBox.FontFamily, this.iTextBox.FontStyle, this.iTextBox.FontWeight, this.iTextBox.FontStretch), this.iTextBox.FontSize, this.iTextBox.Foreground);
      if (e.HorizontalChange > 0.0)
      {
        if (this.thumbDirection == ThumbDirection.Backward)
        {
          if (sender == this.selectionThumb2)
            this.widthChange2 = 0.0;
          else
            this.widthChange1 = 0.0;
        }
        this.thumbDirection = ThumbDirection.Forward;
        if (sender == this.selectionThumb2)
          this.widthChange2 += e.HorizontalChange;
        else
          this.widthChange1 += e.HorizontalChange;
        if (sender != null)
        {
          object tag = ((FrameworkElement) sender).Tag;
          if (tag != null)
          {
            ThumbPosition thumbPosition1 = (ThumbPosition) tag;
            if (thumbPosition1.Position < this.iTextBox.Text.Length)
            {
              FormattedText formattedText2 = new FormattedText(iTextBox.Text.Substring(thumbPosition1.Position, 1), Thread.CurrentThread.CurrentUICulture, iTextBox.FlowDirection, new Typeface(this.iTextBox.FontFamily, this.iTextBox.FontStyle, this.iTextBox.FontWeight, this.iTextBox.FontStretch), this.iTextBox.FontSize, this.iTextBox.Foreground);
              if ((sender != this.selectionThumb2 ? this.widthChange1 : this.widthChange2) >= formattedText2.Width)
              {
                ++thumbPosition1.Position;
                ((FrameworkElement) sender).Tag = (object) thumbPosition1;
                ThumbPosition thumbPosition2 = new ThumbPosition();
                if (sender == this.selectionThumb2)
                {
                  if (this.selectionThumb1 != null)
                    thumbPosition2 = (ThumbPosition) this.selectionThumb1.Tag;
                }
                else
                  thumbPosition2 = (ThumbPosition) this.selectionThumb2.Tag;
                if (thumbPosition1.Position > thumbPosition2.Position)
                {
                  if (this.iTextBox.SelectionLength + 1 <= this.iTextBox.Text.Length)
                    ++this.iTextBox.SelectionLength;
                }
                else if (thumbPosition1.Position < thumbPosition2.Position)
                {
                  if (this.iTextBox.SelectionStart + 1 <= this.iTextBox.Text.Length)
                    ++this.iTextBox.SelectionStart;
                  if (this.iTextBox.SelectionLength > 0 && thumbPosition2.Position != this.iTextBox.Text.Length)
                    --this.iTextBox.SelectionLength;
                }
                else
                {
                  this.iTextBox.SelectionLength = 0;
                  if (this.iTextBox.SelectionStart + 1 <= this.iTextBox.Text.Length)
                    ++this.iTextBox.SelectionStart;
                }
                if (sender == this.selectionThumb2)
                  this.widthChange2 = 0.0;
                else
                  this.widthChange1 = 0.0;
              }
            }
          }
        }
      }
      else if (e.HorizontalChange < 0.0)
      {
        if (this.thumbDirection == ThumbDirection.Forward)
        {
          if (sender == this.selectionThumb2)
            this.widthChange2 = 0.0;
          else
            this.widthChange1 = 0.0;
        }
        this.thumbDirection = ThumbDirection.Backward;
        if (sender == this.selectionThumb2)
          this.widthChange2 += Math.Abs(e.HorizontalChange);
        else
          this.widthChange1 += Math.Abs(e.HorizontalChange);
        if (sender != null)
        {
          object tag = ((FrameworkElement) sender).Tag;
          if (tag != null)
          {
            ThumbPosition thumbPosition3 = (ThumbPosition) tag;
            if (thumbPosition3.Position > 0 && iTextBox.Text.Length > thumbPosition3.Position - 1)
            {
              FormattedText formattedText3 = new FormattedText(iTextBox.Text.Substring(thumbPosition3.Position - 1, 1), Thread.CurrentThread.CurrentUICulture, iTextBox.FlowDirection, new Typeface(this.iTextBox.FontFamily, this.iTextBox.FontStyle, this.iTextBox.FontWeight, this.iTextBox.FontStretch), this.iTextBox.FontSize, this.iTextBox.Foreground);
              if ((this.selectionThumb2 == null || sender != this.selectionThumb2 ? this.widthChange1 : this.widthChange2) >= formattedText3.Width)
              {
                --thumbPosition3.Position;
                ((FrameworkElement) sender).Tag = (object) thumbPosition3;
                ThumbPosition thumbPosition4 = new ThumbPosition();
                if (this.selectionThumb2 != null && sender == this.selectionThumb2)
                {
                  if (this.selectionThumb1.Tag != null)
                    thumbPosition4 = (ThumbPosition) this.selectionThumb1.Tag;
                }
                else if (this.selectionThumb2 != null && this.selectionThumb2.Tag != null)
                  thumbPosition4 = (ThumbPosition) this.selectionThumb2.Tag;
                if (thumbPosition3.Position > thumbPosition4.Position)
                {
                  if (this.iTextBox.SelectionLength > 0)
                    --this.iTextBox.SelectionLength;
                }
                else if (thumbPosition3.Position < thumbPosition4.Position)
                {
                  if (this.iTextBox.SelectionStart > 0)
                  {
                    --this.iTextBox.SelectionStart;
                    ++this.iTextBox.SelectionLength;
                  }
                }
                else
                  this.iTextBox.SelectionLength = 0;
                if (this.selectionThumb2 != null && sender == this.selectionThumb2)
                  this.widthChange2 = 0.0;
                else
                  this.widthChange1 = 0.0;
              }
            }
          }
        }
      }
      ThumbPosition thumbPosition = new ThumbPosition();
      if (this.selectionThumb2 != null && sender == this.selectionThumb2)
      {
        if (this.selectionThumb2.Tag != null)
          thumbPosition = (ThumbPosition) this.selectionThumb2.Tag;
      }
      else if (this.selectionThumb1 != null && this.selectionThumb1.Tag != null)
        thumbPosition = (ThumbPosition) this.selectionThumb1.Tag;
      FormattedText formattedText4 = new FormattedText(iTextBox.Text.Substring(0, thumbPosition.Position), Thread.CurrentThread.CurrentUICulture, iTextBox.FlowDirection, new Typeface(this.iTextBox.FontFamily, this.iTextBox.FontStyle, this.iTextBox.FontWeight, this.iTextBox.FontStretch), this.iTextBox.FontSize, this.iTextBox.Foreground);
      if (this.selectionThumb2 != null && sender == this.selectionThumb2)
        this.selectionThumb2.Arrange(new Rect(formattedText4.Width, this.iTextBox.RenderSize.Height - 15.0, 30.0, 30.0));
      else if (this.selectionThumb1 != null)
        this.selectionThumb1.Arrange(new Rect(formattedText4.Width, this.iTextBox.RenderSize.Height - 15.0, 30.0, 30.0));
    }
    this.isCalledByDragDelta = false;
  }

  protected override int VisualChildrenCount => this.visualCollection.Count;

  protected override Visual GetVisualChild(int index)
  {
    return this.visualCollection.Count > index ? this.visualCollection[index] : (Visual) null;
  }

  internal void Dispose()
  {
    if (this.selectionThumb1 != null)
    {
      this.selectionThumb1.DragDelta -= new DragDeltaEventHandler(this.selectionThumb_DragDelta);
      this.selectionThumb1.PreviewMouseDown -= new MouseButtonEventHandler(this.selectionThumb_PreviewMouseDown);
      this.selectionThumb1.PreviewMouseUp -= new MouseButtonEventHandler(this.selectionThumb_PreviewMouseUp);
      this.selectionThumb1 = (Thumb) null;
    }
    if (this.selectionThumb1 != null)
    {
      this.selectionThumb2.DragDelta -= new DragDeltaEventHandler(this.selectionThumb_DragDelta);
      this.selectionThumb2.PreviewMouseDown -= new MouseButtonEventHandler(this.selectionThumb_PreviewMouseDown);
      this.selectionThumb2.PreviewMouseUp -= new MouseButtonEventHandler(this.selectionThumb_PreviewMouseUp);
      this.selectionThumb2 = (Thumb) null;
    }
    if (this.iTextBox != null)
    {
      this.iTextBox.PreviewMouseDown -= new MouseButtonEventHandler(this.iTextBox_PreviewMouseDown);
      this.iTextBox.PreviewMouseUp -= new MouseButtonEventHandler(this.iTextBox_PreviewMouseUp);
      this.iTextBox.PreviewKeyDown -= new KeyEventHandler(this.iTextBox_PreviewKeyDown);
      this.iTextBox.SelectionChanged -= new RoutedEventHandler(this.iTextBox_SelectionChanged);
      this.iTextBox.LostFocus -= new RoutedEventHandler(this.iTextBox_LostFocus);
    }
    if (this.visualCollection == null)
      return;
    this.visualCollection.Clear();
    this.visualCollection = (VisualCollection) null;
  }
}
