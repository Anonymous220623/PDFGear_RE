// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.PrintingEventArgs
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class PrintingEventArgs : EventArgs
{
  private bool showPrintDialog;
  private bool cancelPrinting;
  private Window printDialog;
  private Visual printVisual;

  public PrintingEventArgs() => this.ShowPrintDialog = true;

  public bool ShowPrintDialog
  {
    get => this.showPrintDialog;
    set => this.showPrintDialog = value;
  }

  public bool CancelPrinting
  {
    get => this.cancelPrinting;
    set => this.cancelPrinting = value;
  }

  public Window PrintDialog
  {
    get => this.printDialog;
    set => this.printDialog = value;
  }

  public Visual PrintVisual
  {
    get => this.printVisual;
    set => this.printVisual = value;
  }
}
