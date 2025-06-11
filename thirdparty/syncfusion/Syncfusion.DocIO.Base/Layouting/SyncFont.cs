// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.SyncFont
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using System.Drawing;

#nullable disable
namespace Syncfusion.Layouting;

internal class SyncFont
{
  private byte m_bFlags;
  private string m_fontname;
  private float m_fontsize;

  internal bool Bold
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  internal bool Italic
  {
    get => ((int) this.m_bFlags & 2) >> 1 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 253 | (value ? 1 : 0) << 1);
  }

  internal bool Underline
  {
    get => ((int) this.m_bFlags & 4) >> 2 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 251 | (value ? 1 : 0) << 2);
  }

  internal bool Strikeout
  {
    get => ((int) this.m_bFlags & 8) >> 3 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 247 | (value ? 1 : 0) << 3);
  }

  public SyncFont(Font font)
  {
    this.m_fontname = font.Name;
    this.m_fontsize = font.SizeInPoints;
    if (font.Bold)
      this.Bold = true;
    if (font.Italic)
      this.Italic = true;
    if (font.Underline)
      this.Underline = true;
    if (!font.Strikeout)
      return;
    this.Strikeout = true;
  }

  public Font GetFont(WordDocument Document)
  {
    FontStyle fontStyle = FontStyle.Regular;
    float fontSize = (double) this.m_fontsize == 0.0 ? 0.5f : this.m_fontsize;
    if (this.Bold)
      fontStyle |= FontStyle.Bold;
    if (this.Italic)
      fontStyle |= FontStyle.Italic;
    if (this.Underline)
      fontStyle |= FontStyle.Underline;
    if (this.Strikeout)
      fontStyle |= FontStyle.Strikeout;
    return Document.FontSettings.GetFont(this.m_fontname, fontSize, fontStyle);
  }
}
