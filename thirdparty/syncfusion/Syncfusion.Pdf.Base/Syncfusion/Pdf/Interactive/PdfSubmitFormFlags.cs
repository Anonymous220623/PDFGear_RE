// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfSubmitFormFlags
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

[Flags]
public enum PdfSubmitFormFlags
{
  IncludeExclude = 1,
  IncludeNoValueFields = 2,
  ExportFormat = 4,
  GetMethod = 8,
  SubmitCoordinates = 16, // 0x00000010
  Xfdf = 32, // 0x00000020
  IncludeAppendSaves = 64, // 0x00000040
  IncludeAnnotations = 128, // 0x00000080
  SubmitPdf = 256, // 0x00000100
  CanonicalFormat = 512, // 0x00000200
  ExclNonUserAnnots = 1024, // 0x00000400
  ExclFKey = 2048, // 0x00000800
  EmbedForm = 4096, // 0x00001000
}
