// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher.LineStyleBooleanProperties
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;

internal class LineStyleBooleanProperties
{
  internal const uint DefaultValue = 46;
  private int m_key;
  private msofbtRGFOPTE m_prop;

  internal bool HasDefined => this.m_prop.ContainsKey(this.m_key);

  internal bool PenAlignInset
  {
    get => this.UsefInsetPenOK && this.InsetPenOK && this.UsefInsetPen && this.InsetPen;
    set => this.UsefInsetPenOK = this.InsetPenOK = this.UsefInsetPen = this.InsetPen = value;
  }

  internal bool UsefLineOpaqueBackColor
  {
    get
    {
      return this.HasDefined && ((this.m_prop[this.m_key] as FOPTEBid).Value & 33554432U /*0x02000000*/) >> 25 != 0U;
    }
    set
    {
      if (!this.HasDefined)
        return;
      (this.m_prop[this.m_key] as FOPTEBid).Value = (uint) ((long) ((this.m_prop[this.m_key] as FOPTEBid).Value & 4261412863U) | (long) ((value ? 1 : 0) << 25));
    }
  }

  internal bool UsefInsetPen
  {
    get
    {
      return this.HasDefined && ((this.m_prop[this.m_key] as FOPTEBid).Value & 4194304U /*0x400000*/) >> 22 != 0U;
    }
    set
    {
      if (!this.HasDefined)
        return;
      (this.m_prop[this.m_key] as FOPTEBid).Value = (uint) ((long) ((this.m_prop[this.m_key] as FOPTEBid).Value & 4290772991U) | (long) ((value ? 1 : 0) << 22));
    }
  }

  internal bool UsefInsetPenOK
  {
    get
    {
      return this.HasDefined && ((this.m_prop[this.m_key] as FOPTEBid).Value & 2097152U /*0x200000*/) >> 21 != 0U;
    }
    set
    {
      if (!this.HasDefined)
        return;
      (this.m_prop[this.m_key] as FOPTEBid).Value = (uint) ((long) ((this.m_prop[this.m_key] as FOPTEBid).Value & 4292870143U) | (long) ((value ? 1 : 0) << 21));
    }
  }

  internal bool UsefArrowheadsOK
  {
    get
    {
      return this.HasDefined && ((this.m_prop[this.m_key] as FOPTEBid).Value & 1048576U /*0x100000*/) >> 20 != 0U;
    }
    set
    {
      if (!this.HasDefined)
        return;
      (this.m_prop[this.m_key] as FOPTEBid).Value = (uint) ((long) ((this.m_prop[this.m_key] as FOPTEBid).Value & 4293918719U) | (long) ((value ? 1 : 0) << 20));
    }
  }

  internal bool UsefLine
  {
    get
    {
      return this.HasDefined && ((this.m_prop[this.m_key] as FOPTEBid).Value & 524288U /*0x080000*/) >> 19 != 0U;
    }
    set
    {
      if (!this.HasDefined)
        return;
      (this.m_prop[this.m_key] as FOPTEBid).Value = (uint) ((long) ((this.m_prop[this.m_key] as FOPTEBid).Value & 4294443007U) | (long) ((value ? 1 : 0) << 19));
    }
  }

  internal bool UsefHitTestLine
  {
    get
    {
      return this.HasDefined && ((this.m_prop[this.m_key] as FOPTEBid).Value & 262144U /*0x040000*/) >> 18 != 0U;
    }
    set
    {
      if (!this.HasDefined)
        return;
      (this.m_prop[this.m_key] as FOPTEBid).Value = (uint) ((long) ((this.m_prop[this.m_key] as FOPTEBid).Value & 4294705151U) | (long) ((value ? 1 : 0) << 18));
    }
  }

  internal bool UsefLineFillShape
  {
    get
    {
      return this.HasDefined && ((this.m_prop[this.m_key] as FOPTEBid).Value & 131072U /*0x020000*/) >> 17 != 0U;
    }
    set
    {
      if (!this.HasDefined)
        return;
      (this.m_prop[this.m_key] as FOPTEBid).Value = (uint) ((long) ((this.m_prop[this.m_key] as FOPTEBid).Value & 4294836223U) | (long) ((value ? 1 : 0) << 17));
    }
  }

