// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Escher.WordImageReader
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;
using System;
using System.Drawing;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Escher;

[CLSCompliant(false)]
internal class WordImageReader : IWordImageReader
{
  private readonly MemoryStream m_dataStream;
  private string m_strImageName = string.Empty;
  private int m_iStartImage;
  private PICF m_picData = new PICF();
  private Image m_bitmap;
  private MsofbtSpContainer m_spContainer;
  private string m_altText;
  private string m_name;
  private byte[] m_unparsedData;
  private int m_dataStreamPosiotion;
  private ImageRecord m_imageRecord;

  internal string ImageName => this.m_strImageName;

  public short Width
  {
    get => this.m_picData.dxaGoal;
    set => this.m_picData.dxaGoal = value;
  }

  public short Height
  {
    get => this.m_picData.dyaGoal;
    set => this.m_picData.dyaGoal = value;
  }

  public Image Image => this.m_bitmap;

  public int WidthScale => (int) this.m_picData.mx;

  public int HeightScale => (int) this.m_picData.my;

  internal MsofbtSpContainer InlineShapeContainer => this.m_spContainer;

  internal PICF PictureDescriptor => this.m_picData;

  internal ImageRecord ImageRecord => this.m_imageRecord;

  internal string AlternativeText
  {
    get => this.m_altText;
    set => this.m_altText = value;
  }

  internal string Name
  {
    get => this.m_name;
    set => this.m_name = value;
  }

  internal byte[] UnparsedData => this.m_unparsedData;

  internal WordImageReader(MemoryStream dataStream, int offset, WordDocument doc)
  {
    try
    {
      if ((long) offset > dataStream.Length)
        return;
      this.m_dataStream = dataStream;
      this.m_iStartImage = offset;
      this.m_dataStream.Position = (long) offset;
      this.m_picData.Read(new BinaryReader((Stream) dataStream));
      this.m_dataStreamPosiotion = (int) this.m_dataStream.Position;
      this.m_spContainer = MsofbtSpContainer.ReadInlineImageContainers(this.m_picData.lcb - (int) this.m_picData.cbHeader, (Stream) this.m_dataStream, doc);
      this.UpdateProps();
      if (this.m_spContainer == null)
      {
        this.UpdateUnParsedData();
      }
      else
      {
        _Blip fromShapeContainer = MsofbtSpContainer.GetBlipFromShapeContainer((BaseEscherRecord) this.m_spContainer);
        if (fromShapeContainer == null)
          return;
        this.m_imageRecord = fromShapeContainer.ImageRecord;
      }
    }
    catch
    {
      if (this.m_spContainer != null)
        return;
      this.UpdateUnParsedData();
    }
  }

  private void UpdateUnParsedData()
  {
    int length = this.m_picData.lcb - (int) this.m_picData.cbHeader;
    if (length <= 0 || (long) length >= this.m_dataStream.Length - (long) this.m_dataStreamPosiotion)
      return;
    this.m_dataStream.Position = 0L;
    byte[] buffer = new byte[this.m_dataStream.Length];
    this.m_dataStream.Read(buffer, 0, buffer.Length);
    this.m_unparsedData = new byte[length];
    for (int index = 0; index < length; ++index)
      this.m_unparsedData[index] = buffer[this.m_dataStreamPosiotion + index];
  }

  private void UpdateProps()
  {
    byte[] complexPropValue1 = this.m_spContainer.GetComplexPropValue(897);
    if (complexPropValue1 != null)
      this.m_altText = Encoding.Unicode.GetString(complexPropValue1).Replace("\0", string.Empty);
    byte[] complexPropValue2 = this.m_spContainer.GetComplexPropValue(896);
    if (complexPropValue2 == null)
      return;
    this.m_name = Encoding.Unicode.GetString(complexPropValue2).Replace("\0", string.Empty);
  }
}
