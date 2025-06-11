// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Annotations.StampAnnotationMoveControl
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using pdfeditor.Controls.Stamp;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Shapes;

#nullable disable
namespace pdfeditor.Controls.Annotations;

public partial class StampAnnotationMoveControl : UserControl, IComponentConnector
{
  private TextStampModel textModel;
  private ImageStampModel imageModel;
  internal Canvas LayoutRoot;
  internal Rectangle DashBorder;
  internal Border TextContentBorder;
  internal Image contentImage;
  private bool _contentLoaded;

  public StampAnnotationMoveControl(TextStampModel textmodel)
  {
    this.InitializeComponent();
    this.contentImage.Visibility = Visibility.Collapsed;
    this.TextContentBorder.Visibility = Visibility.Visible;
    this.textModel = textmodel;
    this.TextContentBorder.DataContext = (object) this.textModel;
    this.contentImage.DataContext = (object) this.imageModel;
    this.TextContentBorder.Child = (UIElement) new StampDefaultTextPreview()
    {
      StampModel = (object) new CustStampModel()
      {
        Text = "Visible",
        TextContent = this.textModel.Text,
        FontColor = this.textModel.Foreground,
        TimeFormat = this.textModel.TimeFormat
      }
    };
    this.TextContentBorder.SizeChanged += new SizeChangedEventHandler(this.TextContentBorder_SizeChanged);
  }

  public StampAnnotationMoveControl(ImageStampModel imgmodel)
  {
    this.InitializeComponent();
    this.contentImage.Visibility = Visibility.Visible;
    this.TextContentBorder.Visibility = Visibility.Collapsed;
    this.imageModel = imgmodel;
    this.contentImage.DataContext = (object) this.imageModel;
    this.TextContentBorder.DataContext = (object) this.textModel;
    this.DashBorder.Width = imgmodel.ImageWidth;
    this.DashBorder.Height = imgmodel.ImageHeight;
  }

  public ImageStampModel ImageModel => this.imageModel;

  public TextStampModel TextModel => this.textModel;

  private void TextContentBorder_SizeChanged(object sender, SizeChangedEventArgs e)
  {
    this.DashBorder.Width = e.NewSize.Width;
    this.DashBorder.Height = e.NewSize.Height;
  }

  public Rect Bounds
  {
    get
    {
      if (this.TextModel == null && this.ImageModel == null)
        return Rect.Empty;
      double num1 = Canvas.GetLeft((UIElement) this);
      double num2 = Canvas.GetTop((UIElement) this);
      if (double.IsNaN(num1))
        num1 = 0.0;
      if (double.IsNaN(num2))
        num2 = 0.0;
      if (this.TextModel != null)
        return new Rect(num1, num2, this.TextModel.TextWidth, this.TextModel.TextHeight);
      return this.ImageModel != null ? new Rect(num1, num2, this.ImageModel.ImageWidth, this.ImageModel.ImageHeight) : Rect.Empty;
    }
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/annotations/stampannotationmovecontrol.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this.LayoutRoot = (Canvas) target;
        break;
      case 2:
        this.DashBorder = (Rectangle) target;
        break;
      case 3:
        this.TextContentBorder = (Border) target;
        break;
      case 4:
        this.contentImage = (Image) target;
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
