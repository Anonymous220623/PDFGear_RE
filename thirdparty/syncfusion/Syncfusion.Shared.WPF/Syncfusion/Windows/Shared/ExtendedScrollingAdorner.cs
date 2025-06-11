// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.ExtendedScrollingAdorner
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Resources;

#nullable disable
namespace Syncfusion.Windows.Shared;

[DesignTimeVisible(false)]
public class ExtendedScrollingAdorner : Adorner
{
  private Thumb valueThumb;
  private VisualCollection visualCollection;
  private Rect adornedElementRect;
  private FrameworkElement fElement;
  private double prevVerticalChange;
  private double prevHorizontalChange;
  private CursorHandler.POINT pnt;
  private bool isThumbMoved;
  private StreamResourceInfo info;
  private Cursor scrollingCursor;

  public bool IsReadOnly
  {
    get
    {
      if (this.fElement is EditorBase)
        return (this.fElement as EditorBase).IsReadOnly;
      return this.fElement is TimeSpanEdit && (this.fElement as TimeSpanEdit).IsReadOnly;
    }
  }

  public ExtendedScrollingAdorner(UIElement adornedElement)
    : base(adornedElement)
  {
    Thumb thumb = new Thumb();
    thumb.Opacity = 0.0;
    this.valueThumb = thumb;
    this.visualCollection = new VisualCollection((Visual) this)
    {
      (Visual) this.valueThumb
    };
    if (this.AdornedElement != null)
      this.adornedElementRect = new Rect(this.AdornedElement.DesiredSize);
    this.fElement = adornedElement as FrameworkElement;
    ResourceDictionary resourceDictionary = new ResourceDictionary()
    {
      Source = new Uri("/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/Generic.xaml", UriKind.RelativeOrAbsolute)
    };
    this.info = Application.GetResourceStream(new Uri("/Syncfusion.Shared.WPF;component/Controls/Editors/Cursors/SizeAllCursor.cur", UriKind.RelativeOrAbsolute));
    if (this.info != null)
      this.scrollingCursor = new Cursor(this.info.Stream);
    this.valueThumb.Style = resourceDictionary[(object) "ExtendedScrollingAdornerStyle"] as Style;
    if (this.fElement != null)
    {
      if (this.fElement.IsLoaded)
        this.ArrangeThumb();
      this.fElement.PreviewMouseMove += new MouseEventHandler(this.fElement_PreviewMouseMove);
      this.fElement.IsKeyboardFocusedChanged += new DependencyPropertyChangedEventHandler(this.fElement_IsKeyboardFocusedChanged);
      this.fElement.IsKeyboardFocusWithinChanged += new DependencyPropertyChangedEventHandler(this.fElement_IsKeyboardFocusWithinChanged);
    }
    this.valueThumb.DragDelta += new DragDeltaEventHandler(this.valueThumb_DragDelta);
    this.valueThumb.PreviewMouseUp += new MouseButtonEventHandler(this.valueThumb_PreviewMouseUp);
    this.valueThumb.PreviewMouseDown += new MouseButtonEventHandler(this.valueThumb_PreviewMouseDown);
    this.valueThumb.PreviewMouseMove += new MouseEventHandler(this.valueThumb_PreviewMouseMove);
  }

  private void valueThumb_PreviewMouseMove(object sender, MouseEventArgs e)
  {
    if (!this.IsReadOnly || this.valueThumb == null)
      return;
    this.valueThumb.Visibility = Visibility.Collapsed;
  }

