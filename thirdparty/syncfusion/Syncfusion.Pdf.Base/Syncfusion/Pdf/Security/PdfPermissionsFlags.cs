// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.PdfPermissionsFlags
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

[Flags]
public enum PdfPermissionsFlags
{
  Default = 0,
  Print = 4,
  EditContent = 8,
  CopyContent = 16, // 0x00000010
  EditAnnotations = 32, // 0x00000020
  FillFields = 256, // 0x00000100
  AccessibilityCopyContent = 512, // 0x00000200
  AssembleDocument = 1024, // 0x00000400
  FullQualityPrint = 2048, // 0x00000800
}
