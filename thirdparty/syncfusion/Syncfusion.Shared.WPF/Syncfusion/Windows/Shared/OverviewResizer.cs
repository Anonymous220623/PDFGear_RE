// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.OverviewResizer
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Shared;

public class OverviewResizer : ContentControl
{
  private Overview ov;
  private double horChange;
  private double verChange;
  private double initWidth;
  private double initHeight;
  private double initx;
  private double inity;
  private double startMouseX;
  private double startMouseY;

  public OverviewResizer() => this.DefaultStyleKey = (object) typeof (OverviewResizer);

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.ov = OverviewResizer.FindParent<Overview>((UIElement) this);
    Thumb templateChild1 = this.GetTemplateChild("PART_OverViewTopLeftCorner") as Thumb;
    Thumb templateChild2 = this.GetTemplateChild("PART_OverViewTop") as Thumb;
    Thumb templateChild3 = this.GetTemplateChild("PART_OverViewTopRightCorner") as Thumb;
    Thumb templateChild4 = this.GetTemplateChild("PART_OverViewBottom") as Thumb;
    Thumb templateChild5 = this.GetTemplateChild("PART_OverViewBottomRightCorner") as Thumb;
    Thumb templateChild6 = this.GetTemplateChild("PART_OverViewBottomLeftCorner") as Thumb;
    Thumb templateChild7 = this.GetTemplateChild("PART_OverViewRight") as Thumb;
    Thumb templateChild8 = this.GetTemplateChild("PART_OverViewLeft") as Thumb;
    templateChild2.DragStarted += new DragStartedEventHandler(this.OverviewResizer_DragStarted);
    templateChild2.DragDelta += new DragDeltaEventHandler(this.OverviewResizer_DragDelta);
    templateChild2.DragCompleted += new DragCompletedEventHandler(this.OverviewResizer_DragDeltaComplete);
    templateChild1.DragStarted += new DragStartedEventHandler(this.OverviewResizer_DragStarted);
    templateChild1.DragDelta += new DragDeltaEventHandler(this.OverviewResizer_DragDelta);
    templateChild1.DragCompleted += new DragCompletedEventHandler(this.OverviewResizer_DragDeltaComplete);
    templateChild3.DragStarted += new DragStartedEventHandler(this.OverviewResizer_DragStarted);
    templateChild3.DragDelta += new DragDeltaEventHandler(this.OverviewResizer_DragDelta);
    templateChild3.DragCompleted += new DragCompletedEventHandler(this.OverviewResizer_DragDeltaComplete);
    templateChild4.DragStarted += new DragStartedEventHandler(this.OverviewResizer_DragStarted);
    templateChild4.DragDelta += new DragDeltaEventHandler(this.OverviewResizer_DragDelta);
    templateChild4.DragCompleted += new DragCompletedEventHandler(this.OverviewResizer_DragDeltaComplete);
    templateChild5.DragStarted += new DragStartedEventHandler(this.OverviewResizer_DragStarted);
    templateChild5.DragDelta += new DragDeltaEventHandler(this.OverviewResizer_DragDelta);
    templateChild5.DragCompleted += new DragCompletedEventHandler(this.OverviewResizer_DragDeltaComplete);
    templateChild6.DragStarted += new DragStartedEventHandler(this.OverviewResizer_DragStarted);
    templateChild6.DragDelta += new DragDeltaEventHandler(this.OverviewResizer_DragDelta);
    templateChild6.DragCompleted += new DragCompletedEventHandler(this.OverviewResizer_DragDeltaComplete);
    templateChild7.DragStarted += new DragStartedEventHandler(this.OverviewResizer_DragStarted);
    templateChild7.DragDelta += new DragDeltaEventHandler(this.OverviewResizer_DragDelta);
    templateChild7.DragCompleted += new DragCompletedEventHandler(this.OverviewResizer_DragDeltaComplete);
    templateChild8.DragStarted += new DragStartedEventHandler(this.OverviewResizer_DragStarted);
    templateChild8.DragDelta += new DragDeltaEventHandler(this.OverviewResizer_DragDelta);
    templateChild8.DragCompleted += new DragCompletedEventHandler(this.OverviewResizer_DragDeltaComplete);
  }

  private void OverviewResizer_DragStarted(object sender, DragStartedEventArgs e)
  {
    if (!this.ov.AllowResize)
      return;
    this.ov.IsResizing = true;
    this.horChange = this.verChange = 0.0;
    this.initWidth = this.ov.VpWidth;
    this.initHeight = this.ov.VpHeight;
    this.initx = this.ov.VpOffsetX;
    this.inity = this.ov.VpOffsetY;
    this.startMouseX = Mouse.GetPosition((IInputElement) this.ov).X;
    this.startMouseY = Mouse.GetPosition((IInputElement) this.ov).Y;
  }

  private void OverviewResizer_DragDelta(object sender, DragDeltaEventArgs e)
  {
    if (!this.ov.AllowResize)
      return;
    VerticalAlignment verticalAlignment = (sender as Thumb).VerticalAlignment;
    HorizontalAlignment horizontalAlignment = (sender as Thumb).HorizontalAlignment;
    this.horChange = Mouse.GetPosition((IInputElement) this.ov).X - this.startMouseX;
    this.verChange = Mouse.GetPosition((IInputElement) this.ov).Y - this.startMouseY;
    this.ov.IsResized = true;
    double num1 = (this.initWidth + this.horChange) / this.initWidth;
    double num2 = (this.initHeight + this.verChange) / this.initHeight;
    double num3 = (this.initWidth - this.horChange) / this.initWidth;
    double num4 = (this.initHeight - this.verChange) / this.initHeight;
    double num5 = 8.0;
    switch (verticalAlignment)
    {
      case VerticalAlignment.Top:
        switch (horizontalAlignment)
        {
          case HorizontalAlignment.Left:
            if (Math.Abs(num4) > Math.Abs(num3))
            {
              if (this.initWidth * num4 <= num5 || this.initHeight * num4 <= num5)
                return;
              this.ov.VpOffsetY = this.inity - (this.initHeight * num4 - this.initHeight);
              (this.ov.Trans as TranslateTransform).Y = this.ov.VpOffsetY;
              this.ov.VpOffsetX = this.initx - (this.initWidth * num4 - this.initWidth);
              (this.ov.Trans as TranslateTransform).X = this.ov.VpOffsetX;
              this.ov.VpWidth = this.initWidth * num4;
              this.ov.VpHeight = this.initHeight * num4;
              return;
            }
            if (this.initWidth * num3 <= num5 || this.initHeight * num3 <= num5)
              return;
            this.ov.VpOffsetY = this.inity - (this.initHeight * num3 - this.initHeight);
            (this.ov.Trans as TranslateTransform).Y = this.ov.VpOffsetY;
            this.ov.VpOffsetX = this.initx - (this.initWidth * num3 - this.initWidth);
            (this.ov.Trans as TranslateTransform).X = this.ov.VpOffsetX;
            this.ov.VpWidth = this.initWidth * num3;
            this.ov.VpHeight = this.initHeight * num3;
            return;
          case HorizontalAlignment.Right:
            if (Math.Abs(num4) > Math.Abs(num1))
            {
              if (this.initWidth * num4 <= num5 || this.initHeight * num4 <= num5)
                return;
              this.ov.VpOffsetY = this.inity - (this.initHeight * num4 - this.initHeight);
              (this.ov.Trans as TranslateTransform).Y = this.ov.VpOffsetY;
              this.ov.VpWidth = this.initWidth * num4;
              this.ov.VpHeight = this.initHeight * num4;
              return;
            }
            if (this.initWidth * num1 <= num5 || this.initHeight * num1 <= num5)
              return;
            this.ov.VpOffsetY = this.inity - (this.initHeight * num1 - this.initHeight);
            (this.ov.Trans as TranslateTransform).Y = this.ov.VpOffsetY;
            this.ov.VpWidth = this.initWidth * num1;
            this.ov.VpHeight = this.initHeight * num1;
            return;
          case HorizontalAlignment.Stretch:
            if (this.initHeight * num4 <= num5 || this.initWidth * num4 <= num5)
              return;
            this.ov.VpWidth = this.initWidth * num4;
            this.ov.VpHeight = this.initHeight * num4;
            this.ov.VpOffsetY = this.inity - (this.initHeight * num4 - this.initHeight);
            (this.ov.Trans as TranslateTransform).Y = this.ov.VpOffsetY;
            this.ov.VpOffsetX = this.initx - (this.initWidth * num4 - this.initWidth) / 2.0;
            (this.ov.Trans as TranslateTransform).X = this.ov.VpOffsetX;
            return;
          default:
            return;
        }
      case VerticalAlignment.Bottom:
        switch (horizontalAlignment)
        {
          case HorizontalAlignment.Left:
            if (Math.Abs(num2) > Math.Abs(num3))
            {
              if (this.initWidth * num2 <= num5 || this.initHeight * num2 <= num5)
                return;
              this.ov.VpOffsetX = this.initx - (this.initWidth * num2 - this.initWidth);
              (this.ov.Trans as TranslateTransform).X = this.ov.VpOffsetX;
              this.ov.VpWidth = this.initWidth * num2;
              this.ov.VpHeight = this.initHeight * num2;
              return;
            }
            if (this.initWidth * num3 <= num5 || this.initHeight * num3 <= num5)
              return;
            this.ov.VpOffsetX = this.initx - (this.initWidth * num3 - this.initWidth);
            (this.ov.Trans as TranslateTransform).X = this.ov.VpOffsetX;
            this.ov.VpWidth = this.initWidth * num3;
            this.ov.VpHeight = this.initHeight * num3;
            return;
          case HorizontalAlignment.Right:
            if (num2 > num1)
            {
              if (this.initWidth * num2 <= num5 || this.initHeight * num2 <= num5)
                return;
              this.ov.VpWidth = this.initWidth * num2;
              this.ov.VpHeight = this.initHeight * num2;
              return;
            }
            if (this.initWidth * num1 <= num5 || this.initHeight * num1 <= num5)
              return;
            this.ov.VpWidth = this.initWidth * num1;
            this.ov.VpHeight = this.initHeight * num1;
            return;
          case HorizontalAlignment.Stretch:
            if (this.initHeight * num2 <= num5 || this.initWidth * num2 <= num5)
              return;
            this.ov.VpWidth = this.initWidth * num2;
            this.ov.VpHeight = this.initHeight * num2;
            this.ov.VpOffsetX = this.initx - (this.initWidth * num2 - this.initWidth) / 2.0;
            (this.ov.Trans as TranslateTransform).X = this.ov.VpOffsetX;
            return;
          default:
            return;
        }
      case VerticalAlignment.Stretch:
        switch (horizontalAlignment)
        {
          case HorizontalAlignment.Left:
            if (this.initWidth * num3 <= num5 || this.initHeight * num3 <= num5)
              return;
            this.ov.VpWidth = this.initWidth * num3;
            this.ov.VpHeight = this.initHeight * num3;
            this.ov.VpOffsetX = this.initx - (this.initWidth * num3 - this.initWidth);
            (this.ov.Trans as TranslateTransform).X = this.ov.VpOffsetX;
            this.ov.VpOffsetY = this.inity - (this.initHeight * num3 - this.initHeight) / 2.0;
            (this.ov.Trans as TranslateTransform).Y = this.ov.VpOffsetY;
            return;
          case HorizontalAlignment.Right:
            if (this.initWidth * num1 <= num5 || this.initHeight * num1 <= num5)
              return;
            this.ov.VpWidth = this.initWidth * num1;
            this.ov.VpHeight = this.initHeight * num1;
            this.ov.VpOffsetY = this.inity - (this.initHeight * num1 - this.initHeight) / 2.0;
            (this.ov.Trans as TranslateTransform).Y = this.ov.VpOffsetY;
            return;
          case HorizontalAlignment.Stretch:
            return;
          default:
            return;
        }
    }
  }

  private void OverviewResizer_DragDeltaComplete(object sender, DragCompletedEventArgs e)
  {
    if (!this.ov.AllowResize)
      return;
    this.ov.UpdateScrollViewer();
    this.ov.IsResizing = false;
  }

  private static T FindParent<T>(UIElement control) where T : UIElement
  {
    if (!(VisualTreeHelper.GetParent((DependencyObject) control) is UIElement parent))
      return default (T);
    return parent is T ? parent as T : OverviewResizer.FindParent<T>(parent);
  }
}
