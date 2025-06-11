// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.ObjectInfoStream
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.CompoundFile.XlsIO;
using Syncfusion.CompoundFile.XlsIO.Native;
using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

internal class ObjectInfoStream : DataStructure
{
  private const int DEF_STRUCT_SIZE = 6;
  private byte[] m_dataBytes;

  internal override int Length => 6;

  internal ObjectInfoStream(CompoundStream compStream)
  {
    byte[] numArray = new byte[compStream.Length];
    compStream.Read(numArray, 0, numArray.Length);
    this.Parse(numArray, 0);
  }

  internal ObjectInfoStream()
  {
  }

  internal override void Parse(byte[] arrData, int iOffset) => this.m_dataBytes = arrData;

  internal override int Save(byte[] arrData, int iOffset)
  {
    throw new NotImplementedException("Not implemented");
  }

  internal void SaveTo(StgStream stgStream)
  {
    this.m_dataBytes = new byte[6]
    {
      (byte) 128 /*0x80*/,
      (byte) 0,
      (byte) 3,
      (byte) 0,
      (byte) 4,
      (byte) 0
    };
    stgStream.Write(this.m_dataBytes, 0, this.m_dataBytes.Length);
  }
}
