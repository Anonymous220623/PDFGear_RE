// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Primitives.IPdfPrimitive
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.IO;

#nullable disable
namespace Syncfusion.Pdf.Primitives;

internal interface IPdfPrimitive
{
  ObjectStatus Status { get; set; }

  bool IsSaving { get; set; }

  int ObjectCollectionIndex { get; set; }

  IPdfPrimitive ClonedObject { get; }

  void Save(IPdfWriter writer);

  IPdfPrimitive Clone(PdfCrossTable crossTable);

  int Position { get; set; }
}
