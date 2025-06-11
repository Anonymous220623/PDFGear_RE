// Decompiled with JetBrains decompiler
// Type: pdfconverter.Models.MapOnlineConverttype
// Assembly: pdfconverter, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 691C26A2-7651-46C3-AA3C-2839EDA9722B
// Assembly location: C:\Program Files\PDFgear\pdfconverter.exe

using CommomLib.Commom;
using System.Collections.Generic;

#nullable disable
namespace pdfconverter.Models;

public static class MapOnlineConverttype
{
  private static readonly Dictionary<ConvFromPDFType, string> OnlineConvertType = new Dictionary<ConvFromPDFType, string>()
  {
    {
      ConvFromPDFType.PDFToWord,
      "pdf2docx"
    },
    {
      ConvFromPDFType.PDFToExcel,
      "pdf2xlsx"
    },
    {
      ConvFromPDFType.PDFToPng,
      "pdf2png"
    },
    {
      ConvFromPDFType.PDFToJpg,
      "pdf2jpeg"
    },
    {
      ConvFromPDFType.PDFToRTF,
      "pdf2rtf"
    },
    {
      ConvFromPDFType.PDFToPPT,
      "pdf2pptx"
    }
  };
  private static readonly Dictionary<string, string> targetInfoDic = new Dictionary<string, string>()
  {
    {
      "pdf2docx",
      ".docx"
    },
    {
      "pdf2xlsx",
      ".xlsx"
    },
    {
      "pdf2png",
      ".zip"
    },
    {
      "pdf2jpeg",
      ".zip"
    },
    {
      "pdf2rtf",
      ".rtf"
    }
  };

  public static string GetOnlineConvertStr(ConvFromPDFType id)
  {
    return MapOnlineConverttype.OnlineConvertType.ContainsKey(id) ? MapOnlineConverttype.OnlineConvertType[id] : string.Empty;
  }

  public static string GetOnlineExtensionStr(string convertype)
  {
    return MapOnlineConverttype.targetInfoDic.ContainsKey(convertype) ? MapOnlineConverttype.targetInfoDic[convertype] : string.Empty;
  }
}
