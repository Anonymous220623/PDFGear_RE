// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.DataStreamParser.OLEObject.OleTypeConvertor
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using System;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.DataStreamParser.OLEObject;

internal class OleTypeConvertor
{
  internal static OleObjectType ToOleType(string oleTypeStr)
  {
    oleTypeStr = oleTypeStr.TrimEnd(new char[1]);
    OleObjectType oleType = OleObjectType.Undefined;
    if (OleTypeConvertor.StartsWithExt(oleTypeStr, "Acrobat Document") || OleTypeConvertor.StartsWithExt(oleTypeStr, "AcroExch.Document.7") || OleTypeConvertor.StartsWithExt(oleTypeStr, "AcroExch.Document.11") || OleTypeConvertor.StartsWithExt(oleTypeStr, "AcroExch.Document.DC"))
      oleType = OleObjectType.AdobeAcrobatDocument;
    else if (OleTypeConvertor.StartsWithExt(oleTypeStr, "Package"))
      oleType = OleObjectType.Package;
    else if (OleTypeConvertor.StartsWithExt(oleTypeStr, "PBrush"))
      oleType = OleObjectType.BitmapImage;
    else if (OleTypeConvertor.StartsWithExt(oleTypeStr, "Media Clip") || OleTypeConvertor.StartsWithExt(oleTypeStr, "MPlayer"))
      oleType = OleObjectType.MediaClip;
    else if (OleTypeConvertor.StartsWithExt(oleTypeStr, "Microsoft Equation 3.0") || OleTypeConvertor.StartsWithExt(oleTypeStr, "Equation.3"))
      oleType = OleObjectType.Equation;
    else if (OleTypeConvertor.StartsWithExt(oleTypeStr, "Microsoft Graph Chart") || OleTypeConvertor.StartsWithExt(oleTypeStr, "MSGraph.Chart.8"))
      oleType = OleObjectType.GraphChart;
    else if (oleTypeStr.Contains("Excel 2003 Worksheet") || OleTypeConvertor.StartsWithExt(oleTypeStr, "Excel.Sheet.8"))
      oleType = OleObjectType.Excel_97_2003_Worksheet;
    else if (oleTypeStr.Contains("Excel Binary Worksheet") || OleTypeConvertor.StartsWithExt(oleTypeStr, "Excel.SheetBinaryMacroEnabled.12"))
      oleType = OleObjectType.ExcelBinaryWorksheet;
    else if (oleTypeStr.Contains("Excel Chart") || OleTypeConvertor.StartsWithExt(oleTypeStr, "Excel.Chart.8"))
      oleType = OleObjectType.ExcelChart;
    else if (oleTypeStr.Contains("Excel Worksheet (code)") || OleTypeConvertor.StartsWithExt(oleTypeStr, "Excel.SheetMacroEnabled.12"))
      oleType = OleObjectType.ExcelMacroWorksheet;
    else if (oleTypeStr.Contains("Excel Worksheet") || OleTypeConvertor.StartsWithExt(oleTypeStr, "Excel.Sheet.12"))
      oleType = OleObjectType.ExcelWorksheet;
    else if (oleTypeStr.Contains("PowerPoint 97-2003 Presentation") || OleTypeConvertor.StartsWithExt(oleTypeStr, "PowerPoint.Show.8"))
      oleType = OleObjectType.PowerPoint_97_2003_Presentation;
    else if (oleTypeStr.Contains("PowerPoint 97-2003 Slide") || OleTypeConvertor.StartsWithExt(oleTypeStr, "PowerPoint.Slide.8"))
      oleType = OleObjectType.PowerPoint_97_2003_Slide;
    else if (oleTypeStr.Contains("PowerPoint Macro-Enabled Presentation") || OleTypeConvertor.StartsWithExt(oleTypeStr, "PowerPoint.ShowMacroEnabled.12"))
      oleType = OleObjectType.PowerPointMacroPresentation;
    else if (oleTypeStr.Contains("PowerPoint Macro-Enabled Slide") || OleTypeConvertor.StartsWithExt(oleTypeStr, "PowerPoint.SlideMacroEnabled.12"))
      oleType = OleObjectType.PowerPointMacroSlide;
    else if (oleTypeStr.Contains("PowerPoint Presentation") || OleTypeConvertor.StartsWithExt(oleTypeStr, "PowerPoint.Show.12"))
      oleType = OleObjectType.PowerPointPresentation;
    else if (oleTypeStr.Contains("PowerPoint Slide") || OleTypeConvertor.StartsWithExt(oleTypeStr, "PowerPoint.Slide.12"))
      oleType = OleObjectType.PowerPointSlide;
    else if (oleTypeStr.Contains("Word 97-2003 Document") || OleTypeConvertor.StartsWithExt(oleTypeStr, "Word.Document.8"))
      oleType = OleObjectType.Word_97_2003_Document;
    else if (oleTypeStr.Contains("Word Document") || OleTypeConvertor.StartsWithExt(oleTypeStr, "Word.Document.12"))
      oleType = OleObjectType.WordDocument;
    else if (oleTypeStr.Contains("Word Macro-Enabled Document") || OleTypeConvertor.StartsWithExt(oleTypeStr, "Word.DocumentMacroEnabled.12"))
      oleType = OleObjectType.WordMacroDocument;
    else if (OleTypeConvertor.StartsWithExt(oleTypeStr, "Microsoft Visio Drawing") || OleTypeConvertor.StartsWithExt(oleTypeStr, "Visio.Drawing.11"))
      oleType = OleObjectType.VisioDrawing;
    else if (OleTypeConvertor.StartsWithExt(oleTypeStr, "OpenDocument Presentation") || OleTypeConvertor.StartsWithExt(oleTypeStr, "PowerPoint.OpenDocumentPresentation.12"))
      oleType = OleObjectType.OpenDocumentPresentation;
    else if (OleTypeConvertor.StartsWithExt(oleTypeStr, "OpenDocument Spreadsheet") || OleTypeConvertor.StartsWithExt(oleTypeStr, "Excel.OpenDocumentSpreadsheet.12"))
      oleType = OleObjectType.OpenDocumentSpreadsheet;
    else if (OleTypeConvertor.StartsWithExt(oleTypeStr, "opendocument.CalcDocument.1"))
      oleType = OleObjectType.OpenOfficeSpreadsheet;
    else if (OleTypeConvertor.StartsWithExt(oleTypeStr, "opendocument.WriterDocument.1"))
      oleType = OleObjectType.OpenOfficeText;
    else if (OleTypeConvertor.StartsWithExt(oleTypeStr, "soffice.StarCalcDocument.6"))
      oleType = OleObjectType.OpenOfficeSpreadsheet1_1;
    else if (OleTypeConvertor.StartsWithExt(oleTypeStr, "soffice.StarWriterDocument.6"))
      oleType = OleObjectType.OpenOfficeText_1_1;
    else if (OleTypeConvertor.StartsWithExt(oleTypeStr, "Video Clip") || OleTypeConvertor.StartsWithExt(oleTypeStr, "AVIFile"))
      oleType = OleObjectType.VideoClip;
    else if (OleTypeConvertor.StartsWithExt(oleTypeStr, "WaveSound") || OleTypeConvertor.StartsWithExt(oleTypeStr, "SoundRec"))
      oleType = OleObjectType.WaveSound;
    else if (OleTypeConvertor.StartsWithExt(oleTypeStr, "WordPad Document") || OleTypeConvertor.StartsWithExt(oleTypeStr, "WordPad.Document.1"))
      oleType = OleObjectType.WordPadDocument;
    return oleType;
  }

