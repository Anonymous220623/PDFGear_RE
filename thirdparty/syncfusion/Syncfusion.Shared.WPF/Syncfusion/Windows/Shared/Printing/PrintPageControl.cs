// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.Printing.PrintPageControl
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Syncfusion.Windows.Shared.Printing;

public class PrintPageControl : ContentControl, IDisposable, INotifyPropertyChanged
{
  private int _pageIndex;
  private bool isdisposed;
  private PrintManager printManager;
  private Viewbox PartViewbox;
  private Border PartScalingBorder;

  public PrintPageControl(PrintManager printManager)
  {
    this.DefaultStyleKey = (object) typeof (PrintPageControl);
    this.printManager = printManager;
  }

  public int PageIndex
  {
    get => this._pageIndex;
    set
    {
      this._pageIndex = value;
      this.RaisePropertyChanged(nameof (PageIndex));
    }
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.PartViewbox = this.GetTemplateChild("PART_Viewbox") as Viewbox;
    this.PartScalingBorder = this.GetTemplateChild("Part_ScalingBorder") as Border;
  }

  internal void Zoom(double percent)
  {
    double num1 = this.printManager.GetPageHeight() / 100.0;
    double num2 = this.printManager.GetPageWidth() / 100.0;
    this.PartViewbox.Height = percent * num1;
    this.PartViewbox.Width = percent * num2;
    this.PartViewbox.Measure(new Size(percent * num1, percent * num2));
  }

  private void RaisePropertyChanged(string propertyName)
  {
    if (this.PropertyChanged == null)
      return;
    this.PropertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
  }

  public event PropertyChangedEventHandler PropertyChanged;

  public void Dispose()
  {
    this.Dispose(true);
    GC.SuppressFinalize((object) this);
  }

  protected virtual void Dispose(bool isDisposing)
  {
    if (this.isdisposed)
      return;
    if (isDisposing)
    {
      this.printManager = (PrintManager) null;
      this.DataContext = (object) null;
      this.PartScalingBorder = (Border) null;
      this.PartViewbox = (Viewbox) null;
    }
    this.isdisposed = true;
  }
}
