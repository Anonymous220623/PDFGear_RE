// Decompiled with JetBrains decompiler
// Type: pdfconverter.Models.MergeStatusToStr
// Assembly: pdfconverter, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 691C26A2-7651-46C3-AA3C-2839EDA9722B
// Assembly location: C:\Program Files\PDFgear\pdfconverter.exe

using pdfconverter.Properties;
using System;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace pdfconverter.Models;

public class MergeStatusToStr : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    MergeStatus mergeStatus = (MergeStatus) value;
    string str = "";
    switch (mergeStatus)
    {
      case MergeStatus.Init:
        str = Resources.FileCovertStatusInit;
        break;
      case MergeStatus.Loading:
        str = Resources.FileConvertStatusLoading;
        break;
      case MergeStatus.Loaded:
        str = Resources.FileCovertStatusLoaded;
        break;
      case MergeStatus.LoadedFailed:
        str = Resources.WinConvertLoadedFailed;
        break;
      case MergeStatus.Unsupport:
        str = Resources.FileCovertStatusUnsupported;
        break;
      case MergeStatus.Merging:
        str = Resources.WinMergeSplitMergeStatusMerging;
        break;
      case MergeStatus.Fail:
        str = Resources.WinMergeSplitMergeStatusFail;
        break;
      case MergeStatus.Succ:
        str = Resources.WinMergeSplitMergeStatusSucc;
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
