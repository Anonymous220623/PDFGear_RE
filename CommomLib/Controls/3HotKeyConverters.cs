// Decompiled with JetBrains decompiler
// Type: CommomLib.Controls.HasHotKeyConverter
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using CommomLib.Commom.HotKeys;
using System;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace CommomLib.Controls;

public class HasHotKeyConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    if (value is string nameGroup)
    {
      foreach (string name in HotKeyListener.SplitKeyNameGroup(nameGroup))
      {
        if (HotKeyListener.TryGetHotKey(name, out HotKeyItem _))
          return (object) true;
      }
    }
    return (object) false;
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    throw new NotImplementedException();
  }
}
