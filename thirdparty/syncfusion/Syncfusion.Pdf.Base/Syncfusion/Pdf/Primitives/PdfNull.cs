// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Primitives.PdfNull
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.IO;

#nullable disable
namespace Syncfusion.Pdf.Primitives;

internal class PdfNull : IPdfPrimitive
{
  private ObjectStatus m_status;
  private bool m_isSaving;
  private int m_index;
  private int m_position = -1;

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

  public void Save(IPdfWriter writer) => writer.Write("null");

  public IPdfPrimitive Clone(PdfCrossTable crossTable) => (IPdfPrimitive) new PdfNull();
}
