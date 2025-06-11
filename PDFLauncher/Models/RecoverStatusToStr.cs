// Decompiled with JetBrains decompiler
// Type: PDFLauncher.Models.RecoverStatusToStr
// Assembly: PDFLauncher, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 94FE2CE7-EB91-4B8B-872D-74808A04C1E6
// Assembly location: C:\Program Files\PDFgear\PDFLauncher.exe

using System;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace PDFLauncher.Models;

public class RecoverStatusToStr : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    int num = (int) value;
    string str = "";
    if (num == 0)
      str = "";
    if (num == 2)
      str = "Recover succeed!";
    if (num == 1)
      str = "Recovering";
    return (object) str;
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    throw new NotImplementedException();
  }
}
