// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.Printing.PrintSettingsBase
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.ComponentModel;
using System.Windows;

#nullable disable
namespace Syncfusion.Windows.Shared.Printing;

public class PrintSettingsBase : INotifyPropertyChanged
{
  private double printPageHeaderHeight;
  private double printPageFooterHeight;
  private Style printPreviewWindowStyle;
  private double printPageWidth = 793.7;
  private double printPageHeight = 1122.52;
  private DataTemplate printPageHeaderTemplate;
  private DataTemplate printPageFooterTemplate;
  private Thickness printPageMargin = new Thickness(94.49);
  private PrintOrientation printPageOrientation;

  public PrintOrientation Orientation
  {
    get => this.printPageOrientation;
    set
    {
      if (this.printPageOrientation == value)
        return;
      this.printPageOrientation = value;
      this.OnPropertyChanged(nameof (Orientation));
    }
  }

  public Style PrintPreviewWindowStyle
  {
    get => this.printPreviewWindowStyle;
    set
    {
      this.printPreviewWindowStyle = value;
      this.OnPropertyChanged(nameof (PrintPreviewWindowStyle));
    }
  }

  public double PageHeight
  {
    get => this.printPageHeight;
    set
    {
      this.printPageHeight = value;
      this.OnPropertyChanged(nameof (PageHeight));
    }
  }

  public double PageWidth
  {
    get => this.printPageWidth;
    set
    {
      this.printPageWidth = value;
      this.OnPropertyChanged(nameof (PageWidth));
    }
  }

  public double PageHeaderHeight
  {
    get => this.printPageHeaderHeight;
    set
    {
      this.printPageHeaderHeight = value;
      this.OnPropertyChanged(nameof (PageHeaderHeight));
    }
  }

  public double PageFooterHeight
  {
    get => this.printPageFooterHeight;
    set
    {
      this.printPageFooterHeight = value;
      this.OnPropertyChanged(nameof (PageFooterHeight));
    }
  }

  public Thickness PageMargin
  {
    get => this.printPageMargin;
    set
    {
      this.printPageMargin = value;
      this.OnPropertyChanged(nameof (PageMargin));
    }
  }

  public DataTemplate PageHeaderTemplate
  {
    get => this.printPageHeaderTemplate;
    set
    {
      this.printPageHeaderTemplate = value;
      this.OnPropertyChanged(nameof (PageHeaderTemplate));
    }
  }

  public DataTemplate PageFooterTemplate
  {
    get => this.printPageFooterTemplate;
    set
    {
      this.printPageFooterTemplate = value;
      this.OnPropertyChanged(nameof (PageFooterTemplate));
    }
  }

  public event PropertyChangedEventHandler PropertyChanged;

  protected void OnPropertyChanged(string propertyName)
  {
    if (this.PropertyChanged == null)
      return;
    this.PropertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
  }
}
