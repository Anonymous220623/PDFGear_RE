// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Barcode.PdfEan8Barcode
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Barcode;

public class PdfEan8Barcode : PdfUnidimensionalBarcode
{
  private string[] codesA = new string[10]
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
  private string[] codesC = new string[10]
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
  private string data;
  private string encodedData;
  private float fontSize = 8f;
  private int quietZonePixel = 4;

  internal int QuietZoneNew
  {
    get => this.quietZonePixel;
    set => this.quietZonePixel = value;
  }

  public PdfEan8Barcode()
  {
  }

  public PdfEan8Barcode(string text)
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
    int index = 0;
    float num1 = 2f;
    if (!this.CheckNumericOnly(this.Text))
      throw new Exception("Numeric only");
    this.data = this.Text.Length != 7 ? this.Text.Substring(0, 7) + this.CheckDigit(this.Text) : this.Text + this.CheckDigit(this.Text);
    this.encodedData = this.GetEncoding();
    float width1;
    float height;
    if (!this.size.IsEmpty)
    {
      num1 = this.Size.Width / (float) this.encodedData.Length;
      width1 = this.Size.Width;
      height = this.Size.Height;
    }
    else
    {
      width1 = num1 * (float) this.encodedData.Length;
      height = width1 / 2f;
    }
    if (this.barHeightEnabled)
    {
      width1 = num1 * (float) this.encodedData.Length;
      height = this.BarHeight;
    }
    this.QuietZoneNew = this.GetQuiteZone();
    float width2 = width1 / (float) this.encodedData.Length;
    float num2 = (float) (int) ((double) width2 * 0.5);
    if ((double) width2 <= 0.0)
      throw new Exception("Image size specified not large enough to draw image");
    float x = location.X;
    float y = location.Y;
    PdfFont font = this.Font != null ? this.Font : (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, this.fontSize);
    PdfGraphics graphics = page.Graphics;
    PdfBrush brush1 = (PdfBrush) new PdfSolidBrush((PdfColor) Color.FromArgb((int) this.BackColor.A, (int) this.BackColor.R, (int) this.BackColor.G, (int) this.BackColor.B));
    PdfBrush brush2 = PdfBrushes.Black;
    if (this.BarColor.A != (byte) 0)
    {
      Color color = Color.FromArgb(this.BarColor.ToArgb());
      if (color != Color.Black)
        brush2 = (PdfBrush) new PdfSolidBrush((PdfColor) color);
    }
    PdfPen pen = new PdfPen(brush2, width2);
    float num3 = 0.0f;
    PdfStringFormat format = new PdfStringFormat();
    format.Alignment = PdfTextAlignment.Center;
    format.LineAlignment = PdfVerticalAlignment.Middle;
    switch (this.textDisplayLocation)
    {
      case TextLocation.Top:
        num3 = 0.0f;
        format.Alignment = PdfTextAlignment.Center;
        break;
      case TextLocation.Bottom:
        num3 = (float) ((int) height - (int) font.Height);
        format.Alignment = PdfTextAlignment.Center;
        break;
    }
    graphics.DrawRectangle(brush1, x, y, width1, height);
    float num4 = 0.0f;
    float num5 = (float) (0.0 + (this.QuietZone.IsAll || (int) this.QuietZone.Top <= 0 ? 0.0 : (double) (int) this.QuietZone.Top));
    float num6 = num4 + (this.QuietZone.IsAll || (int) this.QuietZone.Left <= 0 ? 0.0f : (float) (int) this.QuietZone.Left);
    for (; index < this.encodedData.Length; ++index)
    {
      if (this.encodedData[index] == '1')
      {
        graphics.DrawLine(pen, new PointF((float) index * width2 + num2 + (float) (int) location.X + (float) (int) num6 + (float) this.QuietZoneNew, (float) ((int) location.Y + (int) num5 + this.QuietZoneNew)), new PointF((float) index * width2 + num2 + (float) (int) location.X + (float) (int) num6 + (float) this.QuietZoneNew, (float) ((int) height + (int) location.Y + (int) num5 + this.QuietZoneNew)));
        graphics.DrawRectangle(PdfBrushes.White, new RectangleF(0.0f + x + (float) this.QuietZoneNew, num3 + y + (float) this.QuietZoneNew, width1, font.Height));
        graphics.DrawString(this.data, font, brush2, new RectangleF(0.0f + x + (float) this.QuietZoneNew, y + num3 + (float) this.QuietZoneNew, width1, font.Height), format);
      }
    }
  }

  public new virtual Image ToImage()
  {
    int index = 0;
    float num1 = 2f;
    if (!this.CheckNumericOnly(this.Text))
      throw new Exception("Numeric only");
    this.data = this.Text.Length != 7 ? this.Text.Substring(0, 7) + this.CheckDigit(this.Text) : this.Text + this.CheckDigit(this.Text);
    this.encodedData = this.GetEncoding();
    float width1;
    float height;
    if (!this.size.IsEmpty)
    {
      num1 = this.Size.Width / (float) this.encodedData.Length;
      width1 = this.Size.Width;
      height = this.Size.Height;
    }
    else
    {
      width1 = num1 * (float) this.encodedData.Length;
      height = width1 / 2f;
    }
    if (this.barHeightEnabled)
    {
      width1 = num1 * (float) this.encodedData.Length;
      height = this.BarHeight;
    }
    this.QuietZoneNew = this.GetQuiteZone();
    float width2 = width1 / (float) this.encodedData.Length;
    float num2 = (float) (int) ((double) width2 * 0.5);
    if ((double) width2 <= 0.0)
      throw new Exception("Image size specified not large enough to draw image");
    System.Drawing.Font font = new System.Drawing.Font("Helvetica", this.fontSize);
    Brush brush1 = (Brush) new SolidBrush(Color.FromArgb((int) this.BackColor.A, (int) this.BackColor.R, (int) this.BackColor.G, (int) this.BackColor.B));
    Brush brush2 = Brushes.Black;
    if (this.BarColor.A != (byte) 0)
    {
      Color color = Color.FromArgb(this.BarColor.ToArgb());
      if (color != Color.Black)
        brush2 = (Brush) new SolidBrush(color);
    }
    Pen pen = new Pen(brush2, width2);
    Bitmap image = new Bitmap((int) width1, (int) height);
    System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage((Image) image);
    float num3 = 0.0f;
    StringFormat format = new StringFormat();
    format.Alignment = StringAlignment.Near;
    format.LineAlignment = StringAlignment.Near;
    switch (this.textDisplayLocation)
    {
      case TextLocation.None:
        num3 = (float) (image.Height + font.Height);
        break;
      case TextLocation.Top:
        num3 = 0.0f;
        format.Alignment = StringAlignment.Center;
        break;
      case TextLocation.Bottom:
        num3 = (float) (image.Height - font.Height);
        format.Alignment = StringAlignment.Center;
        break;
    }
    graphics.FillRectangle(brush1, 0.0f, 0.0f, width1, height);
    float num4 = 0.0f;
    float num5 = (float) (0.0 + (this.QuietZone.IsAll || (int) this.QuietZone.Top <= 0 ? 0.0 : (double) (int) this.QuietZone.Top));
    float num6 = num4 + (this.QuietZone.IsAll || (int) this.QuietZone.Left <= 0 ? 0.0f : (float) (int) this.QuietZone.Left);
    for (; index < this.encodedData.Length; ++index)
    {
      if (this.encodedData[index] == '1')
      {
        graphics.DrawLine(pen, new PointF((float) index * width2 + num2 + (float) (int) num6 + (float) this.QuietZoneNew, (float) ((int) num5 + this.QuietZoneNew)), new PointF((float) index * width2 + num2 + (float) (int) num6 + (float) this.QuietZoneNew, (float) ((int) height + (int) num5 + this.QuietZoneNew)));
        graphics.FillRectangle((Brush) new SolidBrush(Color.White), new RectangleF((float) this.QuietZoneNew, num3 + (float) this.QuietZoneNew, (float) image.Width, (float) font.Height));
        graphics.DrawString(this.data, font, brush2, new RectangleF((float) this.QuietZoneNew, num3 + (float) this.QuietZoneNew, (float) image.Width, (float) font.Height), format);
      }
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

  protected bool CheckNumericOnly(string data) => data != null && long.TryParse(data, out long _);

  internal string GetEncoding()
  {
    if (this.data.Length != 8 && this.data.Length != 7)
      throw new Exception("Invalid data length(Barcode text length should be 7 or 8 digits only)");
    string str1 = "101";
    for (int index = 0; index < this.data.Length / 2; ++index)
      str1 += this.codesA[int.Parse(this.data[index].ToString())];
    string str2 = str1 + "01010";
    for (int index = this.data.Length / 2; index < this.data.Length; ++index)
      str2 += this.codesC[int.Parse(this.data[index].ToString())];
    return str2 + "101";
  }

  private string CheckDigit(string data)
  {
    if (data.Length == 7)
    {
      int num1 = 0;
      int num2 = 0;
      for (int startIndex = 0; startIndex <= 6; startIndex += 2)
        num2 += int.Parse(data.Substring(startIndex, 1)) * 3;
      for (int startIndex = 1; startIndex <= 5; startIndex += 2)
        num1 += int.Parse(data.Substring(startIndex, 1));
      int num3 = 10 - (num1 + num2) % 10;
      if (num3 == 10)
        num3 = 0;
      return num3.ToString();
    }
    int num4 = 0;
    int num5 = 0;
    for (int startIndex = 0; startIndex <= 6; startIndex += 2)
      num5 += int.Parse(data.Substring(startIndex, 1)) * 3;
    for (int startIndex = 1; startIndex <= 5; startIndex += 2)
      num4 += int.Parse(data.Substring(startIndex, 1));
    int num6 = 10 - (num4 + num5) % 10;
    if (num6 == 10)
      num6 = 0;
    char ch1 = num6.ToString()[0];
    char ch2 = this.Text[7];
    return (int) ch2 == (int) ch1 ? ch2.ToString() : throw new Exception("Error calculating check digit");
  }
}
