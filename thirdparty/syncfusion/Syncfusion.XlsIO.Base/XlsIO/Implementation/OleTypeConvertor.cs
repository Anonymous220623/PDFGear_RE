// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.OleTypeConvertor
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

internal class OleTypeConvertor
{
  internal const string OleType = "Package";
  private static int m_fileid = 1;
  internal static List<string> files = new List<string>();

  internal static OleObjectType ToOleType(string oleTypeStr)
  {
    return oleTypeStr.StartsWith("Acrobat Document") || oleTypeStr.StartsWith("AcroExch.Document") ? OleObjectType.AdobeAcrobatDocument : (!oleTypeStr.StartsWith("Package") ? (!oleTypeStr.StartsWith("PBrush") ? (oleTypeStr.StartsWith("Media Clip") || oleTypeStr.StartsWith("MPlayer") ? OleObjectType.MediaClip : (oleTypeStr.StartsWith("Microsoft Equation 3.0") || oleTypeStr.StartsWith("Equation.3") ? OleObjectType.Equation : (oleTypeStr.StartsWith("Microsoft Graph Chart") || oleTypeStr.StartsWith("MSGraph.Chart.8") ? OleObjectType.GraphChart : (oleTypeStr.StartsWith("Microsoft Office Excel 2003 Worksheet") || oleTypeStr.StartsWith("Excel.Sheet.8") ? OleObjectType.Excel_97_2003_Worksheet : (oleTypeStr.StartsWith("Microsoft Office Excel Binary Worksheet") || oleTypeStr.StartsWith("Excel.SheetBinaryMacroEnabled.12") ? OleObjectType.ExcelBinaryWorksheet : (oleTypeStr.StartsWith("Microsoft Office Excel Chart") || oleTypeStr.StartsWith("Excel.Chart.8") ? OleObjectType.ExcelChart : (oleTypeStr.StartsWith("Microsoft Office Excel Worksheet (code)") || oleTypeStr.StartsWith("Excel.SheetMacroEnabled.12") ? OleObjectType.ExcelMacroWorksheet : (oleTypeStr.StartsWith("Microsoft Office Excel Worksheet") || oleTypeStr.StartsWith("Excel.Sheet.12") ? OleObjectType.ExcelWorksheet : (oleTypeStr.StartsWith("Microsoft Office PowerPoint 97-2003 Presentation") || oleTypeStr.StartsWith("PowerPoint.Show.8") ? OleObjectType.PowerPoint_97_2003_Presentation : (oleTypeStr.StartsWith("Microsoft Office PowerPoint 97-2003 Slide") || oleTypeStr.StartsWith("PowerPoint.Slide.8") ? OleObjectType.PowerPoint_97_2003_Slide : (oleTypeStr.StartsWith("Microsoft Office PowerPoint Macro-Enabled Presentation") || oleTypeStr.StartsWith("PowerPoint.ShowMacroEnabled.12") ? OleObjectType.PowerPointMacroPresentation : (oleTypeStr.StartsWith("Microsoft Office PowerPoint Macro-Enabled Slide") || oleTypeStr.StartsWith("PowerPoint.SlideMacroEnabled.12") ? OleObjectType.PowerPointMacroSlide : (oleTypeStr.StartsWith("Microsoft Office PowerPoint Presentation") || oleTypeStr.StartsWith("PowerPoint.Show.12") ? OleObjectType.PowerPointPresentation : (oleTypeStr.StartsWith("Microsoft Office PowerPoint Slide") || oleTypeStr.StartsWith("PowerPoint.Slide.12") ? OleObjectType.PowerPointSlide : (oleTypeStr.StartsWith("Microsoft Office Word 97-2003 Document") || oleTypeStr.StartsWith("Word.Document.8") ? OleObjectType.Word_97_2003_Document : (oleTypeStr.StartsWith("Microsoft Office Word Document") || oleTypeStr.StartsWith("Word.Document.12") ? OleObjectType.WordDocument : (oleTypeStr.StartsWith("Microsoft Office Word Macro-Enabled Document") || oleTypeStr.StartsWith("Word.DocumentMacroEnabled.12") ? OleObjectType.WordMacroDocument : (oleTypeStr.StartsWith("Microsoft Visio Drawing") || oleTypeStr.StartsWith("Visio.Drawing.11") ? OleObjectType.VisioDrawing : (oleTypeStr.StartsWith("OpenDocument Presentation") || oleTypeStr.StartsWith("PowerPoint.OpenDocumentPresentation.12") ? OleObjectType.OpenDocumentPresentation : (oleTypeStr.StartsWith("OpenDocument Spreadsheet") || oleTypeStr.StartsWith("Excel.OpenDocumentSpreadsheet.12") ? OleObjectType.OpenDocumentSpreadsheet : (oleTypeStr.StartsWith("OpenDocument Text") || oleTypeStr.StartsWith("Word.OpenDocumentText.12") ? OleObjectType.OpenDocumentText : (!oleTypeStr.StartsWith("opendocument.CalcDocument.1") ? (!oleTypeStr.StartsWith("opendocument.WriterDocument.1") ? (!oleTypeStr.StartsWith("soffice.StarCalcDocument.6") ? (!oleTypeStr.StartsWith("soffice.StarWriterDocument.6") ? (oleTypeStr.StartsWith("Video Clip") || oleTypeStr.StartsWith("AVIFile") ? OleObjectType.VideoClip : (oleTypeStr.StartsWith("WaveSound") || oleTypeStr.StartsWith("SoundRec") ? OleObjectType.WaveSound : OleObjectType.WordPadDocument)) : OleObjectType.OpenOfficeText_1_1) : OleObjectType.OpenOfficeSpreadsheet1_1) : OleObjectType.OpenOfficeText) : OleObjectType.OpenOfficeSpreadsheet)))))))))))))))))))))) : OleObjectType.BitmapImage) : OleObjectType.Package);
  }

