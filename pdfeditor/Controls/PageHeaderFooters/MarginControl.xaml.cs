// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.PageHeaderFooters.MarginControl
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Shapes;

#nullable disable
namespace pdfeditor.Controls.PageHeaderFooters;

public partial class MarginControl : UserControl, IComponentConnector
{
  public static readonly DependencyProperty PreviewFontSizeProperty = DependencyProperty.Register(nameof (PreviewFontSize), typeof (double), typeof (MarginControl), new PropertyMetadata((object) 10.0, (PropertyChangedCallback) ((s, a) => ((MarginControl) s).UpdateLinePosition())));
  public static readonly DependencyProperty PageOriginalWidthProperty = DependencyProperty.Register(nameof (PageOriginalWidth), typeof (double), typeof (MarginControl), new PropertyMetadata((object) 595.0, (PropertyChangedCallback) ((s, a) => ((MarginControl) s).UpdateLinePosition())));
  public static readonly DependencyProperty MarginLeftProperty = DependencyProperty.Register(nameof (MarginLeft), typeof (double), typeof (MarginControl), new PropertyMetadata((object) 0.0, (PropertyChangedCallback) ((s, a) => ((MarginControl) s).UpdateLinePosition())));
  public static readonly DependencyProperty MarginTopProperty = DependencyProperty.Register(nameof (MarginTop), typeof (double), typeof (MarginControl), new PropertyMetadata((object) 0.0, (PropertyChangedCallback) ((s, a) => ((MarginControl) s).UpdateLinePosition())));
  public static readonly DependencyProperty MarginRightProperty = DependencyProperty.Register(nameof (MarginRight), typeof (double), typeof (MarginControl), new PropertyMetadata((object) 0.0, (PropertyChangedCallback) ((s, a) => ((MarginControl) s).UpdateLinePosition())));
  public static readonly DependencyProperty MarginBottomProperty = DependencyProperty.Register(nameof (MarginBottom), typeof (double), typeof (MarginControl), new PropertyMetadata((object) 0.0, (PropertyChangedCallback) ((s, a) => ((MarginControl) s).UpdateLinePosition())));
  public static readonly DependencyProperty EdgeProperty = DependencyProperty.Register(nameof (Edge), typeof (ToreEdge), typeof (MarginControl), new PropertyMetadata((object) ToreEdge.Top, (PropertyChangedCallback) ((s, a) =>
  {
    MarginControl marginControl = (MarginControl) s;
    marginControl.LeftTextBlock.VerticalAlignment = (ToreEdge) a.NewValue == ToreEdge.Top ? VerticalAlignment.Top : VerticalAlignment.Bottom;
    marginControl.CenterTextBlock.VerticalAlignment = (ToreEdge) a.NewValue == ToreEdge.Top ? VerticalAlignment.Top : VerticalAlignment.Bottom;
    marginControl.RightTextBlock.VerticalAlignment = (ToreEdge) a.NewValue == ToreEdge.Top ? VerticalAlignment.Top : VerticalAlignment.Bottom;
    marginControl.UpdateLinePosition();
  })));
  public static readonly DependencyProperty LeftStringProperty = DependencyProperty.Register(nameof (LeftString), typeof (string), typeof (MarginControl), new PropertyMetadata((object) "", (PropertyChangedCallback) ((s, a) => ((MarginControl) s).UpdateLinePosition())));
  public static readonly DependencyProperty CenterStringProperty = DependencyProperty.Register(nameof (CenterString), typeof (string), typeof (MarginControl), new PropertyMetadata((object) "", (PropertyChangedCallback) ((s, a) => ((MarginControl) s).UpdateLinePosition())));
  public static readonly DependencyProperty RightStringProperty = DependencyProperty.Register(nameof (RightString), typeof (string), typeof (MarginControl), new PropertyMetadata((object) "", (PropertyChangedCallback) ((s, a) => ((MarginControl) s).UpdateLinePosition())));
  internal TextBlock LeftTextBlock;
  internal TextBlock CenterTextBlock;
  internal TextBlock RightTextBlock;
  internal Line Line1;
  internal Line Line2;
  internal Line Line3;
  private bool _contentLoaded;

  public MarginControl()
  {
    this.InitializeComponent();
    this.SizeChanged += new SizeChangedEventHandler(this.MarginControl_SizeChanged);
    this.LeftTextBlock.SizeChanged += new SizeChangedEventHandler(this.TextBlock_SizeChanged);
    this.CenterTextBlock.SizeChanged += new SizeChangedEventHandler(this.TextBlock_SizeChanged);
    this.RightTextBlock.SizeChanged += new SizeChangedEventHandler(this.TextBlock_SizeChanged);
  }

  private void TextBlock_SizeChanged(object sender, SizeChangedEventArgs e)
  {
    this.UpdateLinePosition();
  }

  private void MarginControl_SizeChanged(object sender, SizeChangedEventArgs e)
  {
    this.UpdateLinePosition();
  }

  public double PreviewFontSize
  {
    get => (double) this.GetValue(MarginControl.PreviewFontSizeProperty);
    set => this.SetValue(MarginControl.PreviewFontSizeProperty, (object) value);
  }

