// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.ObservableFrameworkElements
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

#nullable disable
namespace Syncfusion.Windows.Shared;

public class ObservableFrameworkElements : ObservableCollection<FrameworkElement>
{
  public void AddRange(IEnumerable<FrameworkElement> list)
  {
    foreach (FrameworkElement frameworkElement in list)
      this.Add(frameworkElement);
  }

  public ObservableFrameworkElements GetCopy()
  {
    ObservableFrameworkElements copy = new ObservableFrameworkElements();
    foreach (FrameworkElement frameworkElement in (IEnumerable<FrameworkElement>) this.Items)
      copy.Add(frameworkElement);
    return copy;
  }

  public void CopyTo(List<FrameworkElement> list)
  {
    list.Clear();
    foreach (FrameworkElement frameworkElement in (IEnumerable<FrameworkElement>) this.Items)
      list.Add(frameworkElement);
  }

  public void RemoveRange(IEnumerable<FrameworkElement> list)
  {
    foreach (FrameworkElement frameworkElement in list)
      this.Remove(frameworkElement);
  }
}
