// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.TiffMetadataParser
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

internal class TiffMetadataParser : IImageMetadataParser
{
  private CatalogedReaderBase m_reader;

  internal TiffMetadataParser(Stream stream)
  {
    this.m_reader = (CatalogedReaderBase) new CatalogedReader(stream);
  }

  public MemoryStream GetMetadata()
  {
    TiffTag tag = new TiffTag();
    switch (this.m_reader.ReadInt16(0))
    {
      case 18761:
        this.m_reader = this.m_reader.WithByteOrder(false);
        break;
      case 19789:
        this.m_reader = this.m_reader.WithByteOrder(true);
        break;
    }
    ushort marker = this.m_reader.ReadUInt16(2);
    tag.SetTiffMarker((int) marker);
    int offset = this.m_reader.ReadInt32(4);
    if ((long) offset >= this.m_reader.Length - 1L)
      offset = 8;
    List<int> processedOffsets = new List<int>();
    TiffMetadataParser.ProcessImageFileDirectory(tag, this.m_reader, (ICollection<int>) processedOffsets, offset);
    return tag.Data;
  }

  internal static void ProcessImageFileDirectory(
    TiffTag tag,
    CatalogedReaderBase reader,
    ICollection<int> processedOffsets,
    int offset)
  {
    int unshiftedOffset = reader.ToUnshiftedOffset(offset);
    if (processedOffsets.Contains(unshiftedOffset))
      return;
    processedOffsets.Add(unshiftedOffset);
    if (offset < 0 || (long) offset >= reader.Length)
      return;
    int entry1 = (int) reader.ReadUInt16(offset);
    if (entry1 > (int) byte.MaxValue && (entry1 & (int) byte.MaxValue) == 0)
    {
      entry1 >>= 8;
      reader = reader.WithByteOrder(!reader.IsBigEndian);
    }
    if ((long) (2 + 12 * entry1 + 4 + offset) > reader.Length)
      return;
    int num = 0;
    for (int entry2 = 0; entry2 < entry1; ++entry2)
    {
      int tagOffset1 = TiffMetadataParser.CalculateTagOffset(offset, entry2);
      int tagId = (int) reader.ReadUInt16(tagOffset1);
      DataTypeID dataTypeId = (DataTypeID) reader.ReadUInt16(tagOffset1 + 2);
      uint componentCount = reader.ReadUInt32(tagOffset1 + 4);
      DataType dataType = DataType.FromTiffFormatCode(dataTypeId);
      long byteCount;
      if (dataType == null)
      {
        if (!tag.TryCustomProcessFormat(tagId, dataTypeId, componentCount, out byteCount))
        {
          if (++num > 5)
            return;
          continue;
        }
      }
      else
        byteCount = (long) componentCount * (long) dataType.Size;
      long tagOffset2;
      if (byteCount > 4L)
      {
        tagOffset2 = (long) reader.ReadUInt32(tagOffset1 + 8);
        if (tagOffset2 + byteCount > reader.Length)
          continue;
      }
      else
        tagOffset2 = (long) (tagOffset1 + 8);
      if (tagOffset2 >= 0L && tagOffset2 <= reader.Length && byteCount >= 0L && tagOffset2 + byteCount <= reader.Length)
      {
        bool flag = false;
        if (byteCount == checked (4L * (long) componentCount))
        {
          for (int index = 0; (long) index < (long) componentCount; ++index)
          {
            if (tag.TryEnterSubDirectory(tagId))
            {
              flag = true;
              uint offset1 = reader.ReadUInt32((int) (tagOffset2 + (long) (index * 4)));
              TiffMetadataParser.ProcessImageFileDirectory(tag, reader, processedOffsets, (int) offset1);
            }
          }
        }
        if (!flag)
          tag.CustomProcessTag((int) tagOffset2, processedOffsets, reader, tagId, (int) byteCount);
      }
    }
    int tagOffset = TiffMetadataParser.CalculateTagOffset(offset, entry1);
    int offset2 = reader.ReadInt32(tagOffset);
    if (offset2 == 0 || (long) offset2 >= reader.Length || offset2 < offset)
      return;
    TiffMetadataParser.ProcessImageFileDirectory(tag, reader, processedOffsets, offset2);
  }

  private static int CalculateTagOffset(int offset, int entry) => offset + 2 + 12 * entry;
}
