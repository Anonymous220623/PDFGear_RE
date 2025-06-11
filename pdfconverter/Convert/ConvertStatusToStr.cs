// Decompiled with JetBrains decompiler
// Type: pdfconverter.Convert.ConvertStatusToStr
// Assembly: pdfconverter, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 691C26A2-7651-46C3-AA3C-2839EDA9722B
// Assembly location: C:\Program Files\PDFgear\pdfconverter.exe

using pdfconverter.Models;
using pdfconverter.Properties;
using System;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace pdfconverter.Convert;

public class ConvertStatusToStr : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    ToPDFItemStatus toPdfItemStatus = (ToPDFItemStatus) value;
    string str = "";
    switch (toPdfItemStatus)
    {
      case ToPDFItemStatus.Init:
        str = Resources.FileCovertStatusInit;
        break;
      case ToPDFItemStatus.Loading:
        str = Resources.FileConvertStatusLoading;
        break;
      case ToPDFItemStatus.Loaded:
        str = Resources.FileCovertStatusLoaded;
        break;
      case ToPDFItemStatus.LoadedFailed:
        str = Resources.WinConvertLoadedFailed;
        break;
      case ToPDFItemStatus.Unsupport:
        str = Resources.FileCovertStatusUnsupported;
        break;
      case ToPDFItemStatus.Working:
        str = Resources.FileConvertStatusConverting;
        break;
      case ToPDFItemStatus.Fail:
        str = Resources.FileConvertStatusConvertFail;
        break;
      case ToPDFItemStatus.Succ:
        str = Resources.FileConvertStatusConvertSucc;
        break;
      case ToPDFItemStatus.Queuing:
        str = "Queue";
        break;
    }
    return (object) str;
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
