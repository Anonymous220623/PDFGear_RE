// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.Extension.FrameworkElementExtension
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System.Windows;

#nullable disable
namespace HandyControl.Tools.Extension;

public static class FrameworkElementExtension
{
  public static double GetValidWidth(this FrameworkElement element)
  {
    if (!double.IsNaN(element.Width))
    {
      if (element.Width > 0.0)
        return element.Width;
    }
    else
    {
      if (element.ActualWidth > 0.0)
        return element.ActualWidth;
      Size desiredSize = element.DesiredSize;
      if (desiredSize.Width > 0.0)
      {
        desiredSize = element.DesiredSize;
        return desiredSize.Width;
      }
    }
    return 0.0;
  }

  public static double GetValidHeight(this FrameworkElement element)
  {
    if (!double.IsNaN(element.Height))
    {
      if (element.Height > 0.0)
        return element.Height;
    }
    else
    {
      if (element.ActualHeight > 0.0)
        return element.ActualHeight;
      Size desiredSize = element.DesiredSize;
      if (desiredSize.Height > 0.0)
      {
        desiredSize = element.DesiredSize;
        return desiredSize.Height;
      }
    }
    return 0.0;
  }
}
