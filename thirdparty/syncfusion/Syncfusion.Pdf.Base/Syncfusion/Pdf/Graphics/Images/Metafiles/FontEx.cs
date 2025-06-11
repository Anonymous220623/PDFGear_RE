// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Images.Metafiles.FontEx
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Native;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Images.Metafiles;

internal class FontEx : IDisposable
{
  private Font m_font;
  private LOGFONT m_structure;

  public FontEx(Font font, LOGFONT structure)
  {
    this.m_font = font != null ? font : throw new ArgumentNullException(nameof (font));
    this.m_structure = structure;
  }

  public Font Font => this.m_font;

  public float Angle => -((float) this.m_structure.lfEscapement / 10f);

  public LOGFONT LogFont => this.m_structure;

  public void Dispose()
  {
    if (this.m_font == null)
      return;
    this.m_font.Dispose();
    this.m_font = (Font) null;
  }
}
