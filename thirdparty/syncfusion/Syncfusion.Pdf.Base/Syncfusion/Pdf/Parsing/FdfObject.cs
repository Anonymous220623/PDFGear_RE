// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.FdfObject
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class FdfObject
{
  private int m_objNumber;
  private int m_genNumber;
  private IPdfPrimitive m_object;
  private IPdfPrimitive m_trailer;

  internal int ObjectNumber => this.m_objNumber;

  internal int GenerationNumber => this.m_genNumber;

  internal IPdfPrimitive Object => this.m_object;

  internal IPdfPrimitive Trailer => this.m_trailer;

  internal FdfObject(PdfNumber objNum, PdfNumber genNum, IPdfPrimitive obj)
  {
    this.m_objNumber = objNum.IntValue;
    this.m_genNumber = genNum.IntValue;
    this.m_object = obj;
  }

  internal FdfObject(IPdfPrimitive obj) => this.m_trailer = obj;
}
