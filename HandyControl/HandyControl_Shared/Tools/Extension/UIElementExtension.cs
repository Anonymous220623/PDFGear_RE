// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.Extension.UIElementExtension
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System.Windows;

#nullable disable
namespace HandyControl.Tools.Extension;

public static class UIElementExtension
{
  public static void Show(this UIElement element) => element.Visibility = Visibility.Visible;

  public static void Show(this UIElement element, bool show)
  {
    element.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
  }

  public static void Hide(this UIElement element) => element.Visibility = Visibility.Hidden;

  public static void Collapse(this UIElement element) => element.Visibility = Visibility.Collapsed;
}
