// Decompiled with JetBrains decompiler
// Type: CommomLib.Controls.HotKeyNameConverter
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using CommomLib.Commom.HotKeys;
using System;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace CommomLib.Controls;

public class HotKeyNameConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    if (value is string nameGroup)
    {
      foreach (string name in HotKeyListener.SplitKeyNameGroup(nameGroup))
      {
        HotKeyItem hotKey;
        if (HotKeyListener.TryGetHotKey(name, out hotKey))
        {
          string str = hotKey.ToString(HotKeyItem.FormatType.Normal);
          return parameter is string format ? (object) string.Format(format, (object) str) : (object) str;
        }
      }
    }
    return (object) "";
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    throw new NotImplementedException();
  }
}
