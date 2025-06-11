// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.CFELayout
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class CFELayout
{
  private ushort m_flag;
  private int m_id;
  private CombineBracketsType m_combineBrackets;

  internal bool Combine
  {
    get => ((int) this.m_flag & 2) != 0;
    set => this.m_flag = (ushort) ((int) this.m_flag & 65533 | (value ? 1 : 0) << 1);
  }

  internal bool Vertical
  {
    get => ((int) this.m_flag & 1) != 0;
    set => this.m_flag = (ushort) ((int) this.m_flag & 65534 | (value ? 1 : 0));
  }

  internal bool VerticalCompress
  {
    get => ((int) this.m_flag & 4096 /*0x1000*/) != 0;
    set => this.m_flag = (ushort) ((int) this.m_flag & 61439 /*0xEFFF*/ | (value ? 1 : 0) << 12);
  }

  internal int ID
  {
    get => this.m_id;
    set => this.m_id = value;
  }

  internal CombineBracketsType CombineBracketsType
  {
    get => this.m_combineBrackets;
    set
    {
      this.m_flag = (ushort) ((int) this.m_flag & 65279 | 256 /*0x0100*/);
      this.m_combineBrackets = value;
    }
  }

  internal bool HasCombineBracketsType() => ((int) this.m_flag & 256 /*0x0100*/) != 0;

  internal void UpdateCFELayout(ushort ufel, int iFELayoutId)
  {
    this.Vertical = ((int) ufel & 1) != 0;
    this.Combine = ((int) ufel & 2) != 0;
    this.VerticalCompress = ((int) ufel & 4096 /*0x1000*/) != 0;
    this.ID = iFELayoutId;
    this.CombineBracketsType = (CombineBracketsType) ((int) ufel >> 8 & 7);
  }

  internal byte[] GetCFELayoutBytes()
  {
    byte[] dst = new byte[6];
    if (((int) this.m_flag & 256 /*0x0100*/) != 0)
    {
      this.m_flag &= (ushort) 65279;
      this.m_flag = (ushort) ((int) this.m_flag & 63743 | (int) this.CombineBracketsType << 8);
    }
    Buffer.BlockCopy((Array) BitConverter.GetBytes(this.m_flag), 0, (Array) dst, 0, 2);
    Buffer.BlockCopy((Array) BitConverter.GetBytes(this.ID), 0, (Array) dst, 2, 4);
    return dst;
  }
}
