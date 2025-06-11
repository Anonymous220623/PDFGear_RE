// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.UtilsConvertParse
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System;

#nullable disable
namespace CommomLib.Commom;

public static class UtilsConvertParse
{
  public static object ToEnum(string str)
  {
    ConvToPDFType convToPdfType = ConvToPDFType.WordToPDF;
    try
    {
      convToPdfType = (ConvToPDFType) Enum.Parse(typeof (ConvToPDFType), str);
    }
    catch
    {
      try
      {
        return Enum.Parse(typeof (ConvFromPDFType), str);
      }
      catch
      {
      }
    }
    return (object) convToPdfType;
  }
}
