// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.AnnotationsRW
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter;

[CLSCompliant(false)]
internal class AnnotationsRW : SubDocumentRW
{
  private List<string> m_grpXstAtnOwners;
  private AnnotationsRW.AnnotationBookmarks m_bookmarks;
  private AnnotationDescriptor m_currDescriptor;
  private int m_descIndex = -1;

  internal AnnotationsRW(Stream stream, Fib fib)
    : base(stream, fib)
  {
  }

  internal AnnotationsRW() => this.m_bookmarks = new AnnotationsRW.AnnotationBookmarks();

  internal override void Read(Stream stream, Fib fib)
  {
    base.Read(stream, fib);
    this.ReadGXAO((int) fib.FibRgFcLcb97FcGrpXstAtnOwners, (int) fib.FibRgFcLcb97LcbGrpXstAtnOwners);
    this.m_bookmarks = new AnnotationsRW.AnnotationBookmarks(this.m_reader, fib);
  }

  internal override void Write(Stream stream, Fib fib)
  {
    base.Write(stream, fib);
    this.WriteGXAO();
    this.m_bookmarks.Write(this.m_writer, fib);
  }

  internal void AddDescriptor(AnnotationDescriptor atrd, int pos, int bkmkStart, int bkmkEnd)
  {
    this.AddDescriptor(atrd, pos);
    int tagBkmk = atrd.TagBkmk;
    if (tagBkmk == -1)
      return;
    this.m_bookmarks.Add(tagBkmk, new AnnotationsRW.AnnotationBookmark(bkmkStart, bkmkEnd));
  }

  internal void AddDescriptor(AnnotationDescriptor atrd, int pos)
  {
    this.m_descriptorsAnnot.Add(atrd);
    this.AddRefPosition(pos);
  }

  internal int AddGXAO(string gxao)
  {
    int num = this.m_grpXstAtnOwners.IndexOf(gxao);
    if (num == -1)
    {
      num = this.m_grpXstAtnOwners.Count;
      this.m_grpXstAtnOwners.Add(gxao);
    }
    return num;
  }

  internal AnnotationDescriptor GetDescriptor(int index)
  {
    if (index != this.m_descIndex && index < this.m_descriptorsAnnot.Count)
    {
      this.m_currDescriptor = this.m_descriptorsAnnot[index];
      this.m_descIndex = index;
    }
    return this.m_currDescriptor;
  }

  internal string GetUser(int index)
  {
    AnnotationDescriptor descriptor = this.GetDescriptor(index);
    return descriptor == null || (int) descriptor.IndexToGrpOwner >= this.m_grpXstAtnOwners.Count ? "" : this.m_grpXstAtnOwners[(int) descriptor.IndexToGrpOwner].ToString();
  }

  internal int GetBookmarkStartOffset(int index)
  {
    AnnotationDescriptor descriptor = this.GetDescriptor(index);
    if (descriptor.TagBkmk == -1)
      return 0;
    AnnotationsRW.AnnotationBookmark bookmark = this.m_bookmarks[descriptor.TagBkmk];
    return bookmark == null ? 0 : this.m_refPositions[index] - bookmark.Start;
  }

  internal int GetBookmarkEndOffset(int index)
  {
    AnnotationDescriptor descriptor = this.GetDescriptor(index);
    if (descriptor.TagBkmk == -1)
      return 0;
    AnnotationsRW.AnnotationBookmark bookmark = this.m_bookmarks[descriptor.TagBkmk];
    return bookmark == null ? 0 : bookmark.End - this.m_refPositions[index];
  }

  internal int GetPosition(int index) => this.m_refPositions[index];

  private void ReadGXAO(int pos, int length)
  {
    if (length <= 0)
      return;
    this.m_reader.BaseStream.Position = (long) pos;
    for (int index = (int) this.m_reader.ReadInt16(); index != 0; index = this.m_reader.BaseStream.Position != (long) (pos + length) ? (int) this.m_reader.ReadInt16() : 0)
      this.AddGXAO(Encoding.Unicode.GetString(this.m_reader.ReadBytes(index * 2)));
  }

  private void WriteGXAO()
  {
    if (this.m_grpXstAtnOwners.Count <= 0)
      return;
    this.m_fib.FibRgFcLcb97FcGrpXstAtnOwners = (uint) this.m_writer.BaseStream.Position;
    int index = 0;
    for (int count = this.m_grpXstAtnOwners.Count; index < count; ++index)
    {
      string grpXstAtnOwner = this.m_grpXstAtnOwners[index];
      this.m_writer.Write((short) grpXstAtnOwner.Length);
      this.m_writer.Write(Encoding.Unicode.GetBytes(grpXstAtnOwner));
    }
    this.m_fib.FibRgFcLcb97LcbGrpXstAtnOwners = (uint) ((ulong) this.m_writer.BaseStream.Position - (ulong) this.m_fib.FibRgFcLcb97FcGrpXstAtnOwners);
  }