  internal static string ToString(OleObjectType oleType, bool isWord2003)
  {
    string str = string.Empty;
    switch (oleType)
    {
      case OleObjectType.AdobeAcrobatDocument:
        str = isWord2003 ? "Acrobat Document" : "AcroExch.Document.7";
        break;
      case OleObjectType.BitmapImage:
        str = "PBrush";
        break;
      case OleObjectType.MediaClip:
        str = isWord2003 ? "Media Clip" : "MPlayer";
        break;
      case OleObjectType.Equation:
        str = isWord2003 ? "Microsoft Equation 3.0" : "Equation.3";
        break;
      case OleObjectType.GraphChart:
        str = isWord2003 ? "Microsoft Graph Chart" : "MSGraph.Chart.8";
        break;
      case OleObjectType.Excel_97_2003_Worksheet:
        str = isWord2003 ? "Microsoft Office Excel 2003 Worksheet" : "Excel.Sheet.8";
        break;
      case OleObjectType.ExcelBinaryWorksheet:
        str = isWord2003 ? "Microsoft Office Excel Binary Worksheet" : "Excel.SheetBinaryMacroEnabled.12";
        break;
      case OleObjectType.ExcelChart:
        str = isWord2003 ? "Microsoft Office Excel Chart" : "Excel.Chart.8";
        break;
      case OleObjectType.ExcelMacroWorksheet:
        str = isWord2003 ? "Microsoft Office Excel Worksheet (code)" : "Excel.SheetMacroEnabled.12";
        break;
      case OleObjectType.ExcelWorksheet:
        str = isWord2003 ? "Microsoft Office Excel Worksheet" : "Excel.Sheet.12";
        break;
      case OleObjectType.PowerPoint_97_2003_Presentation:
        str = isWord2003 ? "Microsoft Office PowerPoint 97-2003 Presentation" : "PowerPoint.Show.8";
        break;
      case OleObjectType.PowerPoint_97_2003_Slide:
        str = isWord2003 ? "Microsoft Office PowerPoint 97-2003 Slide" : "PowerPoint.Slide.8";
        break;
      case OleObjectType.PowerPointMacroPresentation:
        str = isWord2003 ? "Microsoft Office PowerPoint Macro-Enabled Presentation" : "PowerPoint.ShowMacroEnabled.12";
        break;
      case OleObjectType.PowerPointMacroSlide:
        str = isWord2003 ? "Microsoft Office PowerPoint Macro-Enabled Slide" : "PowerPoint.SlideMacroEnabled.12";
        break;
      case OleObjectType.PowerPointPresentation:
        str = isWord2003 ? "Microsoft Office PowerPoint Presentation" : "PowerPoint.Show.12";
        break;
      case OleObjectType.PowerPointSlide:
        str = isWord2003 ? "Microsoft Office PowerPoint Slide" : "PowerPoint.Slide.12";
        break;
      case OleObjectType.Word_97_2003_Document:
        str = isWord2003 ? "Microsoft Office Word 97-2003 Document" : "Word.Document.8";
        break;
      case OleObjectType.WordDocument:
        str = isWord2003 ? "Microsoft Office Word Document" : "Word.Document.12";
        break;
      case OleObjectType.WordMacroDocument:
        str = isWord2003 ? "Microsoft Office Word Macro-Enabled Document" : "Word.DocumentMacroEnabled.12";
        break;
      case OleObjectType.VisioDrawing:
        str = isWord2003 ? "Microsoft Visio Drawing" : "Visio.Drawing.11";
        break;
      case OleObjectType.MIDISequence:
        str = "MIDI Sequence";
        break;
      case OleObjectType.OpenDocumentPresentation:
        str = isWord2003 ? "OpenDocument Presentation" : "PowerPoint.OpenDocumentPresentation.12";
        break;
      case OleObjectType.OpenDocumentSpreadsheet:
        str = isWord2003 ? "OpenDocument Spreadsheet" : "Excel.OpenDocumentSpreadsheet.12";
        break;
      case OleObjectType.OpenOfficeSpreadsheet1_1:
        str = "soffice.StarCalcDocument.6";
        break;
      case OleObjectType.OpenOfficeText_1_1:
        str = "soffice.StarWriterDocument.6";
        break;
      case OleObjectType.Package:
        str = "Package";
        break;
      case OleObjectType.VideoClip:
        str = isWord2003 ? "Video Clip" : "AVIFile";
        break;
      case OleObjectType.WaveSound:
        str = isWord2003 ? "WaveSound" : "SoundRec";
        break;
      case OleObjectType.WordPadDocument:
        str = isWord2003 ? "WordPad Document" : "WordPad.Document.1";
        break;
      case OleObjectType.OpenOfficeSpreadsheet:
        str = "opendocument.CalcDocument.1";
        break;
      case OleObjectType.OpenOfficeText:
        str = "opendocument.WriterDocument.1";
        break;
    }
    return str;
  }

