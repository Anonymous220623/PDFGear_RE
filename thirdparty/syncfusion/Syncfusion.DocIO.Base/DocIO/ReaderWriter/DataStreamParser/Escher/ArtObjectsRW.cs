// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher.ArtObjectsRW
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;

[CLSCompliant(false)]
internal class ArtObjectsRW : BaseWordRecord
{
  private Stream m_stream;
  private DocIOSortedList<WordSubdocument, DocIOSortedList<int, FileShapeAddress>> m_fspas;
  private DocIOSortedList<WordSubdocument, DocIOSortedList<int, TextBoxStoryDescriptor>> m_txbxs;
  private DocIOSortedList<WordSubdocument, DocIOSortedList<int, BreakDescriptor>> m_txbxBkds;
  private int m_txBxMainEndPos;
  private int m_txBxHeaderEndPos;

  internal DocIOSortedList<int, FileShapeAddress> MainDocFSPAs
  {
    get
    {
      return this.m_fspas.ContainsKey(WordSubdocument.Main) ? this.m_fspas[WordSubdocument.Main] : (DocIOSortedList<int, FileShapeAddress>) null;
    }
  }

  internal DocIOSortedList<int, TextBoxStoryDescriptor> MainDocTxBxs
  {
    get
    {
      return this.m_txbxs.ContainsKey(WordSubdocument.Main) ? this.m_txbxs[WordSubdocument.Main] : (DocIOSortedList<int, TextBoxStoryDescriptor>) null;
    }
  }

  internal DocIOSortedList<int, BreakDescriptor> MainDocTxBxBKDs
  {
    get
    {
      return this.m_txbxBkds.ContainsKey(WordSubdocument.Main) ? this.m_txbxBkds[WordSubdocument.Main] : (DocIOSortedList<int, BreakDescriptor>) null;
    }
  }

  internal DocIOSortedList<int, FileShapeAddress> HfDocFSPAs
  {
    get
    {
      return this.m_fspas.ContainsKey(WordSubdocument.HeaderFooter) ? this.m_fspas[WordSubdocument.HeaderFooter] : (DocIOSortedList<int, FileShapeAddress>) null;
    }
  }

  internal DocIOSortedList<int, TextBoxStoryDescriptor> HfDocTxBxs
  {
    get
    {
      return this.m_txbxs.ContainsKey(WordSubdocument.HeaderFooter) ? this.m_txbxs[WordSubdocument.HeaderFooter] : (DocIOSortedList<int, TextBoxStoryDescriptor>) null;
    }
  }

  internal DocIOSortedList<int, BreakDescriptor> HfDocTxBxBKDs
  {
    get
    {
      return this.m_txbxBkds.ContainsKey(WordSubdocument.HeaderFooter) ? this.m_txbxBkds[WordSubdocument.HeaderFooter] : (DocIOSortedList<int, BreakDescriptor>) null;
    }
  }

  internal int StructsCount
  {
    get
    {
      return (this.MainDocFSPAs == null ? 0 : this.MainDocFSPAs.Count) + (this.HfDocFSPAs == null ? 0 : this.HfDocFSPAs.Count) + (this.HfDocTxBxs == null ? 0 : this.HfDocTxBxs.Count) + (this.MainDocTxBxs == null ? 0 : this.MainDocTxBxs.Count);
    }
  }

  internal ArtObjectsRW(Fib fib, Stream stream)
    : this()
  {
    this.Read(stream, fib);
  }

  internal ArtObjectsRW()
  {
    this.m_fspas = new DocIOSortedList<WordSubdocument, DocIOSortedList<int, FileShapeAddress>>();
    this.m_txbxBkds = new DocIOSortedList<WordSubdocument, DocIOSortedList<int, BreakDescriptor>>();
    this.m_txbxs = new DocIOSortedList<WordSubdocument, DocIOSortedList<int, TextBoxStoryDescriptor>>();
  }

  internal void AddFSPA(FileShapeAddress fspa, WordSubdocument docType, int pos)
  {
    if (!this.m_fspas.ContainsKey(docType))
    {
      DocIOSortedList<int, FileShapeAddress> docIoSortedList = new DocIOSortedList<int, FileShapeAddress>();
      this.m_fspas.Add(docType, docIoSortedList);
    }
    this.m_fspas[docType].Add(pos, fspa);
  }

