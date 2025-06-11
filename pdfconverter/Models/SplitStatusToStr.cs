// Decompiled with JetBrains decompiler
// Type: pdfconverter.Models.SplitStatusToStr
// Assembly: pdfconverter, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 691C26A2-7651-46C3-AA3C-2839EDA9722B
// Assembly location: C:\Program Files\PDFgear\pdfconverter.exe

using pdfconverter.Properties;
using System;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace pdfconverter.Models;

public class SplitStatusToStr : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    SplitStatus splitStatus = (SplitStatus) value;
    string str = "";
    switch (splitStatus)
    {
      case SplitStatus.Init:
        str = Resources.FileCovertStatusInit;
        break;
      case SplitStatus.Loading:
        str = Resources.FileConvertStatusLoading;
        break;
      case SplitStatus.Loaded:
        str = Resources.FileCovertStatusLoaded;
        break;
      case SplitStatus.LoadedFailed:
        str = Resources.WinConvertLoadedFailed;
        break;
      case SplitStatus.Unsupport:
        str = Resources.FileCovertStatusUnsupported;
        break;
      case SplitStatus.Spliting:
        str = Resources.WinMergeSplitSplitStatusSplitting;
        break;
      case SplitStatus.Fail:
        str = Resources.WinMergeSplitSplitStatusFail;
        break;
      case SplitStatus.Succ:
        str = Resources.WinMergeSplitSplitStatusSucc;
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
