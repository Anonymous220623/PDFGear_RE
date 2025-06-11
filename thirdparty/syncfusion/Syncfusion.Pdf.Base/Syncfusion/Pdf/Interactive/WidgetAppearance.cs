// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.WidgetAppearance
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Primitives;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

internal class WidgetAppearance : IPdfWrapper
{
  private PdfColor m_borderColor = new PdfColor((byte) 0, (byte) 0, (byte) 0);
  private PdfColor m_backColor = new PdfColor(byte.MaxValue, byte.MaxValue, byte.MaxValue);
  private string m_normalCaption = string.Empty;
  private PdfDictionary m_dictionary = new PdfDictionary();
  private int m_rotationAngle;

  internal int RotationAngle
  {
    get => this.m_rotationAngle;
    set
    {
      this.m_rotationAngle = value;
      this.m_dictionary.SetProperty("R", (IPdfPrimitive) new PdfNumber(this.m_rotationAngle));
    }
  }

  public PdfColor BorderColor
  {
    get => this.m_borderColor;
    set
    {
      if (!(this.m_borderColor != value))
        return;
      this.m_borderColor = value;
      if (value.A == (byte) 0)
        this.m_dictionary.SetProperty("BC", (IPdfPrimitive) new PdfArray(new float[0]));
      else
        this.m_dictionary.SetProperty("BC", (IPdfPrimitive) this.m_borderColor.ToArray());
    }
  }

  public PdfColor BackColor
  {
    get => this.m_backColor;
    set
    {
      if (!(this.m_backColor != value))
        return;
      this.m_backColor = value;
      if (this.m_backColor.A == (byte) 0)
      {
        this.m_dictionary.SetProperty("BC", (IPdfPrimitive) new PdfArray(new float[3]));
        this.m_dictionary.Remove("BG");
      }
      else
        this.m_dictionary.SetProperty("BG", (IPdfPrimitive) this.m_backColor.ToArray());
    }
  }

  public string NormalCaption
  {
    get => this.m_normalCaption;
    set
    {
      if (!(this.m_normalCaption != value))
        return;
      this.m_normalCaption = value;
      this.m_dictionary.SetString("CA", this.m_normalCaption);
    }
  }

  public WidgetAppearance()
  {
    this.m_dictionary.SetProperty("BC", (IPdfPrimitive) this.m_borderColor.ToArray());
    this.m_dictionary.SetProperty("BG", (IPdfPrimitive) this.m_backColor.ToArray());
  }

  public IPdfPrimitive Element => (IPdfPrimitive) this.m_dictionary;
}
