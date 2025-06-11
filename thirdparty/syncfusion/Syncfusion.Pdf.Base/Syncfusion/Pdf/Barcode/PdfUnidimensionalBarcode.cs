// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Barcode.PdfUnidimensionalBarcode
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.Pdf.Barcode;

public abstract class PdfUnidimensionalBarcode : PdfBarcode
{
  private const float UpcBarWidth = 1.0734f;
  private const int dpi = 96 /*0x60*/;
  internal int barcodeSpaceCount;
  internal bool isCheckDigitAdded;
  internal bool continuous;
  protected bool check;
  private Dictionary<char, BarcodeSymbolTable> barcodeSymbols = new Dictionary<char, BarcodeSymbolTable>();
  private Dictionary<string, BarcodeSymbolTable> barcodeSymbolsString = new Dictionary<string, BarcodeSymbolTable>();
  internal TextLocation textDisplayLocation;
  private PdfFont font;
  internal char startSymbol;
  internal char stopSymbol;
  private string validatorExpression = string.Empty;
  private Regex codeValidator;
  private bool showCheckDigit;
  private bool enableCheckDigit;
  internal float interCharacterGap;
  internal float barcodeToTextGapHeight;
  private string barcodeEncodeText = string.Empty;
  private PdfBarcodeTextAlignment textAlignment;
  private bool encodeStartStopSymbols;
  internal bool isFontModified;
  private bool isContainsSmallSize;

  public PdfUnidimensionalBarcode()
  {
    this.startSymbol = char.MinValue;
    this.stopSymbol = char.MinValue;
    this.interCharacterGap = 1f;
    this.barcodeToTextGapHeight = 5f;
    if (PdfDocument.ConformanceLevel == PdfConformanceLevel.None)
      this.font = (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, 8f);
    this.textAlignment = PdfBarcodeTextAlignment.Center;
    this.textDisplayLocation = TextLocation.Bottom;
    this.encodeStartStopSymbols = true;
    this.enableCheckDigit = false;
  }

  public PdfFont Font
  {
    get => this.font;
    set
    {
      this.font = value;
      this.isFontModified = true;
    }
  }

  public TextLocation TextDisplayLocation
  {
    get => this.textDisplayLocation;
    set => this.textDisplayLocation = value;
  }

  public bool ShowCheckDigit
  {
    get => this.showCheckDigit;
    set => this.showCheckDigit = value;
  }

  public bool EnableCheckDigit
  {
    get => this.enableCheckDigit;
    set => this.enableCheckDigit = value;
  }

  public float BarcodeToTextGapHeight
  {
    get => this.barcodeToTextGapHeight;
    set
    {
      this.barcodeToTextGapHeight = (double) value >= 0.0 ? value : throw new PdfBarcodeException("Text to barcode gap cannot be negative.");
    }
  }

  public PdfBarcodeTextAlignment TextAlignment
  {
    get => this.textAlignment;
    set => this.textAlignment = value;
  }

  public bool EncodeStartStopSymbols
  {
    get => this.encodeStartStopSymbols;
    set => this.encodeStartStopSymbols = value;
  }

  internal Dictionary<char, BarcodeSymbolTable> BarcodeSymbols
  {
    get => this.barcodeSymbols;
    set => this.barcodeSymbols = value;
  }

  internal Dictionary<string, BarcodeSymbolTable> BarcodeSymbolsString
  {
    get => this.barcodeSymbolsString;
    set => this.barcodeSymbolsString = value;
  }

  internal char StartSymbol
  {
    get => this.startSymbol;
    set => this.startSymbol = value;
  }

  internal char StopSymbol
  {
    get => this.stopSymbol;
    set => this.stopSymbol = value;
  }

  internal string ValidatorExpression
  {
    get => this.validatorExpression;
    set => this.validatorExpression = value;
  }

  internal float IntercharacterGap
  {
    get => this.interCharacterGap;
    set => this.interCharacterGap = value;
  }

