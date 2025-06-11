// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.CustomFont
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;
using System.Drawing.Text;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

internal class CustomFont
{
  private PrivateFontCollection m_fontCollection;
  private Dictionary<string, Stream> m_embeddedFonts = new Dictionary<string, Stream>((IEqualityComparer<string>) System.StringComparer.InvariantCultureIgnoreCase);

  internal PrivateFontCollection FontCollection
  {
    get => this.m_fontCollection;
    set
    {
      if (value == null)
        return;
      this.m_fontCollection = value;
    }
  }

  internal Dictionary<string, Stream> EmbeddedFonts
  {
    get => this.m_embeddedFonts;
    set
    {
      if (value == null)
        return;
      this.m_embeddedFonts = value;
    }
  }

  internal CustomFont()
  {
  }
}
