// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.Printing.CustomPageSizeDialog
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace Syncfusion.Windows.Shared.Printing;

public class CustomPageSizeDialog : Window, IComponentConnector
{
  private PrintOptionsControl optionsControl;
  internal TextBlock width;
  internal UpDown PrintWidthUPDowm;
  internal TextBlock height;
  internal UpDown HeightUPDowm;
  internal Button OkButton;
  internal Button CancelButton;
  private bool _contentLoaded;

  public CustomPageSizeDialog(PrintOptionsControl printOptionsControl, Size pagesize)
  {
    this.InitializeComponent();
    this.DataContext = (object) this;
    this.optionsControl = printOptionsControl;
    this.PageHeight = pagesize.Height;
    this.PageWidth = pagesize.Width;
  }

  public double PageHeight { get; set; }

  public double PageWidth { get; set; }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    if (!(this.GetTemplateChild("CloseButton") is Button templateChild))
      return;
    templateChild.ToolTip = (object) null;
  }

  protected override void OnClosing(CancelEventArgs e)
  {
    this.OkButton.Click -= new RoutedEventHandler(this.OkButton_Click);
    this.CancelButton.Click -= new RoutedEventHandler(this.CancelButton_Click);
    this.optionsControl = (PrintOptionsControl) null;
  }

  protected override void OnPreviewKeyDown(KeyEventArgs e)
  {
    if (e.Key == Key.Escape)
    {
      this.optionsControl.ResetPageSizeToPreviousValue();
      this.Close();
    }
    else if (e.Key == Key.Return)
    {
      this.optionsControl.SetPageSize(new Size(this.PageWidth, this.PageHeight));
      this.Close();
    }
    base.OnPreviewKeyDown(e);
  }

  private void CancelButton_Click(object sender, RoutedEventArgs e)
  {
    this.optionsControl.ResetPageSizeToPreviousValue();
    this.Close();
  }

  private void OkButton_Click(object sender, RoutedEventArgs e)
  {
    this.optionsControl.SetPageSize(new Size(this.PageWidth, this.PageHeight));
    this.Close();
  }

  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [DebuggerNonUserCode]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/Syncfusion.Shared.Wpf;component/print/themes/custompagesize.xaml", UriKind.Relative));
  }

  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [DebuggerNonUserCode]
  internal Delegate _CreateDelegate(Type delegateType, string handler)
  {
    return Delegate.CreateDelegate(delegateType, (object) this, handler);
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this.width = (TextBlock) target;
        break;
      case 2:
        this.PrintWidthUPDowm = (UpDown) target;
        break;
      case 3:
        this.height = (TextBlock) target;
        break;
      case 4:
        this.HeightUPDowm = (UpDown) target;
        break;
      case 5:
        this.OkButton = (Button) target;
        this.OkButton.Click += new RoutedEventHandler(this.OkButton_Click);
        break;
      case 6:
        this.CancelButton = (Button) target;
        this.CancelButton.Click += new RoutedEventHandler(this.CancelButton_Click);
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
