// Decompiled with JetBrains decompiler
// Type: PDFLauncher.Models.RecoverToOperationBtnVisibility
// Assembly: PDFLauncher, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 94FE2CE7-EB91-4B8B-872D-74808A04C1E6
// Assembly location: C:\Program Files\PDFgear\PDFLauncher.exe

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

#nullable disable
namespace PDFLauncher.Models;

public class RecoverToOperationBtnVisibility : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return (RecoverStatus) value == RecoverStatus.Converted ? (object) Visibility.Visible : (object) Visibility.Collapsed;
  }

  object IValueConverter.ConvertBack(
    object value,
    Type targetType,
    object parameter,
    CultureInfo culture)
  {
    throw new NotImplementedException();
  }
}
