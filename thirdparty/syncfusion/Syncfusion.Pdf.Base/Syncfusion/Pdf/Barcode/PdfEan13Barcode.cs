// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Barcode.PdfEan13Barcode
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System;
using System.Drawing;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Barcode;

public class PdfEan13Barcode : PdfUnidimensionalBarcode
{
  private float minimumAllowableScale = 0.8f;
  private float maximumAllowableScale = 2f;
  private float fWidth = 150f;
  private float fHeight = 100f;
  private float fontSize = 8f;
  private float scale = 1.5f;
  private int quietZonePixel = 4;
  private string[] OddLeft = new string[10]
  {
    "0001101",
    "0011001",
    "0010011",
    "0111101",
    "0100011",
    "0110001",
    "0101111",
    "0111011",
    "0110111",
    "0001011"
  };
  private string[] EvenLeft = new string[10]
  {
    "0100111",
    "0110011",
    "0011011",
    "0100001",
    "0011101",
    "0111001",
    "0000101",
    "0010001",
    "0001001",
    "0010111"
  };
  private string[] Right = new string[10]
  {
    "1110010",
    "1100110",
    "1101100",
    "1000010",
    "1011100",
    "1001110",
    "1010000",
    "1000100",
    "1001000",
    "1110100"
  };
  private string QuiteZone = "000000000";
  private string LeadTail = "101";
  private string Separator = "01010";
  private string countryCode = "00";
  private string manufacturerCode;
  private string productCode;
  private string checksumDigit;

  internal int QuietZoneNew
  {
    get => this.quietZonePixel;
    set => this.quietZonePixel = value;
  }

  internal float MinimumAllowableScale => this.minimumAllowableScale;

  internal float MaximumAllowableScale => this.maximumAllowableScale;

  internal float Width => this.fWidth;

  internal float Height => this.fHeight;

  internal float FontSize => this.fontSize;

  internal float Scale
  {
    get => this.scale;
    set
    {
      if ((double) value < (double) this.minimumAllowableScale || (double) value > (double) this.maximumAllowableScale)
        throw new Exception($"Scale value out of allowable range.  Value must be between {this.minimumAllowableScale.ToString()} and {this.maximumAllowableScale.ToString()}");
      this.scale = value;
    }
  }

  internal string CountryCode
  {
    get => this.countryCode;
    set
    {
      while (value.Length < 2)
        value = "0" + value;
      this.countryCode = value;
    }
  }

  internal string ManufacturerCode
  {
    get => this.manufacturerCode;
    set => this.manufacturerCode = value;
  }

  internal string ProductCode
  {
    get => this.productCode;
    set => this.productCode = value;
  }

  internal string ChecksumDigit
  {
    get => this.checksumDigit;
    set
    {
      int int32 = Convert.ToInt32(value);
      if (int32 < 0 || int32 > 9)
        throw new Exception("The Check Digit must be between 0 and 9.");
      this.checksumDigit = value;
    }
  }

  public PdfEan13Barcode()
  {
  }

  public PdfEan13Barcode(string text)
    : this()
  {
    this.Text = text;
  }

  public new void Draw(PdfPageBase page, float x, float y, float width, float height)
  {
    this.Size = new SizeF(width, height);
    this.Location = new PointF(x, y);
    this.Draw(page);
  }

  public new void Draw(PdfPageBase page, PointF location, SizeF size)
  {
    this.Draw(page, location.X, location.Y, size.Width, size.Height);
  }