  public double PageOriginalWidth
  {
    get => (double) this.GetValue(MarginControl.PageOriginalWidthProperty);
    set => this.SetValue(MarginControl.PageOriginalWidthProperty, (object) value);
  }

  public double MarginLeft
  {
    get => (double) this.GetValue(MarginControl.MarginLeftProperty);
    set => this.SetValue(MarginControl.MarginLeftProperty, (object) value);
  }

  public double MarginTop
  {
    get => (double) this.GetValue(MarginControl.MarginTopProperty);
    set => this.SetValue(MarginControl.MarginTopProperty, (object) value);
  }

  public double MarginRight
  {
    get => (double) this.GetValue(MarginControl.MarginRightProperty);
    set => this.SetValue(MarginControl.MarginRightProperty, (object) value);
  }

  public double MarginBottom
  {
    get => (double) this.GetValue(MarginControl.MarginBottomProperty);
    set => this.SetValue(MarginControl.MarginBottomProperty, (object) value);
  }

  public ToreEdge Edge
  {
    get => (ToreEdge) this.GetValue(MarginControl.EdgeProperty);
    set => this.SetValue(MarginControl.EdgeProperty, (object) value);
  }

  public string LeftString
  {
    get => (string) this.GetValue(MarginControl.LeftStringProperty);
    set => this.SetValue(MarginControl.LeftStringProperty, (object) value);
  }

  public string CenterString
  {
    get => (string) this.GetValue(MarginControl.CenterStringProperty);
    set => this.SetValue(MarginControl.CenterStringProperty, (object) value);
  }

  public string RightString
  {
    get => (string) this.GetValue(MarginControl.RightStringProperty);
    set => this.SetValue(MarginControl.RightStringProperty, (object) value);
  }

  private void UpdatePreviewFontSize()
  {
    if (this.ActualWidth == 0.0 || this.ActualHeight == 0.0)
      return;
    double num = this.PreviewFontSize * (this.ActualWidth / this.PageOriginalWidth);
    this.LeftTextBlock.FontSize = num;
    this.CenterTextBlock.FontSize = num;
    this.RightTextBlock.FontSize = num;
  }

  private void UpdateLinePosition()
  {
    if (this.ActualWidth == 0.0 || this.ActualHeight == 0.0)
      return;
    double pageOriginalWidth = this.PageOriginalWidth;
    double actualWidth = this.ActualWidth;
    double num1 = actualWidth / pageOriginalWidth;
    Line line2 = this.Line2;
    Line line3 = this.Line3;
    double left = this.MarginLeft * num1;
    double right = this.MarginRight * num1;
    line2.X1 = left;
    line2.X2 = left;
    line2.Y1 = 0.0;
    line2.Y2 = this.ActualHeight;
    line3.X1 = actualWidth - right;
    line3.X2 = actualWidth - right;
    line3.Y1 = 0.0;
    line3.Y2 = this.ActualHeight;
    this.Line1.X1 = 0.0;
    this.Line1.X2 = actualWidth;
    this.UpdatePreviewFontSize();
    if (this.Edge == ToreEdge.Top)
    {
      double num2 = this.MarginTop * num1;
      this.Line1.Y1 = num2 + 1.0;
      this.Line1.Y2 = num2 + 1.0;
      this.LeftTextBlock.Margin = new Thickness(left, num2 - MarginControl.GetTextBlockHeight(this.LeftTextBlock), 0.0, 0.0);
      this.CenterTextBlock.Margin = new Thickness(0.0, num2 - MarginControl.GetTextBlockHeight(this.CenterTextBlock), 0.0, 0.0);
      this.RightTextBlock.Margin = new Thickness(0.0, num2 - MarginControl.GetTextBlockHeight(this.RightTextBlock), right, 0.0);
    }
    else
    {
      if (this.Edge != ToreEdge.Bottom)
        return;
      double num3 = this.MarginBottom * num1;
      this.Line1.Y1 = this.ActualHeight - num3 - 1.0;
      this.Line1.Y2 = this.ActualHeight - num3 - 1.0;
      this.LeftTextBlock.Margin = new Thickness(left, 0.0, 0.0, num3 - MarginControl.GetTextBlockHeight(this.LeftTextBlock));
      this.CenterTextBlock.Margin = new Thickness(0.0, 0.0, 0.0, num3 - MarginControl.GetTextBlockHeight(this.CenterTextBlock));
      this.RightTextBlock.Margin = new Thickness(0.0, 0.0, right, num3 - MarginControl.GetTextBlockHeight(this.RightTextBlock));
    }
  }

  private static double GetTextBlockHeight(TextBlock block)
  {
    return !double.IsNaN(block.ActualHeight) ? block.ActualHeight : 0.0;
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/pageheaderfooters/margincontrol.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this.LeftTextBlock = (TextBlock) target;
        break;
      case 2:
        this.CenterTextBlock = (TextBlock) target;
        break;
      case 3:
        this.RightTextBlock = (TextBlock) target;
        break;
      case 4:
        this.Line1 = (Line) target;
        break;
      case 5:
        this.Line2 = (Line) target;
        break;
      case 6:
        this.Line3 = (Line) target;
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
