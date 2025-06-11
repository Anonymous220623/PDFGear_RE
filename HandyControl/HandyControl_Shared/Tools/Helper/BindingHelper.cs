// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.Helper.BindingHelper
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

#nullable disable
namespace HandyControl.Tools.Helper;

public class BindingHelper
{
  public static string GetString(object source, string path)
  {
    if (string.IsNullOrEmpty(path))
      return source != null ? source.ToString() : string.Empty;
    DependencyObject target = new DependencyObject();
    Binding binding = new Binding(path)
    {
      Mode = BindingMode.OneTime,
      Source = source
    };
    BindingOperations.SetBinding(target, TextSearch.TextProperty, (BindingBase) binding);
    string str = (string) target.GetValue(TextSearch.TextProperty);
    BindingOperations.ClearBinding(target, TextSearch.TextProperty);
    return str;
  }
}
