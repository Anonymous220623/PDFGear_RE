// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.DataStreamParser.OLEObject.ObjectInfoStream
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using System;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.DataStreamParser.OLEObject;

internal class ObjectInfoStream
{
  private const int DEF_STRUCT_SIZE = 6;
  private byte[] m_dataBytes;

  internal int Length => 6;

  internal ObjectInfoStream(Stream stream) => this.Parse((stream as MemoryStream).ToArray(), 0);

  internal ObjectInfoStream()
  {
  }

  internal void Parse(byte[] arrData, int iOffset) => this.m_dataBytes = arrData;

  internal int Save(byte[] arrData, int iOffset)
  {
    throw new NotImplementedException("Not implemented");
  }

  internal void SaveTo(Stream stream, OleLinkType linkType, OleObjectType oleType)
  {
    switch (oleType)
    {
      case OleObjectType.Undefined:
      case OleObjectType.WordPadDocument:
        if (linkType == OleLinkType.Embed)
        {
          this.m_dataBytes = new byte[6]
          {
            (byte) 0,
            (byte) 0,
            (byte) 3,
            (byte) 0,
            (byte) 4,
            (byte) 0
          };
          break;
        }
        this.m_dataBytes = new byte[6]
        {
          (byte) 16 /*0x10*/,
          (byte) 2,
          (byte) 3,
          (byte) 0,
          (byte) 13,
          (byte) 0
        };
        break;
      case OleObjectType.AdobeAcrobatDocument:
      case OleObjectType.Excel_97_2003_Worksheet:
      case OleObjectType.ExcelBinaryWorksheet:
      case OleObjectType.ExcelMacroWorksheet:
      case OleObjectType.ExcelWorksheet:
      case OleObjectType.PowerPoint_97_2003_Presentation:
      case OleObjectType.PowerPointMacroPresentation:
      case OleObjectType.PowerPointMacroSlide:
      case OleObjectType.PowerPointPresentation:
      case OleObjectType.PowerPointSlide:
      case OleObjectType.WordMacroDocument:
      case OleObjectType.VisioDrawing:
      case OleObjectType.OpenDocumentPresentation:
      case OleObjectType.OpenDocumentSpreadsheet:
      case OleObjectType.OpenOfficeSpreadsheet1_1:
      case OleObjectType.OpenOfficeText_1_1:
      case OleObjectType.OpenOfficeSpreadsheet:
      case OleObjectType.OpenOfficeText:
        if (linkType == OleLinkType.Embed)
        {
          this.m_dataBytes = new byte[6]
          {
            (byte) 64 /*0x40*/,
            (byte) 0,
            (byte) 3,
            (byte) 0,
            (byte) 4,
            (byte) 0
          };
          break;
        }
        this.m_dataBytes = new byte[6]
        {
          (byte) 16 /*0x10*/,
          (byte) 0,
          (byte) 3,
          (byte) 0,
          (byte) 13,
          (byte) 0
        };
        break;
      case OleObjectType.BitmapImage:
      case OleObjectType.MIDISequence:
      case OleObjectType.VideoClip:
        if (linkType == OleLinkType.Embed)
        {
          this.m_dataBytes = new byte[6]
          {
            (byte) 0,
            (byte) 0,
            (byte) 3,
            (byte) 0,
            (byte) 4,
            (byte) 0
          };
          break;
        }
        this.m_dataBytes = new byte[6]
        {
          (byte) 16 /*0x10*/,
          (byte) 0,
          (byte) 3,
          (byte) 0,
          (byte) 4,
          (byte) 0
        };
        break;
      case OleObjectType.MediaClip:
      case OleObjectType.Package:
      case OleObjectType.WaveSound:
        this.m_dataBytes = new byte[6]
        {
          (byte) 64 /*0x40*/,
          (byte) 0,
          (byte) 3,
          (byte) 0,
          (byte) 4,
          (byte) 0
        };
        break;
      case OleObjectType.Equation:
        if (linkType == OleLinkType.Embed)
        {
          this.m_dataBytes = new byte[6]
          {
            (byte) 0,
            (byte) 0,
            (byte) 3,
            (byte) 0,
            (byte) 4,
            (byte) 0
          };
          break;
        }
        break;
      case OleObjectType.GraphChart:
      case OleObjectType.ExcelChart:
        if (linkType == OleLinkType.Embed)
        {
          this.m_dataBytes = new byte[6]
          {
            (byte) 0,
            (byte) 2,
            (byte) 3,
            (byte) 0,
            (byte) 13,
            (byte) 0
          };
          break;
        }
        this.m_dataBytes = new byte[6]
        {
          (byte) 16 /*0x10*/,
          (byte) 0,
          (byte) 3,
          (byte) 0,
          (byte) 13,
          (byte) 0
        };
        break;
      case OleObjectType.PowerPoint_97_2003_Slide:
      case OleObjectType.WordDocument:
        if (linkType == OleLinkType.Embed)
        {
          this.m_dataBytes = new byte[6]
          {
            (byte) 0,
            (byte) 0,
            (byte) 3,
            (byte) 0,
            (byte) 1,
            (byte) 0
          };
          break;
        }
        this.m_dataBytes = new byte[6]
        {
          (byte) 16 /*0x10*/,
          (byte) 0,
          (byte) 3,
          (byte) 0,
          (byte) 13,
          (byte) 0
        };
        break;
      case OleObjectType.Word_97_2003_Document:
        if (linkType == OleLinkType.Embed)
        {
          this.m_dataBytes = new byte[6]
          {
            (byte) 0,
            (byte) 2,
            (byte) 3,
            (byte) 0,
            (byte) 1,
            (byte) 0
          };
          break;
        }
        this.m_dataBytes = new byte[6]
        {
          (byte) 16 /*0x10*/,
          (byte) 2,
          (byte) 3,
          (byte) 0,
          (byte) 13,
          (byte) 0
        };
        break;
    }
    stream.Write(this.m_dataBytes, 0, this.m_dataBytes.Length);
  }
}