  internal void AddTxbx(
    WordSubdocument docType,
    TextBoxStoryDescriptor txbxStoryDesc,
    BreakDescriptor txbxBKDesc,
    int pos)
  {
    if (!this.m_txbxs.ContainsKey(docType))
    {
      DocIOSortedList<int, TextBoxStoryDescriptor> docIoSortedList = new DocIOSortedList<int, TextBoxStoryDescriptor>();
      this.m_txbxs.Add(docType, docIoSortedList);
    }
    this.m_txbxs[docType].Add(pos, txbxStoryDesc);
    if (!this.m_txbxBkds.ContainsKey(docType))
    {
      DocIOSortedList<int, BreakDescriptor> docIoSortedList = new DocIOSortedList<int, BreakDescriptor>();
      this.m_txbxBkds.Add(docType, docIoSortedList);
    }
    this.m_txbxBkds[docType].Add(pos, txbxBKDesc);
    if (docType == WordSubdocument.Main)
      this.m_txBxMainEndPos = pos + 3;
    if (docType != WordSubdocument.HeaderFooter)
      return;
    this.m_txBxHeaderEndPos = pos + 3;
  }

  internal void Read(Stream stream, Fib fib)
  {
    this.m_stream = stream;
    this.ReadShapeFSPA(WordSubdocument.Main, (int) fib.FibRgFcLcb97FcPlcSpaMom, (int) fib.FibRgFcLcb97LcbPlcSpaMom);
    this.ReadShapeFSPA(WordSubdocument.HeaderFooter, (int) fib.FibRgFcLcb97FcPlcSpaHdr, (int) fib.FibRgFcLcb97LcbPlcSpaHdr);
    this.ReadTxbx(WordSubdocument.Main, (int) fib.FibRgFcLcb97FcPlcftxbxTxt, (int) fib.FibRgFcLcb97LcbPlcftxbxTxt);
    this.ReadTxbx(WordSubdocument.HeaderFooter, (int) fib.FibRgFcLcb97FcPlcfHdrtxbxTxt, (int) fib.FibRgFcLcb97LcbPlcfHdrtxbxTxt);
    this.ReadTxbxBkd(WordSubdocument.Main, (int) fib.FibRgFcLcb97FcPlcfTxbxBkd, (int) fib.FibRgFcLcb97LcbPlcfTxbxBkd);
    this.ReadTxbxBkd(WordSubdocument.HeaderFooter, (int) fib.FibRgFcLcb97FcPlcfTxbxHdrBkd, (int) fib.FibRgFcLcb97LcbPlcfTxbxHdrBkd);
  }

  internal void Write(Stream stream, Fib fib, int endMain, int endHeader)
  {
    this.m_stream = stream;
    this.WriteFSPAs(fib, endMain, endHeader);
    this.WriteTxBxs(fib, endMain);
    this.WriteTxBxBKDs(fib);
  }

  internal int GetTxbxPosition(bool isHdrTxbx, int index)
  {
    if (isHdrTxbx && this.HfDocTxBxs != null)
      return index != this.HfDocTxBxs.Count ? this.GetKey(this.HfDocTxBxs, index) : this.GetKey(this.HfDocTxBxs, index - 1) + 3;
    if (this.MainDocTxBxs == null)
      return 0;
    return index != this.MainDocTxBxs.Count ? this.GetKey(this.MainDocTxBxs, index) : this.GetKey(this.MainDocTxBxs, index - 1) + 3;
  }

  internal int GetShapeObjectId(WordSubdocument subDocType, int txbxIndex)
  {
    return (subDocType != WordSubdocument.TextBox ? this.GetByIndex(this.HfDocTxBxs, txbxIndex) : this.GetByIndex(this.MainDocTxBxs, txbxIndex)).ShapeIdent;
  }

  internal override void Close()
  {
    base.Close();
    this.m_stream = (Stream) null;
    if (this.m_fspas != null)
    {
      foreach (TypedSortedListEx<int, FileShapeAddress> typedSortedListEx in (IEnumerable<DocIOSortedList<int, FileShapeAddress>>) this.m_fspas.Values)
        typedSortedListEx.Clear();
      this.m_fspas.Clear();
      this.m_fspas = (DocIOSortedList<WordSubdocument, DocIOSortedList<int, FileShapeAddress>>) null;
    }
    if (this.m_txbxs != null)
    {
      foreach (TypedSortedListEx<int, TextBoxStoryDescriptor> typedSortedListEx in (IEnumerable<DocIOSortedList<int, TextBoxStoryDescriptor>>) this.m_txbxs.Values)
        typedSortedListEx.Clear();
      this.m_txbxs.Clear();
      this.m_txbxs = (DocIOSortedList<WordSubdocument, DocIOSortedList<int, TextBoxStoryDescriptor>>) null;
    }
    if (this.m_txbxBkds == null)
      return;
    foreach (TypedSortedListEx<int, BreakDescriptor> typedSortedListEx in (IEnumerable<DocIOSortedList<int, BreakDescriptor>>) this.m_txbxBkds.Values)
      typedSortedListEx.Clear();
    this.m_txbxBkds.Clear();
    this.m_txbxBkds = (DocIOSortedList<WordSubdocument, DocIOSortedList<int, BreakDescriptor>>) null;
  }

