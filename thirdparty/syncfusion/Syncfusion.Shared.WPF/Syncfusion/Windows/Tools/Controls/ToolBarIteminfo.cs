// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Tools.Controls.ToolBarIteminfo
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Tools.Controls;

public class ToolBarIteminfo : INotifyPropertyChanged
{
  public string Label { get; set; }

  public ImageSource Icon { get; set; }

  public object Host { get; set; }

  public bool IsChecked
  {
    get => ToolBarAdv.GetIsAvailable(this.Host as DependencyObject);
    set => ToolBarAdv.SetIsAvailable(this.Host as DependencyObject, value);
  }

  public DataTemplate IconTemplate { get; set; }

  public event PropertyChangedEventHandler PropertyChanged;

  public void OnPropertyChanged(string property)
  {
    if (this.PropertyChanged == null)
      return;
    this.PropertyChanged((object) this, new PropertyChangedEventArgs(property));
  }

  public ToolBarIteminfo()
  {
    object host = this.Host;
    this.PropertyChanged += new PropertyChangedEventHandler(this.ToolBarIteminfo_PropertyChanged);
  }

  private void ToolBarIteminfo_PropertyChanged(object sender, PropertyChangedEventArgs e)
  {
    if (!(e.PropertyName == "IsChecked"))
      return;
    ToolBarAdv.SetIsAvailable(this.Host as DependencyObject, this.IsChecked);
  }
}
