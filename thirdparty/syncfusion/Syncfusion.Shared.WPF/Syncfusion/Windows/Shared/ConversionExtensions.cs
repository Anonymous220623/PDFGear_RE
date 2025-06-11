// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.ConversionExtensions
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Globalization;

#nullable disable
namespace Syncfusion.Windows.Shared;

public static class ConversionExtensions
{
  public static long? ConvertToInt64Null(this string text)
  {
    long result;
    return long.TryParse(text, NumberStyles.Any, (IFormatProvider) NumberFormatInfo.InvariantInfo, out result) ? new long?(result) : new long?();
  }

  public static double? ConvertToDoubleNull(
    this string text,
    CultureInfo culture,
    NumberFormatInfo numberformat)
  {
    double? doubleNull;
    if (numberformat != null && culture != null)
    {
      double result;
      doubleNull = double.TryParse(text, NumberStyles.Any, (IFormatProvider) numberformat, out result) ? new double?(result) : new double?();
    }
    else
    {
      NumberFormatInfo provider = culture == null ? NumberFormatInfo.InvariantInfo : culture.NumberFormat;
      double result;
      doubleNull = double.TryParse(text, NumberStyles.Any, (IFormatProvider) provider, out result) ? new double?(result) : new double?();
    }
    return doubleNull;
  }

  public static Decimal? ConvertToDecimalNull(this string text)
  {
    Decimal result;
    return Decimal.TryParse(text, NumberStyles.Any, (IFormatProvider) NumberFormatInfo.InvariantInfo, out result) ? new Decimal?(result) : new Decimal?();
  }
}