  public virtual void Draw(PdfPageBase page, PointF location)
  {
    this.Location = location;
    bool flag1 = false;
    if (page is PdfPage && (page as PdfPage).Document != null)
      flag1 = (page as PdfPage).Document.AutoTag;
    switch (this)
    {
      case PdfGS1Code128Barcode _:
      case PdfCode128Barcode _:
        this.DrawRevamped(page, location);
        break;
      default:
        string text = this.Text;
        this.Text = this.Text.Replace("[FNC1]", "");
        if (!this.Validate(this.Text.Replace("[FNC1]", "")))
          throw new PdfBarcodeException("Barcode Text contains characters that are not accepted by this barcode specification.");
        string str = this.GetTextToEncode();
        this.barcodeEncodeText = str;
        float num1 = 0.0f;
        if (str == null || str.Length == 0)
          throw new PdfBarcodeException("Barcode Text cannot be null or empty.");
        if (this.encodeStartStopSymbols && this.startSymbol != char.MinValue && this.stopSymbol != char.MinValue)
          str = this.startSymbol.ToString() + str + this.stopSymbol.ToString();
        SizeF sizeF1 = this.Font.MeasureString(this.Text);
        float width1;
        float height;
        if (this.size != SizeF.Empty)
        {
          width1 = this.Size.Width;
          height = this.Size.Height;
        }
        else
        {
          float num2 = this.QuietZone.Left + this.QuietZone.Right;
          foreach (char character in str)
            num2 += this.GetCharWidth(character) + this.interCharacterGap;
          width1 = this.continuous ? (this.ExtendedText.Length <= 0 ? num2 - this.interCharacterGap : num2 - this.interCharacterGap * (float) (this.ExtendedText.Length - this.Text.Length)) : num2 - this.interCharacterGap * (float) str.Length;
          height = this.QuietZone.Top + this.QuietZone.Bottom + this.BarHeight;
          if (this.textDisplayLocation == TextLocation.Top)
            height += sizeF1.Height;
          if (!this.barHeightEnabled)
            this.Size = new SizeF(width1, height);
        }
        PdfBrush brush1 = (PdfBrush) new PdfSolidBrush((PdfColor) Color.FromArgb((int) this.BackColor.A, (int) this.BackColor.R, (int) this.BackColor.G, (int) this.BackColor.B));
        RectangleF rectangle = new RectangleF();
        if (this.textDisplayLocation != TextLocation.None && this.size == SizeF.Empty)
          height += this.BarcodeToTextGapHeight;
        if (this.textDisplayLocation == TextLocation.Top)
          rectangle = new RectangleF(location, new SizeF(width1, height));
        if (this.textDisplayLocation == TextLocation.Bottom)
          rectangle = new RectangleF(location, new SizeF(width1, height));
        if (this.textDisplayLocation == TextLocation.None)
        {
          if (this.QuietZone.IsAll && (double) this.QuietZone.All > 0.0)
            height -= 2f * this.QuietZone.All;
          else if ((double) this.QuietZone.Top > 0.0 || (double) this.QuietZone.Bottom > 0.0)
          {
            float num3 = this.QuietZone.Top + this.QuietZone.Bottom;
            height -= num3;
          }
          rectangle = new RectangleF(location, new SizeF(width1, height));
        }
        PdfTemplate template = (PdfTemplate) null;
        if (flag1)
        {
          template = new PdfTemplate(new SizeF(width1, height));
          template.Graphics.DrawRectangle(brush1, rectangle);
        }
        else if (this.size != SizeF.Empty)
          page.Graphics.DrawRectangle(brush1, new RectangleF(location.X, location.Y, this.size.Width, this.size.Height));
        else
          page.Graphics.DrawRectangle(brush1, rectangle);
        this.Bounds = rectangle;
        float x1;
        if (this.size != SizeF.Empty)
        {
          this.GetSizeValue();
          float num4 = 0.0f;
          foreach (char character in str)
            num4 += this.GetCharBarsCount(character);
          float num5 = width1 / num4;
          if ((double) this.NarrowBarWidth <= 1.0 || (double) this.NarrowBarWidth >= (double) num5 || (double) num5 >= (double) this.NarrowBarWidth)
          {
            this.NarrowBarWidth = (float) (((double) width1 - ((double) this.QuietZone.Left + (double) this.QuietZone.Right)) / ((double) num4 + (double) this.barcodeSpaceCount));
            this.IntercharacterGap = !(this is PdfCodeUpcBarcode) ? this.NarrowBarWidth : this.NarrowBarWidth * 1.0734f;
          }
          this.BarHeight = height - (this.QuietZone.Top + this.QuietZone.Bottom) - sizeF1.Height - this.BarcodeToTextGapHeight;
          x1 = this.QuietZone.Left + rectangle.Left;
        }
        else
          x1 = this.QuietZone.Left + rectangle.Left;
        num1 = 0.0f + rectangle.Top;
        float y = this.textDisplayLocation != TextLocation.Top ? location.Y + this.QuietZone.Top : (!this.QuietZone.IsAll || (double) this.QuietZone.All <= 0.0 ? ((double) this.QuietZone.Bottom > 0.0 || (double) this.QuietZone.Top > 0.0 && (double) this.QuietZone.Bottom > 0.0 ? ((double) this.QuietZone.Top <= 0.0 || (double) this.QuietZone.Bottom <= 0.0 ? location.Y + (sizeF1.Height + this.BarcodeToTextGapHeight) : location.Y + (this.QuietZone.Top + sizeF1.Height + this.BarcodeToTextGapHeight)) : location.Y + (rectangle.Height - this.BarHeight)) : location.Y + (rectangle.Height - this.QuietZone.Top - this.BarHeight));
        for (int index1 = 0; index1 < str.Length; ++index1)
        {
          char ch = str[index1];
          foreach (KeyValuePair<char, BarcodeSymbolTable> barcodeSymbol in this.BarcodeSymbols)
          {
            BarcodeSymbolTable barcodeSymbolTable = barcodeSymbol.Value;
            if ((int) barcodeSymbolTable.Symbol == (int) ch)
            {
              byte[] bars = barcodeSymbolTable.Bars;
              RectangleF barRect = new RectangleF();
              for (int index2 = 0; index2 < bars.Length; ++index2)
              {
                float width2 = !(this is PdfCodeUpcBarcode) ? (float) bars[index2] * this.NarrowBarWidth : (float) ((double) bars[index2] * (double) this.NarrowBarWidth * 1.0734000205993652);
                if (bars[index2] == (byte) 0)
                  width2 = this.NarrowBarWidth;
                barRect = this.textDisplayLocation != TextLocation.None ? new RectangleF(x1, y, width2, this.BarHeight) : new RectangleF(x1, y, width2, rectangle.Height);
                if (this is PdfCodeUpcBarcode && index1 > 1 && index1 <= 7)
                {
                  if (index2 % 2 != 0)
                  {
                    if (flag1)
                      x1 += this.PaintRectangleTag(template, barRect);
                    else
                      x1 += this.PaintRectangle(page, barRect);
                  }
                  else
                    x1 += width2;
                }
                else if (index2 % 2 == 0)
                {
                  if (flag1)
                    x1 += this.PaintRectangleTag(template, barRect);
                  else
                    x1 += this.PaintRectangle(page, barRect);
                }
                else
                {
                  x1 += width2;
                  if (this is PdfCodeUpcBarcode && bars.Length % 2 == 0 && index1 == 1 && index2 == bars.Length - 1)
                    x1 -= width2;
                }
              }
              if (bars.Length % 2 != 0 || this is PdfCodeUpcBarcode && bars.Length % 2 == 0 && index1 == 7)
              {
                if (this.size != SizeF.Empty)
                {
                  x1 += this.IntercharacterGap;
                  if (bars.Length % 2 != 0)
                    --this.barcodeSpaceCount;
                }
                else
                  x1 += this.IntercharacterGap;
              }
            }
          }
        }
        if (flag1)
          page.Graphics.DrawPdfTemplate(template, location);
        if (this.textDisplayLocation != TextLocation.None)
        {
          this.Text = this.Text.Trim(this.startSymbol);
          this.Text = this.Text.Trim(this.stopSymbol);
          PdfStringFormat format = new PdfStringFormat((PdfTextAlignment) this.textAlignment);
          PdfBrush brush2 = (PdfBrush) new PdfSolidBrush((PdfColor) Color.FromArgb((int) this.TextColor.A, (int) this.TextColor.R, (int) this.TextColor.G, (int) this.TextColor.B));
          float x2;
          float width3;
          if (this.textAlignment == PdfBarcodeTextAlignment.Left)
          {
            x2 = rectangle.Left + this.QuietZone.Left;
            width3 = rectangle.Width;
          }
          else if (this.textAlignment == PdfBarcodeTextAlignment.Right)
          {
            x2 = rectangle.Left;
            width3 = rectangle.Width - this.QuietZone.Right;
          }
          else
          {
            x2 = rectangle.Left + this.QuietZone.Left;
            width3 = rectangle.Width - (this.QuietZone.Right + this.QuietZone.Left);
          }
          if (!this.isFontModified && this.size != SizeF.Empty)
          {
            SizeF sizeF2 = this.Font.MeasureString(this.Text);
            float size = this.Font.Size;
            int num6 = 0;
            while ((double) sizeF2.Width > (double) width1)
            {
              this.Font = (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, --size);
              sizeF2 = this.Font.MeasureString(this.Text);
              if ((double) sizeF2.Width <= (double) width1)
              {
                this.Font = (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, size);
                break;
              }
              ++num6;
            }
          }
          string s = string.Empty;
          bool flag2 = false;
          if (text.Contains("FNC1"))
          {
            string[] strArray = text.Split(new string[1]
            {
              "[FNC1]"
            }, StringSplitOptions.RemoveEmptyEntries);
            for (int index = 0; index < strArray.Length; ++index)
            {
              flag2 = true;
              s = !strArray[index].Contains("(") || !strArray[index].Contains(")") ? s + strArray[index].Insert(0, "(").Insert(3, ")") : s + strArray[index];
            }
          }
          if (this.textDisplayLocation == TextLocation.Top)
          {
            RectangleF layoutRectangle = new RectangleF(new PointF(x2, location.Y + this.QuietZone.Top), new SizeF(width3, sizeF1.Height));
            if (!flag2)
              page.Graphics.DrawString(this.Text, this.Font, brush2, layoutRectangle, format);
            else
              page.Graphics.DrawString(s, this.Font, brush2, layoutRectangle, format);
          }
          else
          {
            RectangleF layoutRectangle = new RectangleF();
            layoutRectangle = new RectangleF(new PointF(x2, location.Y + this.QuietZone.Top + this.barcodeToTextGapHeight + this.BarHeight), new SizeF(width3, sizeF1.Height));
            if (this is PdfCodeUpcBarcode)
            {
              char[] charArray = str.ToCharArray();
              string empty = string.Empty;
              for (int index = 0; index < charArray.Length; ++index)
              {
                if (index > 1 && index <= 7 || index > 8 && index <= 14)
                  empty += (string) (object) charArray[index];
              }
              page.Graphics.DrawString(empty, this.Font, brush2, layoutRectangle, format);
            }
            else if (!flag2)
              page.Graphics.DrawString(this.Text, this.Font, brush2, layoutRectangle, format);
            else
              page.Graphics.DrawString(s, this.Font, brush2, layoutRectangle, format);
          }
        }
        this.Text = text;
        break;
    }
  }

  public void Draw(PdfPageBase page, PointF location, SizeF size)
  {
    this.Draw(page, location.X, location.Y, size.Width, size.Height);
  }

