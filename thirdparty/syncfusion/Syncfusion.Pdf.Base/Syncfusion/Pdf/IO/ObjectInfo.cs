// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.IO.ObjectInfo
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;

#nullable disable
namespace Syncfusion.Pdf.IO;

internal class ObjectInfo
{
  private bool m_bModified;
  private IPdfPrimitive m_object;
  private PdfReference m_reference;

  internal bool Modified
  {
    get
    {
      bool bModified = this.m_bModified;
      if (this.Object is IPdfChangable pdfChangable)
        bModified |= pdfChangable.Changed;
      return bModified;
    }
  }

  internal PdfReference Reference => this.m_reference;

  internal IPdfPrimitive Object
  {
    get => this.m_object;
    set => this.m_object = value != null ? value : throw new ArgumentNullException(nameof (Object));
  }

  internal ObjectInfo(IPdfPrimitive obj)
  {
    this.m_object = obj != null ? obj : throw new ArgumentNullException(nameof (obj));
    this.m_bModified = true;
  }

  internal ObjectInfo(IPdfPrimitive obj, PdfReference reference)
  {
    if (obj == null)
      throw new ArgumentNullException(nameof (obj));
    if (reference == (PdfReference) null)
      throw new ArgumentNullException(nameof (reference));
    this.m_object = obj;
    this.m_reference = reference;
  }

  public void SetModified() => this.m_bModified = true;

  internal void SetReference(PdfReference reference)
  {
    if (reference == (PdfReference) null)
      throw new ArgumentNullException(nameof (reference));
    this.m_reference = !(this.m_reference != (PdfReference) null) ? reference : throw new ArgumentException("The object has the reference bound to it.", nameof (reference));
  }

  public override string ToString()
  {
    return $"{(this.m_reference != (PdfReference) null ? this.m_reference.ToString() : string.Empty)} : {this.Object.GetType().Name}";
  }

  public static bool operator ==(ObjectInfo oi, object obj)
  {
    bool flag = false;
    if (oi != (object) null)
      flag = oi.Equals(obj);
    return flag;
  }

  public static bool operator !=(ObjectInfo oi, object obj) => !(oi == obj);

  public override bool Equals(object obj)
  {
    bool flag = false;
    if (obj != null)
    {
      IPdfPrimitive pdfPrimitive = obj as IPdfPrimitive;
      ObjectInfo objectInfo = obj as ObjectInfo;
      if (pdfPrimitive != null)
        flag = this.Object == pdfPrimitive;
      else if (objectInfo != (object) null)
        flag = objectInfo.Object == this.Object;
    }
    return flag;
  }
}