  public new void Draw(PdfPageBase page, RectangleF rectangle)
  {
    this.Draw(page, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
  }

  public new virtual void Draw(PdfPageBase page) => this.Draw(page, this.Location);

  public new virtual void Draw(PdfPageBase page, PointF location)
  {
    if (string.IsNullOrEmpty(this.Text) || this.Text.Length > 13)
      throw new PdfBarcodeException("Barcode text should be neither empty nor exceed more than 13 digits");
    float width1;
    float height1;
    if (!this.size.IsEmpty)
    {
      width1 = this.Size.Width;
      height1 = this.Size.Height;
    }
    else
    {
      width1 = this.Width;
      height1 = this.Height;
    }
    if (this.barHeightEnabled)
    {
      width1 = this.Width;
      height1 = this.BarHeight;
    }
    this.QuietZoneNew = this.GetQuiteZone();
    float width2 = width1 / 113f;
    PdfBrush brush1 = (PdfBrush) new PdfSolidBrush((PdfColor) Color.FromArgb((int) this.BackColor.A, (int) this.BackColor.R, (int) this.BackColor.G, (int) this.BackColor.B));
    PdfBrush brush2 = PdfBrushes.Black;
    if (this.BarColor.A != (byte) 0)
    {
      Color color = Color.FromArgb(this.BarColor.ToArgb());
      if (color != Color.Black)
        brush2 = (PdfBrush) new PdfSolidBrush((PdfColor) color);
    }
    if (this.Font != null && !this.isFontModified)
      this.Font.Size = width1 / (float) this.Text.Length;
    PdfFont font = this.Font != null ? this.Font : (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, this.fontSize * this.Scale);
    StringBuilder stringBuilder1 = new StringBuilder();
    StringBuilder stringBuilder2 = new StringBuilder();
    float num1 = 0.0f;
    this.CalculateChecksumDigit();
    if (stringBuilder2.Length == 12)
      stringBuilder2.AppendFormat("{0}{1}", (object) this.Text, (object) this.ChecksumDigit);
    else
      stringBuilder2.AppendFormat("{0}{1}", (object) this.Text.Substring(0, 12), (object) this.ChecksumDigit);
    string str1 = stringBuilder2.ToString();
    string str2 = this.ConvertLeftPattern(str1.Substring(0, 7));
    stringBuilder1.AppendFormat("{0}{1}{2}{3}{4}{1}{0}", (object) this.QuiteZone, (object) this.LeadTail, (object) str2, (object) this.Separator, (object) this.ConvertToDigitPatterns(str1.Substring(7), this.Right));
    string text = stringBuilder1.ToString();
    float height2 = font.MeasureString(text).Height;
    PdfGraphics graphics = page.Graphics;
    float num2 = location.X;
    float y = location.Y;
    PdfStringFormat format = new PdfStringFormat();
    format.Alignment = PdfTextAlignment.Center;
    format.LineAlignment = PdfVerticalAlignment.Middle;
    float num3;
    switch (this.textDisplayLocation)
    {
      case TextLocation.None:
        num3 = y - height1 - height2;
        format.Alignment = PdfTextAlignment.Center;
        break;
      case TextLocation.Top:
        y = height2;
        num3 = y - height2;
        format.Alignment = PdfTextAlignment.Center;
        format.LineAlignment = PdfVerticalAlignment.Top;
        break;
      case TextLocation.Bottom:
        num3 = y + (height1 - height2);
        format.Alignment = PdfTextAlignment.Justify;
        break;
      default:
        num3 = y + (height1 - height2);
        break;
    }
    graphics.DrawRectangle(brush1, num1 + num2, y, width1, height1);
    float num4 = num3 + (this.QuietZone.IsAll || (int) this.QuietZone.Top <= 0 ? 0.0f : (float) (int) this.QuietZone.Top);
    float num5 = num1 + (this.QuietZone.IsAll || (int) this.QuietZone.Left <= 0 ? 0.0f : (float) (int) this.QuietZone.Left);
    for (int startIndex = 0; startIndex < stringBuilder1.Length; ++startIndex)
    {
      if (text.Substring(startIndex, 1) == "1")
      {
        if ((double) num2 == (double) location.X)
          num2 = num5;
        if (startIndex > 12 && startIndex < 55 || startIndex > 57 && startIndex < 101)
          graphics.DrawRectangle(brush2, location.X + num5 + (float) this.QuietZoneNew, y + (float) this.QuietZoneNew, width2, height1 - height2);
        else
          graphics.DrawRectangle(brush2, location.X + num5 + (float) this.QuietZoneNew, y + (float) this.QuietZoneNew, width2, height1);
      }
      num5 += width2;
      if (!this.QuietZone.IsAll && (int) this.QuietZone.Left > 0)
      {
        double left = (double) this.QuietZone.Left;
      }
    }
    if (this.textDisplayLocation == TextLocation.None)
      return;
    if (this.textDisplayLocation == TextLocation.Top)
    {
      float num6 = num2 - font.MeasureString(this.CountryCode.Substring(0, 1)).Width;
      graphics.DrawString(str1.Substring(0, 1), font, brush2, new PointF(location.X + num6 + (float) this.QuietZoneNew, num4 + (float) this.QuietZoneNew), format);
      float num7 = num6 + (font.MeasureString(str1.Substring(0, 1)).Width + 53f * width2 - font.MeasureString(str1.Substring(1, 6)).Width);
      graphics.DrawString(str1.Substring(1, 6), font, brush2, new PointF(location.X + num7 + (float) this.QuietZoneNew, num4 + (float) this.QuietZoneNew), format);
      float num8 = num7 + (font.MeasureString(str1.Substring(1, 6)).Width + 11f * width2);
      graphics.DrawString(str1.Substring(7), font, brush2, new PointF(location.X + num8 + (float) this.QuietZoneNew, num4 + (float) this.QuietZoneNew), format);
    }
    else
    {
      float num9 = num2 - font.MeasureString(this.CountryCode.Substring(0, 1)).Width;
      graphics.DrawString(str1.Substring(0, 1), font, brush2, new PointF(location.X + num9 + (float) this.QuietZoneNew, num4 + (float) this.QuietZoneNew));
      float num10 = num9 + (font.MeasureString(str1.Substring(0, 1)).Width + 43f * width2 - font.MeasureString(str1.Substring(1, 6)).Width);
      graphics.DrawString(str1.Substring(1, 6), font, brush2, new PointF(location.X + num10 + (float) this.QuietZoneNew, num4 + (float) this.QuietZoneNew));
      float num11 = num10 + (font.MeasureString(str1.Substring(1, 6)).Width + 11f * width2);
      graphics.DrawString(str1.Substring(7), font, brush2, new PointF(location.X + num11 + (float) this.QuietZoneNew, num4 + (float) this.QuietZoneNew));
    }
  }

  public new virtual Image ToImage()
  {
    if (string.IsNullOrEmpty(this.Text) || this.Text.Length > 13)
      throw new PdfBarcodeException("Barcode text should be neither empty nor exceed more than 13 digits");
    float width1;
    float height1;
    if (!this.size.IsEmpty)
    {
      width1 = this.Size.Width;
      height1 = this.Size.Height;
    }
    else
    {
      width1 = this.Width;
      height1 = this.Height;
    }
    if (this.barHeightEnabled)
    {
      width1 = this.Width;
      height1 = this.BarHeight;
    }
    this.QuietZoneNew = this.GetQuiteZone();
    float width2 = width1 / 113f;
    StringBuilder stringBuilder1 = new StringBuilder();
    StringBuilder stringBuilder2 = new StringBuilder();
    System.Drawing.Font font = new System.Drawing.Font("Helvetica", this.fontSize);
    Brush brush1 = (Brush) new SolidBrush(Color.FromArgb((int) this.BackColor.A, (int) this.BackColor.R, (int) this.BackColor.G, (int) this.BackColor.B));
    Brush brush2 = (Brush) new SolidBrush(Color.Black);
    if (this.BarColor.A != (byte) 0)
    {
      Color color = Color.FromArgb(this.BarColor.ToArgb());
      if (color != Color.Black)
        brush2 = (Brush) new SolidBrush(color);
    }
    this.CalculateChecksumDigit();
    if (stringBuilder2.Length == 12)
      stringBuilder2.AppendFormat("{0}{1}", (object) this.Text, (object) this.ChecksumDigit);
    else
      stringBuilder2.AppendFormat("{0}{1}", (object) this.Text.Substring(0, 12), (object) this.ChecksumDigit);
    string str1 = stringBuilder2.ToString();
    string str2 = this.ConvertLeftPattern(str1.Substring(0, 7));
    stringBuilder1.AppendFormat("{0}{1}{2}{3}{4}{1}{0}", (object) this.QuiteZone, (object) this.LeadTail, (object) str2, (object) this.Separator, (object) this.ConvertToDigitPatterns(str1.Substring(7), this.Right));
    string text = stringBuilder1.ToString();
    float height2 = System.Drawing.Graphics.FromImage((Image) new Bitmap((int) width1, (int) height1)).MeasureString(text, font).Height;
    float num1 = 0.0f;
    float num2 = this.Location.X;
    float num3 = this.Location.Y;
    StringFormat format = new StringFormat();
    format.Alignment = StringAlignment.Near;
    format.LineAlignment = StringAlignment.Near;
    float num4;
    switch (this.textDisplayLocation)
    {
      case TextLocation.None:
        num4 = num3 - height1 - height2;
        break;
      case TextLocation.Top:
        num3 = height2;
        num4 = num3 - height2;
        height1 += height2;
        format.Alignment = StringAlignment.Near;
        break;
      case TextLocation.Bottom:
        num4 = num3 + (height1 - height2);
        format.Alignment = StringAlignment.Center;
        break;
      default:
        num4 = num3 + (height1 - height2);
        break;
    }
    Bitmap image = new Bitmap((int) width1, (int) height1 + (int) height2);
    System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage((Image) image);
    graphics.FillRectangle(brush1, 0.0f, 0.0f, width1, height1 + height2);
    float num5 = num4 + (this.QuietZone.IsAll || (int) this.QuietZone.Top <= 0 ? 0.0f : (float) (int) this.QuietZone.Top);
    float num6 = num1 + (this.QuietZone.IsAll || (int) this.QuietZone.Left <= 0 ? 0.0f : (float) (int) this.QuietZone.Left);
    for (int startIndex = 0; startIndex < stringBuilder1.Length; ++startIndex)
    {
      if (text.Substring(startIndex, 1) == "1")
      {
        if ((double) num2 == (double) this.Location.X)
          num2 = num6;
        if (startIndex > 12 && startIndex < 55 || startIndex > 57 && startIndex < 101)
          graphics.FillRectangle(brush2, this.Location.X + num6 + (float) this.QuietZoneNew, num3 + (float) this.QuietZoneNew, width2, height1 - height2);
        else
          graphics.FillRectangle(brush2, this.Location.X + num6 + (float) this.QuietZoneNew, num3 + (float) this.QuietZoneNew, width2, height1);
      }
      num6 += width2;
      if (!this.QuietZone.IsAll && (int) this.QuietZone.Left > 0)
      {
        double left = (double) this.QuietZone.Left;
      }
    }
    if (this.textDisplayLocation == TextLocation.Top)
    {
      float num7 = num2 - graphics.MeasureString(this.CountryCode.Substring(0, 1), font).Width;
      graphics.DrawString(str1.Substring(0, 1), font, brush2, new PointF(this.Location.X + num7 + (float) this.QuietZoneNew, num5 + (float) this.QuietZoneNew), format);
      float num8 = num7 + (graphics.MeasureString(str1.Substring(0, 1), font).Width + 43f * width2 - graphics.MeasureString(str1.Substring(1, 6), font).Width);
      graphics.DrawString(str1.Substring(1, 6), font, brush2, new PointF(this.Location.X + num8 + (float) this.QuietZoneNew, num5 + (float) this.QuietZoneNew), format);
      float num9 = num8 + (graphics.MeasureString(str1.Substring(1, 6), font).Width + 11f * width2);
      graphics.DrawString(str1.Substring(7), font, brush2, new PointF(this.Location.X + num9 + (float) this.QuietZoneNew, num5 + (float) this.QuietZoneNew), format);
    }
    else
    {
      float num10 = num2 - graphics.MeasureString(this.CountryCode.Substring(0, 1), font).Width;
      graphics.DrawString(str1.Substring(0, 1), font, brush2, new PointF(num10 + (float) this.QuietZoneNew, num5 + (float) this.QuietZoneNew));
      float num11 = num10 + (graphics.MeasureString(str1.Substring(0, 1), font).Width + 43f * width2 - graphics.MeasureString(str1.Substring(1, 6), font).Width);
      graphics.DrawString(str1.Substring(1, 6), font, brush2, new PointF(num11 + (float) this.QuietZoneNew, num5 + (float) this.QuietZoneNew));
      float num12 = num11 + (graphics.MeasureString(str1.Substring(1, 6), font).Width + 11f * width2);
      graphics.DrawString(str1.Substring(7), font, brush2, new PointF(num12 + (float) this.QuietZoneNew, num5 + (float) this.QuietZoneNew));
    }
    return (Image) image;
  }

  public new Image ToImage(SizeF size)
  {
    this.Size = size;
    return this.ToImage();
  }

  internal int GetQuiteZone()
  {
    int quiteZone = 2;
    if (this.QuietZone.IsAll && (double) this.QuietZone.All > 0.0)
      quiteZone = (int) this.QuietZone.All;
    return quiteZone;
  }

  private string ConvertLeftPattern(string sLeft)
  {
    switch (sLeft.Substring(0, 1))
    {
      case "0":
        return this.CountryCode0(sLeft.Substring(1));
      case "1":
        return this.CountryCode1(sLeft.Substring(1));
      case "2":
        return this.CountryCode2(sLeft.Substring(1));
      case "3":
        return this.CountryCode3(sLeft.Substring(1));
      case "4":
        return this.CountryCode4(sLeft.Substring(1));
      case "5":
        return this.CountryCode5(sLeft.Substring(1));
      case "6":
        return this.CountryCode6(sLeft.Substring(1));
      case "7":
        return this.CountryCode7(sLeft.Substring(1));
      case "8":
        return this.CountryCode8(sLeft.Substring(1));
      case "9":
        return this.CountryCode9(sLeft.Substring(1));
      default:
        return "";
    }
  }

  private string CountryCode0(string sLeft) => this.ConvertToDigitPatterns(sLeft, this.OddLeft);

  private string CountryCode1(string sLeft)
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(this.ConvertToDigitPatterns(sLeft.Substring(0, 1), this.OddLeft));
    stringBuilder.Append(this.ConvertToDigitPatterns(sLeft.Substring(1, 1), this.OddLeft));
    stringBuilder.Append(this.ConvertToDigitPatterns(sLeft.Substring(2, 1), this.EvenLeft));
    stringBuilder.Append(this.ConvertToDigitPatterns(sLeft.Substring(3, 1), this.OddLeft));
    stringBuilder.Append(this.ConvertToDigitPatterns(sLeft.Substring(4, 1), this.EvenLeft));
    stringBuilder.Append(this.ConvertToDigitPatterns(sLeft.Substring(5, 1), this.EvenLeft));
    return stringBuilder.ToString();
  }