  internal static Guid GetGUID(OleObjectType type)
  {
    Guid guid = Guid.NewGuid();
    string g = (string) null;
    switch (type)
    {
      case OleObjectType.AdobeAcrobatDocument:
        g = "b801ca65-a1fc-11d0-85ad-444553540000";
        break;
      case OleObjectType.BitmapImage:
        g = "0003000a-0000-0000-c000-000000000046";
        break;
      case OleObjectType.Equation:
        g = "0002ce02-0000-0000-c000-000000000046";
        break;
      case OleObjectType.GraphChart:
        g = "00020803-0000-0000-c000-000000000046";
        break;
      case OleObjectType.Excel_97_2003_Worksheet:
        g = "00020820-0000-0000-c000-000000000046";
        break;
      case OleObjectType.ExcelBinaryWorksheet:
        g = "00020833-0000-0000-c000-000000000046";
        break;
      case OleObjectType.ExcelChart:
        g = "00020821-0000-0000-c000-000000000046";
        break;
      case OleObjectType.ExcelMacroWorksheet:
        g = "00020832-0000-0000-c000-000000000046";
        break;
      case OleObjectType.ExcelWorksheet:
        g = "00020830-0000-0000-c000-000000000046";
        break;
      case OleObjectType.PowerPoint_97_2003_Presentation:
        g = "64818d10-4f9b-11cf-86ea-00aa00b929e8";
        break;
      case OleObjectType.PowerPoint_97_2003_Slide:
        g = "64818d11-4f9b-11cf-86ea-00aa00b929e8";
        break;
      case OleObjectType.PowerPointMacroPresentation:
        g = "dc020317-e6e2-4a62-b9fa-b3efe16626f4";
        break;
      case OleObjectType.PowerPointMacroSlide:
        g = "3c18eae4-bc25-4134-b7df-1eca1337dddc";
        break;
      case OleObjectType.PowerPointPresentation:
        g = "cf4f55f4-8f87-4d47-80bb-5808164bb3f8";
        break;
      case OleObjectType.PowerPointSlide:
        g = "048eb43e-2059-422f-95e0-557da96038af";
        break;
      case OleObjectType.Word_97_2003_Document:
        g = "00020906-0000-0000-c000-000000000046";
        break;
      case OleObjectType.WordDocument:
        g = "f4754c9b-64f5-4b40-8af4-679732ac0607";
        break;
      case OleObjectType.WordMacroDocument:
        g = "18a06b6b-2f3f-4e2b-a611-52be631b2d22";
        break;
      case OleObjectType.VisioDrawing:
        g = "00021a14-0000-0000-c000-000000000046";
        break;
      case OleObjectType.MIDISequence:
        g = "00022603-0000-0000-c000-000000000046";
        break;
      case OleObjectType.OpenDocumentPresentation:
        g = "c282417b-2662-44b8-8a94-3bff61c50900";
        break;
      case OleObjectType.OpenDocumentSpreadsheet:
        g = "eabcecdb-cc1c-4a6f-b4e3-7f888a5adfc8";
        break;
      case OleObjectType.OpenOfficeSpreadsheet1_1:
        g = "7b342dc4-139a-4a46-8a93-db0827ccee9c";
        break;
      case OleObjectType.OpenOfficeText_1_1:
        g = "30a2652a-ddf7-45e7-aca6-3eab26fc8a4e";
        break;
      case OleObjectType.Package:
        g = "0003000c-0000-0000-c000-000000000046";
        break;
      case OleObjectType.WordPadDocument:
        g = "73fddc80-aea9-101a-98a7-00aa00374959";
        break;
      case OleObjectType.OpenOfficeSpreadsheet:
        g = "7fa8ae11-b3e3-4d88-aabf-255526cd1ce8";
        break;
      case OleObjectType.OpenOfficeText:
        g = "f616b81f-7bb8-4f22-b8a5-47428d59f8ad";
        break;
    }
    if (g != null)
      guid = new Guid(g);
    return guid;
  }

  private static bool StartsWithExt(string text, string value) => text.StartsWithExt(value);
}
