// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.Printing.CustomMarginDialog
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

public class CustomMarginDialog : Window, IComponentConnector
{
  private PrintOptionsControl OptionsControl;
  internal TextBlock left;
  internal UpDown PrintMarginLeftUPDowm;
  internal TextBlock right;
  internal UpDown PrintMarginRightUPDowm;
  internal TextBlock top;
  internal UpDown PrintMarginTopUPDowm;
  internal TextBlock bottom;
  internal UpDown PrintMarginBottomUPDowm;
  internal Button OkButton;
  internal Button CancelButton;
  private bool _contentLoaded;

  public CustomMarginDialog(PrintOptionsControl control, Thickness pageMargin)
  {
    this.InitializeComponent();
    this.OptionsControl = control;
    this.TopMargin = pageMargin.Top;
    this.BottomMargin = pageMargin.Bottom;
    this.LeftMargin = pageMargin.Left;
    this.RightMargin = pageMargin.Right;
    this.DataContext = (object) this;
  }

  public double LeftMargin { get; set; }

  public double RightMargin { get; set; }

  public double TopMargin { get; set; }

  public double BottomMargin { get; set; }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    if (!(this.GetTemplateChild("CloseButton") is Button templateChild))
      return;
    templateChild.ToolTip = (object) null;
  }

  protected override void OnPreviewKeyDown(KeyEventArgs e)
  {
    if (e.Key == Key.Escape)
    {
      this.OptionsControl.ResetPageMaringToPreviousValue();
      this.Close();
    }
    else if (e.Key == Key.Return)
    {
      this.OptionsControl.SetCustomMargin(new Thickness(this.LeftMargin, this.TopMargin, this.RightMargin, this.BottomMargin));
      this.Close();
    }
    base.OnPreviewKeyDown(e);
  }

  protected override void OnClosing(CancelEventArgs e)
  {
    this.OkButton.Click -= new RoutedEventHandler(this.OkButton_Click);
    this.CancelButton.Click -= new RoutedEventHandler(this.CancelButton_Click);
    this.OptionsControl = (PrintOptionsControl) null;
  }

  private void OkButton_Click(object sender, RoutedEventArgs e)
  {
    this.OptionsControl.SetCustomMargin(new Thickness(this.LeftMargin, this.TopMargin, this.RightMargin, this.BottomMargin));
    this.Close();
  }

  private void CancelButton_Click(object sender, RoutedEventArgs e)
  {
    this.OptionsControl.ResetPageMaringToPreviousValue();
    this.Close();
  }

  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [DebuggerNonUserCode]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/Syncfusion.Shared.Wpf;component/print/themes/custommargin.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  internal Delegate _CreateDelegate(Type delegateType, string handler)
  {
    return Delegate.CreateDelegate(delegateType, (object) this, handler);
  }

  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [DebuggerNonUserCode]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this.left = (TextBlock) target;
        break;
      case 2:
        this.PrintMarginLeftUPDowm = (UpDown) target;
        break;
      case 3:
        this.right = (TextBlock) target;
        break;
      case 4:
        this.PrintMarginRightUPDowm = (UpDown) target;
        break;
      case 5:
        this.top = (TextBlock) target;
        break;
      case 6:
        this.PrintMarginTopUPDowm = (UpDown) target;
        break;
      case 7:
        this.bottom = (TextBlock) target;
        break;
      case 8:
        this.PrintMarginBottomUPDowm = (UpDown) target;
        break;
      case 9:
        this.OkButton = (Button) target;
        this.OkButton.Click += new RoutedEventHandler(this.OkButton_Click);
        break;
      case 10:
        this.CancelButton = (Button) target;
        this.CancelButton.Click += new RoutedEventHandler(this.CancelButton_Click);
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
