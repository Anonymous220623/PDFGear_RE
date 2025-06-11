// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.Extension.ColLayout
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Tools.Converter;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Windows.Markup;

#nullable disable
namespace HandyControl.Tools.Extension;

[TypeConverter(typeof (ColLayoutConverter))]
public class ColLayout : MarkupExtension
{
  public const int ColMaxCellCount = 24;
  public const int HalfColMaxCellCount = 12;
  public const int XsMaxWidth = 768 /*0x0300*/;
  public const int SmMaxWidth = 992;
  public const int MdMaxWidth = 1200;
  public const int LgMaxWidth = 1920;
  public const int XlMaxWidth = 2560 /*0x0A00*/;

  public int Xs { get; set; } = 24;

  public int Sm { get; set; } = 12;

  public int Md { get; set; } = 8;

  public int Lg { get; set; } = 6;

  public int Xl { get; set; } = 4;

  public int Xxl { get; set; } = 2;

  public ColLayout()
  {
  }

  public ColLayout(int uniformWidth)
  {
    this.Xs = uniformWidth;
    this.Sm = uniformWidth;
    this.Md = uniformWidth;
    this.Lg = uniformWidth;
    this.Xl = uniformWidth;
    this.Xxl = uniformWidth;
  }

  public ColLayout(int xs, int sm, int md, int lg, int xl, int xxl)
  {
    this.Xs = xs;
    this.Sm = sm;
    this.Md = md;
    this.Lg = lg;
    this.Xl = xl;
    this.Xxl = xxl;
  }

  public override object ProvideValue(IServiceProvider serviceProvider)
  {
    return (object) new ColLayout()
    {
      Xs = this.Xs,
      Sm = this.Sm,
      Md = this.Md,
      Lg = this.Lg,
      Xl = this.Xl,
      Xxl = this.Xxl
    };
  }

  public static ColLayoutStatus GetLayoutStatus(double width)
  {
    if (width < 1200.0)
    {
      if (width >= 992.0)
        return ColLayoutStatus.Md;
      return width < 768.0 ? ColLayoutStatus.Xs : ColLayoutStatus.Sm;
    }
    if (width >= 2560.0)
      return ColLayoutStatus.Xxl;
    return width < 1920.0 ? ColLayoutStatus.Lg : ColLayoutStatus.Xl;
  }

  public override string ToString()
  {
    CultureInfo currentCulture = CultureInfo.CurrentCulture;
    char numericListSeparator = TokenizerHelper.GetNumericListSeparator((IFormatProvider) currentCulture);
    StringBuilder stringBuilder = new StringBuilder(128 /*0x80*/);
    stringBuilder.Append(this.Xs.ToString((IFormatProvider) currentCulture));
    stringBuilder.Append(numericListSeparator);
    stringBuilder.Append(this.Sm.ToString((IFormatProvider) currentCulture));
    stringBuilder.Append(numericListSeparator);
    stringBuilder.Append(this.Md.ToString((IFormatProvider) currentCulture));
    stringBuilder.Append(numericListSeparator);
    stringBuilder.Append(this.Lg.ToString((IFormatProvider) currentCulture));
    stringBuilder.Append(numericListSeparator);
    stringBuilder.Append(this.Xl.ToString((IFormatProvider) currentCulture));
    stringBuilder.Append(numericListSeparator);
    stringBuilder.Append(this.Xxl.ToString((IFormatProvider) currentCulture));
    return stringBuilder.ToString();
  }
}
