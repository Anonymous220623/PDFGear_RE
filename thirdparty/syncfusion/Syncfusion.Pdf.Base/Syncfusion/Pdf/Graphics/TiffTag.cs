// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.TiffTag
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics.Images.Decoder;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

internal class TiffTag
{
  private Stack<TagDirectory> m_dictionary = new Stack<TagDirectory>();
  private TagDirectory m_currentTag;
  private List<TagDirectory> m_directories = new List<TagDirectory>();
  private MemoryStream m_stream;

  internal MemoryStream Data => this.m_stream;

  internal void PushDirectory(TagDirectory tag)
  {
    if (this.m_currentTag != null)
    {
      this.m_dictionary.Push(this.m_currentTag);
      tag.Parent = this.m_currentTag;
    }
    this.m_currentTag = tag;
    this.m_directories.Add(this.m_currentTag);
  }

  internal void SetTiffMarker(int marker)
  {
    switch (marker)
    {
      case 42:
      case 20306:
      case 21330:
        this.PushDirectory(new TagDirectory("Exif IFD0"));
        break;
      case 85:
        this.PushDirectory(new TagDirectory("PanasonicRaw Exif IFD0"));
        break;
    }
  }

  internal bool TryCustomProcessFormat(
    int tagId,
    DataTypeID formatCode,
    uint componentCount,
    out long byteCount)
  {
    byteCount = 0L;
    if (formatCode == (DataTypeID.RationalUnsigned | DataTypeID.Int16Signed))
    {
      byteCount = 4L * (long) componentCount;
      return true;
    }
    return formatCode == (DataTypeID) 0;
  }

  internal bool TryEnterSubDirectory(int tagId)
  {
    if (tagId == 330)
    {
      this.PushDirectory(new TagDirectory("Exif SubIFD"));
      return true;
    }
    if (this.m_currentTag.Name == "Exif IFD0" || this.m_currentTag.Name == "PanasonicRaw Exif IFD0")
    {
      if (tagId == 34665)
      {
        this.PushDirectory(new TagDirectory("Exif SubIFD"));
        return true;
      }
      if (tagId == 34853)
      {
        this.PushDirectory(new TagDirectory("GPS"));
        return true;
      }
    }
    else if (this.m_currentTag.Name == "Exif SubIFD")
    {
      if (tagId == 40965)
      {
        this.PushDirectory(new TagDirectory("Interoperability"));
        return true;
      }
    }
    else if (this.m_currentTag.Name == "Olympus Makernote")
    {
      switch (tagId)
      {
        case 8208:
          this.PushDirectory(new TagDirectory("Olympus Equipment"));
          return true;
        case 8224:
          this.PushDirectory(new TagDirectory("Olympus Camera Settings"));
          return true;
        case 8240:
          this.PushDirectory(new TagDirectory("Olympus Raw Development"));
          return true;
        case 8241:
          this.PushDirectory(new TagDirectory("Olympus Raw Development 2"));
          return true;
        case 8256:
          this.PushDirectory(new TagDirectory("Olympus Image Processing"));
          return true;
        case 8272:
          this.PushDirectory(new TagDirectory("Olympus Focus Info"));
          return true;
        case 12288 /*0x3000*/:
          this.PushDirectory(new TagDirectory("Olympus Raw Info"));
          return true;
        case 16384 /*0x4000*/:
          this.PushDirectory(new TagDirectory("Olympus Makernote"));
          return true;
      }
    }
    return false;
  }

  private bool HandlePrintIM(TagDirectory directory, int tagId)
  {
    switch (tagId)
    {
      case 3584 /*0x0E00*/:
        if (directory.Name == "Casio Makernote" || directory.Name == "Kyocera/Contax Makernote" || directory.Name == "Nikon Makernote" || directory.Name == "Olympus Makernote" || directory.Name == "Panasonic Makernote" || directory.Name == "Pentax Makernote" || directory.Name == "Ricoh Makernote" || directory.Name == "Sanyo Makernote" || directory.Name == "Sony Makernote")
          return true;
        break;
      case 50341:
        return true;
    }
    return false;
  }