  internal FileShapeAddress FindFileShape(WordSubdocument docType, int CP)
  {
    return this.m_fspas[docType]?[CP];
  }

  private void ReadShapeFSPA(WordSubdocument docType, int pos, int length)
  {
    this.m_stream.Position = (long) pos;
    DocIOSortedList<int, FileShapeAddress> docIoSortedList = new DocIOSortedList<int, FileShapeAddress>();
    this.m_fspas[docType] = docIoSortedList;
    if (length == 0)
      return;
    int[] positions = this.GetPositions(26, length);
    int index1 = 0;
    for (int index2 = positions.Length - 1; index1 < index2; ++index1)
    {
      FileShapeAddress fileShapeAddress = new FileShapeAddress(this.m_stream);
      bool flag = false;
      for (int index3 = 0; index3 < docIoSortedList.Keys.Count; ++index3)
      {
        if (docIoSortedList[docIoSortedList.Keys[index3]].Spid == fileShapeAddress.Spid)
          flag = true;
      }
      if (!flag)
        docIoSortedList.Add(positions[index1], fileShapeAddress);
    }
  }

  private void ReadTxbx(WordSubdocument docType, int pos, int length)
  {
    if (length <= 0)
      return;
    DocIOSortedList<int, TextBoxStoryDescriptor> docIoSortedList = new DocIOSortedList<int, TextBoxStoryDescriptor>();
    this.m_txbxs[docType] = docIoSortedList;
    this.m_stream.Position = (long) pos;
    int[] positions = this.GetPositions(TextBoxStoryDescriptor.DEF_TXBX_LENGTH, length);
    for (int index = 0; index < positions.Length - 1; ++index)
    {
      TextBoxStoryDescriptor boxStoryDescriptor = new TextBoxStoryDescriptor(this.m_stream);
      docIoSortedList.Add(positions[index], boxStoryDescriptor);
    }
  }

  private void ReadTxbxBkd(WordSubdocument docType, int pos, int length)
  {
    if (length <= 0)
      return;
    DocIOSortedList<int, BreakDescriptor> docIoSortedList = new DocIOSortedList<int, BreakDescriptor>();
    this.m_txbxBkds[docType] = docIoSortedList;
    this.m_stream.Position = (long) pos;
    int[] positions = this.GetPositions(6, length);
    for (int index = 0; index < positions.Length - 1; ++index)
    {
      BreakDescriptor breakDescriptor = new BreakDescriptor(this.m_stream);
      docIoSortedList.Add(positions[index], breakDescriptor);
    }
  }

  private int[] GetPositions(int structSize, int length)
  {
    int length1 = (length - 4) / (structSize + 4) + 1;
    int[] positions = new int[length1];
    for (int index = 0; index < length1; ++index)
      positions[index] = (int) BaseWordRecord.ReadUInt32(this.m_stream);
    return positions;
  }

  private void WriteFSPAs(Fib fib, int endMain, int endHeader)
  {
    if (this.MainDocFSPAs == null && this.HfDocFSPAs == null)
      return;
    if (this.MainDocFSPAs != null)
    {
      fib.FibRgFcLcb97FcPlcSpaMom = (uint) this.m_stream.Position;
      this.WriteArtObjectsFSPAs(this.MainDocFSPAs, endMain);
      fib.FibRgFcLcb97LcbPlcSpaMom = (uint) ((ulong) this.m_stream.Position - (ulong) fib.FibRgFcLcb97FcPlcSpaMom);
    }
    if (this.HfDocFSPAs == null)
      return;
    fib.FibRgFcLcb97FcPlcSpaHdr = (uint) this.m_stream.Position;
    this.WriteArtObjectsFSPAs(this.HfDocFSPAs, endHeader);
    fib.FibRgFcLcb97LcbPlcSpaHdr = (uint) ((ulong) this.m_stream.Position - (ulong) fib.FibRgFcLcb97FcPlcSpaHdr);
  }