  public void Draw(PdfPageBase page, RectangleF rectangle)
  {
    this.Draw(page, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
  }

  public void Draw(PdfPageBase page, float x, float y, float width, float height)
  {
    bool flag = false;
    if (page is PdfPage && (page as PdfPage).Document != null)
      flag = (page as PdfPage).Document.AutoTag;
    switch (this)
    {
      case PdfGS1Code128Barcode _:
      case PdfCode128CBarcode _:
        this.DrawRevamped(page, x, y, width, height);
        break;
      case PdfCode128Barcode _:
        this.DrawRevamped(page, x, y, width, height);
        break;
      default:
        string text = this.Text;
        if (!this.Validate(this.Text))
          throw new PdfBarcodeException("Barcode Text contains characters that are not accepted by this barcode specification.");
        string str = this.GetTextToEncode();
        float num1 = 0.0f;
        this.barcodeSpaceCount = 0;
        float num2 = this.NarrowBarWidth;
        if (str == null || str.Length == 0)
          throw new PdfBarcodeException("Barcode Text cannot be null or empty.");
        if (this.encodeStartStopSymbols && this.startSymbol != char.MinValue && this.stopSymbol != char.MinValue)
          str = this.startSymbol.ToString() + str + this.stopSymbol.ToString();
        SizeF sizeF1 = this.Font.MeasureString(this.Text);
        float width1 = width;
        float height1 = height;
        this.Location = new PointF(x, y);
        this.Size = new SizeF(width1, height1);
        PdfBrush brush1 = (PdfBrush) new PdfSolidBrush((PdfColor) Color.FromArgb((int) this.BackColor.A, (int) this.BackColor.R, (int) this.BackColor.G, (int) this.BackColor.B));
        RectangleF rectangle = new RectangleF();
        if (this.textDisplayLocation == TextLocation.Top)
        {
          if (this.size == SizeF.Empty)
            height1 += this.BarcodeToTextGapHeight;
          rectangle = new RectangleF(x, y, width1, height1);
        }
        if (this.textDisplayLocation == TextLocation.Bottom)
        {
          if (this.size == SizeF.Empty)
            height1 += this.BarcodeToTextGapHeight;
          rectangle = new RectangleF(x, y, width1, height1);
        }
        if (this.textDisplayLocation == TextLocation.None)
        {
          if (this.QuietZone.IsAll && (double) this.QuietZone.All > 0.0)
            height1 -= 2f * this.QuietZone.All;
          else if ((double) this.QuietZone.Top > 0.0 || (double) this.QuietZone.Bottom > 0.0)
          {
            float num3 = this.QuietZone.Top + this.QuietZone.Bottom;
            height1 -= num3;
          }
          rectangle = new RectangleF(x, y, width1, height1);
        }
        PdfTemplate template = (PdfTemplate) null;
        if (flag)
        {
          template = new PdfTemplate(new SizeF(width, height));
          template.Graphics.DrawRectangle(brush1, rectangle);
        }
        else if (this.size != SizeF.Empty)
          page.Graphics.DrawRectangle(brush1, new RectangleF(x, y, this.size.Width, this.size.Height));
        else
          page.Graphics.DrawRectangle(brush1, rectangle);
        this.Bounds = rectangle;
        this.GetSizeValue();
        float num4 = 0.0f;
        foreach (char character in str)
          num4 += this.GetCharBarsCount(character);
        float num5 = width1 / num4;
        if ((double) this.NarrowBarWidth <= 1.0 || (double) this.NarrowBarWidth >= (double) num5 || (double) num5 >= (double) this.NarrowBarWidth && (double) this.NarrowBarWidth != (double) num5)
        {
          num2 = (float) (((double) width1 - ((double) this.QuietZone.Left + (double) this.QuietZone.Right)) / ((double) num4 + (double) this.barcodeSpaceCount));
          this.IntercharacterGap = !(this is PdfCodeUpcBarcode) ? num2 : num2 * 1.0734f;
        }
        this.BarHeight = height1 - (this.QuietZone.Top + this.QuietZone.Bottom) - sizeF1.Height - this.BarcodeToTextGapHeight;
        float x1 = this.QuietZone.Left + rectangle.Left;
        num1 = 0.0f + rectangle.Top;
        float y1 = this.textDisplayLocation != TextLocation.Top ? y + this.QuietZone.Top : (!this.QuietZone.IsAll || (double) this.QuietZone.All <= 0.0 ? ((double) this.QuietZone.Bottom > 0.0 || (double) this.QuietZone.Top > 0.0 && (double) this.QuietZone.Bottom > 0.0 ? ((double) this.QuietZone.Top <= 0.0 || (double) this.QuietZone.Bottom <= 0.0 ? y + (sizeF1.Height + this.BarcodeToTextGapHeight) : y + (this.QuietZone.Top + sizeF1.Height + this.BarcodeToTextGapHeight)) : y + (rectangle.Height - this.BarHeight)) : y + (rectangle.Height - this.QuietZone.Top - this.BarHeight));
        for (int index1 = 0; index1 < str.Length; ++index1)
        {
          char ch = str[index1];
          foreach (KeyValuePair<char, BarcodeSymbolTable> barcodeSymbol in this.BarcodeSymbols)
          {
            BarcodeSymbolTable barcodeSymbolTable = barcodeSymbol.Value;
            if ((int) barcodeSymbolTable.Symbol == (int) ch)
            {
              byte[] bars = barcodeSymbolTable.Bars;
              RectangleF barRect = new RectangleF();
              for (int index2 = 0; index2 < bars.Length; ++index2)
              {
                float width2 = !(this is PdfCodeUpcBarcode) ? (float) bars[index2] * num2 : (float) ((double) bars[index2] * (double) num2 * 1.0734000205993652);
                barRect = this.textDisplayLocation != TextLocation.None ? new RectangleF(x1, y1, width2, this.BarHeight) : new RectangleF(x1, y1, width2, rectangle.Height);
                if (this is PdfCodeUpcBarcode && index1 > 1 && index1 <= 7)
                {
                  if (index2 % 2 != 0)
                  {
                    if (flag)
                      x1 += this.PaintRectangleTag(template, barRect);
                    else
                      x1 += this.PaintRectangle(page, barRect);
                  }
                  else
                    x1 += width2;
                }
                else if (index2 % 2 == 0)
                {
                  if (flag)
                    x1 += this.PaintRectangleTag(template, barRect);
                  else
                    x1 += this.PaintRectangle(page, barRect);
                }
                else
                {
                  x1 += width2;
                  if (this is PdfCodeUpcBarcode && bars.Length % 2 == 0 && index1 == 1 && index2 == bars.Length - 1)
                    x1 -= width2;
                }
              }
              if (bars.Length % 2 != 0 || this is PdfCodeUpcBarcode && bars.Length % 2 == 0 && index1 == 7)
                x1 += this.IntercharacterGap;
            }
          }
        }
        if (flag)
          page.Graphics.DrawPdfTemplate(template, new PointF(x, y));
        if (this.textDisplayLocation != TextLocation.None)
        {
          this.Text = this.Text.Trim(this.startSymbol);
          this.Text = this.Text.Trim(this.stopSymbol);
          PdfStringFormat format = new PdfStringFormat((PdfTextAlignment) this.textAlignment);
          PdfBrush brush2 = (PdfBrush) new PdfSolidBrush((PdfColor) Color.FromArgb((int) this.TextColor.A, (int) this.TextColor.R, (int) this.TextColor.G, (int) this.TextColor.B));
          float x2;
          float width3;
          if (this.textAlignment == PdfBarcodeTextAlignment.Left)
          {
            x2 = rectangle.Left + this.QuietZone.Left;
            width3 = rectangle.Width;
          }
          else if (this.textAlignment == PdfBarcodeTextAlignment.Right)
          {
            x2 = rectangle.Left;
            width3 = rectangle.Width - this.QuietZone.Right;
          }
          else
          {
            x2 = rectangle.Left + this.QuietZone.Left;
            width3 = rectangle.Width - (this.QuietZone.Right + this.QuietZone.Left);
          }
          if (!this.isFontModified)
          {
            SizeF sizeF2 = this.Font.MeasureString(this.Text);
            float size = this.Font.Size;
            int num6 = 0;
            while ((double) sizeF2.Width > (double) width1)
            {
              this.Font = (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, --size);
              sizeF2 = this.Font.MeasureString(this.Text);
              if ((double) sizeF2.Width <= (double) width1)
              {
                this.Font = (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, size);
                break;
              }
              ++num6;
            }
          }
          if (this.textDisplayLocation == TextLocation.Top)
          {
            RectangleF layoutRectangle = new RectangleF(new PointF(x2, y + this.QuietZone.Top), new SizeF(width3, sizeF1.Height));
            page.Graphics.DrawString(this.Text, this.Font, brush2, layoutRectangle, format);
          }
          else
          {
            RectangleF layoutRectangle = new RectangleF();
            layoutRectangle = new RectangleF(new PointF(x2, y + this.QuietZone.Top + this.barcodeToTextGapHeight + this.BarHeight), new SizeF(width3, sizeF1.Height));
            if (this is PdfCodeUpcBarcode)
            {
              char[] charArray = str.ToCharArray();
              string empty = string.Empty;
              for (int index = 0; index < charArray.Length; ++index)
              {
                if (index > 1 && index <= 7 || index > 8 && index <= 14)
                  empty += (string) (object) charArray[index];
              }
              page.Graphics.DrawString(empty, this.Font, brush2, layoutRectangle, format);
            }
            else
              page.Graphics.DrawString(this.Text, this.Font, brush2, layoutRectangle, format);
          }
        }
        this.Text = text;
        break;
    }
  }

  public virtual void Draw(PdfPageBase page) => this.Draw(page, this.Location);

  internal void DrawRevamped(PdfPageBase page, PointF location)
  {
    string text = this.Text;
    this.isCheckDigitAdded = false;
    PdfBarcodeQuietZones quietZone = this.QuietZone;
    if (!(this is PdfGS1Code128Barcode) && !this.Validate(this.Text.Replace("(", "").Replace(")", "").Replace("[FNC1]", "")) && !(this is PdfCode128Barcode))
      throw new PdfBarcodeException("Barcode Text contains characters that are not accepted by this barcode specification.");
    if ((double) this.NarrowBarWidth < 1.0)
      this.NarrowBarWidth = 1f;
    List<byte[]> textToEncodeList = this.GetTextToEncodeList();
    string s = string.Empty;
    bool flag = false;
    if (this.Text.Contains("FNC1"))
    {
      string[] strArray = this.Text.Split(new string[1]
      {
        "[FNC1]"
      }, StringSplitOptions.RemoveEmptyEntries);
      for (int index = 0; index < strArray.Length; ++index)
      {
        flag = true;
        s = !strArray[index].Contains("(") || !strArray[index].Contains(")") ? s + strArray[index].Insert(0, "(").Insert(3, ")") : s + strArray[index];
      }
    }
    float num1 = 0.0f;
    if (this.Text == null || this.Text.Length == 0)
      throw new PdfBarcodeException("Barcode Text cannot be null or empty.");
    System.Drawing.Font font = new System.Drawing.Font(this.Font.Name.ToString(), this.Font.Size);
    SizeF sizeF1 = this.Font.MeasureString(this.Text);
    float width1;
    float height;
    if (this.size != SizeF.Empty)
    {
      int num2 = 0;
      foreach (byte[] numArray in textToEncodeList)
      {
        foreach (byte num3 in numArray)
          num2 += (int) num3;
      }
      if ((double) this.NarrowBarWidth <= 1.0 || (double) this.NarrowBarWidth >= (double) (this.Size.Width / (float) num2) || (double) (this.Size.Width / (float) num2) >= (double) this.NarrowBarWidth)
        this.NarrowBarWidth = (this.Size.Width - (this.QuietZone.Left + this.QuietZone.Right)) / (float) num2;
      this.BarHeight = this.Size.Height - this.QuietZone.Top - this.QuietZone.Bottom - sizeF1.Height - this.BarcodeToTextGapHeight;
      width1 = this.Size.Width;
      height = this.Size.Height;
    }
    else
    {
      sizeF1.Height = (float) font.Height;
      width1 = this.QuietZone.Left + this.QuietZone.Right;
      int num4 = 0;
      foreach (byte[] numArray in textToEncodeList)
      {
        foreach (byte num5 in numArray)
        {
          width1 += (float) num5 * this.NarrowBarWidth;
          num4 += (int) num5;
        }
      }
      height = this.QuietZone.Top + this.QuietZone.Bottom + this.BarHeight + sizeF1.Height + this.BarcodeToTextGapHeight;
      if (this.TextDisplayLocation == TextLocation.None)
        height -= sizeF1.Height + this.BarcodeToTextGapHeight;
      if (!this.barHeightEnabled)
        this.Size = new SizeF(width1, height);
    }
    Color color = Color.FromArgb(this.BackColor.ToArgb());
    PdfBrush brush = (PdfBrush) new PdfSolidBrush((PdfColor) color);
    page.Graphics.DrawRectangle(brush, new RectangleF(location, new SizeF(width1, height)));
    RectangleF rectangle = new RectangleF();
    if (this.TextDisplayLocation == TextLocation.Top)
    {
      PointF location1 = location;
      if (this.QuietZone.IsAll && (double) this.QuietZone.All > 0.0)
        height -= this.QuietZone.All;
      rectangle = new RectangleF(location1, new SizeF(width1, height));
    }
    else if (this.TextDisplayLocation == TextLocation.Bottom)
    {
      if (this.QuietZone.IsAll && (double) this.QuietZone.All > 0.0)
        height -= this.QuietZone.All;
      rectangle = new RectangleF(location, new SizeF(width1, height));
    }
    else
    {
      if (this.QuietZone.IsAll && (double) this.QuietZone.All > 0.0)
        height -= 2f * this.QuietZone.All;
      else if ((double) this.QuietZone.Top > 0.0 || (double) this.QuietZone.Bottom > 0.0)
      {
        float num6 = this.QuietZone.Top + this.QuietZone.Bottom;
        height -= num6;
      }
      rectangle = new RectangleF(location, new SizeF(width1, height));
    }
    Color.FromArgb(this.BackColor.ToArgb());
    SolidBrush solidBrush1 = new SolidBrush(color);
    page.Graphics.DrawRectangle((PdfBrush) new PdfSolidBrush(this.BackColor), rectangle);
    this.Bounds = rectangle;
    if (this.Location != PointF.Empty)
      this.Location = this.Bounds.Location;
    if (this.size == SizeF.Empty)
      this.Size = this.Bounds.Size;
    float x1 = this.QuietZone.Left + rectangle.Left;
    num1 = 0.0f + rectangle.Top;
    float y = this.TextDisplayLocation != TextLocation.Top ? location.Y + this.QuietZone.Top : ((double) this.QuietZone.Bottom <= 0.0 || this.QuietZone.IsAll && (double) this.QuietZone.All > 0.0 ? location.Y + (rectangle.Height - this.BarHeight) : ((double) this.QuietZone.Top <= 0.0 ? location.Y + (sizeF1.Height + this.BarcodeToTextGapHeight) : location.Y + (this.QuietZone.Top + sizeF1.Height + this.BarcodeToTextGapHeight)));
    foreach (byte[] numArray in textToEncodeList)
    {
      int num7 = 0;
      RectangleF rectangleF = new RectangleF();
      foreach (float num8 in numArray)
      {
        float width2 = num8 * this.NarrowBarWidth;
        RectangleF barRect = this.textDisplayLocation != TextLocation.None ? new RectangleF(x1, y, width2, this.BarHeight) : new RectangleF(x1, y, width2, rectangle.Height);
        if (num7 % 2 == 0)
          x1 += this.PaintToImage(page.Graphics, barRect);
        else
          x1 += width2;
        ++num7;
      }
    }
    if (this.TextDisplayLocation != TextLocation.None)
    {
      this.Text = this.Text.Trim(this.startSymbol);
      this.Text = this.Text.Trim(this.stopSymbol);
      PdfStringFormat format = new PdfStringFormat();
      format.LineAlignment = PdfVerticalAlignment.Middle;
      format.Alignment = this.TextAlignment != PdfBarcodeTextAlignment.Left ? (this.TextAlignment != PdfBarcodeTextAlignment.Right ? PdfTextAlignment.Center : PdfTextAlignment.Right) : PdfTextAlignment.Left;
      SolidBrush solidBrush2 = new SolidBrush(Color.FromArgb(this.TextColor.ToArgb()));
      float x2;
      float width3;
      if (this.TextAlignment == PdfBarcodeTextAlignment.Left)
      {
        x2 = rectangle.Left + this.QuietZone.Left;
        width3 = rectangle.Width;
      }
      else if (this.TextAlignment == PdfBarcodeTextAlignment.Right)
      {
        x2 = rectangle.Left;
        width3 = rectangle.Width - this.QuietZone.Right;
      }
      else
      {
        x2 = rectangle.Left + this.QuietZone.Left;
        width3 = rectangle.Width - (this.QuietZone.Right + this.QuietZone.Left);
      }
      if (!this.isFontModified && this.size != SizeF.Empty)
      {
        SizeF sizeF2 = this.Font.MeasureString(this.Text);
        float size = this.Font.Size;
        int num9 = 0;
        while ((double) sizeF2.Width > (double) width1)
        {
          this.Font = (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, --size);
          sizeF2 = this.Font.MeasureString(this.Text);
          if ((double) sizeF2.Width <= (double) width1)
          {
            this.Font = (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, size);
            break;
          }
          ++num9;
        }
      }
      if (this.textDisplayLocation == TextLocation.Top)
      {
        RectangleF layoutRectangle = new RectangleF(new PointF(x2, location.Y + this.QuietZone.Top), new SizeF(width3, sizeF1.Height));
        layoutRectangle.Y = (double) layoutRectangle.Y < 0.0 ? 0.0f : layoutRectangle.Y;
        if (!flag)
          page.Graphics.DrawString(this.Text, this.Font, (PdfBrush) new PdfSolidBrush(this.TextColor), layoutRectangle, format);
        else
          page.Graphics.DrawString(s, this.Font, (PdfBrush) new PdfSolidBrush(this.TextColor), layoutRectangle, format);
      }
      else
      {
        RectangleF layoutRectangle = new RectangleF(new PointF(x2, location.Y + this.QuietZone.Top + this.BarcodeToTextGapHeight + this.BarHeight), new SizeF(width3, sizeF1.Height));
        if (!flag)
          page.Graphics.DrawString(this.Text, this.Font, (PdfBrush) new PdfSolidBrush(this.TextColor), layoutRectangle, format);
        else
          page.Graphics.DrawString(s, this.Font, (PdfBrush) new PdfSolidBrush(this.TextColor), layoutRectangle, format);
      }
    }
    this.QuietZone = quietZone;
    this.Text = text;
  }

  internal void DrawRevamped(PdfPageBase page, float x, float y, float width, float height)
  {
    string text = this.Text;
    this.isCheckDigitAdded = false;
    PdfBarcodeQuietZones quietZone = this.QuietZone;
    if (!this.Validate(this.Text.Replace("(", "").Replace(")", "")) && !(this is PdfCode128Barcode))
      throw new PdfBarcodeException("Barcode Text contains characters that are not accepted by this barcode specification.");
    if ((double) this.NarrowBarWidth < 1.0)
      this.NarrowBarWidth = 1f;
    List<byte[]> textToEncodeList = this.GetTextToEncodeList();
    float num1 = 0.0f;
    if (this.Text == null || this.Text.Length == 0)
      throw new PdfBarcodeException("Barcode Text cannot be null or empty.");
    System.Drawing.Font font = new System.Drawing.Font(this.Font.Name.ToString(), this.Font.Size);
    SizeF sizeF1 = this.Font.MeasureString(this.Text);
    int num2 = 0;
    foreach (byte[] numArray in textToEncodeList)
    {
      foreach (byte num3 in numArray)
        num2 += (int) num3;
    }
    if ((double) this.NarrowBarWidth <= 1.0 || (double) this.NarrowBarWidth >= (double) (width / (float) num2))
      this.NarrowBarWidth = (width - (this.QuietZone.Left + this.QuietZone.Right)) / (float) num2;
    this.BarHeight = height - this.QuietZone.Top - this.QuietZone.Bottom - sizeF1.Height - this.BarcodeToTextGapHeight;
    this.Location = new PointF(x, y);
    this.Size = new SizeF(width, height);
    Color color = Color.FromArgb(this.BackColor.ToArgb());
    PdfBrush brush = (PdfBrush) new PdfSolidBrush((PdfColor) color);
    page.Graphics.DrawRectangle(brush, x, y, width, height);
    RectangleF rectangle = new RectangleF();
    if (this.TextDisplayLocation == TextLocation.Top)
    {
      if (this.size == SizeF.Empty)
        height += this.BarcodeToTextGapHeight;
      if (this.QuietZone.IsAll && (double) this.QuietZone.All > 0.0)
        height -= this.QuietZone.All;
      rectangle = new RectangleF(x, y, width, height);
    }
    else if (this.TextDisplayLocation == TextLocation.Bottom)
    {
      if (this.size == SizeF.Empty)
        height += this.BarcodeToTextGapHeight;
      if (this.QuietZone.IsAll && (double) this.QuietZone.All > 0.0)
        height -= this.QuietZone.All;
      rectangle = new RectangleF(x, y, width, height);
    }
    else
    {
      if (this.QuietZone.IsAll && (double) this.QuietZone.All > 0.0)
        height -= 2f * this.QuietZone.All;
      else if ((double) this.QuietZone.Top > 0.0 || (double) this.QuietZone.Bottom > 0.0)
      {
        float num4 = this.QuietZone.Top + this.QuietZone.Bottom;
        height -= num4;
      }
      rectangle = new RectangleF(x, y, width, height);
    }
    Color.FromArgb(this.BackColor.ToArgb());
    SolidBrush solidBrush1 = new SolidBrush(color);
    page.Graphics.DrawRectangle((PdfBrush) new PdfSolidBrush(this.BackColor), rectangle);
    this.Bounds = rectangle;
    float x1 = this.QuietZone.Left + rectangle.Left;
    num1 = 0.0f + rectangle.Top;
    float y1 = this.TextDisplayLocation != TextLocation.Top ? y + this.QuietZone.Top : ((double) this.QuietZone.Bottom <= 0.0 || this.QuietZone.IsAll && (double) this.QuietZone.All > 0.0 ? y + (rectangle.Height - this.BarHeight) : ((double) this.QuietZone.Top <= 0.0 ? y + (sizeF1.Height + this.BarcodeToTextGapHeight) : y + (this.QuietZone.Top + sizeF1.Height + this.BarcodeToTextGapHeight)));
    foreach (byte[] numArray in textToEncodeList)
    {
      int num5 = 0;
      RectangleF rectangleF = new RectangleF();
      foreach (float num6 in numArray)
      {
        float width1 = num6 * this.NarrowBarWidth;
        RectangleF barRect = this.textDisplayLocation != TextLocation.None ? new RectangleF(x1, y1, width1, this.BarHeight) : new RectangleF(x1, y1, width1, rectangle.Height);
        if (num5 % 2 == 0)
          x1 += this.PaintToImage(page.Graphics, barRect);
        else
          x1 += width1;
        ++num5;
      }
    }
    if (this.TextDisplayLocation != TextLocation.None)
    {
      this.Text = this.Text.Trim(this.startSymbol);
      this.Text = this.Text.Trim(this.stopSymbol);
      PdfStringFormat format = new PdfStringFormat();
      format.LineAlignment = PdfVerticalAlignment.Middle;
      format.Alignment = this.TextAlignment != PdfBarcodeTextAlignment.Left ? (this.TextAlignment != PdfBarcodeTextAlignment.Right ? PdfTextAlignment.Center : PdfTextAlignment.Right) : PdfTextAlignment.Left;
      SolidBrush solidBrush2 = new SolidBrush(Color.FromArgb(this.TextColor.ToArgb()));
      float x2;
      float width2;
      if (this.TextAlignment == PdfBarcodeTextAlignment.Left)
      {
        x2 = rectangle.Left + this.QuietZone.Left;
        width2 = rectangle.Width;
      }
      else if (this.TextAlignment == PdfBarcodeTextAlignment.Right)
      {
        x2 = rectangle.Left;
        width2 = rectangle.Width - this.QuietZone.Right;
      }
      else
      {
        x2 = rectangle.Left + this.QuietZone.Left;
        width2 = rectangle.Width - (this.QuietZone.Right + this.QuietZone.Left);
      }
      if (!this.isFontModified)
      {
        SizeF sizeF2 = this.Font.MeasureString(this.Text);
        float size = this.Font.Size;
        int num7 = 0;
        while ((double) sizeF2.Width > (double) width)
        {
          this.Font = (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, --size);
          sizeF2 = this.Font.MeasureString(this.Text);
          if ((double) sizeF2.Width <= (double) width)
          {
            this.Font = (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, size);
            break;
          }
          ++num7;
        }
      }
      if (this.textDisplayLocation == TextLocation.Top)
      {
        RectangleF layoutRectangle = new RectangleF(new PointF(x2, y + this.QuietZone.Top), new SizeF(width2, sizeF1.Height));
        layoutRectangle.Y = (double) layoutRectangle.Y < 0.0 ? 0.0f : layoutRectangle.Y;
        page.Graphics.DrawString(this.Text, this.Font, (PdfBrush) new PdfSolidBrush(this.TextColor), layoutRectangle, format);
      }
      else
      {
        RectangleF layoutRectangle = new RectangleF(new PointF(x2, y + this.QuietZone.Top + this.BarcodeToTextGapHeight + this.BarHeight), new SizeF(width2, sizeF1.Height));
        page.Graphics.DrawString(this.Text, this.Font, (PdfBrush) new PdfSolidBrush(this.TextColor), layoutRectangle, format);
      }
    }
    this.QuietZone = quietZone;
    this.Text = text;
  }

  internal float PaintToImage(PdfGraphics g, RectangleF barRect)
  {
    g.DrawRectangle((PdfBrush) new PdfSolidBrush(this.BarColor), barRect);
    return barRect.Width;
  }

  public virtual Image ToImage()
  {
    PdfBarcodeQuietZones barcodeQuietZones = new PdfBarcodeQuietZones();
    this.barcodeSpaceCount = 0;
    this.isContainsSmallSize = false;
    if (this.size != SizeF.Empty && (double) this.GetSizeValue().Width > (double) this.size.Width)
      this.isContainsSmallSize = true;
    string text = this.Text;
    barcodeQuietZones.Left = this.PointToPixelConverter(this.QuietZone.Left);
    barcodeQuietZones.Right = this.PointToPixelConverter(this.QuietZone.Right);
    barcodeQuietZones.Top = this.PointToPixelConverter(this.QuietZone.Top);
    barcodeQuietZones.Bottom = this.PointToPixelConverter(this.QuietZone.Bottom);
    float num1 = this.PointToPixelConverter(this.NarrowBarWidth);
    bool startStopSymbols = this.EncodeStartStopSymbols;
    PdfColor backColor = this.BackColor;
    char startSymbol = this.StartSymbol;
    char stopSymbol = this.StopSymbol;
    PdfFont font1 = this.Font;
    float num2 = this.PointToPixelConverter(this.interCharacterGap);
    bool continuous = this.continuous;
    string extendedText = this.ExtendedText;
    float height1 = this.PointToPixelConverter(this.BarHeight);
    float pixelConverter1 = this.PointToPixelConverter(this.BarcodeToTextGapHeight);
    TextLocation textDisplayLocation = this.TextDisplayLocation;
    PdfBarcodeTextAlignment textAlignment = this.TextAlignment;
    bool isFontModified = this.isFontModified;
    Dictionary<char, BarcodeSymbolTable> barcodeSymbols = this.BarcodeSymbols;
    PdfColor textColor = this.TextColor;
    switch (this)
    {
      case PdfGS1Code128Barcode _:
      case PdfCode128Barcode _:
        return this.ToImageRevamped();
      default:
        PointF empty = PointF.Empty;
        if (this is PdfCode128CBarcode)
          this.size = this.Size;
        if (!this.Validate(text))
          throw new PdfBarcodeException("Barcode Text contains characters that are not accepted by this barcode specification.");
        if ((double) num1 < 1.0)
        {
          num1 = 1f;
          this.NarrowBarWidth = 1f;
        }
        string str = this.GetTextToEncode();
        if (this is PdfCode128CBarcode)
          this.GetTextToEncodeList();
        float num3 = 0.0f;
        if (str == null || str.Length == 0)
          throw new PdfBarcodeException("Barcode Text cannot be null or empty.");
        if (startStopSymbols && startSymbol != char.MinValue && stopSymbol != char.MinValue)
          str = startSymbol.ToString() + str + stopSymbol.ToString();
        System.Drawing.Font font2 = new System.Drawing.Font(font1.Name.ToString(), font1.Size);
        SizeF sizeF1 = font1.MeasureString(text);
        SizeF size1;
        float width1;
        float height2;
        if (this.size != SizeF.Empty)
        {
          size1 = this.Size;
          width1 = this.PointToPixelConverter(size1.Width);
          height2 = this.PointToPixelConverter(this.Size.Height);
          sizeF1.Height = this.PointToPixelConverter(sizeF1.Height);
          sizeF1.Width = this.PointToPixelConverter(sizeF1.Width);
        }
        else
        {
          sizeF1.Height = (float) font2.Height;
          float num4 = barcodeQuietZones.Left + barcodeQuietZones.Right;
          foreach (char character in str)
            num4 += this.GetCharWidth(character) + num2;
          width1 = this.continuous ? num4 - (extendedText.Length > 0 ? num2 * (float) (this.ExtendedText.Length - this.Text.Length) : num2) : num4 - num2 * (float) str.Length;
          height2 = barcodeQuietZones.Top + barcodeQuietZones.Bottom + height1 + sizeF1.Height + pixelConverter1;
          if (textDisplayLocation == TextLocation.None)
            height2 -= sizeF1.Height + pixelConverter1;
        }
        PdfSolidBrush pdfSolidBrush = new PdfSolidBrush((PdfColor) Color.FromArgb(backColor.ToArgb()));
        Bitmap image = new Bitmap((int) width1, (int) height2, PixelFormat.Format32bppRgb);
        if (this.isContainsSmallSize)
          image.SetResolution(300f, 300f);
        float pixelConverter2 = this.PointToPixelConverter(1.0734f);
        System.Drawing.Graphics g = System.Drawing.Graphics.FromImage((Image) image);
        g.FillRectangle(Brushes.White, new Rectangle(0, 0, image.Width, image.Height));
        RectangleF rect1 = new RectangleF();
        if (textDisplayLocation == TextLocation.Top || textDisplayLocation == TextLocation.Bottom)
        {
          if (this.size == SizeF.Empty)
            height2 += pixelConverter1;
          rect1 = new RectangleF(empty, new SizeF(width1, height2));
        }
        else
        {
          if (barcodeQuietZones.IsAll && (double) barcodeQuietZones.All > 0.0)
            height2 -= 2f * barcodeQuietZones.All;
          else if ((double) barcodeQuietZones.Top > 0.0 || (double) barcodeQuietZones.Bottom > 0.0)
          {
            float num5 = barcodeQuietZones.Top + barcodeQuietZones.Bottom;
            height2 -= num5;
          }
          rect1 = new RectangleF(empty, new SizeF(width1, height2));
        }
        Brush brush1 = (Brush) new SolidBrush(Color.FromArgb(backColor.ToArgb()));
        if (this.size != SizeF.Empty)
        {
          if (this.isContainsSmallSize)
          {
            System.Drawing.Graphics graphics = g;
            Brush brush2 = brush1;
            PointF location = empty;
            size1 = this.Size;
            double pixelConverter3 = (double) this.PointToPixelConverter(size1.Width);
            size1 = this.Size;
            double pixelConverter4 = (double) this.PointToPixelConverter(size1.Height);
            SizeF size2 = new SizeF((float) pixelConverter3, (float) pixelConverter4);
            RectangleF rect2 = new RectangleF(location, size2);
            graphics.FillRectangle(brush2, rect2);
          }
          else
            g.FillRectangle(brush1, new RectangleF(empty, new SizeF(this.size.Width, this.size.Height)));
        }
        else
          g.FillRectangle(brush1, rect1);
        this.Bounds = rect1;
        float x1;
        if (this.size != SizeF.Empty)
        {
          this.GetSizeValue();
          float num6 = 0.0f;
          foreach (char character in str)
            num6 += this.GetCharBarsCount(character);
          float num7 = width1 / num6;
          if ((double) num1 <= 1.0 || (double) num1 >= (double) num7 || (double) num7 >= (double) num1)
          {
            num1 = (float) (((double) width1 - ((double) barcodeQuietZones.Left + (double) barcodeQuietZones.Right)) / ((double) num6 + (double) this.barcodeSpaceCount));
            num2 = !(this is PdfCodeUpcBarcode) ? num1 : num1 * pixelConverter2;
          }
          height1 = height2 - (barcodeQuietZones.Top + barcodeQuietZones.Bottom) - sizeF1.Height - pixelConverter1;
          x1 = barcodeQuietZones.Left + rect1.Left;
        }
        else
          x1 = barcodeQuietZones.Left + rect1.Left;
        num3 = rect1.Top;
        float y = textDisplayLocation != TextLocation.Top ? empty.Y + barcodeQuietZones.Top : (!barcodeQuietZones.IsAll || (double) barcodeQuietZones.All <= 0.0 ? ((double) barcodeQuietZones.Bottom > 0.0 || (double) barcodeQuietZones.Top > 0.0 && (double) barcodeQuietZones.Bottom > 0.0 ? ((double) barcodeQuietZones.Top <= 0.0 || (double) barcodeQuietZones.Bottom <= 0.0 ? empty.Y + (sizeF1.Height + pixelConverter1) : empty.Y + (barcodeQuietZones.Top + sizeF1.Height + pixelConverter1)) : empty.Y + (rect1.Height - height1)) : empty.Y + (rect1.Height - barcodeQuietZones.Top - height1));
        for (int index1 = 0; index1 < str.Length; ++index1)
        {
          char ch = str[index1];
          foreach (KeyValuePair<char, BarcodeSymbolTable> keyValuePair in barcodeSymbols)
          {
            BarcodeSymbolTable barcodeSymbolTable = keyValuePair.Value;
            if ((int) barcodeSymbolTable.Symbol == (int) ch)
            {
              byte[] bars = barcodeSymbolTable.Bars;
              RectangleF barRect = new RectangleF();
              for (int index2 = 0; index2 < bars.Length; ++index2)
              {
                float width2 = !(this is PdfCodeUpcBarcode) ? (float) bars[index2] * num1 : (float) bars[index2] * num1 * pixelConverter2;
                if (bars[index2] == (byte) 0)
                  width2 = num1;
                barRect = textDisplayLocation != TextLocation.None ? new RectangleF(x1, y, width2, height1) : new RectangleF(x1, y, width2, rect1.Height);
                if (this is PdfCodeUpcBarcode && index1 > 1 && index1 <= 7)
                {
                  if (index2 % 2 != 0)
                    x1 += this.PaintToImage(ref g, barRect);
                  else
                    x1 += width2;
                }
                else if (index2 % 2 == 0)
                {
                  x1 += this.PaintToImage(ref g, barRect);
                }
                else
                {
                  x1 += width2;
                  if (this is PdfCodeUpcBarcode && bars.Length % 2 == 0 && index1 == 1 && index2 == bars.Length - 1)
                    x1 -= width2;
                }
              }
              if (bars.Length % 2 != 0 || this is PdfCodeUpcBarcode && bars.Length % 2 == 0 && index1 == 7)
              {
                if (this.size != SizeF.Empty)
                {
                  x1 += num2;
                  if (bars.Length % 2 != 0)
                    --this.barcodeSpaceCount;
                }
                else
                  x1 += num2;
              }
            }
          }
        }
        if (textDisplayLocation != TextLocation.None)
        {
          this.Text = this.Text.Trim(this.startSymbol);
          this.Text = this.Text.Trim(this.stopSymbol);
          StringFormat format = new StringFormat();
          format.LineAlignment = StringAlignment.Center;
          switch (textAlignment)
          {
            case PdfBarcodeTextAlignment.Left:
              format.Alignment = StringAlignment.Near;
              break;
            case PdfBarcodeTextAlignment.Right:
              format.Alignment = StringAlignment.Far;
              break;
            default:
              format.Alignment = StringAlignment.Center;
              break;
          }
          SolidBrush solidBrush = new SolidBrush(Color.FromArgb(textColor.ToArgb()));
          float x2;
          float width3;
          switch (textAlignment)
          {
            case PdfBarcodeTextAlignment.Left:
              x2 = rect1.Left + barcodeQuietZones.Left;
              width3 = rect1.Width;
              break;
            case PdfBarcodeTextAlignment.Right:
              x2 = rect1.Left;
              width3 = rect1.Width - barcodeQuietZones.Right;
              break;
            default:
              x2 = rect1.Left + barcodeQuietZones.Left;
              width3 = rect1.Width - (barcodeQuietZones.Right + barcodeQuietZones.Left);
              break;
          }
          if (!isFontModified && this.size != SizeF.Empty)
          {
            SizeF sizeF2 = g.MeasureString(text, font2) with
            {
              Height = this.PointToPixelConverter(sizeF1.Height),
              Width = this.PointToPixelConverter(sizeF1.Height)
            };
            float size3 = font2.Size;
            int num8 = 0;
            while ((double) sizeF2.Width > (double) width1)
            {
              System.Drawing.Font font3 = new System.Drawing.Font("Helvetica", --size3);
              sizeF2 = g.MeasureString(text, font3);
              if ((double) sizeF2.Width <= (double) width1)
              {
                font2 = font3;
                break;
              }
              ++num8;
            }
          }
          if (textDisplayLocation == TextLocation.Top)
          {
            RectangleF layoutRectangle = new RectangleF(new PointF(x2, empty.Y + barcodeQuietZones.Top), new SizeF(width3, sizeF1.Height));
            layoutRectangle.Y = (double) layoutRectangle.Y < 0.0 ? 0.0f : layoutRectangle.Y;
            g.DrawString(text, font2, (Brush) solidBrush, layoutRectangle, format);
          }
          else
          {
            RectangleF layoutRectangle = new RectangleF(new PointF(x2, empty.Y + barcodeQuietZones.Top + pixelConverter1 + height1), new SizeF(width3, sizeF1.Height));
            g.DrawString(text, font2, (Brush) solidBrush, layoutRectangle, format);
          }
        }
        return (Image) image;
    }
  }

  public Image ToImage(SizeF size)
  {
    this.Size = size;
    return this.ToImage();
  }

  private float PointToPixelConverter(float value)
  {
    float pixelConverter = value;
    if (this.isContainsSmallSize)
      pixelConverter = new PdfUnitConvertor(300f).ConvertToPixels(value, PdfGraphicsUnit.Point);
    return pixelConverter;
  }

  internal Image ToImageRevamped()
  {
    PointF empty = PointF.Empty;
    string text = this.Text;
    this.isCheckDigitAdded = false;
    PdfBarcodeQuietZones quietZone = this.QuietZone;
    if (!this.Validate(this.Text.Replace("(", "").Replace(")", "").Replace(" ", "")) && !(this is PdfCode128Barcode))
      throw new PdfBarcodeException("Barcode Text contains characters that are not accepted by this barcode specification.");
    if ((double) this.NarrowBarWidth < 1.0)
      this.NarrowBarWidth = 1f;
    List<byte[]> textToEncodeList = this.GetTextToEncodeList();
    float num1 = 0.0f;
    if (this.Text == null || this.Text.Length == 0)
      throw new PdfBarcodeException("Barcode Text cannot be null or empty.");
    System.Drawing.Font font1 = new System.Drawing.Font(this.Font.Name.ToString(), this.Font.Size);
    SizeF sizeF1 = this.Font.MeasureString(this.Text);
    float width1;
    float height;
    if (this.size != SizeF.Empty)
    {
      int num2 = 0;
      foreach (byte[] numArray in textToEncodeList)
      {
        foreach (byte num3 in numArray)
          num2 += (int) num3;
      }
      this.NarrowBarWidth = (this.Size.Width - (this.QuietZone.Left + this.QuietZone.Right)) / (float) num2;
      this.BarHeight = this.Size.Height - this.QuietZone.Top - this.QuietZone.Bottom - sizeF1.Height - this.BarcodeToTextGapHeight;
      width1 = this.Size.Width;
      height = this.Size.Height;
    }
    else
    {
      sizeF1.Height = (float) font1.Height;
      width1 = this.QuietZone.Left + this.QuietZone.Right;
      foreach (byte[] numArray in textToEncodeList)
      {
        foreach (byte num4 in numArray)
          width1 += (float) num4 * this.NarrowBarWidth;
      }
      height = this.QuietZone.Top + this.QuietZone.Bottom + this.BarHeight + sizeF1.Height + this.BarcodeToTextGapHeight;
      if (this.TextDisplayLocation == TextLocation.None)
        height -= sizeF1.Height + this.BarcodeToTextGapHeight;
      if (!this.barHeightEnabled)
        this.Size = new SizeF(width1, height);
    }
    Color color = Color.FromArgb(this.BackColor.ToArgb());
    PdfSolidBrush pdfSolidBrush = new PdfSolidBrush((PdfColor) color);
    Bitmap imageRevamped = new Bitmap((int) width1, (int) height, PixelFormat.Format32bppRgb);
    System.Drawing.Graphics g = System.Drawing.Graphics.FromImage((Image) imageRevamped);
    g.FillRectangle(Brushes.White, new Rectangle(0, 0, imageRevamped.Width, imageRevamped.Height));
    RectangleF rect = new RectangleF();
    if (this.TextDisplayLocation == TextLocation.Top)
    {
      PointF location = empty;
      if (this.QuietZone.IsAll && (double) this.QuietZone.All > 0.0)
        height -= this.QuietZone.All;
      rect = new RectangleF(location, new SizeF(width1, height));
    }
    else if (this.TextDisplayLocation == TextLocation.Bottom)
    {
      if (this.QuietZone.IsAll && (double) this.QuietZone.All > 0.0)
        height -= this.QuietZone.All;
      rect = new RectangleF(empty, new SizeF(width1, height));
    }
    else
    {
      if (this.QuietZone.IsAll && (double) this.QuietZone.All > 0.0)
        height -= 2f * this.QuietZone.All;
      else if ((double) this.QuietZone.Top > 0.0 || (double) this.QuietZone.Bottom > 0.0)
      {
        float num5 = this.QuietZone.Top + this.QuietZone.Bottom;
        height -= num5;
      }
      rect = new RectangleF(empty, new SizeF(width1, height));
    }
    Color.FromArgb(this.BackColor.ToArgb());
    Brush brush = (Brush) new SolidBrush(color);
    if (this.size != SizeF.Empty)
      g.FillRectangle(brush, new RectangleF(empty, this.size));
    else
      g.FillRectangle(brush, rect);
    this.Bounds = rect;
    float x1 = this.QuietZone.Left + rect.Left;
    num1 = 0.0f + rect.Top;
    float y = this.TextDisplayLocation != TextLocation.Top ? empty.Y + this.QuietZone.Top : ((double) this.QuietZone.Bottom <= 0.0 || this.QuietZone.IsAll && (double) this.QuietZone.All > 0.0 ? empty.Y + (rect.Height - this.BarHeight) : ((double) this.QuietZone.Top <= 0.0 ? empty.Y + (sizeF1.Height + this.BarcodeToTextGapHeight) : empty.Y + (this.QuietZone.Top + sizeF1.Height + this.BarcodeToTextGapHeight)));
    RectangleF rectangleF = new RectangleF();
    foreach (byte[] numArray in textToEncodeList)
    {
      int num6 = 0;
      foreach (float num7 in numArray)
      {
        float width2 = num7 * this.NarrowBarWidth;
        RectangleF barRect = this.TextDisplayLocation != TextLocation.None ? new RectangleF(x1, y, width2, this.barHeight) : new RectangleF(x1, y, width2, rect.Height);
        if (num6 % 2 == 0)
          x1 += this.PaintToImage(ref g, barRect);
        else
          x1 += width2;
        ++num6;
      }
    }
    if (this.TextDisplayLocation != TextLocation.None)
    {
      this.Text = this.Text.Trim(this.startSymbol);
      this.Text = this.Text.Trim(this.stopSymbol);
      StringFormat format = new StringFormat();
      format.LineAlignment = StringAlignment.Center;
      format.Alignment = this.TextAlignment != PdfBarcodeTextAlignment.Left ? (this.TextAlignment != PdfBarcodeTextAlignment.Right ? StringAlignment.Center : StringAlignment.Far) : StringAlignment.Near;
      SolidBrush solidBrush = new SolidBrush(Color.FromArgb(this.TextColor.ToArgb()));
      float x2;
      float width3;
      if (this.TextAlignment == PdfBarcodeTextAlignment.Left)
      {
        x2 = rect.Left + this.QuietZone.Left;
        width3 = rect.Width;
      }
      else if (this.TextAlignment == PdfBarcodeTextAlignment.Right)
      {
        x2 = rect.Left;
        width3 = rect.Width - this.QuietZone.Right;
      }
      else
      {
        x2 = rect.Left + this.QuietZone.Left;
        width3 = rect.Width - (this.QuietZone.Right + this.QuietZone.Left);
      }
      if (!this.isFontModified && this.size != SizeF.Empty)
      {
        SizeF sizeF2 = g.MeasureString(this.Text, font1);
        float size = font1.Size;
        int num8 = 0;
        while ((double) sizeF2.Width > (double) width1)
        {
          System.Drawing.Font font2 = new System.Drawing.Font("Helvetica", --size);
          sizeF2 = g.MeasureString(this.Text, font2);
          if ((double) sizeF2.Width <= (double) width1)
          {
            font1 = font2;
            break;
          }
          ++num8;
        }
      }
      if (this.textDisplayLocation == TextLocation.Top)
      {
        RectangleF layoutRectangle = new RectangleF(new PointF(x2, empty.Y + this.QuietZone.Top), new SizeF(width3, sizeF1.Height));
        layoutRectangle.Y = (double) layoutRectangle.Y < 0.0 ? 0.0f : layoutRectangle.Y;
        g.DrawString(this.Text, font1, (Brush) solidBrush, layoutRectangle, format);
      }
      else
      {
        SizeF sizeF3 = g.MeasureString(this.Text, font1);
        float size = font1.Size;
        int num9 = 0;
        while ((double) sizeF3.Width > (double) width1)
        {
          System.Drawing.Font font3 = new System.Drawing.Font("Helvetica", --size);
          sizeF3 = g.MeasureString(this.Text, font3);
          if ((double) sizeF3.Width <= (double) width1)
            font1 = font3;
          width3 = sizeF3.Width;
          ++num9;
        }
        RectangleF layoutRectangle = new RectangleF(new PointF(x2, empty.Y + this.QuietZone.Top + this.BarcodeToTextGapHeight + this.BarHeight), new SizeF(width3, sizeF1.Height));
        g.DrawString(this.Text, font1, (Brush) solidBrush, layoutRectangle, format);
      }
    }
    this.QuietZone = quietZone;
    this.Text = text;
    return (Image) imageRevamped;
  }

  internal float GetCharBarsCount(char character)
  {
    float charBarsCount = 0.0f;
    foreach (KeyValuePair<char, BarcodeSymbolTable> barcodeSymbol in this.BarcodeSymbols)
    {
      BarcodeSymbolTable barcodeSymbolTable = barcodeSymbol.Value;
      if ((int) barcodeSymbolTable.Symbol == (int) character)
      {
        byte[] bars = barcodeSymbolTable.Bars;
        for (int index = 0; index < bars.Length; ++index)
        {
          if (!this.check && bars.Length % 2 != 0)
            this.continuous = true;
          charBarsCount += (float) bars[index];
          this.check = true;
        }
        if (this.size != SizeF.Empty && bars.Length % 2 != 0)
          ++this.barcodeSpaceCount;
        return charBarsCount;
      }
    }
    return 0.0f;
  }

  protected internal override bool Validate(string data)
  {
    this.codeValidator = new Regex(this.validatorExpression, RegexOptions.Compiled);
    return this.codeValidator.Match(data).Success;
  }

  protected internal override SizeF GetSizeValue()
  {
    switch (this)
    {
      case PdfGS1Code128Barcode _:
      case PdfCode128Barcode _:
        List<byte[]> textToEncodeList = this.GetTextToEncodeList();
        System.Drawing.Font font = new System.Drawing.Font(this.Font.Name.ToString(), this.Font.Size);
        SizeF sizeF = this.Font.MeasureString(this.Text) with
        {
          Height = (float) font.Height
        };
        float width = this.QuietZone.Left + this.QuietZone.Right;
        float num1 = (double) this.NarrowBarWidth < 1.0 ? 1f : this.NarrowBarWidth;
        foreach (byte[] numArray in textToEncodeList)
        {
          foreach (byte num2 in numArray)
            width += (float) num2 * num1;
        }
        float height1 = this.QuietZone.Top + this.QuietZone.Bottom + this.BarHeight + sizeF.Height + this.BarcodeToTextGapHeight;
        if (this.TextDisplayLocation == TextLocation.None)
          height1 -= sizeF.Height + this.BarcodeToTextGapHeight;
        return new SizeF(width, height1);
      default:
        float height2 = this.GetHeight();
        return new SizeF(this.BarcodeWidth(), height2);
    }
  }

  internal float BarcodeWidth()
  {
    string text = this.Text;
    if (this.EnableCheckDigit && this.isCheckDigitAdded)
      this.isCheckDigitAdded = false;
    string empty = string.Empty;
    string str = !(this.barcodeEncodeText != string.Empty) ? this.GetTextToEncode() : this.barcodeEncodeText;
    this.isCheckDigitAdded = false;
    this.ExtendedText = "";
    this.Text = text;
    if (str == null || str.Length == 0)
      throw new PdfBarcodeException("Barcode Text cannot be null or empty.");
    if (this.encodeStartStopSymbols && this.startSymbol != char.MinValue && this.stopSymbol != char.MinValue)
      str = this.startSymbol.ToString() + str + this.stopSymbol.ToString();
    float num1 = 0.0f;
    switch (this)
    {
      case PdfCode128Barcode _:
      case PdfGS1Code128Barcode _:
        List<byte[]> textToEncodeList = this.GetTextToEncodeList();
        if (textToEncodeList != null)
        {
          foreach (byte[] numArray in textToEncodeList)
          {
            foreach (byte num2 in numArray)
              num1 += (float) num2 * this.NarrowBarWidth;
          }
        }
        return num1;
      default:
        float num3 = this.QuietZone.Left + this.QuietZone.Right;
        foreach (char character in str)
          num3 += this.GetCharWidth(character) + this.interCharacterGap;
        foreach (char ch in str)
        {
          foreach (KeyValuePair<char, BarcodeSymbolTable> barcodeSymbol in this.BarcodeSymbols)
          {
            BarcodeSymbolTable barcodeSymbolTable = barcodeSymbol.Value;
            if ((int) barcodeSymbolTable.Symbol == (int) ch)
            {
              byte[] bars = barcodeSymbolTable.Bars;
              for (int index = 0; index < bars.Length; ++index)
              {
                float num4 = (float) bars[index] * this.NarrowBarWidth;
                num1 += num4;
              }
              if (bars.Length % 2 != 0)
                num1 += this.IntercharacterGap;
            }
          }
        }
        return num1 + (this.QuietZone.Left + this.QuietZone.Right);
    }
  }

  protected internal virtual void GetExtendedTextValue()
  {
  }

  protected internal virtual char[] CalculateCheckDigit() => (char[]) null;

  internal float GetCharWidth(char character)
  {
    float charWidth = 0.0f;
    foreach (KeyValuePair<char, BarcodeSymbolTable> barcodeSymbol in this.BarcodeSymbols)
    {
      BarcodeSymbolTable barcodeSymbolTable = barcodeSymbol.Value;
      if ((int) barcodeSymbolTable.Symbol == (int) character)
      {
        byte[] bars = barcodeSymbolTable.Bars;
        for (int index = 0; index < bars.Length; ++index)
        {
          if (!this.check && bars.Length % 2 != 0)
            this.continuous = true;
          charWidth += (float) bars[index] * this.NarrowBarWidth;
          this.check = true;
        }
        return charWidth;
      }
    }
    return 0.0f;
  }

  internal virtual string GetTextToEncode()
  {
    this.Text = this.Text.Replace("[FNC1]", "");
    this.Text = this.Text.Replace("[FNC2]", "ñ");
    this.Text = this.Text.Replace("[FNC3]", "ò");
    if (!this.Validate(this.Text))
      throw new PdfBarcodeException("Barcode text contains characters that are not accepted by this barcode specification.");
    if (!this.EnableCheckDigit)
      this.GetExtendedTextValue();
    char[] checkDigit = this.CalculateCheckDigit();
    string str;
    if (!this.ExtendedText.Equals(string.Empty))
      str = this.ExtendedText.Trim('*');
    else
      str = this.Text.Trim('*');
    string textToEncode = str;
    if (this.isCheckDigitAdded || !this.EnableCheckDigit || checkDigit == null || checkDigit.Length == 0)
      return textToEncode;
    if (!(this is PdfCodeUpcBarcode))
    {
      if (this.EnableCheckDigit && checkDigit[checkDigit.Length - 1] != char.MinValue && !this.isCheckDigitAdded)
      {
        foreach (char ch in checkDigit)
          textToEncode += ch.ToString();
      }
    }
    else if (this is PdfCodeUpcBarcode)
      textToEncode = new string(checkDigit);
    if (this.ShowCheckDigit && checkDigit[checkDigit.Length - 1] != char.MinValue && !this.isCheckDigitAdded)
    {
      if ((int) textToEncode[textToEncode.Length - 1] != (int) checkDigit[checkDigit.Length - 1])
      {
        foreach (char ch in checkDigit)
          textToEncode += ch.ToString();
      }
      this.isCheckDigitAdded = true;
      if (this.ExtendedText.Equals(string.Empty))
      {
        textToEncode = this.Text;
        foreach (char ch in checkDigit)
          textToEncode += ch.ToString();
      }
      else
      {
        textToEncode = this.ExtendedText;
        foreach (char ch in checkDigit)
          textToEncode += ch.ToString();
      }
    }
    if (this.ShowCheckDigit)
    {
      foreach (char ch in checkDigit)
        this.Text += ch.ToString();
    }
    this.isCheckDigitAdded = true;
    return textToEncode;
  }

  protected virtual List<byte[]> GetTextToEncodeList()
  {
    return new List<byte[]>() { new byte[0] };
  }

  protected virtual float PaintRectangle(PdfPageBase page, RectangleF barRect)
  {
    PdfBrush brush = (PdfBrush) new PdfSolidBrush((PdfColor) Color.FromArgb((int) this.BarColor.A, (int) this.BarColor.R, (int) this.BarColor.G, (int) this.BarColor.B));
    page.Graphics.DrawRectangle(brush, barRect);
    return barRect.Width;
  }

  protected virtual float PaintRectangleTag(PdfTemplate template, RectangleF barRect)
  {
    PdfBrush brush = (PdfBrush) new PdfSolidBrush((PdfColor) Color.FromArgb((int) this.BarColor.A, (int) this.BarColor.R, (int) this.BarColor.G, (int) this.BarColor.B));
    template.Graphics.DrawRectangle(brush, barRect);
    return barRect.Width;
  }

  private float GetHeight()
  {
    float height = this.QuietZone.Top + this.QuietZone.Bottom + this.BarHeight;
    SizeF sizeF = this.Font.MeasureString(this.Text);
    if (this.textDisplayLocation == TextLocation.Bottom || this.textDisplayLocation == TextLocation.Top)
      height += sizeF.Height + this.BarcodeToTextGapHeight;
    return height;
  }

  internal float PaintToImage(ref System.Drawing.Graphics g, RectangleF barRect)
  {
    SolidBrush solidBrush = new SolidBrush(Color.FromArgb(this.BarColor.ToArgb()));
    g.FillRectangle((Brush) solidBrush, barRect);
    return barRect.Width;
  }
}
