// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.CommonStruct
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2;

internal abstract class CommonStruct
{
  internal ProgressMgr m_progress;
  internal CommonStruct.JpegState m_global_state;

  public abstract bool IsDecompressor { get; }

  public ProgressMgr Progress
  {
    get => this.m_progress;
    set
    {
      this.m_progress = value != null ? value : throw new ArgumentNullException(nameof (value));
    }
  }

  public static jvirt_array<byte> CreateSamplesArray(int samplesPerRow, int numberOfRows)
  {
    return new jvirt_array<byte>(samplesPerRow, numberOfRows, new jvirt_array<byte>.Allocator(CommonStruct.AllocJpegSamples));
  }

  public static jvirt_array<JBLOCK> CreateBlocksArray(int blocksPerRow, int numberOfRows)
  {
    return new jvirt_array<JBLOCK>(blocksPerRow, numberOfRows, new jvirt_array<JBLOCK>.Allocator(CommonStruct.allocJpegBlocks));
  }

  public static byte[][] AllocJpegSamples(int samplesPerRow, int numberOfRows)
  {
    byte[][] numArray = new byte[numberOfRows][];
    for (int index = 0; index < numberOfRows; ++index)
      numArray[index] = new byte[samplesPerRow];
    return numArray;
  }

  private static JBLOCK[][] allocJpegBlocks(int blocksPerRow, int numberOfRows)
  {
    JBLOCK[][] jblockArray = new JBLOCK[numberOfRows][];
    for (int index1 = 0; index1 < numberOfRows; ++index1)
    {
      jblockArray[index1] = new JBLOCK[blocksPerRow];
      for (int index2 = 0; index2 < blocksPerRow; ++index2)
        jblockArray[index1][index2] = new JBLOCK();
    }
    return jblockArray;
  }

  public void jpeg_abort()
  {
    if (this.IsDecompressor)
    {
      this.m_global_state = CommonStruct.JpegState.DSTATE_START;
      if (!(this is DecompressStruct decompressStruct))
        return;
      decompressStruct.m_marker_list = (List<MarkerStruct>) null;
    }
    else
      this.m_global_state = CommonStruct.JpegState.CSTATE_START;
  }

  public void jpeg_destroy() => this.m_global_state = CommonStruct.JpegState.DESTROYED;

  internal enum JpegState
  {
    DESTROYED = 0,
    CSTATE_START = 100, // 0x00000064
    CSTATE_SCANNING = 101, // 0x00000065
    CSTATE_RAW_OK = 102, // 0x00000066
    CSTATE_WRCOEFS = 103, // 0x00000067
    DSTATE_START = 200, // 0x000000C8
    DSTATE_INHEADER = 201, // 0x000000C9
    DSTATE_READY = 202, // 0x000000CA
    DSTATE_PRELOAD = 203, // 0x000000CB
    DSTATE_PRESCAN = 204, // 0x000000CC
    DSTATE_SCANNING = 205, // 0x000000CD
    DSTATE_RAW_OK = 206, // 0x000000CE
    DSTATE_BUFIMAGE = 207, // 0x000000CF
    DSTATE_BUFPOST = 208, // 0x000000D0
    DSTATE_RDCOEFS = 209, // 0x000000D1
    DSTATE_STOPPING = 210, // 0x000000D2
  }
}
