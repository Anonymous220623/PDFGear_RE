// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.Printing.PrintPreviewAreaControl
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace Syncfusion.Windows.Shared.Printing;

public class PrintPreviewAreaControl : Control, INotifyPropertyChanged, IDisposable
{
  private int totalPages;
  private bool isdisposed;
  private PrintManager printManager;
  private bool isPageIndexSetFromOverride;
  internal PrintPreviewPanel PartPrintWindowPanel;
  public static readonly DependencyProperty ZoomFactorProperty = DependencyProperty.Register(nameof (ZoomFactor), typeof (double), typeof (PrintPreviewAreaControl), new PropertyMetadata((object) 100.0, new PropertyChangedCallback(PrintPreviewAreaControl.OnZoomFactorDependencyPropertyChanged)));
  public static readonly DependencyProperty PageIndexProperty = DependencyProperty.Register(nameof (PageIndex), typeof (int), typeof (PrintPreviewAreaControl), new PropertyMetadata((object) 1, new PropertyChangedCallback(PrintPreviewAreaControl.OnPageIndexChanged)));
  private ICommand firstCommand;
  private ICommand previousCommand;
  private ICommand nextCommand;
  private ICommand lastCommand;

  public PrintPreviewAreaControl()
  {
    this.DefaultStyleKey = (object) typeof (PrintPreviewAreaControl);
  }

  public int TotalPages
  {
    get => this.totalPages;
    internal set
    {
      this.totalPages = value;
      this.OnPropertyChanged(nameof (TotalPages));
    }
  }

  public PrintManager PrintManager
  {
    get
    {
      if (this.printManager != null && !this.printManager.isPagesInitialized)
        this.printManager.InitializePrint();
      return this.printManager;
    }
    set
    {
      this.printManager = value;
      this.OnPropertyChanged(nameof (PrintManager));
    }
  }

  public double ZoomFactor
  {
    get => (double) this.GetValue(PrintPreviewAreaControl.ZoomFactorProperty);
    set => this.SetValue(PrintPreviewAreaControl.ZoomFactorProperty, (object) value);
  }

  private static void OnZoomFactorDependencyPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as PrintPreviewAreaControl).OnZoomFactorChanged((double) e.NewValue);
  }

  public int PageIndex
  {
    get => (int) this.GetValue(PrintPreviewAreaControl.PageIndexProperty);
    set => this.SetValue(PrintPreviewAreaControl.PageIndexProperty, (object) value);
  }

  private static void OnPageIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    PrintPreviewAreaControl previewAreaControl = d as PrintPreviewAreaControl;
    if (previewAreaControl.isPageIndexSetFromOverride || previewAreaControl.PartPrintWindowPanel == null || previewAreaControl.PrintManager == null)
      return;
    previewAreaControl.PartPrintWindowPanel.SetVerticalOffset((double) ((int) e.NewValue - 1) * (previewAreaControl.PartPrintWindowPanel.ExtentHeight / (double) previewAreaControl.PrintManager.PageCount));
  }

  internal void UpdateZoomFactor()
  {
    double num1 = this.PrintManager.PageHeight / 100.0;
    double num2 = this.PrintManager.PageWidth / 100.0;
    int num3 = (int) (this.PartPrintWindowPanel.ViewportHeight / num1);
    int num4 = (int) (this.PartPrintWindowPanel.ViewportWidth / num2);
    this.ZoomFactor = (double) Math.Max(10, num3 < num4 ? num3 : num4);
  }

  internal void OnZoomFactorChanged(double value)
  {
    if (this.PartPrintWindowPanel == null)
      return;
    this.PartPrintWindowPanel.Child.Zoom(value);
    double height1 = this.PrintManager.PageHeight / 100.0 * value;
    if (height1 < this.PartPrintWindowPanel.ViewportHeight)
      height1 = this.PartPrintWindowPanel.ActualHeight + (this.PartPrintWindowPanel.ScrollOwner.ComputedHorizontalScrollBarVisibility == Visibility.Visible ? SystemParameters.ScrollWidth : 0.0);
    double height2 = (double) this.PrintManager.PageCount * height1;
    this.PartPrintWindowPanel.UpdateScrollInfo(new Size(this.PartPrintWindowPanel.ViewportWidth, height1), new Size(this.PartPrintWindowPanel.ViewportWidth, height2));
    this.PartPrintWindowPanel.SetVerticalOffset((double) (this.PartPrintWindowPanel.Child.PageIndex - 1) * height1);
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.PartPrintWindowPanel = this.GetTemplateChild("PART_PrintPreviewPanel") as PrintPreviewPanel;
    if (this.PartPrintWindowPanel == null || this.PrintManager == null)
      return;
    this.PartPrintWindowPanel.SetPrintManager(this.PrintManager);
    this.PartPrintWindowPanel.SetPageIndex = (Action<int>) (index =>
    {
      this.isPageIndexSetFromOverride = true;
      this.PageIndex = index;
      this.isPageIndexSetFromOverride = false;
    });
    this.PartPrintWindowPanel.InValidateParent = (Action) (() =>
    {
      this.TotalPages = this.PrintManager.PageCount;
      if (this.PrintManager.PageRangeSelection == PageRangeSelection.AllPages)
      {
        this.PrintManager.PrintOptionsControl.ToPage = this.TotalPages;
        this.PrintManager.ToPage = this.TotalPages;
      }
      else if (this.printManager.SelectedScaleIndex == 1)
      {
        if (this.PrintManager.ToPage > this.TotalPages)
        {
          this.PrintManager.PrintOptionsControl.ToPage = this.TotalPages;
          this.PrintManager.ToPage = this.TotalPages;
        }
        else
          this.PrintManager.PrintOptionsControl.ToPage = this.PrintManager.ToPage;
        if (this.printManager.FromPage > this.TotalPages)
        {
          this.PrintManager.PrintOptionsControl.FromPage = 1;
          this.printManager.FromPage = 1;
        }
        else
          this.PrintManager.PrintOptionsControl.FromPage = this.printManager.FromPage;
      }
      else
      {
        this.PrintManager.PrintOptionsControl.FromPage = 1;
        this.PrintManager.PrintOptionsControl.ToPage = this.TotalPages;
        this.PrintManager.ToPage = this.TotalPages;
      }
    });
    this.TotalPages = this.PrintManager.PageCount;
    if (this.PrintManager.PageRangeSelection == PageRangeSelection.AllPages)
    {
      this.PrintManager.PrintOptionsControl.ToPage = this.TotalPages;
      this.PrintManager.ToPage = this.TotalPages;
      if (this.totalPages == 0)
      {
        this.PrintManager.PrintOptionsControl.FromPage = this.TotalPages;
        this.PrintManager.FromPage = this.TotalPages;
      }
      else
      {
        this.PrintManager.PrintOptionsControl.FromPage = 1;
        this.PrintManager.FromPage = 1;
      }
    }
    else if (this.totalPages == 0)
    {
      this.PrintManager.PrintOptionsControl.FromPage = this.TotalPages;
      this.PrintManager.FromPage = this.TotalPages;
      this.PrintManager.PrintOptionsControl.ToPage = this.TotalPages;
      this.PrintManager.ToPage = this.TotalPages;
    }
    else if (this.PrintManager.ToPage > this.TotalPages || this.PrintManager.FromPage == 0 && this.printManager.ToPage == 0 && this.totalPages > 0)
    {
      this.PrintManager.PrintOptionsControl.FromPage = 1;
      this.PrintManager.FromPage = 1;
      this.PrintManager.PrintOptionsControl.ToPage = this.TotalPages;
      this.PrintManager.ToPage = this.TotalPages;
    }
    else
      this.PrintManager.PrintOptionsControl.ToPage = this.PrintManager.ToPage;
    if (this.PartPrintWindowPanel.Child == null)
      return;
    this.PartPrintWindowPanel.Child.Loaded += new RoutedEventHandler(this.OnPrintPreviewControlLoaded);
  }

  private void OnPrintPreviewControlLoaded(object sender, RoutedEventArgs e)
  {
    this.UpdateZoomFactor();
    this.PrintManager.PrintOptionsControl.InitializePrintOptionsWindow();
  }

  public ICommand FirstCommand
  {
    get
    {
      return this.firstCommand ?? (this.firstCommand = (ICommand) new BaseCommand(new Action<object>(this.OnFirstCommandClicked), (Predicate<object>) (o => this.PrintManager != null && this.PrintManager.PageCount > 0 && this.PartPrintWindowPanel != null && this.PartPrintWindowPanel.Child.PageIndex != 1)));
    }
  }

  private void OnFirstCommandClicked(object obj)
  {
    if (this.PartPrintWindowPanel == null)
      return;
    this.PartPrintWindowPanel.SetVerticalOffset(0.0);
  }

  public ICommand PreviousCommand
  {
    get
    {
      return this.previousCommand ?? (this.previousCommand = (ICommand) new BaseCommand(new Action<object>(this.OnPreviousCommandClicked), (Predicate<object>) (o => this.PrintManager != null && this.PrintManager.PageCount > 0 && this.PartPrintWindowPanel != null && this.PartPrintWindowPanel.Child.PageIndex != 1)));
    }
  }

  private void OnPreviousCommandClicked(object obj)
  {
    if (this.PartPrintWindowPanel == null)
      return;
    int pageIndex = this.PartPrintWindowPanel.Child.PageIndex;
    if (pageIndex <= 1 || pageIndex > this.PrintManager.PageCount)
      return;
    this.PartPrintWindowPanel.SetVerticalOffset((double) (pageIndex - 2) * (this.PartPrintWindowPanel.ExtentHeight / (double) this.PrintManager.PageCount));
  }

  public ICommand NextCommand
  {
    get
    {
      return this.nextCommand ?? (this.nextCommand = (ICommand) new BaseCommand(new Action<object>(this.OnNextCommandClicked), (Predicate<object>) (o => this.PrintManager != null && this.PrintManager.PageCount > 0 && this.PartPrintWindowPanel != null && this.PartPrintWindowPanel.Child.PageIndex < this.PrintManager.PageCount)));
    }
  }

  private void OnNextCommandClicked(object obj)
  {
    if (this.PartPrintWindowPanel == null)
      return;
    int pageIndex = this.PartPrintWindowPanel.Child.PageIndex;
    if (pageIndex <= 0 || pageIndex >= this.PrintManager.PageCount)
      return;
    this.PartPrintWindowPanel.SetVerticalOffset((double) pageIndex * (this.PartPrintWindowPanel.ExtentHeight / (double) this.PrintManager.PageCount));
  }

  public ICommand LastCommand
  {
    get
    {
      return this.lastCommand ?? (this.lastCommand = (ICommand) new BaseCommand(new Action<object>(this.OnLastCommandClicked), (Predicate<object>) (o => this.PrintManager != null && this.PrintManager.PageCount > 0 && this.PartPrintWindowPanel != null && this.PartPrintWindowPanel.Child.PageIndex < this.PrintManager.PageCount)));
    }
  }

  private void OnLastCommandClicked(object obj)
  {
    if (this.PartPrintWindowPanel == null)
      return;
    this.PartPrintWindowPanel.SetVerticalOffset((double) (this.PrintManager.PageCount - 1) * (this.PartPrintWindowPanel.ExtentHeight / (double) this.PrintManager.PageCount));
  }

  public event PropertyChangedEventHandler PropertyChanged;

  private void OnPropertyChanged(string propertyName)
  {
    if (this.PropertyChanged == null)
      return;
    this.PropertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
  }

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
      this.PartPrintWindowPanel = (PrintPreviewPanel) null;
      this.printManager = (PrintManager) null;
    }
    this.isdisposed = true;
  }
}
