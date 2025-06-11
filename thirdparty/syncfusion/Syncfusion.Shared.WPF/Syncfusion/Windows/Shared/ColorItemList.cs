// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.ColorItemList
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Shared;

[DesignTimeVisible(false)]
public class ColorItemList : ObservableCollection<ColorItem>
{
  public ColorItemList()
  {
    foreach (PropertyInfo property in typeof (Brushes).GetProperties(BindingFlags.Static | BindingFlags.Public))
    {
      if (property.PropertyType == typeof (SolidColorBrush))
        this.Add(new ColorItem(property.Name, (SolidColorBrush) property.GetValue((object) null, (object[]) null)));
    }
  }
}
