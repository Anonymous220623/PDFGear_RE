// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.Converters.BooleanToHotkeyInvokeActionConverter
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom.HotKeys;
using System;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace pdfeditor.Utils.Converters;

internal class BooleanToHotkeyInvokeActionConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    HotKeyInvokeAction result = HotKeyInvokeAction.None;
    if (value is bool flag && flag)
      return (object) result;
    if (string.IsNullOrEmpty((string) parameter))
      result = HotKeyInvokeAction.Invoke;
    else if (!Enum.TryParse<HotKeyInvokeAction>((string) parameter, true, out result))
      result = HotKeyInvokeAction.None;
    return (object) result;
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    throw new NotImplementedException();
  }
}
