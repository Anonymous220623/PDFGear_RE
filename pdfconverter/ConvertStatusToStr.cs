// Decompiled with JetBrains decompiler
// Type: pdfconverter.ConvertStatusToStr
// Assembly: pdfconverter, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 691C26A2-7651-46C3-AA3C-2839EDA9722B
// Assembly location: C:\Program Files\PDFgear\pdfconverter.exe

using pdfconverter.Properties;
using System;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace pdfconverter;

public class ConvertStatusToStr : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    FileCovertStatus fileCovertStatus = (FileCovertStatus) value;
    string str = "";
    switch (fileCovertStatus)
    {
      case FileCovertStatus.ConvertInit:
        str = Resources.FileCovertStatusInit;
        break;
      case FileCovertStatus.ConvertLoading:
        str = Resources.FileConvertStatusLoading;
        break;
      case FileCovertStatus.ConvertLoaded:
        str = Resources.FileCovertStatusLoaded;
        break;
      case FileCovertStatus.ConvertLoadedFailed:
        str = Resources.WinConvertLoadedFailed;
        break;
      case FileCovertStatus.ConvertUnsupport:
        str = Resources.FileCovertStatusUnsupported;
        break;
      case FileCovertStatus.ConvertCoverting:
        str = Resources.FileConvertStatusConverting;
        break;
      case FileCovertStatus.ConvertFail:
        str = Resources.FileConvertStatusConvertFail;
        break;
      case FileCovertStatus.ConvertSucc:
        str = Resources.FileConvertStatusConvertSucc;
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