  internal override void Close()
  {
    base.Close();
    if (this.m_grpXstAtnOwners != null)
    {
      this.m_grpXstAtnOwners.Clear();
      this.m_grpXstAtnOwners = (List<string>) null;
    }
    if (this.m_bookmarks != null)
      this.m_bookmarks = (AnnotationsRW.AnnotationBookmarks) null;
    if (this.m_currDescriptor == null)
      return;
    this.m_currDescriptor = (AnnotationDescriptor) null;
  }

  protected override void Init()
  {
    base.Init();
    this.m_grpXstAtnOwners = new List<string>();
  }

  protected override void ReadTxtPositions()
  {
    int lcb97LcbPlcfandTxt = (int) this.m_fib.FibRgFcLcb97LcbPlcfandTxt;
    if (lcb97LcbPlcfandTxt <= 0)
      return;
    this.m_reader.BaseStream.Position = (long) this.m_fib.FibRgFcLcb97FcPlcfandTxt;
    this.ReadTxtPositions(lcb97LcbPlcfandTxt / 4);
  }

  protected override void ReadDescriptors()
  {
    int lcb97lcbPlcfandRef = (int) this.m_fib.FibRgFcLcb97lcbPlcfandRef;
    if (lcb97lcbPlcfandRef <= 0)
      return;
    this.m_reader.BaseStream.Position = (long) this.m_fib.FibRgFcLcb97FcPlcfandRef;
    this.ReadDescriptors(lcb97lcbPlcfandRef, 30);
    base.ReadDescriptors();
  }

  protected override void ReadDescriptor(BinaryReader reader, int pos, int posNext)
  {
    if (reader.BaseStream.Position >= reader.BaseStream.Length)
      return;
    base.ReadDescriptor(reader, pos, posNext);
    this.m_descriptorsAnnot.Add(new AnnotationDescriptor(reader));
  }

  protected override void WriteDescriptors()
  {
    this.m_fib.FibRgFcLcb97FcPlcfandRef = (uint) this.m_writer.BaseStream.Position;
    this.WriteRefPositions(this.m_endReference);
    int index = 0;
    for (int count = this.m_descriptorsAnnot.Count; index < count; ++index)
      this.m_descriptorsAnnot[index].Write(this.m_writer);
    this.m_fib.FibRgFcLcb97lcbPlcfandRef = (uint) ((ulong) this.m_writer.BaseStream.Position - (ulong) this.m_fib.FibRgFcLcb97FcPlcfandRef);
  }

  protected override void WriteTxtPositions()
  {
    if (this.m_txtPositions.Count <= 0)
      return;
    this.m_fib.FibRgFcLcb97FcPlcfandTxt = (uint) this.m_writer.BaseStream.Position;
    this.WriteTxtPositionsBase();
    this.m_fib.FibRgFcLcb97LcbPlcfandTxt = (uint) ((ulong) this.m_writer.BaseStream.Position - (ulong) this.m_fib.FibRgFcLcb97FcPlcfandTxt);
  }

  internal class AnnotationBookmark
  {
    private int m_iStartPos = -1;
    private int m_iEndPos = -1;

    internal int Start
    {
      get => this.m_iStartPos;
      set => this.m_iStartPos = value;
    }

    internal int End
    {
      get => this.m_iEndPos;
      set => this.m_iEndPos = value;
    }

    internal AnnotationBookmark(int start, int end)
    {
      this.m_iEndPos = end;
      this.m_iStartPos = start;
      if (end >= start)
        return;
      end = start;
      this.m_iEndPos = end;
    }
  }

  internal class AnnotationBookmarks
  {
    private BookmarkDescriptor m_descriptor;
    private List<int> m_keys = new List<int>();
    private int m_bookmarkCount;

    internal AnnotationsRW.AnnotationBookmark this[int key]
    {
      get
      {
        if (!this.m_keys.Contains(key))
          return (AnnotationsRW.AnnotationBookmark) null;
        int i = this.m_keys.IndexOf(key);
        return new AnnotationsRW.AnnotationBookmark(this.m_descriptor.GetBeginPos(i), this.m_descriptor.GetEndPos(i));
      }
    }

    internal AnnotationBookmarks(BinaryReader reader, Fib fib) => this.Read(reader, fib);

    internal AnnotationBookmarks() => this.m_descriptor = new BookmarkDescriptor();