  private void fElement_IsKeyboardFocusWithinChanged(
    object sender,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(bool) e.NewValue || this.valueThumb == null)
      return;
    this.valueThumb.Visibility = Visibility.Collapsed;
    this.valueThumb.Cursor = Cursors.IBeam;
  }

  private void fElement_IsKeyboardFocusedChanged(
    object sender,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(bool) e.NewValue || this.valueThumb == null)
      return;
    this.valueThumb.Visibility = Visibility.Collapsed;
    this.valueThumb.Cursor = Cursors.IBeam;
  }

  private void valueThumb_PreviewMouseDown(object sender, MouseButtonEventArgs e)
  {
    CursorHandler.GetCursorPos(out this.pnt);
  }

  private void fElement_PreviewMouseMove(object sender, MouseEventArgs e)
  {
    if (this.valueThumb == null)
      return;
    if (!this.IsReadOnly)
    {
      if (this.fElement.IsFocused)
        return;
      this.valueThumb.Visibility = Visibility.Visible;
      this.valueThumb.Cursor = this.scrollingCursor;
    }
    else
    {
      this.valueThumb.Visibility = Visibility.Collapsed;
      this.valueThumb.Cursor = Cursors.IBeam;
    }
  }

  private void valueThumb_PreviewMouseUp(object sender, MouseButtonEventArgs e)
  {
    if (this.AdornedElement is TextBox adornedElement)
    {
      if (!adornedElement.IsFocused && !this.isThumbMoved)
      {
        adornedElement.Focus();
        if (this.valueThumb != null)
          this.valueThumb.Visibility = Visibility.Collapsed;
      }
      else
      {
        if (this.valueThumb != null)
          this.valueThumb.Cursor = this.scrollingCursor;
        CursorHandler.SetCursorPos(this.pnt.X, this.pnt.Y);
      }
    }
    this.isThumbMoved = false;
  }

  private void valueThumb_DragDelta(object sender, DragDeltaEventArgs e)
  {
    if (this.valueThumb != null)
      this.valueThumb.Cursor = Cursors.None;
    if (this.IsReadOnly)
      return;
    if (e.HorizontalChange > this.prevHorizontalChange || e.VerticalChange < this.prevVerticalChange)
      this.ValueChange(true);
    else
      this.ValueChange(false);
    this.prevHorizontalChange = e.HorizontalChange;
    this.prevVerticalChange = e.VerticalChange;
    if (Math.Abs(e.HorizontalChange) <= 5.0 && Math.Abs(e.VerticalChange) <= 5.0)
      return;
    this.isThumbMoved = true;
  }

  private void ValueChange(bool increase)
  {
    if (this.AdornedElement is IntegerTextBox)
    {
      IntegerTextBox adornedElement = this.AdornedElement as IntegerTextBox;
      if (increase)
      {
        long? nullable1 = adornedElement.Value;
        long scrollInterval = (long) adornedElement.ScrollInterval;
        long? nullable2 = nullable1.HasValue ? new long?(nullable1.GetValueOrDefault() + scrollInterval) : new long?();
        long? nullable3 = nullable2;
        long maxValue = adornedElement.MaxValue;
        if ((nullable3.GetValueOrDefault() > maxValue ? 0 : (nullable3.HasValue ? 1 : 0)) != 0 && (adornedElement.MaxLength == 0 || nullable2.ToString().Length <= adornedElement.MaxLength))
          adornedElement.Value = nullable2;
      }
      else
      {
        long? nullable4 = adornedElement.Value;
        long scrollInterval = (long) adornedElement.ScrollInterval;
        long? nullable5 = nullable4.HasValue ? new long?(nullable4.GetValueOrDefault() - scrollInterval) : new long?();
        long? nullable6 = nullable5;
        long minValue = adornedElement.MinValue;
        if ((nullable6.GetValueOrDefault() < minValue ? 0 : (nullable6.HasValue ? 1 : 0)) != 0)
          adornedElement.Value = nullable5;
      }
    }
    if (this.AdornedElement is DoubleTextBox)
    {
      DoubleTextBox adornedElement = this.AdornedElement as DoubleTextBox;
      if (increase)
      {
        double? nullable7 = adornedElement.Value;
        double scrollInterval = adornedElement.ScrollInterval;
        double? nullable8 = nullable7.HasValue ? new double?(nullable7.GetValueOrDefault() + scrollInterval) : new double?();
        double? nullable9 = nullable8;
        double maxValue = adornedElement.MaxValue;
        if ((nullable9.GetValueOrDefault() > maxValue ? 0 : (nullable9.HasValue ? 1 : 0)) != 0 && (adornedElement.MaxLength == 0 || nullable8.ToString().Length <= adornedElement.MaxLength))
          adornedElement.Value = nullable8;
      }
      else
      {
        double? nullable10 = adornedElement.Value;
        double scrollInterval = adornedElement.ScrollInterval;
        double? nullable11 = nullable10.HasValue ? new double?(nullable10.GetValueOrDefault() - scrollInterval) : new double?();
        double? nullable12 = nullable11;
        double minValue = adornedElement.MinValue;
        if ((nullable12.GetValueOrDefault() < minValue ? 0 : (nullable12.HasValue ? 1 : 0)) != 0)
          adornedElement.Value = nullable11;
      }
    }
    if (this.AdornedElement is PercentTextBox)
    {
      PercentTextBox adornedElement = this.AdornedElement as PercentTextBox;
      if (increase)
      {
        double? percentValue = adornedElement.PercentValue;
        double scrollInterval = adornedElement.ScrollInterval;
        double? nullable13 = percentValue.HasValue ? new double?(percentValue.GetValueOrDefault() + scrollInterval) : new double?();
        double? nullable14 = nullable13;
        double maxValue = adornedElement.MaxValue;
        if ((nullable14.GetValueOrDefault() > maxValue ? 0 : (nullable14.HasValue ? 1 : 0)) != 0)
        {
          double? nullable15 = nullable13;
          double minValue = adornedElement.MinValue;
          if ((nullable15.GetValueOrDefault() < minValue ? 0 : (nullable15.HasValue ? 1 : 0)) != 0)
            adornedElement.PercentValue = nullable13;
        }
      }
      else
      {
        double? percentValue = adornedElement.PercentValue;
        double scrollInterval = adornedElement.ScrollInterval;
        double? nullable16 = percentValue.HasValue ? new double?(percentValue.GetValueOrDefault() - scrollInterval) : new double?();
        double? nullable17 = nullable16;
        double minValue = adornedElement.MinValue;
        if ((nullable17.GetValueOrDefault() < minValue ? 0 : (nullable17.HasValue ? 1 : 0)) != 0)
          adornedElement.PercentValue = nullable16;
      }
    }
    if (this.AdornedElement is CurrencyTextBox)
    {
      CurrencyTextBox adornedElement = this.AdornedElement as CurrencyTextBox;
      if (increase)
      {
        Decimal? nullable18 = adornedElement.Value;
        Decimal scrollInterval = (Decimal) adornedElement.ScrollInterval;
        Decimal? nullable19 = nullable18.HasValue ? new Decimal?(nullable18.GetValueOrDefault() + scrollInterval) : new Decimal?();
        Decimal? nullable20 = nullable19;
        Decimal maxValue = adornedElement.MaxValue;
        if ((!(nullable20.GetValueOrDefault() <= maxValue) ? 0 : (nullable20.HasValue ? 1 : 0)) != 0 && (adornedElement.MaxLength == 0 || nullable19.ToString().Length <= adornedElement.MaxLength))
          adornedElement.Value = nullable19;
      }
      else
      {
        Decimal? nullable21 = adornedElement.Value;
        Decimal scrollInterval = (Decimal) adornedElement.ScrollInterval;
        Decimal? nullable22 = nullable21.HasValue ? new Decimal?(nullable21.GetValueOrDefault() - scrollInterval) : new Decimal?();
        Decimal? nullable23 = nullable22;
        Decimal minValue = adornedElement.MinValue;
        if ((!(nullable23.GetValueOrDefault() >= minValue) ? 0 : (nullable23.HasValue ? 1 : 0)) != 0)
          adornedElement.Value = nullable22;
      }
    }
    if (!(this.AdornedElement is TimeSpanEdit))
      return;
    TimeSpanEdit adornedElement1 = this.AdornedElement as TimeSpanEdit;
    if (increase)
      adornedElement1.UpExecute();
    else
      adornedElement1.DownExecute();
  }

  private void ArrangeThumb()
  {
    if (this.valueThumb == null)
      return;
    if (this.fElement != null)
      this.valueThumb.Arrange(new Rect(this.adornedElementRect.TopLeft.X, this.adornedElementRect.TopLeft.Y, this.fElement.RenderSize.Width, this.fElement.RenderSize.Height));
    this.valueThumb.Visibility = Visibility.Collapsed;
  }

  protected override int VisualChildrenCount => this.visualCollection.Count;

  protected override Visual GetVisualChild(int index)
  {
    return this.visualCollection.Count > index ? this.visualCollection[index] : (Visual) null;
  }
}