  private string CountryCode2(string sLeft)
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(this.ConvertToDigitPatterns(sLeft.Substring(0, 1), this.OddLeft));
    stringBuilder.Append(this.ConvertToDigitPatterns(sLeft.Substring(1, 1), this.OddLeft));
    stringBuilder.Append(this.ConvertToDigitPatterns(sLeft.Substring(2, 1), this.EvenLeft));
    stringBuilder.Append(this.ConvertToDigitPatterns(sLeft.Substring(3, 1), this.EvenLeft));
    stringBuilder.Append(this.ConvertToDigitPatterns(sLeft.Substring(4, 1), this.OddLeft));
    stringBuilder.Append(this.ConvertToDigitPatterns(sLeft.Substring(5, 1), this.EvenLeft));
    return stringBuilder.ToString();
  }

  private string CountryCode3(string sLeft)
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(this.ConvertToDigitPatterns(sLeft.Substring(0, 1), this.OddLeft));
    stringBuilder.Append(this.ConvertToDigitPatterns(sLeft.Substring(1, 1), this.OddLeft));
    stringBuilder.Append(this.ConvertToDigitPatterns(sLeft.Substring(2, 1), this.EvenLeft));
    stringBuilder.Append(this.ConvertToDigitPatterns(sLeft.Substring(3, 1), this.EvenLeft));
    stringBuilder.Append(this.ConvertToDigitPatterns(sLeft.Substring(4, 1), this.EvenLeft));
    stringBuilder.Append(this.ConvertToDigitPatterns(sLeft.Substring(5, 1), this.OddLeft));
    return stringBuilder.ToString();
  }

  private string CountryCode4(string sLeft)
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(this.ConvertToDigitPatterns(sLeft.Substring(0, 1), this.OddLeft));
    stringBuilder.Append(this.ConvertToDigitPatterns(sLeft.Substring(1, 1), this.EvenLeft));
    stringBuilder.Append(this.ConvertToDigitPatterns(sLeft.Substring(2, 1), this.OddLeft));
    stringBuilder.Append(this.ConvertToDigitPatterns(sLeft.Substring(3, 1), this.OddLeft));
    stringBuilder.Append(this.ConvertToDigitPatterns(sLeft.Substring(4, 1), this.EvenLeft));
    stringBuilder.Append(this.ConvertToDigitPatterns(sLeft.Substring(5, 1), this.EvenLeft));
    return stringBuilder.ToString();
  }

  private string CountryCode5(string sLeft)
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(this.ConvertToDigitPatterns(sLeft.Substring(0, 1), this.OddLeft));
    stringBuilder.Append(this.ConvertToDigitPatterns(sLeft.Substring(1, 1), this.EvenLeft));
    stringBuilder.Append(this.ConvertToDigitPatterns(sLeft.Substring(2, 1), this.EvenLeft));
    stringBuilder.Append(this.ConvertToDigitPatterns(sLeft.Substring(3, 1), this.OddLeft));
    stringBuilder.Append(this.ConvertToDigitPatterns(sLeft.Substring(4, 1), this.OddLeft));
    stringBuilder.Append(this.ConvertToDigitPatterns(sLeft.Substring(5, 1), this.EvenLeft));
    return stringBuilder.ToString();
  }

  private string CountryCode6(string sLeft)
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(this.ConvertToDigitPatterns(sLeft.Substring(0, 1), this.OddLeft));
    stringBuilder.Append(this.ConvertToDigitPatterns(sLeft.Substring(1, 1), this.EvenLeft));
    stringBuilder.Append(this.ConvertToDigitPatterns(sLeft.Substring(2, 1), this.EvenLeft));
    stringBuilder.Append(this.ConvertToDigitPatterns(sLeft.Substring(3, 1), this.EvenLeft));
    stringBuilder.Append(this.ConvertToDigitPatterns(sLeft.Substring(4, 1), this.OddLeft));
    stringBuilder.Append(this.ConvertToDigitPatterns(sLeft.Substring(5, 1), this.OddLeft));
    return stringBuilder.ToString();
  }

  private string CountryCode7(string sLeft)
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(this.ConvertToDigitPatterns(sLeft.Substring(0, 1), this.OddLeft));
    stringBuilder.Append(this.ConvertToDigitPatterns(sLeft.Substring(1, 1), this.EvenLeft));
    stringBuilder.Append(this.ConvertToDigitPatterns(sLeft.Substring(2, 1), this.OddLeft));
    stringBuilder.Append(this.ConvertToDigitPatterns(sLeft.Substring(3, 1), this.EvenLeft));
    stringBuilder.Append(this.ConvertToDigitPatterns(sLeft.Substring(4, 1), this.OddLeft));
    stringBuilder.Append(this.ConvertToDigitPatterns(sLeft.Substring(5, 1), this.EvenLeft));
    return stringBuilder.ToString();
  }

  private string CountryCode8(string sLeft)
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(this.ConvertToDigitPatterns(sLeft.Substring(0, 1), this.OddLeft));
    stringBuilder.Append(this.ConvertToDigitPatterns(sLeft.Substring(1, 1), this.EvenLeft));
    stringBuilder.Append(this.ConvertToDigitPatterns(sLeft.Substring(2, 1), this.OddLeft));
    stringBuilder.Append(this.ConvertToDigitPatterns(sLeft.Substring(3, 1), this.EvenLeft));
    stringBuilder.Append(this.ConvertToDigitPatterns(sLeft.Substring(4, 1), this.EvenLeft));
    stringBuilder.Append(this.ConvertToDigitPatterns(sLeft.Substring(5, 1), this.OddLeft));
    return stringBuilder.ToString();
  }

  private string CountryCode9(string sLeft)
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(this.ConvertToDigitPatterns(sLeft.Substring(0, 1), this.OddLeft));
    stringBuilder.Append(this.ConvertToDigitPatterns(sLeft.Substring(1, 1), this.EvenLeft));
    stringBuilder.Append(this.ConvertToDigitPatterns(sLeft.Substring(2, 1), this.EvenLeft));
    stringBuilder.Append(this.ConvertToDigitPatterns(sLeft.Substring(3, 1), this.OddLeft));
    stringBuilder.Append(this.ConvertToDigitPatterns(sLeft.Substring(4, 1), this.EvenLeft));
    stringBuilder.Append(this.ConvertToDigitPatterns(sLeft.Substring(5, 1), this.OddLeft));
    return stringBuilder.ToString();
  }

  private string ConvertToDigitPatterns(string inputNumber, string[] patterns)
  {
    StringBuilder stringBuilder = new StringBuilder();
    for (int startIndex = 0; startIndex < inputNumber.Length; ++startIndex)
    {
      int int32 = Convert.ToInt32(inputNumber.Substring(startIndex, 1));
      stringBuilder.Append(patterns[int32]);
    }
    return stringBuilder.ToString();
  }

  internal void CalculateChecksumDigit()
  {
    string text = this.Text;
    int num = 0;
    int length1 = text.Length;
    if (text.Length == 13)
    {
      for (int index = text.Length - 1; index >= 1; --index)
      {
        int int32 = Convert.ToInt32(text.Substring(index - 1, 1));
        if (index % 2 == 0)
          num += int32 * 3;
        else
          num += int32;
      }
      char ch1 = ((10 - num % 10) % 10).ToString()[0];
      char ch2 = text[12];
      this.ChecksumDigit = (int) ch2 == (int) ch1 ? ch2.ToString() : throw new Exception("Error calculating check digit");
    }
    else
    {
      for (int length2 = text.Length; length2 >= 1; --length2)
      {
        int int32 = Convert.ToInt32(text.Substring(length2 - 1, 1));
        if (length2 % 2 == 0)
          num += int32 * 3;
        else
          num += int32;
      }
      this.ChecksumDigit = ((10 - num % 10) % 10).ToString();
    }
  }
}
