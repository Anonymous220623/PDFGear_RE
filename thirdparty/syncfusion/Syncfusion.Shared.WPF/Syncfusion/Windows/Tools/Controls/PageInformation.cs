// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Tools.Controls.PageInformation
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.ComponentModel;

#nullable disable
namespace Syncfusion.Windows.Tools.Controls;

public class PageInformation : INotifyPropertyChanged
{
  private int pageNumber;
  private int totalPages;

  internal PageInformation()
  {
  }

  public int PageNumber
  {
    internal set
    {
      this.pageNumber = value;
      this.OnPropertyChanged(nameof (PageNumber));
    }
    get => this.pageNumber;
  }

  public int TotalPages
  {
    internal set
    {
      this.totalPages = value;
      this.OnPropertyChanged(nameof (TotalPages));
    }
    get => this.totalPages;
  }

  public event PropertyChangedEventHandler PropertyChanged;

  protected void OnPropertyChanged(string name)
  {
    PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
    if (propertyChanged == null)
      return;
    propertyChanged((object) this, new PropertyChangedEventArgs(name));
  }
}
