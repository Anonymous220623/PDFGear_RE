// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Barcode.PdfBarcode
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Barcode;

public class PdfBarcode
{
  protected internal RectangleF bounds;
  private PdfColor backColor;
  private PdfColor barColor;
  private PdfColor textColor;
  private float narrowBarWidth;
  private float wideBarWidth;
  private PointF location;
  protected internal SizeF size;
  private string text = string.Empty;
  private PdfBarcodeQuietZones quietZones;
  private float width;
  private float height;
  protected float barHeight;
  private string extendedText = string.Empty;
  protected bool barHeightEnabled;

  public PdfBarcode() => this.Initialize();

  public PdfBarcode(string text)
    : this()
  {
    this.Text = text;
  }

  public PdfColor BackColor
  {
    get => this.backColor;
    set => this.backColor = value;
  }

  public PdfColor BarColor
  {
    get => this.barColor;
    set => this.barColor = value;
  }

  public PdfColor TextColor
  {
    get => this.textColor;
    set => this.textColor = value;
  }

  public float NarrowBarWidth
  {
    get => this.narrowBarWidth;
    set => this.narrowBarWidth = value;
  }

  public string Text
  {
    get => this.text;
    set => this.text = value;
  }

  public PointF Location
  {
    get => this.location;
    set => this.location = value;
  }

  public PdfBarcodeQuietZones QuietZone
  {
    get => this.quietZones;
    set => this.quietZones = value;
  }

  public float BarHeight
  {
    get => this.barHeight;
    set
    {
      this.barHeight = value;
      this.barHeightEnabled = true;
    }
  }

  public SizeF Size
  {
    get => this.size != SizeF.Empty ? this.size : this.GetSizeValue();
    set => this.size = value;
  }

  public RectangleF Bounds
  {
    get => this.bounds;
    set => this.bounds = value;
  }

  internal string ExtendedText
  {
    get => this.extendedText;
    set => this.extendedText = value;
  }

  protected internal virtual bool Validate(string data) => true;

  protected internal virtual SizeF GetSizeValue() => new SizeF(0.0f, 0.0f);

  private void Initialize()
  {
    this.barColor = new PdfColor(Color.Black);
    this.backColor = new PdfColor(Color.White);
    this.textColor = new PdfColor(Color.Black);
    this.narrowBarWidth = 0.864f;
    this.wideBarWidth = 2.592f;
    this.quietZones = new PdfBarcodeQuietZones()
    {
      All = 0.0f
    };
    this.barHeight = 80f;
  }
}
