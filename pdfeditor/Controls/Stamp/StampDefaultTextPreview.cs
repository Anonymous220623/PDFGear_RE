// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Stamp.StampDefaultTextPreview
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using pdfeditor.Controls.Annotations;
using pdfeditor.ViewModels;
using PDFKit.Utils.StampUtils;
using System;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace pdfeditor.Controls.Stamp;

public class StampDefaultTextPreview : FrameworkElement
{
  private Visual child;
  public static readonly DependencyProperty StampModelProperty = DependencyProperty.Register(nameof (StampModel), typeof (object), typeof (StampDefaultTextPreview), new PropertyMetadata((object) null, (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is StampDefaultTextPreview defaultTextPreview2) || a.NewValue == a.OldValue)
      return;
    defaultTextPreview2.UpdateStampModel();
  })));
  public static readonly DependencyProperty BorderThicknessProperty = DependencyProperty.Register(nameof (BorderThickness), typeof (double), typeof (StampDefaultTextPreview), new PropertyMetadata((object) 2.0, (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is StampDefaultTextPreview defaultTextPreview4) || a.NewValue == a.OldValue)
      return;
    defaultTextPreview4.UpdateStampModel();
  })));

  public StampDefaultTextPreview()
  {
    this.HorizontalAlignment = HorizontalAlignment.Stretch;
    this.VerticalAlignment = VerticalAlignment.Stretch;
    this.SizeChanged += new SizeChangedEventHandler(this.StampDefaultTextPreview_SizeChanged);
  }

  public object StampModel
  {
    get => this.GetValue(StampDefaultTextPreview.StampModelProperty);
    set => this.SetValue(StampDefaultTextPreview.StampModelProperty, value);
  }

  public double BorderThickness
  {
    get => (double) this.GetValue(StampDefaultTextPreview.BorderThicknessProperty);
    set => this.SetValue(StampDefaultTextPreview.BorderThicknessProperty, (object) value);
  }

  public void ForceRender() => this.UpdateStampModel();

  private void UpdateStampModel()
  {
    if (this.child != null)
      this.RemoveVisualChild(this.child);
    this.child = (Visual) null;
    if (this.ActualWidth > 0.0)
    {
      if (this.ActualHeight > 0.0)
      {
        try
        {
          string content = "";
          string dateTimeFormat = "";
          Color color = new Color();
          if (this.StampModel is CustStampModel stampModel3)
          {
            if (stampModel3 != null && stampModel3.Text == "Visible" && !string.IsNullOrEmpty(stampModel3.TextContent))
            {
              content = stampModel3.TextContent;
              color = (Color) ColorConverter.ConvertFromString(stampModel3.FontColor);
            }
          }
          else if (this.StampModel is TextStampModel stampModel2)
          {
            content = stampModel2.Text;
            color = (Color) ColorConverter.ConvertFromString(stampModel2.Foreground);
          }
          else if (this.StampModel is IStampTextModel stampModel1)
          {
            content = stampModel1.TextContent;
            color = (Color) ColorConverter.ConvertFromString(stampModel1.FontColor);
            StampTextModel stampTextModel = stampModel1 as StampTextModel;
          }
          if (!string.IsNullOrEmpty(content))
            this.child = StampUtil.CreateDefaultTextPreviewVisual(content, this.ActualWidth, this.ActualHeight, color, dateTimeFormat, CultureInfoUtils.ActualAppLanguage, this.BorderThickness);
        }
        catch
        {
        }
      }
    }
    if (this.child != null)
      this.AddVisualChild(this.child);
    this.InvalidateVisual();
  }

  private void StampDefaultTextPreview_SizeChanged(object sender, SizeChangedEventArgs e)
  {
    this.UpdateStampModel();
  }

  protected override int VisualChildrenCount => this.child != null ? 1 : 0;

  protected override Visual GetVisualChild(int index)
  {
    return index == 0 && this.child != null ? this.child : throw new ArgumentOutOfRangeException(nameof (index));
  }
}
