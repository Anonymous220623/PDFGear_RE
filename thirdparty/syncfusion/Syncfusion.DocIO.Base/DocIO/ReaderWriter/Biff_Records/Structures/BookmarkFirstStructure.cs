// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures.BookmarkFirstStructure
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures;

internal class BookmarkFirstStructure
{
  private int m_beginCP;
  private short m_endIndex;
  private int m_props;

  internal int BeginPos
  {
    get => this.m_beginCP;
    set => this.m_beginCP = value;
  }

  internal int Props
  {
    get => this.m_props;
    set => this.m_props = value;
  }

  internal short EndIndex
  {
    get => this.m_endIndex;
    set => this.m_endIndex = value;
  }

  internal byte[] SavePos()
  {
    byte[] numArray = new byte[4];
    BitConverter.GetBytes(this.m_beginCP).CopyTo((Array) numArray, 0);
    return numArray;
  }

  internal byte[] SaveProps()
  {
    byte[] numArray = new byte[4];
    BitConverter.GetBytes(this.m_endIndex).CopyTo((Array) numArray, 0);
    BitConverter.GetBytes((ushort) this.m_props).CopyTo((Array) numArray, 2);
    return numArray;
  }
}
