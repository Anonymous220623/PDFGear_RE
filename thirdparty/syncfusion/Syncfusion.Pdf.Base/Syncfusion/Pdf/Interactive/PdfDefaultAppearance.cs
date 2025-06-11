// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfDefaultAppearance
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

internal class PdfDefaultAppearance
{
  private PdfColor m_foreColor = new PdfColor((byte) 0, (byte) 0, (byte) 0);
  private string m_fontName = string.Empty;
  private float m_fontSize;

  public string FontName
  {
    get => this.m_fontName;
    set
    {
      if (!(this.m_fontName != value))
        return;
      this.m_fontName = value;
    }
  }

  public float FontSize
  {
    get => this.m_fontSize;
    set
    {
      if ((double) this.m_fontSize == (double) value)
        return;
      this.m_fontSize = value;
    }
  }

  public PdfColor ForeColor
  {
    get => this.m_foreColor;
    set
    {
      if (!(this.m_foreColor != value))
        return;
      this.m_foreColor = value;
    }
  }

  public override string ToString()
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append("/");
    stringBuilder.Append(this.FontName);
    stringBuilder.Append(" ");
    stringBuilder.Append(this.m_fontSize.ToString());
    stringBuilder.Append(" ");
    stringBuilder.Append("Tf");
    stringBuilder.Append(" ");
    stringBuilder.Append(this.m_foreColor.ToString(PdfColorSpace.RGB, false));
    return stringBuilder.ToString();
  }
}
