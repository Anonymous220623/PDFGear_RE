// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.ShadingDescriptor
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records;

internal class ShadingDescriptor
{
  internal const int DEF_SHD_LENGTH = 2;
  internal const int DEF_SHD_NEW_LENGTH = 10;
  private uint m_foreColorExt = 4278190080 /*0xFF000000*/;
  private uint m_backColorExt = 4278190080 /*0xFF000000*/;
  private TextureStyle m_pattern;

  internal Color ForeColor
  {
    get
    {
      return this.m_foreColorExt != 4278190080U /*0xFF000000*/ ? WordColor.ConvertRGBToColor(this.m_foreColorExt) : Color.Empty;
    }
    set => this.m_foreColorExt = WordColor.ConvertColorToRGB(value);
  }

  internal Color BackColor
  {
    get
    {
      return this.m_backColorExt != 4278190080U /*0xFF000000*/ ? WordColor.ConvertRGBToColor(this.m_backColorExt) : Color.Empty;
    }
    set => this.m_backColorExt = WordColor.ConvertColorToRGB(value);
  }

  internal TextureStyle Pattern
  {
    get => this.m_pattern;
    set => this.m_pattern = value;
  }

  internal static int StructLength => 2;

  internal ShadingDescriptor(short shd) => this.Read(shd);

  internal ShadingDescriptor()
  {
  }

  internal ShadingDescriptor Clone()
  {
    return new ShadingDescriptor()
    {
      m_foreColorExt = this.m_foreColorExt,
      m_backColorExt = this.m_backColorExt,
      m_pattern = this.m_pattern
    };
  }

  internal void Read(short shd)
  {
    this.m_foreColorExt = WordColor.ConvertIdToRGB((int) shd & 31 /*0x1F*/);
    this.m_backColorExt = WordColor.ConvertIdToRGB(((int) shd & 992) >> 5);
    this.m_pattern = (TextureStyle) (((int) shd & 64512) >> 10);
  }

  internal short Save()
  {
    int num1 = 0;
    num1 = (int) WordColor.ArgbArray[0] | WordColor.ConvertRGBToId(this.m_foreColorExt);
    int num2 = (int) WordColor.ArgbArray[0] | WordColor.ConvertRGBToId(this.m_backColorExt) << 5;
    return this.m_pattern != TextureStyle.TextureNil ? (short) (num2 | (int) this.m_pattern << 10) : (short) num2;
  }

  internal void ReadNewShd(byte[] shd, int offset)
  {
    this.m_foreColorExt = BitConverter.ToUInt32(shd, offset);
    this.m_backColorExt = BitConverter.ToUInt32(shd, offset + 4);
    this.m_pattern = (TextureStyle) BitConverter.ToUInt16(shd, offset + 8);
  }

  internal byte[] SaveNewShd()
  {
    byte[] numArray = new byte[10];
    BitConverter.GetBytes(this.m_foreColorExt).CopyTo((Array) numArray, 0);
    BitConverter.GetBytes(this.m_backColorExt).CopyTo((Array) numArray, 4);
    BitConverter.GetBytes((ushort) this.m_pattern).CopyTo((Array) numArray, 8);
    return numArray;
  }
}