  internal static string ToOleString(OleObjectType oleType)
  {
    string oleString = string.Empty;
    switch (oleType)
    {
      case OleObjectType.AdobeAcrobatDocument:
        oleString = "AcroExch.Document.7";
        break;
      case OleObjectType.BitmapImage:
        oleString = "PBrush";
        break;
      case OleObjectType.MediaClip:
        oleString = "MPlayer";
        break;
      case OleObjectType.Equation:
        oleString = "Equation.3";
        break;
      case OleObjectType.GraphChart:
        oleString = "MSGraph.Chart.8";
        break;
      case OleObjectType.Excel_97_2003_Worksheet:
        oleString = "Excel.Sheet.8";
        break;
      case OleObjectType.ExcelBinaryWorksheet:
        oleString = "Excel.SheetBinaryMacroEnabled.12";
        break;
      case OleObjectType.ExcelChart:
        oleString = "Excel.Chart.8";
        break;
      case OleObjectType.ExcelMacroWorksheet:
        oleString = "Excel.SheetMacroEnabled.12";
        break;
      case OleObjectType.ExcelWorksheet:
        oleString = "Excel.Sheet.12";
        break;
      case OleObjectType.PowerPoint_97_2003_Presentation:
        oleString = "PowerPoint.Show.8";
        break;
      case OleObjectType.PowerPoint_97_2003_Slide:
        oleString = "PowerPoint.Slide.8";
        break;
      case OleObjectType.PowerPointMacroPresentation:
        oleString = "PowerPoint.ShowMacroEnabled.12";
        break;
      case OleObjectType.PowerPointMacroSlide:
        oleString = "PowerPoint.SlideMacroEnabled.12";
        break;
      case OleObjectType.PowerPointPresentation:
        oleString = "PowerPoint.Show.12";
        break;
      case OleObjectType.PowerPointSlide:
        oleString = "PowerPoint.Slide.12";
        break;
      case OleObjectType.Word_97_2003_Document:
        oleString = "Word.Document.8";
        break;
      case OleObjectType.WordDocument:
        oleString = "Word.Document.12";
        break;
      case OleObjectType.WordMacroDocument:
        oleString = "Word.DocumentMacroEnabled.12";
        break;
      case OleObjectType.VisioDrawing:
        oleString = "Visio.Drawing.11";
        break;
      case OleObjectType.MIDISequence:
        oleString = "MIDI Sequence";
        break;
      case OleObjectType.OpenDocumentPresentation:
        oleString = "PowerPoint.OpenDocumentPresentation.12";
        break;
      case OleObjectType.OpenDocumentSpreadsheet:
        oleString = "Excel.OpenDocumentSpreadsheet.12";
        break;
      case OleObjectType.OpenDocumentText:
        oleString = "Word.OpenDocumentText.12";
        break;
      case OleObjectType.OpenOfficeSpreadsheet1_1:
        oleString = "soffice.StarCalcDocument.6";
        break;
      case OleObjectType.OpenOfficeText_1_1:
        oleString = "soffice.StarWriterDocument.6";
        break;
      case OleObjectType.Package:
        oleString = "Package";
        break;
      case OleObjectType.VideoClip:
        oleString = "AVIFile";
        break;
      case OleObjectType.WaveSound:
        oleString = "SoundRec";
        break;
      case OleObjectType.WordPadDocument:
        oleString = "WordPad.Document.1";
        break;
      case OleObjectType.OpenOfficeSpreadsheet:
        oleString = "opendocument.CalcDocument.1";
        break;
      case OleObjectType.OpenOfficeText:
        oleString = "opendocument.WriterDocument.1";
        break;
    }
    return oleString;
  }

  internal static Guid GetGUID()
  {
    Guid guid = Guid.NewGuid();
    string g = "0003000c-0000-0000-c000-000000000046";
    if (g != null)
      guid = new Guid(g);
    return guid;
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
      case OleObjectType.OpenDocumentText:
        g = "1b261b22-ac6a-4e68-a870-ab5080e8687b";
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

  internal static string GetOleFileName()
  {
    return $"oleObject{OleTypeConvertor.GetNextFileId().ToString()}.bin";
  }

  internal static int GetNextFileId() => OleTypeConvertor.m_fileid++;

  internal static void Add(string fileName) => OleTypeConvertor.files.Add(fileName);
}
