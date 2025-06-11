// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Primitives.PdfReference
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.IO;
using System;
using System.Globalization;

#nullable disable
namespace Syncfusion.Pdf.Primitives;

internal class PdfReference : IPdfPrimitive
{
  public readonly long ObjNum;
  public readonly int GenNum;
  private ObjectStatus m_status;
  private bool m_isSaving;
  private int m_index;
  private int m_position = -1;

  public PdfReference(long objNum, int genNum)
  {
    this.ObjNum = objNum;
    this.GenNum = genNum;
  }

  public PdfReference(string objNum, string genNum)
  {
    double result1 = 0.0;
    double result2 = 0.0;
    if (!double.TryParse(objNum, NumberStyles.Integer, (IFormatProvider) CultureInfo.InvariantCulture, out result1))
      throw new ArgumentException("Invalid format (must be an integer)", nameof (objNum));
    if (!double.TryParse(genNum, NumberStyles.Integer, (IFormatProvider) CultureInfo.InvariantCulture, out result2))
      throw new ArgumentException("Invalid format (must be an integer)", nameof (genNum));
    this.ObjNum = (long) (int) result1;
    this.GenNum = (int) result2;
  }

  public ObjectStatus Status
  {
    get => this.m_status;
    set => this.m_status = value;
  }

  public bool IsSaving
  {
    get => this.m_isSaving;
    set => this.m_isSaving = value;
  }

  public int ObjectCollectionIndex
  {
    get => this.m_index;
    set => this.m_index = value;
  }

  public int Position
  {
    get => this.m_position;
    set => this.m_position = value;
  }

  public IPdfPrimitive ClonedObject => (IPdfPrimitive) null;

  public override string ToString() => $"{this.ObjNum} {this.GenNum} R";

  public override bool Equals(object obj)
  {
    PdfReference pdfReference = obj as PdfReference;
    return !(pdfReference == (PdfReference) null) && pdfReference.ObjNum == this.ObjNum && pdfReference.GenNum == this.GenNum;
  }

  public override int GetHashCode() => (int) (this.ObjNum + (long) this.GenNum << 24);

  IPdfPrimitive IPdfPrimitive.Clone(PdfCrossTable crossTable) => (IPdfPrimitive) null;

  public static bool operator ==(PdfReference ref1, PdfReference ref2)
  {
    object obj1 = (object) ref1;
    object obj2 = (object) ref2;
    if (obj1 == null || obj2 == null)
      return obj1 == obj2;
    return ref1.ObjNum == ref2.ObjNum && ref1.GenNum == ref2.GenNum;
  }

  public static bool operator !=(PdfReference ref1, PdfReference ref2) => !(ref1 == ref2);

  public void Save(IPdfWriter writer) => writer.Write(this.ToString());
}
