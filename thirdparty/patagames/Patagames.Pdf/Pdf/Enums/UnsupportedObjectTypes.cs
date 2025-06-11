// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.UnsupportedObjectTypes
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>flags for type of unsupport object.</summary>
public enum UnsupportedObjectTypes
{
  FPDF_UNSP_DOC_XFAFORM = 1,
  FPDF_UNSP_DOC_PORTABLECOLLECTION = 2,
  FPDF_UNSP_DOC_ATTACHMENT = 3,
  FPDF_UNSP_DOC_SECURITY = 4,
  FPDF_UNSP_DOC_SHAREDREVIEW = 5,
  FPDF_UNSP_DOC_SHAREDFORM_ACROBAT = 6,
  FPDF_UNSP_DOC_SHAREDFORM_FILESYSTEM = 7,
  FPDF_UNSP_DOC_SHAREDFORM_EMAIL = 8,
  FPDF_UNSP_ANNOT_3DANNOT = 11, // 0x0000000B
  FPDF_UNSP_ANNOT_MOVIE = 12, // 0x0000000C
  FPDF_UNSP_ANNOT_SOUND = 13, // 0x0000000D
  FPDF_UNSP_ANNOT_SCREEN_MEDIA = 14, // 0x0000000E
  FPDF_UNSP_ANNOT_SCREEN_RICHMEDIA = 15, // 0x0000000F
  FPDF_UNSP_ANNOT_ATTACHMENT = 16, // 0x00000010
  FPDF_UNSP_ANNOT_SIG = 17, // 0x00000011
}
