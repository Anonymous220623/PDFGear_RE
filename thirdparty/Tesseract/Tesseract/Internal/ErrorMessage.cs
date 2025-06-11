// Decompiled with JetBrains decompiler
// Type: Tesseract.Internal.ErrorMessage
// Assembly: Tesseract, Version=4.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: C5D5562D-D917-402B-968F-9F8B28C3D951
// Assembly location: D:\PDFGear\bin\Tesseract.dll

#nullable disable
namespace Tesseract.Internal;

internal static class ErrorMessage
{
  private const string ErrorMessageFormat = "{0}. See {1} for details.";
  private const string WikiUrlFormat = "https://github.com/charlesw/tesseract/wiki/Error-{0}";

  public static string Format(int errorNumber, string messageFormat, params object[] messageArgs)
  {
    return $"{string.Format(messageFormat, messageArgs)}. See {ErrorMessage.ErrorPageUrl(errorNumber)} for details.";
  }

  public static string ErrorPageUrl(int errorNumber)
  {
    return $"https://github.com/charlesw/tesseract/wiki/Error-{errorNumber}";
  }
}
