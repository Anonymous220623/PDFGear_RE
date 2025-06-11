// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher._Blip
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO.DLS.Entities;
using Syncfusion.DocIO.ReaderWriter.Escher;
using System;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;

internal abstract class _Blip(WordDocument doc) : BaseEscherRecord(doc)
{
  protected const int DEF_UID_LENGTH = 16 /*0x10*/;
  private Guid m_guid;
  private Guid m_guid2;

  internal MSOBlipType Type => (MSOBlipType) (this.Header.Type - 61464);

  internal abstract byte[] ImageBytes { get; set; }

  internal abstract ImageRecord ImageRecord { get; set; }

  internal ImageFormat ImageFormat
  {
    get
    {
      switch (this.Type)
      {
        case MSOBlipType.msoblipEMF:
          return ImageFormat.Emf;
        case MSOBlipType.msoblipWMF:
          return ImageFormat.Wmf;
        case MSOBlipType.msoblipJPEG:
          return ImageFormat.Jpeg;
        case MSOBlipType.msoblipPNG:
          return ImageFormat.Png;
        case MSOBlipType.msoblipDIB:
          return ImageFormat.Bmp;
        default:
          throw new Exception(this.Type.ToString() + "is not supported");
      }
    }
  }

  internal Guid Uid
  {
    get => this.m_guid;
    set => this.m_guid = value;
  }

  internal Guid Uid2
  {
    get => this.m_guid2;
    set => this.m_guid2 = value;
  }

  internal bool IsDib => this.Type == MSOBlipType.msoblipDIB;

  protected void ReadGuid(Stream stream)
  {
    byte[] numArray1 = new byte[16 /*0x10*/];
    stream.Read(numArray1, 0, numArray1.Length);
    this.m_guid = new Guid(numArray1);
    if (!this.HasUid2())
      return;
    byte[] numArray2 = new byte[16 /*0x10*/];
    stream.Read(numArray2, 0, numArray2.Length);
    this.m_guid2 = new Guid(numArray2);
  }

  internal bool HasUid2()
  {
    if (this.Header.Type == MSOFBT.msofbtBlipEMF && this.Header.Instance == 981 || this.Header.Type == MSOFBT.msofbtBlipWMF && this.Header.Instance == 535 || this.Header.Type == (MSOFBT) 61468 && this.Header.Instance == 1347 || (this.Header.Type == MSOFBT.msofbtBlipJPEG || this.Header.Type == (MSOFBT) 61482) && (this.Header.Instance == 1131 || this.Header.Instance == 1763) || this.Header.Type == MSOFBT.msofbtBlipPNG && this.Header.Instance == 1761 || this.Header.Type == MSOFBT.msofbtBlipDIB && this.Header.Instance == 1961)
      return true;
    return this.Header.Type == (MSOFBT) 61481 && this.Header.Instance == 1765;
  }

  internal abstract override BaseEscherRecord Clone();
}
