// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontHelper
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal static class SystemFontHelper
{
  public static bool UnboxInteger(object number, out int res)
  {
    switch (number)
    {
      case sbyte num1:
        res = (int) num1;
        break;
      case short num2:
        res = (int) num2;
        break;
      case int num3:
        res = num3;
        break;
      case double num4:
        res = (int) num4;
        break;
      default:
        res = 0;
        return false;
    }
    return true;
  }

  public static bool UnboxReal(object number, out double res)
  {
    switch (number)
    {
      case sbyte num1:
        res = (double) num1;
        break;
      case short num2:
        res = (double) num2;
        break;
      case int num3:
        res = (double) num3;
        break;
      case double num4:
        res = num4;
        break;
      default:
        res = 0.0;
        return false;
    }
    return true;
  }

  public static byte[] CreateByteArray(params byte[] bytes) => bytes;
}
