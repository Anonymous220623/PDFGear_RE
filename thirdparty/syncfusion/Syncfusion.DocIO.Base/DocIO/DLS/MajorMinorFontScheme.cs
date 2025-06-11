// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.MajorMinorFontScheme
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class MajorMinorFontScheme
{
  private Dictionary<string, string> m_fontTypefaces;
  private FontSchemeStruct m_fontSchemeStruct;
  private List<FontSchemeStruct> m_fontSchemeList;

  internal Dictionary<string, string> FontTypeface
  {
    get => this.m_fontTypefaces;
    set => this.m_fontTypefaces = new Dictionary<string, string>();
  }

  internal List<FontSchemeStruct> FontSchemeList
  {
    get => this.m_fontSchemeList;
    set => this.m_fontSchemeList = new List<FontSchemeStruct>();
  }

  internal FontSchemeStruct FontSchemeStructure
  {
    get => this.m_fontSchemeStruct;
    set => this.m_fontSchemeStruct = new FontSchemeStruct();
  }

  public MajorMinorFontScheme()
  {
    this.m_fontSchemeStruct = new FontSchemeStruct();
    this.m_fontTypefaces = new Dictionary<string, string>();
    this.m_fontSchemeList = new List<FontSchemeStruct>();
  }

  internal void Close()
  {
    if (this.m_fontTypefaces != null)
    {
      this.m_fontTypefaces.Clear();
      this.m_fontTypefaces = (Dictionary<string, string>) null;
    }
    if (this.m_fontSchemeList == null)
      return;
    this.m_fontSchemeList.Clear();
    this.m_fontSchemeList = (List<FontSchemeStruct>) null;
  }
}
