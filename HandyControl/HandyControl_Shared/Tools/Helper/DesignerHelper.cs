// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.DesignerHelper
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System.ComponentModel;
using System.Windows;

#nullable disable
namespace HandyControl.Tools;

public class DesignerHelper
{
  private static bool? _isInDesignMode;

  public static bool IsInDesignMode
  {
    get
    {
      if (!DesignerHelper._isInDesignMode.HasValue)
        DesignerHelper._isInDesignMode = new bool?((bool) DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty, typeof (FrameworkElement)).Metadata.DefaultValue);
      return DesignerHelper._isInDesignMode.Value;
    }
  }
}
