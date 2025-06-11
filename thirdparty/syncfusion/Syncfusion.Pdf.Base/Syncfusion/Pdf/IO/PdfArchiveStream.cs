// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.IO.PdfArchiveStream
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using Syncfusion.Pdf.Security;
using System;
using System.Collections;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.IO;

internal class PdfArchiveStream : PdfStream
{
  private SortedListEx m_indices;
  private MemoryStream m_objects;
  private StreamWriter m_writer;
  private IPdfWriter m_objectWriter;
  private PdfDocumentBase m_document;

  internal int ObjCount => this.m_indices.Count;

  internal PdfArchiveStream(PdfDocumentBase document)
  {
    this.m_document = document != null ? document : throw new ArgumentNullException(nameof (document));
    this.m_objects = new MemoryStream(1000);
    this.m_objectWriter = (IPdfWriter) new PdfWriter((Stream) this.m_objects);
    this.m_objectWriter.Document = this.m_document;
    this.m_indices = new SortedListEx(16 /*0x10*/);
  }

  public void SaveObject(IPdfPrimitive obj, PdfReference reference)
  {
    this.m_indices[(object) this.m_objectWriter.Position] = (object) reference.ObjNum;
    PdfSecurity security = this.m_document.Security;
    bool enabled = security.Enabled;
    security.Enabled = false;
    lock (PdfDocument.Cache)
      obj.Save(this.m_objectWriter);
    security.Enabled = enabled;
    this.m_objectWriter.Write("\r\n");
  }

  public int GetIndex(long objNum) => this.m_indices.IndexOfValue((object) objNum);

  public override void Save(IPdfWriter writer)
  {
    using (MemoryStream memoryStream = new MemoryStream((int) this.m_objects.Length + 100))
    {
      using (this.m_writer = new StreamWriter((Stream) memoryStream))
      {
        this.SaveIndices();
        this.m_writer.Flush();
        this["First"] = (IPdfPrimitive) new PdfNumber(this.m_writer.BaseStream.Position);
        this.SaveObjects();
        this.m_writer.Flush();
        this.Data = memoryStream.ToArray();
      }
    }
    this["N"] = (IPdfPrimitive) new PdfNumber(this.m_indices.Count);
    this["Type"] = (IPdfPrimitive) new PdfName("ObjStm");
    base.Save(writer);
  }

  internal new void Clear()
  {
    this.m_indices.Clear();
    if (this.m_objects != null)
      this.m_objects.Close();
    if (this.m_writer != null)
      this.m_writer.Close();
    if (this.m_objectWriter != null)
      this.m_objectWriter = (IPdfWriter) null;
    base.Clear();
  }

  private void SaveObjects()
  {
    byte[] array = this.m_objects.ToArray();
    this.m_writer.BaseStream.Write(array, 0, array.Length);
  }

  private void SaveIndices()
  {
    foreach (long key in (IEnumerable) this.m_indices.Keys)
    {
      this.m_writer.Write(this.m_indices[(object) key]);
      this.m_writer.Write(" ");
      this.m_writer.Write(key);
      this.m_writer.Write("\r\n");
    }
  }

  private class ObjInfo
  {
    internal IPdfPrimitive Obj;
    internal int Index;

    internal ObjInfo(IPdfPrimitive obj)
    {
      this.Obj = obj;
      this.Index = 0;
    }
  }
}