  private void WriteTxBxs(Fib fib, int endCharacter)
  {
    if (this.MainDocTxBxs == null && this.HfDocTxBxs == null)
      return;
    if (this.MainDocTxBxs != null)
    {
      fib.FibRgFcLcb97FcPlcftxbxTxt = (uint) this.m_stream.Position;
      this.WriteArtObjectsTxBxs(this.MainDocTxBxs, endCharacter);
      fib.FibRgFcLcb97LcbPlcftxbxTxt = (uint) ((ulong) this.m_stream.Position - (ulong) fib.FibRgFcLcb97FcPlcftxbxTxt);
    }
    if (this.HfDocTxBxs == null)
      return;
    fib.FibRgFcLcb97FcPlcfHdrtxbxTxt = (uint) this.m_stream.Position;
    this.WriteArtObjectsTxBxs(this.HfDocTxBxs, endCharacter);
    fib.FibRgFcLcb97LcbPlcfHdrtxbxTxt = (uint) ((ulong) this.m_stream.Position - (ulong) fib.FibRgFcLcb97FcPlcfHdrtxbxTxt);
  }

  private void WriteTxBxBKDs(Fib fib)
  {
    if (this.MainDocTxBxBKDs == null && this.HfDocTxBxBKDs == null)
      return;
    if (this.MainDocTxBxBKDs != null)
    {
      fib.FibRgFcLcb97FcPlcfTxbxBkd = (uint) this.m_stream.Position;
      this.WriteArtObjectsTxBxBKDs(this.MainDocTxBxBKDs, this.m_txBxMainEndPos);
      fib.FibRgFcLcb97LcbPlcfTxbxBkd = (uint) ((ulong) this.m_stream.Position - (ulong) fib.FibRgFcLcb97FcPlcfTxbxBkd);
    }
    if (this.HfDocTxBxBKDs == null)
      return;
    fib.FibRgFcLcb97FcPlcfTxbxHdrBkd = (uint) this.m_stream.Position;
    this.WriteArtObjectsTxBxBKDs(this.HfDocTxBxBKDs, this.m_txBxHeaderEndPos);
    fib.FibRgFcLcb97LcbPlcfTxbxHdrBkd = (uint) ((ulong) this.m_stream.Position - (ulong) fib.FibRgFcLcb97FcPlcfTxbxHdrBkd);
  }

  private void WriteArtObjectsTxBxBKDs(DocIOSortedList<int, BreakDescriptor> stList, int endPos)
  {
    foreach (int key in (IEnumerable<int>) stList.Keys)
      BaseWordRecord.WriteInt32(this.m_stream, key);
    BaseWordRecord.WriteInt32(this.m_stream, endPos);
    foreach (BreakDescriptor breakDescriptor in (IEnumerable<BreakDescriptor>) stList.Values)
      breakDescriptor.Write(this.m_stream);
  }

  private void WriteArtObjectsTxBxs(
    DocIOSortedList<int, TextBoxStoryDescriptor> stList,
    int endPos)
  {
    foreach (int key in (IEnumerable<int>) stList.Keys)
      BaseWordRecord.WriteInt32(this.m_stream, key);
    BaseWordRecord.WriteInt32(this.m_stream, endPos);
    foreach (TextBoxStoryDescriptor boxStoryDescriptor in (IEnumerable<TextBoxStoryDescriptor>) stList.Values)
      boxStoryDescriptor.Write(this.m_stream);
  }

  private void WriteArtObjectsFSPAs(DocIOSortedList<int, FileShapeAddress> stList, int endPos)
  {
    foreach (int key in (IEnumerable<int>) stList.Keys)
      BaseWordRecord.WriteInt32(this.m_stream, key);
    BaseWordRecord.WriteInt32(this.m_stream, endPos);
    foreach (FileShapeAddress fileShapeAddress in (IEnumerable<FileShapeAddress>) stList.Values)
      fileShapeAddress.Write(this.m_stream);
  }

  private TextBoxStoryDescriptor GetByIndex(
    DocIOSortedList<int, TextBoxStoryDescriptor> col,
    int index)
  {
    return index > col.Count - 1 || index < 0 ? (TextBoxStoryDescriptor) null : col.Values[index];
  }

  private int GetKey(DocIOSortedList<int, TextBoxStoryDescriptor> col, int index)
  {
    return index > col.Count - 1 || index < 0 ? -1 : col.Keys[index];
  }
}
