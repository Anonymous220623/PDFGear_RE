// Decompiled with JetBrains decompiler
// Type: PDFLauncher.Utils.UIElementExtension
// Assembly: PDFLauncher, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 94FE2CE7-EB91-4B8B-872D-74808A04C1E6
// Assembly location: C:\Program Files\PDFgear\PDFLauncher.exe

using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace PDFLauncher.Utils;

public static class UIElementExtension
{
  public static readonly DependencyProperty ExtendContextMenuDataContextProperty = DependencyProperty.RegisterAttached("ExtendContextMenuDataContext", typeof (ContextMenu), typeof (UIElementExtension), new PropertyMetadata((object) null, (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is FrameworkElement frameworkElement2))
      return;
    if (a.OldValue is ContextMenu oldValue2)
      oldValue2.DataContext = (object) null;
    if (!(a.NewValue is ContextMenu newValue2))
      return;
    newValue2.DataContext = (object) frameworkElement2;
  })));

  public static void SetExtendContextMenuDataContext(DependencyObject obj, ContextMenu value)
  {
    obj.SetValue(UIElementExtension.ExtendContextMenuDataContextProperty, (object) value);
  }
}
