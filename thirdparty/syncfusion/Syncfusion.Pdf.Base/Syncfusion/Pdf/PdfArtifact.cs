// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfArtifact
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.IO;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf;

public class PdfArtifact : PdfTag
{
  private RectangleF m_boundingBox;
  private PdfArtifactType m_artifactType;
  private PdfArtifactSubType m_subType;
  private PdfAttached m_attached;

  public PdfArtifact()
  {
    this.m_boundingBox = new RectangleF();
    this.m_artifactType = PdfArtifactType.None;
    this.m_subType = PdfArtifactSubType.None;
    if (PdfCatalog.StructTreeRoot != null)
      return;
    PdfCatalog.m_structTreeRoot = new PdfStructTreeRoot();
  }

  public PdfArtifact(PdfArtifactType type)
    : this()
  {
    this.m_artifactType = type;
  }

  public PdfArtifact(PdfArtifactType type, PdfAttached attached)
    : this()
  {
    this.m_artifactType = type;
    this.m_attached = attached;
  }

  public PdfArtifact(PdfArtifactType type, PdfAttached attached, PdfArtifactSubType subType)
    : this()
  {
    this.m_artifactType = type;
    this.m_attached = attached;
    this.m_subType = subType;
  }

  public PdfArtifact(
    PdfArtifactType type,
    RectangleF bBox,
    PdfAttached attached,
    PdfArtifactSubType subType)
    : this()
  {
    this.m_artifactType = type;
    this.m_boundingBox = bBox;
    this.m_attached = attached;
    this.m_subType = subType;
  }

  public RectangleF BoundingBox
  {
    get => this.m_boundingBox;
    set => this.m_boundingBox = value;
  }

  public PdfArtifactType ArtifactType
  {
    get => this.m_artifactType;
    set => this.m_artifactType = value;
  }

  public PdfArtifactSubType SubType
  {
    get => this.m_subType;
    set => this.m_subType = value;
  }

  public PdfAttached Attached
  {
    get => this.m_attached;
    set => this.m_attached = value;
  }
}
