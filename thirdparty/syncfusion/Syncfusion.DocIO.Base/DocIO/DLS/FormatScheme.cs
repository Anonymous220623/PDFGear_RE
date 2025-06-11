// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.FormatScheme
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class FormatScheme
{
  private string m_fmtSchemeName;
  private List<FillFormat> m_bgFillFormats;
  private List<FillFormat> m_fillFormats;
  private List<LineFormat> m_lnStyleList;
  private List<EffectFormat> m_effectList;

  internal string FmtName
  {
    get => this.m_fmtSchemeName;
    set => this.m_fmtSchemeName = value;
  }

  internal List<FillFormat> BgFillFormats
  {
    get => this.m_bgFillFormats;
    set => this.m_bgFillFormats = new List<FillFormat>();
  }

  internal List<FillFormat> FillFormats
  {
    get => this.m_fillFormats;
    set => this.m_fillFormats = new List<FillFormat>();
  }

  internal List<LineFormat> LnStyleScheme
  {
    get => this.m_lnStyleList;
    set => this.m_lnStyleList = new List<LineFormat>();
  }

  internal List<EffectFormat> EffectStyles
  {
    get
    {
      if (this.m_effectList == null)
        this.m_effectList = new List<EffectFormat>();
      return this.m_effectList;
    }
    set => this.m_effectList = value;
  }

  internal FormatScheme()
  {
    this.m_fillFormats = new List<FillFormat>();
    this.m_bgFillFormats = new List<FillFormat>();
    this.m_lnStyleList = new List<LineFormat>();
    this.m_effectList = new List<EffectFormat>();
  }

  internal void Close()
  {
    if (this.m_bgFillFormats != null)
    {
      foreach (FillFormat bgFillFormat in this.m_bgFillFormats)
        bgFillFormat.Close();
      this.m_bgFillFormats.Clear();
      this.m_bgFillFormats = (List<FillFormat>) null;
    }
    if (this.m_fillFormats != null)
    {
      foreach (FillFormat fillFormat in this.m_fillFormats)
        fillFormat.Close();
      this.m_fillFormats.Clear();
      this.m_fillFormats = (List<FillFormat>) null;
    }
    if (this.m_effectList != null)
    {
      foreach (EffectFormat effect in this.m_effectList)
        effect.Close();
      this.m_effectList.Clear();
      this.m_effectList = (List<EffectFormat>) null;
    }
    if (this.m_lnStyleList == null)
      return;
    foreach (LineFormat lnStyle in this.m_lnStyleList)
      lnStyle.Close();
    this.m_lnStyleList.Clear();
    this.m_lnStyleList = (List<LineFormat>) null;
  }
}
