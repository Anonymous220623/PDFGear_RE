// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfStructureElement
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.IO;
using System;

#nullable disable
namespace Syncfusion.Pdf;

public class PdfStructureElement : PdfTag
{
  private PdfTagType m_tagType;
  private PdfStructureElement m_parent;
  private string m_altText;
  private string m_title;
  private string m_language;
  private string m_abbrevation;
  private string m_actualText;
  internal string m_name;
  private int m_order;
  private bool isAdded;
  private ScopeType m_scope = ScopeType.None;

  public PdfStructureElement()
  {
    this.m_tagType = PdfTagType.None;
    if (PdfCatalog.StructTreeRoot != null)
      return;
    PdfCatalog.m_structTreeRoot = new PdfStructTreeRoot();
  }

  public PdfStructureElement(PdfTagType tag)
    : this()
  {
    this.m_tagType = tag;
    this.m_name = Guid.NewGuid().ToString();
  }

  public string Abbrevation
  {
    get => this.m_abbrevation;
    set => this.m_abbrevation = value;
  }

  public string ActualText
  {
    get => this.m_actualText;
    set => this.m_actualText = value;
  }

  public string AlternateText
  {
    get => this.m_altText;
    set => this.m_altText = value;
  }

  public string Language
  {
    get => this.m_language;
    set => this.m_language = value;
  }

  public override int Order
  {
    get => this.m_order;
    set => this.m_order = value;
  }

  public PdfStructureElement Parent
  {
    get => this.m_parent;
    set => this.m_parent = value;
  }

  public PdfTagType TagType
  {
    get => this.m_tagType;
    set => this.m_tagType = value;
  }

  public string Title
  {
    get => this.m_title;
    set => this.m_title = value;
  }

  internal bool IsAdded
  {
    get => this.isAdded;
    set => this.isAdded = value;
  }

  public ScopeType Scope
  {
    get => this.m_scope;
    set => this.m_scope = value;
  }
}