  internal bool UsefNoLineDrawDash
  {
    get
    {
      return this.HasDefined && ((this.m_prop[this.m_key] as FOPTEBid).Value & 65536U /*0x010000*/) >> 16 /*0x10*/ != 0U;
    }
    set
    {
      if (!this.HasDefined)
        return;
      (this.m_prop[this.m_key] as FOPTEBid).Value = (uint) ((long) ((this.m_prop[this.m_key] as FOPTEBid).Value & 4294901759U) | (long) ((value ? 1 : 0) << 16 /*0x10*/));
    }
  }

  internal bool LineOpaqueBackColor
  {
    get
    {
      return this.HasDefined && ((this.m_prop[this.m_key] as FOPTEBid).Value & 512U /*0x0200*/) >> 9 != 0U;
    }
    set
    {
      if (!this.HasDefined)
        return;
      (this.m_prop[this.m_key] as FOPTEBid).Value = (uint) ((long) ((this.m_prop[this.m_key] as FOPTEBid).Value & 4294966783U) | (long) ((value ? 1 : 0) << 9));
    }
  }

  internal bool InsetPen
  {
    get
    {
      return this.HasDefined && ((this.m_prop[this.m_key] as FOPTEBid).Value & 64U /*0x40*/) >> 6 != 0U;
    }
    set
    {
      if (!this.HasDefined)
        return;
      (this.m_prop[this.m_key] as FOPTEBid).Value = (uint) ((long) ((this.m_prop[this.m_key] as FOPTEBid).Value & 4294967231U) | (long) ((value ? 1 : 0) << 6));
    }
  }

  internal bool InsetPenOK
  {
    get
    {
      return this.HasDefined && ((this.m_prop[this.m_key] as FOPTEBid).Value & 32U /*0x20*/) >> 5 != 0U;
    }
    set
    {
      if (!this.HasDefined)
        return;
      (this.m_prop[this.m_key] as FOPTEBid).Value = (uint) ((long) ((this.m_prop[this.m_key] as FOPTEBid).Value & 4294967263U) | (long) ((value ? 1 : 0) << 5));
    }
  }

  internal bool ArrowheadsOK
  {
    get
    {
      return this.HasDefined && ((this.m_prop[this.m_key] as FOPTEBid).Value & 16U /*0x10*/) >> 4 != 0U;
    }
    set
    {
      if (!this.HasDefined)
        return;
      (this.m_prop[this.m_key] as FOPTEBid).Value = (uint) ((long) ((this.m_prop[this.m_key] as FOPTEBid).Value & 4294967279U) | (long) ((value ? 1 : 0) << 4));
    }
  }

  internal bool Line
  {
    get => this.HasDefined && ((this.m_prop[this.m_key] as FOPTEBid).Value & 8U) >> 3 != 0U;
    set
    {
      if (!this.HasDefined)
        return;
      (this.m_prop[this.m_key] as FOPTEBid).Value = (uint) ((long) ((this.m_prop[this.m_key] as FOPTEBid).Value & 4294967287U) | (long) ((value ? 1 : 0) << 3));
    }
  }

  internal bool HitTestLine
  {
    get => this.HasDefined && ((this.m_prop[this.m_key] as FOPTEBid).Value & 4U) >> 2 != 0U;
    set
    {
      if (!this.HasDefined)
        return;
      (this.m_prop[this.m_key] as FOPTEBid).Value = (uint) ((long) ((this.m_prop[this.m_key] as FOPTEBid).Value & 4294967291U) | (long) ((value ? 1 : 0) << 2));
    }
  }

  internal bool LineFillShape
  {
    get => this.HasDefined && ((this.m_prop[this.m_key] as FOPTEBid).Value & 2U) >> 1 != 0U;
    set
    {
      if (!this.HasDefined)
        return;
      (this.m_prop[this.m_key] as FOPTEBid).Value = (uint) ((long) ((this.m_prop[this.m_key] as FOPTEBid).Value & 4294967293U) | (long) ((value ? 1 : 0) << 1));
    }
  }

  internal bool NoLineDrawDash
  {
    get => this.HasDefined && ((int) (this.m_prop[this.m_key] as FOPTEBid).Value & 1) != 0;
    set
    {
      if (!this.HasDefined)
        return;
      (this.m_prop[this.m_key] as FOPTEBid).Value = (uint) ((long) ((this.m_prop[this.m_key] as FOPTEBid).Value & 4294967294U) | (value ? 1L : 0L));
    }
  }

  internal LineStyleBooleanProperties(msofbtRGFOPTE prop, int key)
  {
    this.m_prop = prop;
    this.m_key = key;
  }
}