    internal void Read(BinaryReader reader, Fib fib)
    {
      this.ReadSttbf(reader, (int) fib.FibRgFcLcb97FcSttbfAtnBkmk, (int) fib.FibRgFcLcb97LcbSttbfAtnBkmk);
      if (this.m_bookmarkCount <= 0)
        return;
      this.m_descriptor = new BookmarkDescriptor(reader.BaseStream, this.m_bookmarkCount, (int) fib.FibRgFcLcb97FcPlcfAtnBkf, (int) fib.FibRgFcLcb97LcbPlcfAtnBkf, (int) fib.FibRgFcLcb97FcPlcfAtnBkl, (int) fib.FibRgFcLcb97LcbPlcfAtnBkl);
    }

    internal void Add(int key, AnnotationsRW.AnnotationBookmark bookmark)
    {
      this.m_keys.Add(key);
      this.m_descriptor.Add(bookmark.Start);
      this.m_descriptor.SetEndPos(this.m_descriptor.BookmarkCount - 1, bookmark.End);
    }

    internal void Write(BinaryWriter writer, Fib fib)
    {
      if (this.m_descriptor.BookmarkCount == 0)
        return;
      int bookmarkCount = this.m_descriptor.BookmarkCount;
      int[] keys = new int[bookmarkCount];
      int[] numArray = new int[bookmarkCount];
      for (int index = 0; index < this.m_descriptor.BookmarkCount; ++index)
      {
        keys[index] = this.m_descriptor.GetBeginPos(index);
        numArray[index] = this.m_keys[index];
      }
      Array.Sort((Array) keys, (Array) numArray, (IComparer) new AnnotationsRW.AnnotationBookmarks.Comparer());
      fib.FibRgFcLcb97FcSttbfAtnBkmk = (uint) writer.BaseStream.Position;
      this.WriteSttbf(writer, numArray);
      fib.FibRgFcLcb97LcbSttbfAtnBkmk = (uint) ((ulong) writer.BaseStream.Position - (ulong) fib.FibRgFcLcb97FcSttbfAtnBkmk);
      int end = fib.CcpText + 2;
      fib.FibRgFcLcb97FcPlcfAtnBkf = (uint) writer.BaseStream.Position;
      this.WriteBKF(writer, keys, numArray, end);
      fib.FibRgFcLcb97LcbPlcfAtnBkf = (uint) ((ulong) writer.BaseStream.Position - (ulong) fib.FibRgFcLcb97FcPlcfAtnBkf);
      fib.FibRgFcLcb97FcPlcfAtnBkl = (uint) writer.BaseStream.Position;
      this.WriteBKL(writer, end);
      fib.FibRgFcLcb97LcbPlcfAtnBkl = (uint) ((ulong) writer.BaseStream.Position - (ulong) fib.FibRgFcLcb97FcPlcfAtnBkl);
    }

    internal void Close()
    {
      if (this.m_descriptor != null)
        this.m_descriptor = (BookmarkDescriptor) null;
      if (this.m_keys == null)
        return;
      this.m_keys.Clear();
      this.m_keys = (List<int>) null;
    }

    private void ReadSttbf(BinaryReader reader, int start, int length)
    {
      if (length <= 0)
        return;
      reader.BaseStream.Position = (long) (start + 2);
      this.m_bookmarkCount = (int) reader.ReadInt16();
      int num = (int) reader.ReadInt16();
      for (int index = 0; index < this.m_bookmarkCount; ++index)
      {
        reader.ReadInt32();
        this.m_keys.Add(reader.ReadInt32());
        reader.ReadInt32();
      }
    }

    private void WriteSttbf(BinaryWriter writer, int[] values)
    {
      writer.Write((short) -1);
      writer.Write((short) this.m_descriptor.BookmarkCount);
      writer.Write((short) 10);
      for (int index = 0; index < values.Length; ++index)
      {
        writer.Write(16777216 /*0x01000000*/);
        writer.Write(values[index]);
        writer.Write(-1);
      }
    }

    private void WriteBKF(BinaryWriter writer, int[] keys, int[] values, int end)
    {
      int[] numArray = new int[keys.Length];
      int index1 = 0;
      for (int index2 = 0; index2 < keys.Length; ++index2)
      {
        numArray[index1] = this.m_keys.IndexOf(values[index2]);
        writer.Write(keys[index2]);
        ++index1;
      }
      writer.Write(end);
      for (int index3 = 0; index3 < keys.Length; ++index3)
        writer.Write(numArray[index3]);
    }

    private void WriteBKL(BinaryWriter writer, int end)
    {
      for (int i = 0; i < this.m_descriptor.BookmarkCount; ++i)
        writer.Write(this.m_descriptor.GetEndPos(i));
      writer.Write(end);
    }

    internal class Comparer : IComparer
    {
      public int Compare(object x, object y)
      {
        int num1 = (int) x;
        int num2 = (int) y;
        if (num1 > num2)
          return 1;
        return num1 != num2 ? -1 : 0;
      }
    }
  }
}
