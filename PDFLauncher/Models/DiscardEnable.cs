// Decompiled with JetBrains decompiler
// Type: PDFLauncher.Models.DiscardEnable
// Assembly: PDFLauncher, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 94FE2CE7-EB91-4B8B-872D-74808A04C1E6
// Assembly location: C:\Program Files\PDFgear\PDFLauncher.exe

using System;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace PDFLauncher.Models;

public class DiscardEnable : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return (object) (int.Parse(value.ToString()) > 0);
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    throw new NotImplementedException();
  }
}