  internal void CustomProcessTag(
    int tagOffset,
    ICollection<int> processedIfdOffsets,
    CatalogedReaderBase reader,
    int tagId,
    int byteCount)
  {
    if (tagId == 0 || byteCount == 0 || tagId == 37500 && this.m_currentTag.Name == "Exif SubIFD" || tagId == 33723 && this.m_currentTag.Name == "Exif IFD0" && reader.ReadSignedByte(tagOffset) == (sbyte) 28 || tagId == 34377 && this.m_currentTag.Name == "Exif IFD0")
      return;
    if (tagId == 34675)
      this.m_directories.Add(new TagDirectory("ICC Profile"));
    else if (tagId == 700 && (this.m_currentTag.Name == "Exif IFD0" || this.m_currentTag.Name == "Exif SubIFD"))
    {
      byte[] buffer = reader.ReadNullTerminatedBytes(tagOffset, byteCount);
      if (this.m_stream == null)
        this.m_stream = new MemoryStream();
      this.m_stream.Write(buffer, 0, buffer.Length);
      this.m_directories.Add(new TagDirectory("XMP"));
    }
    else if (this.HandlePrintIM(this.m_currentTag, tagId))
      this.m_directories.Add(new TagDirectory("PrintIM"));
    else if (this.m_currentTag.Name == "Olympus Makernote")
    {
      switch (tagId)
      {
        case 8208:
          this.PushDirectory(new TagDirectory("Olympus Equipment"));
          TiffMetadataParser.ProcessImageFileDirectory(this, reader, processedIfdOffsets, tagOffset);
          break;
        case 8224:
          this.PushDirectory(new TagDirectory("Olympus Camera Settings"));
          TiffMetadataParser.ProcessImageFileDirectory(this, reader, processedIfdOffsets, tagOffset);
          break;
        case 8240:
          this.PushDirectory(new TagDirectory("Olympus Raw Development"));
          TiffMetadataParser.ProcessImageFileDirectory(this, reader, processedIfdOffsets, tagOffset);
          break;
        case 8241:
          this.PushDirectory(new TagDirectory("Olympus Raw Development 2"));
          TiffMetadataParser.ProcessImageFileDirectory(this, reader, processedIfdOffsets, tagOffset);
          break;
        case 8256:
          this.PushDirectory(new TagDirectory("Olympus Image Processing"));
          TiffMetadataParser.ProcessImageFileDirectory(this, reader, processedIfdOffsets, tagOffset);
          break;
        case 8272:
          this.PushDirectory(new TagDirectory("Olympus Focus Info"));
          TiffMetadataParser.ProcessImageFileDirectory(this, reader, processedIfdOffsets, tagOffset);
          break;
        case 12288 /*0x3000*/:
          this.PushDirectory(new TagDirectory("Olympus Raw Info"));
          TiffMetadataParser.ProcessImageFileDirectory(this, reader, processedIfdOffsets, tagOffset);
          break;
        case 16384 /*0x4000*/:
          this.PushDirectory(new TagDirectory("Olympus Makernote"));
          TiffMetadataParser.ProcessImageFileDirectory(this, reader, processedIfdOffsets, tagOffset);
          break;
      }
    }
    if (this.m_currentTag.Name == "PanasonicRaw Exif IFD0")
    {
      switch (tagId)
      {
        case 19:
          this.m_directories.Add(new TagDirectory("PanasonicRaw WbInfo"));
          break;
        case 39:
          this.m_directories.Add(new TagDirectory("PanasonicRaw WbInfo2"));
          break;
        case 281:
          this.m_directories.Add(new TagDirectory("PanasonicRaw DistortionInfo"));
          break;
      }
    }
    if (tagId != 46 || !(this.m_currentTag.Name == "PanasonicRaw Exif IFD0"))
      return;
    this.m_stream = new JpegDecoder((Stream) new MemoryStream(reader.GetBytes(tagOffset, byteCount)), true).MetadataStream;
  }
}
