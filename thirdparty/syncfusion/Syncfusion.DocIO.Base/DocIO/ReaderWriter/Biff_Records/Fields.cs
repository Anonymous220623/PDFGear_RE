// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.Fields
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records;

[CLSCompliant(false)]
internal class Fields : BaseWordRecord
{
  internal int DEF_FLD_SIZE = 2;
  private BinaryReader m_reader;
  private DocIOSortedList<int, FieldDescriptor> m_curList;
  private Dictionary<WordSubdocument, DocIOSortedList<int, FieldDescriptor>> m_fieldsList = new Dictionary<WordSubdocument, DocIOSortedList<int, FieldDescriptor>>();

  internal DocIOSortedList<int, FieldDescriptor> MainFields
  {
    get
    {
      return this.m_fieldsList.ContainsKey(WordSubdocument.Main) ? this.m_fieldsList[WordSubdocument.Main] : (DocIOSortedList<int, FieldDescriptor>) null;
    }
  }

  internal DocIOSortedList<int, FieldDescriptor> HFFields
  {
    get
    {
      return this.m_fieldsList.ContainsKey(WordSubdocument.HeaderFooter) ? this.m_fieldsList[WordSubdocument.HeaderFooter] : (DocIOSortedList<int, FieldDescriptor>) null;
    }
  }

  internal DocIOSortedList<int, FieldDescriptor> FtnFields
  {
    get
    {
      return this.m_fieldsList.ContainsKey(WordSubdocument.Footnote) ? this.m_fieldsList[WordSubdocument.Footnote] : (DocIOSortedList<int, FieldDescriptor>) null;
    }
  }

  internal DocIOSortedList<int, FieldDescriptor> AtnFields
  {
    get
    {
      return this.m_fieldsList.ContainsKey(WordSubdocument.Annotation) ? this.m_fieldsList[WordSubdocument.Annotation] : (DocIOSortedList<int, FieldDescriptor>) null;
    }
  }

  internal DocIOSortedList<int, FieldDescriptor> EdnFields
  {
    get
    {
      return this.m_fieldsList.ContainsKey(WordSubdocument.Endnote) ? this.m_fieldsList[WordSubdocument.Endnote] : (DocIOSortedList<int, FieldDescriptor>) null;
    }
  }

  internal DocIOSortedList<int, FieldDescriptor> TxbxFields
  {
    get
    {
      return this.m_fieldsList.ContainsKey(WordSubdocument.TextBox) ? this.m_fieldsList[WordSubdocument.TextBox] : (DocIOSortedList<int, FieldDescriptor>) null;
    }
  }

  internal DocIOSortedList<int, FieldDescriptor> HdrTxbxFields
  {
    get
    {
      return this.m_fieldsList.ContainsKey(WordSubdocument.HeaderTextBox) ? this.m_fieldsList[WordSubdocument.HeaderTextBox] : (DocIOSortedList<int, FieldDescriptor>) null;
    }
  }

  internal Fields(Fib fib, BinaryReader reader)
  {
    this.m_fieldsList = new Dictionary<WordSubdocument, DocIOSortedList<int, FieldDescriptor>>();
    this.m_reader = reader;
    this.ReadFieldsForSubDoc(WordSubdocument.Main, fib.FibRgFcLcb97FcPlcfFldMom, fib.FibRgFcLcb97LcbPlcfFldMom);
    this.ReadFieldsForSubDoc(WordSubdocument.HeaderFooter, fib.FibRgFcLcb97FcPlcfFldHdr, fib.FibRgFcLcb97LcbPlcfFldHdr);
    this.ReadFieldsForSubDoc(WordSubdocument.Footnote, fib.FibRgFcLcb97FcPlcfFldFtn, fib.FibRgFcLcb97LcbPlcfFldFtn);
    this.ReadFieldsForSubDoc(WordSubdocument.Annotation, fib.FibRgFcLcb97FcPlcfFldAtn, fib.FibRgFcLcb97LcbPlcfFldAtn);
    this.ReadFieldsForSubDoc(WordSubdocument.Endnote, fib.FibRgFcLcb97FcPlcfFldEdn, fib.FibRgFcLcb97LcbPlcfFldEdn);
    this.ReadFieldsForSubDoc(WordSubdocument.TextBox, fib.FibRgFcLcb97FcPlcfFldTxbx, fib.FibRgFcLcb97LcbPlcfFldTxbx);
    this.ReadFieldsForSubDoc(WordSubdocument.HeaderTextBox, fib.FibRgFcLcb97FcPlcffldHdrTxbx, fib.FibRgFcLcb97LcbPlcffldHdrTxbx);
  }

  internal Fields()
  {
  }

  internal void AddField(WordSubdocument docType, FieldDescriptor fld, int pos)
  {
    if (!this.m_fieldsList.ContainsKey(docType))
    {
      DocIOSortedList<int, FieldDescriptor> docIoSortedList = new DocIOSortedList<int, FieldDescriptor>();
      this.m_fieldsList.Add(docType, docIoSortedList);
    }
    this.m_fieldsList[docType].Add(pos, fld);
  }

  internal DocIOSortedList<int, FieldDescriptor> GetFieldsForSubDoc(WordSubdocument type)
  {
    return this.m_fieldsList[type];
  }

  internal void Write(Stream stream, uint endPosition, WordSubdocument subDocument)
  {
    if (this.m_fieldsList.Count <= 0 || !this.m_fieldsList.ContainsKey(subDocument))
      return;
    this.WriteFieldsForSubDocument(this.m_fieldsList[subDocument], stream, endPosition);
  }

  internal override void Close()
  {
    base.Close();
    if (this.m_reader != null)
      this.m_reader.Close();
    if (this.m_curList != null)
    {
      this.m_curList.Clear();
      this.m_curList = (DocIOSortedList<int, FieldDescriptor>) null;
    }
    if (this.m_fieldsList == null)
      return;
    foreach (TypedSortedListEx<int, FieldDescriptor> typedSortedListEx in this.m_fieldsList.Values)
      typedSortedListEx.Clear();
    this.m_fieldsList.Clear();
    this.m_fieldsList = (Dictionary<WordSubdocument, DocIOSortedList<int, FieldDescriptor>>) null;
  }

  internal FieldDescriptor FindFld(WordSubdocument docType, int pos)
  {
    return this.m_fieldsList[docType][pos];
  }

  private void ReadFieldDescriptor(BinaryReader reader, int pos, int posNext)
  {
    FieldDescriptor fieldDescriptor = new FieldDescriptor(reader);
    this.m_curList[pos] = fieldDescriptor;
  }

  private void ReadFieldsForSubDoc(WordSubdocument docType, uint pos, uint length)
  {
    this.m_curList = new DocIOSortedList<int, FieldDescriptor>();
    this.m_fieldsList[docType] = this.m_curList;
    this.m_reader.BaseStream.Position = (long) (int) pos;
    PosStructReader.Read(this.m_reader, (int) length, this.DEF_FLD_SIZE, new PosStructReaderDelegate(this.ReadFieldDescriptor));
  }

  private void WriteFieldsForSubDocument(
    DocIOSortedList<int, FieldDescriptor> stList,
    Stream stream,
    uint endPosition)
  {
    int index = 0;
    for (int count = stList.Count; index < count; ++index)
      BaseWordRecord.WriteInt32(stream, stList.GetKey(index));
    BaseWordRecord.WriteUInt32(stream, endPosition);
    foreach (int key in (IEnumerable<int>) stList.Keys)
      stList[key].Write(stream);
  }
}
