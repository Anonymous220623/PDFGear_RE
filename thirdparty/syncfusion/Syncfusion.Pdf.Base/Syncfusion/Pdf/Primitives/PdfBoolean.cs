// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Primitives.PdfBoolean
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.IO;

#nullable disable
namespace Syncfusion.Pdf.Primitives;

internal class PdfBoolean : IPdfPrimitive
{
  private bool m_value;
  private ObjectStatus m_status;
  private bool m_isSaving;
  private int m_index;
  private int m_position = -1;

  public bool Value
  {
    get => this.m_value;
    set => this.m_value = value;
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

  internal PdfBoolean()
  {
  }

  internal PdfBoolean(bool value) => this.m_value = value;

  private string BoolToStr(bool value) => !value ? "false" : "true";

  public IPdfPrimitive Clone(PdfCrossTable crossTable)
  {
    return (IPdfPrimitive) new PdfBoolean(this.m_value);
  }

  public void Save(IPdfWriter writer) => writer.Write(this.BoolToStr(this.m_value));
}
