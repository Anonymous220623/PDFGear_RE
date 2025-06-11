// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Barcode.PdfBarcodeQuietZones
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Barcode;

public sealed class PdfBarcodeQuietZones
{
  private const float DEF_MARGIN = 0.0f;
  private float right;
  private float top;
  private float left;
  private float bottom;

  public float Right
  {
    get => this.right;
    set => this.right = value;
  }

  public float Top
  {
    get => this.top;
    set => this.top = value;
  }

  public float Left
  {
    get => this.left;
    set => this.left = value;
  }

  public float Bottom
  {
    get => this.bottom;
    set => this.bottom = value;
  }

  public float All
  {
    get => this.right;
    set => this.right = this.top = this.left = this.bottom = value;
  }

  public bool IsAll
  {
    get
    {
      return (double) this.left == (double) this.top && (double) this.left == (double) this.right && (double) this.left == (double) this.bottom;
    }
  }
}
