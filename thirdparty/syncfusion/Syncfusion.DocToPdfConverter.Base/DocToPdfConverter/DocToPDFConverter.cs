// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocToPDFConverter.DocToPDFConverter
// Assembly: Syncfusion.DocToPDFConverter.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 84EFC094-D348-494C-A410-44F5807BB0D3
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocToPdfConverter.Base.dll

using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO.DLS.Rendering;
using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using Syncfusion.DocToPdfConverter.Rendering;
using Syncfusion.Layouting;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Interactive;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.DocToPDFConverter;

[ToolboxItem(false)]
public class DocToPDFConverter : Component, IDisposable
{
  private List<WPageSetup> m_pageSettings;
  private WordDocument m_wordDocument;
  private PdfDocument m_pdfDocument;
  private PdfPage m_currentPage;
  private DocToPDFConverterSettings m_settings = new DocToPDFConverterSettings();
  private byte m_flag;

  private List<WPageSetup> PageSettings
  {
    get
    {
      if (this.m_pageSettings == null)
        this.m_pageSettings = new List<WPageSetup>();
      return this.m_pageSettings;
    }
  }

  public DocToPDFConverterSettings Settings
  {
    get => this.m_settings;
    set => this.m_settings = value;
  }

  public bool IsCanceled
  {
    get => ((int) this.m_flag & 1) != 0;
    internal set => this.m_flag |= value ? (byte) 1 : (byte) 0;
  }

  internal bool IsTrial
  {
    get => ((int) this.m_flag & 2) >> 1 != 0;
    set => this.m_flag = (byte) ((int) this.m_flag & 253 | (value ? 1 : 0) << 1);
  }

  public DocToPDFConverter() => this.m_pageSettings = new List<WPageSetup>();

  internal void Close()
  {
    if (this.m_pageSettings != null)
    {
      this.m_pageSettings.Clear();
      this.m_pageSettings = (List<WPageSetup>) null;
    }
    if (this.m_settings != null)
      this.m_settings = (DocToPDFConverterSettings) null;
    if (this.m_wordDocument != null)
      this.m_wordDocument = (WordDocument) null;
    if (this.m_pdfDocument != null)
      this.m_pdfDocument = (PdfDocument) null;
    if (this.m_currentPage == null)
      return;
    this.m_currentPage = (PdfPage) null;
  }

  public new void Dispose() => this.Close();

  public PdfDocument ConvertToPDF(WordDocument wordDocument)
  {
    if (DocumentLayouter.IsAzureCompatible)
      this.Settings.EnableFastRendering = true;
    this.m_wordDocument = wordDocument;
    this.ShowWarnings();
    if (this.Settings.EnableAlternateChunks)
      this.m_wordDocument.UpdateAlternateChunks();
    this.m_wordDocument.SortByZIndex(false);
    if (this.IsCanceled)
      return (PdfDocument) null;
    this.EmbedDocumentFonts();
    DocumentLayouter layouter = (DocumentLayouter) null;
    if (this.Settings.UpdateDocumentFields)
      layouter = this.m_wordDocument.UpdateDocumentFieldsInOptimalWay();
    if (layouter == null)
    {
      layouter = new DocumentLayouter();
      layouter.Layout((IWordDocument) this.m_wordDocument);
    }
    this.UpdateTrackChangesBalloonsCount(layouter);
    if (this.Settings.PdfConformanceLevel != PdfConformanceLevel.None)
      layouter.EnablePdfConformanceLevel = true;
    return this.Settings.EnableFastRendering ? this.DrawDirectWordToPDF(layouter) : this.DrawToPDF(layouter);
  }

  private void UpdateTrackChangesBalloonsCount(DocumentLayouter layouter)
  {
    int num = 0;
    foreach (Page page in (List<Page>) layouter.Pages)
    {
      if (page.TrackChangesMarkups != null)
        num += page.TrackChangesMarkups.Count;
    }
    this.m_wordDocument.TrackChangesBalloonCount = num;
  }

  private void EmbedDocumentFonts()
  {
    if (this.m_wordDocument.FFNStringTable == null)
      return;
    PrivateFontCollection privateFontCollection = (PrivateFontCollection) null;
    foreach (FontFamilyNameRecord familyNameRecord in this.m_wordDocument.FFNStringTable.FontFamilyNameRecords)
    {
      foreach (KeyValuePair<string, Dictionary<string, DictionaryEntry>> embedFont in familyNameRecord.EmbedFonts)
      {
        foreach (KeyValuePair<string, DictionaryEntry> keyValuePair in embedFont.Value)
        {
          DictionaryEntry dictionaryEntry = keyValuePair.Value;
          string guidString = this.ParseGuidString(dictionaryEntry.Key.ToString());
          if (guidString != null)
          {
            MemoryStream font = (MemoryStream) dictionaryEntry.Value;
            MemoryStream outStream = new MemoryStream();
            this.DeObfuscateFont((Stream) font, outStream, guidString);
            byte[] numArray = new byte[outStream.Length];
            outStream.Position = 0L;
            outStream.Read(numArray, 0, (int) outStream.Length);
            IntPtr num = Marshal.AllocCoTaskMem((int) outStream.Length);
            Marshal.Copy(numArray, 0, num, (int) outStream.Length);
            if (privateFontCollection == null)
              privateFontCollection = new PrivateFontCollection();
            privateFontCollection.AddMemoryFont(num, (int) outStream.Length);
            outStream.Position = 0L;
            string fontStreamName = this.GetFontStreamName((Stream) outStream);
            string key = fontStreamName + embedFont.Key.Replace("embed", "_");
            if (!this.m_wordDocument.FontSettings.FontStreams.ContainsKey(key))
              this.m_wordDocument.FontSettings.FontStreams.Add(key, (Stream) new MemoryStream(outStream.ToArray()));
            if (!this.m_wordDocument.FontSettings.FontNames.ContainsKey(familyNameRecord.FontName))
              this.m_wordDocument.FontSettings.FontNames.Add(familyNameRecord.FontName, fontStreamName);
            font.Close();
            outStream.Close();
          }
        }
      }
    }
    if (privateFontCollection == null || privateFontCollection.Families == null || privateFontCollection.Families.Length <= 0)
      return;
    this.m_wordDocument.FontSettings.PrivateFonts = privateFontCollection;
  }

  private string GetFontStreamName(Stream stream)
  {
    string fontStreamName = string.Empty;
    BinaryReader reader = new BinaryReader(stream);
    if (reader != null)
    {
      TtfReader ttfReader = new TtfReader(reader);
      fontStreamName = ttfReader.Metrics.FontFamily;
      ttfReader.Close();
      reader.Close();
    }
    return fontStreamName;
  }

  private string ParseGuidString(string guidString)
  {
    try
    {
      guidString = new Guid(guidString).ToString("N");
      return guidString;
    }
    catch (Exception ex)
    {
      return (string) null;
    }
  }

  private void DeObfuscateFont(Stream font, MemoryStream outStream, string fontGuid)
  {
    byte[] numArray = new byte[16 /*0x10*/];
    for (int index = 0; index < numArray.Length; ++index)
      numArray[index] = Convert.ToByte(fontGuid.Substring(index * 2, 2), 16 /*0x10*/);
    byte[] buffer1 = new byte[32 /*0x20*/];
    font.Position = 0L;
    font.Read(buffer1, 0, 32 /*0x20*/);
    for (int index1 = 0; index1 < 32 /*0x20*/; ++index1)
    {
      int index2 = numArray.Length - index1 % numArray.Length - 1;
      buffer1[index1] = (byte) ((uint) buffer1[index1] ^ (uint) numArray[index2]);
    }
    outStream.Write(buffer1, 0, 32 /*0x20*/);
    byte[] buffer2 = new byte[4096 /*0x1000*/];
    int count;
    while ((count = font.Read(buffer2, 0, 4096 /*0x1000*/)) > 0)
      outStream.Write(buffer2, 0, count);
    outStream.Position = 0L;
  }

  public PdfDocument ConvertToPDF(string fileName)
  {
    WordDocument wordDocument = new WordDocument(fileName, FormatType.Automatic);
    PdfDocument pdf = this.ConvertToPDF(wordDocument);
    wordDocument.Close();
    return pdf;
  }

  public PdfDocument ConvertToPDF(Stream stream)
  {
    WordDocument wordDocument = new WordDocument(stream, FormatType.Automatic);
    PdfDocument pdf = this.ConvertToPDF(wordDocument);
    wordDocument.Close();
    return pdf;
  }

  private PdfDocument CreateDocument()
  {
    PdfDocument document = this.Settings.PdfConformanceLevel == PdfConformanceLevel.None ? new PdfDocument() : new PdfDocument(this.Settings.PdfConformanceLevel);
    PdfDocument.EnableCache = false;
    if (this.Settings.EnableFastRendering)
      document.EnableMemoryOptimization = true;
    document.PageSettings.Margins.All = 0.0f;
    document.FileStructure.CrossReferenceType = PdfCrossReferenceType.CrossReferenceTable;
    document.FileStructure.Version = PdfVersion.Version1_4;
    return document;
  }

  private PdfSection AddSection(WPageSetup pageSetup)
  {
    PdfSection pdfSection = this.m_pdfDocument.Sections.Add();
    pdfSection.PageSettings.Margins.All = 0.0f;
    if (this.m_wordDocument.TrackChangesBalloonCount > 0)
    {
      pdfSection.PageSettings.Margins.Top = 80f;
      pdfSection.PageSettings.Margins.Bottom = 80f;
    }
    pdfSection.PageSettings.Orientation = pageSetup.Orientation == PageOrientation.Portrait || pageSetup.Orientation.ToString() == "1" ? PdfPageOrientation.Portrait : PdfPageOrientation.Landscape;
    pdfSection.PageSettings.Height = pageSetup.PageSize.Height;
    pdfSection.PageSettings.Width = pageSetup.PageSize.Width;
    return pdfSection;
  }

  private void InitPagesSettings(DocumentLayouter layouter)
  {
    for (int index = 0; index < layouter.Pages.Count; ++index)
      this.PageSettings.Add(layouter.Pages[index].Setup);
  }

  private void AddDocumentProperties(BuiltinDocumentProperties docProperties)
  {
    if (!string.IsNullOrEmpty(docProperties.Author))
      this.m_pdfDocument.DocumentInformation.Author = docProperties.Author;
    if (!string.IsNullOrEmpty(docProperties.CreateDate.ToString()))
      this.m_pdfDocument.DocumentInformation.CreationDate = docProperties.CreateDate;
    if (!string.IsNullOrEmpty(docProperties.Company))
    {
      this.m_pdfDocument.DocumentInformation.Creator = docProperties.Company;
      this.m_pdfDocument.DocumentInformation.Producer = docProperties.Company;
    }
    if (!string.IsNullOrEmpty(docProperties.Keywords))
      this.m_pdfDocument.DocumentInformation.Keywords = docProperties.Keywords;
    if (!string.IsNullOrEmpty(docProperties.Subject))
      this.m_pdfDocument.DocumentInformation.Subject = docProperties.Subject;
    if (string.IsNullOrEmpty(docProperties.Title))
      return;
    this.m_pdfDocument.DocumentInformation.Title = docProperties.Title;
  }

  private void AddHyperLinks(List<Dictionary<string, RectangleF>> hyperlinks)
  {
    for (int index = 0; index < hyperlinks.Count; ++index)
    {
      foreach (KeyValuePair<string, RectangleF> keyValuePair in hyperlinks[index])
      {
        RectangleF rectangle = keyValuePair.Value;
        string key = keyValuePair.Key;
        if (!key.Equals(string.Empty))
        {
          if (this.m_wordDocument.TrackChangesBalloonCount > 0)
            rectangle = this.ScaleRectangle(rectangle);
          PdfUriAnnotation annotation = new PdfUriAnnotation(rectangle);
          annotation.Uri = key;
          annotation.Border.Width = 0.0f;
          this.m_currentPage.Annotations.Add((PdfAnnotation) annotation);
        }
      }
    }
  }

  private void AddBookmarkHyperlinks(
    List<Dictionary<string, DocumentLayouter.BookmarkHyperlink>> bookmarkHyperlinks)
  {
    for (int index = 0; index < bookmarkHyperlinks.Count; ++index)
    {
      foreach (KeyValuePair<string, DocumentLayouter.BookmarkHyperlink> keyValuePair in bookmarkHyperlinks[index])
      {
        DocumentLayouter.BookmarkHyperlink bookmarkHyperlink = keyValuePair.Value;
        if (bookmarkHyperlink.SourcePageNumber == this.m_pdfDocument.Pages.IndexOf(this.m_currentPage) + 1 && !keyValuePair.Key.Equals(string.Empty))
        {
          if (this.m_wordDocument.TrackChangesBalloonCount > 0)
          {
            int num1 = bookmarkHyperlink.SourcePageNumber - 1;
            if (num1 >= 0 && num1 < this.PageSettings.Count)
              bookmarkHyperlink.SourceBounds = this.ScaleRectangle(bookmarkHyperlink.SourceBounds);
            int num2 = bookmarkHyperlink.TargetPageNumber - 1;
            if (num2 >= 0 && num2 < this.PageSettings.Count)
              bookmarkHyperlink.TargetBounds = this.ScaleRectangle(bookmarkHyperlink.TargetBounds);
          }
          PdfDocumentLinkAnnotation annotation = new PdfDocumentLinkAnnotation(bookmarkHyperlink.SourceBounds);
          annotation.Border = new PdfAnnotationBorder(0.0f);
          annotation.Name = bookmarkHyperlink.HyperlinkValue;
          if (this.m_pdfDocument.Pages.Count >= bookmarkHyperlink.TargetPageNumber && bookmarkHyperlink.TargetPageNumber != 0)
          {
            annotation.Destination = new PdfDestination((PdfPageBase) this.m_pdfDocument.Pages[bookmarkHyperlink.TargetPageNumber - 1]);
            annotation.Destination.Location = bookmarkHyperlink.TargetBounds.Location;
          }
          this.m_currentPage.Annotations.Add((PdfAnnotation) annotation);
        }
      }
    }
  }

  private RectangleF ScaleRectangle(RectangleF rectangle)
  {
    float num = 0.75f;
    return new RectangleF(rectangle.X * num, rectangle.Y * num, rectangle.Width * num, rectangle.Height * num);
  }

  private void AddDocumentBookmarks(List<Syncfusion.DocIO.Rendering.BookmarkPosition> bookmarks)
  {
    PdfBookmark[] pdfBookmarkArray = new PdfBookmark[9];
    int[] previousLevelArray = new int[bookmarks.Count];
    int previousLevel = 0;
    for (int index = 0; index < bookmarks.Count; ++index)
    {
      Syncfusion.DocIO.Rendering.BookmarkPosition bookmark = bookmarks[index];
      if (!string.IsNullOrEmpty(bookmark.BookmarkName) && bookmark.PageNumber != 0 && bookmark.PageNumber <= this.m_pdfDocument.Pages.Count)
      {
        if ((this.Settings.ExportBookmarks & ExportBookmarkType.Headings) == ExportBookmarkType.Headings && bookmark.BookmarkStyle > 0)
        {
          int bookmarkStyle = bookmark.BookmarkStyle;
          previousLevelArray[index] = bookmarkStyle;
          if (bookmarkStyle == 1 || index == 0)
          {
            PdfBookmark pdfBookmark = this.m_pdfDocument.Bookmarks.Add(bookmark.BookmarkName);
            pdfBookmarkArray[bookmarkStyle - 1] = pdfBookmark;
            pdfBookmark.Destination = new PdfDestination((PdfPageBase) this.m_pdfDocument.Pages[bookmark.PageNumber - 1]);
            pdfBookmark.Destination.Location = bookmark.Bounds.Location;
            previousLevel = bookmarkStyle;
          }
          else if (this.HasParentNode(ref previousLevel, previousLevelArray, index))
          {
            PdfBookmark pdfBookmark = pdfBookmarkArray[previousLevel - 1].Insert(pdfBookmarkArray[previousLevel - 1].Count, bookmark.BookmarkName);
            if (bookmarkStyle == 10)
              pdfBookmarkArray[bookmarkStyle - 2] = pdfBookmark;
            else
              pdfBookmarkArray[bookmarkStyle - 1] = pdfBookmark;
            pdfBookmark.Destination = new PdfDestination((PdfPageBase) this.m_pdfDocument.Pages[bookmark.PageNumber - 1]);
            pdfBookmark.Destination.Location = bookmark.Bounds.Location;
            previousLevel = bookmarkStyle;
          }
          else
          {
            PdfBookmark pdfBookmark = this.m_pdfDocument.Bookmarks.Add(bookmark.BookmarkName);
            if (bookmarkStyle == 10)
              pdfBookmarkArray[bookmarkStyle - 2] = pdfBookmark;
            else
              pdfBookmarkArray[bookmarkStyle - 1] = pdfBookmark;
            pdfBookmark.Destination = new PdfDestination((PdfPageBase) this.m_pdfDocument.Pages[bookmark.PageNumber - 1]);
            pdfBookmark.Destination.Location = bookmark.Bounds.Location;
            previousLevel = bookmarkStyle;
          }
        }
        else
        {
          PdfBookmark pdfBookmark = this.m_pdfDocument.Bookmarks.Add(bookmark.BookmarkName);
          pdfBookmark.Destination = new PdfDestination((PdfPageBase) this.m_pdfDocument.Pages[bookmark.PageNumber - 1]);
          pdfBookmark.Destination.Location = bookmark.Bounds.Location;
        }
      }
    }
  }

  private void AddDocumentBookmarks(List<Syncfusion.DocToPdfConverter.Rendering.BookmarkPosition> bookmarks)
  {
    PdfBookmark[] pdfBookmarkArray = new PdfBookmark[9];
    int[] previousLevelArray = new int[bookmarks.Count];
    int previousLevel = 0;
    for (int index = 0; index < bookmarks.Count; ++index)
    {
      Syncfusion.DocToPdfConverter.Rendering.BookmarkPosition bookmark = bookmarks[index];
      if (!string.IsNullOrEmpty(bookmark.BookmarkName) && bookmark.PageNumber != 0 && bookmark.PageNumber <= this.m_pdfDocument.Pages.Count)
      {
        if ((this.Settings.ExportBookmarks & ExportBookmarkType.Headings) == ExportBookmarkType.Headings && bookmark.BookmarkStyle > 0)
        {
          int bookmarkStyle = bookmark.BookmarkStyle;
          previousLevelArray[index] = bookmarkStyle;
          if (bookmarkStyle == 1 || index == 0)
          {
            PdfBookmark pdfBookmark = this.m_pdfDocument.Bookmarks.Add(bookmark.BookmarkName);
            pdfBookmarkArray[bookmarkStyle - 1] = pdfBookmark;
            pdfBookmark.Destination = new PdfDestination((PdfPageBase) this.m_pdfDocument.Pages[bookmark.PageNumber - 1]);
            pdfBookmark.Destination.Location = bookmark.Bounds.Location;
            previousLevel = bookmarkStyle;
          }
          else if (this.HasParentNode(ref previousLevel, previousLevelArray, index))
          {
            PdfBookmark pdfBookmark = pdfBookmarkArray[previousLevel - 1].Insert(pdfBookmarkArray[previousLevel - 1].Count, bookmark.BookmarkName);
            if (bookmarkStyle == 10)
              pdfBookmarkArray[bookmarkStyle - 2] = pdfBookmark;
            else
              pdfBookmarkArray[bookmarkStyle - 1] = pdfBookmark;
            pdfBookmark.Destination = new PdfDestination((PdfPageBase) this.m_pdfDocument.Pages[bookmark.PageNumber - 1]);
            pdfBookmark.Destination.Location = bookmark.Bounds.Location;
            previousLevel = bookmarkStyle;
          }
          else
          {
            PdfBookmark pdfBookmark = this.m_pdfDocument.Bookmarks.Add(bookmark.BookmarkName);
            if (bookmarkStyle == 10)
              pdfBookmarkArray[bookmarkStyle - 2] = pdfBookmark;
            else
              pdfBookmarkArray[bookmarkStyle - 1] = pdfBookmark;
            pdfBookmark.Destination = new PdfDestination((PdfPageBase) this.m_pdfDocument.Pages[bookmark.PageNumber - 1]);
            pdfBookmark.Destination.Location = bookmark.Bounds.Location;
            previousLevel = bookmarkStyle;
          }
        }
        else
        {
          PdfBookmark pdfBookmark = this.m_pdfDocument.Bookmarks.Add(bookmark.BookmarkName);
          pdfBookmark.Destination = new PdfDestination((PdfPageBase) this.m_pdfDocument.Pages[bookmark.PageNumber - 1]);
          pdfBookmark.Destination.Location = bookmark.Bounds.Location;
        }
      }
    }
  }

  private bool HasParentNode(ref int previousLevel, int[] previousLevelArray, int i)
  {
    for (int index = i; index > 0; --index)
    {
      if (previousLevelArray[i] > previousLevelArray[index - 1] && previousLevelArray[index - 1] != 0)
      {
        previousLevel = previousLevelArray[index - 1];
        return true;
      }
    }
    return false;
  }

  private void EditableFormField(PdfPage page, DocumentLayouter layouter)
  {
    for (int index1 = 0; index1 < layouter.EditableFormFieldinEMF.Count; ++index1)
    {
      LayoutedWidget layoutedWidget = layouter.EditableFormFieldinEMF[index1];
      if (layoutedWidget.Widget is WCheckBox)
      {
        WCheckBox widget = layoutedWidget.Widget as WCheckBox;
        PdfCheckBoxField field = new PdfCheckBoxField((PdfPageBase) page, widget.Name);
        field.Bounds = layoutedWidget.Bounds;
        field.Visible = widget.Enabled;
        field.Checked = widget.Checked;
        field.ToolTip = widget.StatusBarHelp;
        field.ForeColor = (PdfColor) widget.CharacterFormat.HighlightColor;
        field.BackColor = (PdfColor) widget.CharacterFormat.TextColor;
        field.BorderColor = (PdfColor) Color.White;
        page.Document.Form.Fields.Add((PdfField) field);
      }
      if (layoutedWidget.Widget is WDropDownFormField)
      {
        WDropDownFormField widget = layoutedWidget.Widget as WDropDownFormField;
        PdfComboBoxField field = new PdfComboBoxField((PdfPageBase) page, widget.Name);
        if (string.IsNullOrEmpty(widget.Name))
          widget.Name = "dropdownformfield";
        float width = 15f + layoutedWidget.Bounds.Width;
        layoutedWidget.Bounds = new RectangleF(layoutedWidget.Bounds.X, layoutedWidget.Bounds.Y, width, layoutedWidget.Bounds.Height);
        field.Editable = true;
        field.Bounds = layoutedWidget.Bounds;
        field.ToolTip = widget.StatusBarHelp;
        field.BorderColor = (PdfColor) Color.Empty;
        field.Visible = widget.Enabled;
        field.Font = (PdfFont) new PdfTrueTypeFont(widget.CharacterFormat.Font);
        int index2 = 0;
        for (int index3 = widget.DropDownItems.Count - 1; index2 <= index3; ++index2)
          field.Items.Add(new PdfListFieldItem(widget.DropDownItems[index2].Text, widget.DropDownItems[index2].Text));
        field.SelectedIndex = widget.DropDownSelectedIndex;
        field.SelectedValue = widget.DropDownValue;
        page.Document.Form.Fields.Add((PdfField) field);
      }
      if (layoutedWidget.Widget is WTextRange && (layoutedWidget.Widget as WTextRange).PreviousSibling is WFieldMark)
      {
        WTextRange widget = layoutedWidget.Widget as WTextRange;
        if ((widget.PreviousSibling as WFieldMark).ParentField is WTextFormField parentField)
        {
          PdfTextBoxField field = new PdfTextBoxField((PdfPageBase) page, parentField.Name);
          field.Text = parentField.Text;
          field.ForeColor = (PdfColor) (widget.CharacterFormat.TextColor.IsEmpty ? parentField.CharacterFormat.TextColor : widget.CharacterFormat.TextColor);
          field.Font = (PdfFont) new PdfTrueTypeFont(widget.CharacterFormat.Font);
          field.Bounds = new RectangleF(layoutedWidget.Bounds.X, layoutedWidget.Bounds.Y, layoutedWidget.Bounds.Width, layoutedWidget.Bounds.Height);
          field.BackColor = (PdfColor) (widget.CharacterFormat.HighlightColor.IsEmpty ? parentField.CharacterFormat.HighlightColor : widget.CharacterFormat.HighlightColor);
          field.BorderColor = (PdfColor) Color.Empty;
          field.DefaultValue = parentField.DefaultText;
          field.MaxLength = parentField.MaximumLength;
          field.Visible = parentField.Enabled;
          page.Document.Form.Fields.Add((PdfField) field);
        }
      }
      if (layoutedWidget.Widget is SplitStringWidget && (layoutedWidget.Widget as SplitStringWidget).RealStringWidget is WTextRange && ((layoutedWidget.Widget as SplitStringWidget).RealStringWidget as WTextRange).PreviousSibling is WFieldMark)
      {
        WTextRange realStringWidget = (layoutedWidget.Widget as SplitStringWidget).RealStringWidget as WTextRange;
        if ((realStringWidget.PreviousSibling as WFieldMark).ParentField is WTextFormField parentField)
        {
          PdfTextBoxField field = new PdfTextBoxField((PdfPageBase) page, parentField.Name);
          field.Text = parentField.Text;
          field.ForeColor = (PdfColor) (realStringWidget.CharacterFormat.TextColor.IsEmpty ? parentField.CharacterFormat.TextColor : realStringWidget.CharacterFormat.TextColor);
          field.Font = (PdfFont) new PdfTrueTypeFont(realStringWidget.CharacterFormat.Font);
          field.Bounds = new RectangleF(layoutedWidget.Bounds.X, layoutedWidget.Bounds.Y, layoutedWidget.Bounds.Width, layoutedWidget.Bounds.Height);
          field.BackColor = (PdfColor) (realStringWidget.CharacterFormat.HighlightColor.IsEmpty ? parentField.CharacterFormat.HighlightColor : realStringWidget.CharacterFormat.HighlightColor);
          field.BorderColor = (PdfColor) Color.Empty;
          field.DefaultValue = parentField.DefaultText;
          field.MaxLength = parentField.MaximumLength;
          field.Visible = parentField.Enabled;
          page.Document.Form.Fields.Add((PdfField) field);
        }
      }
    }
    layouter.EditableFormFieldinEMF.Clear();
  }

  private PdfDocument DrawToPDF(DocumentLayouter layouter)
  {
    this.InitPagesSettings(layouter);
    int count = layouter.Pages.Count;
    this.m_pdfDocument = this.CreateDocument();
    List<KeyValuePair<string, bool>> commentStartMarks = (List<KeyValuePair<string, bool>>) null;
    for (int index = 0; index < count; ++index)
    {
      PdfPage page = this.AddSection(this.PageSettings[index]).Pages.Add();
      this.m_currentPage = page;
      layouter.PreserveFormField = this.m_settings.PreserveFormFields;
      layouter.ExportBookmarks = this.Settings.ExportBookmarks;
      layouter.PreserveOleEquationAsBitmap = this.Settings.PreserveOleEquationAsBitmap;
      bool isContainsComplexScript = false;
      layouter.DrawToImage(index, -1, ImageType.Metafile, ref isContainsComplexScript, ref commentStartMarks);
      PdfMetafile pdfMetafile = (PdfMetafile) PdfImage.FromImage(layouter.PageResult.Pages[0].PageImage);
      pdfMetafile.IsWordToPDF = true;
      pdfMetafile.AdjustMetaFile = this.Settings.RecreateNestedMetafile;
      pdfMetafile.ComplexScript = isContainsComplexScript || this.Settings.AutoDetectComplexScript;
      pdfMetafile.IsEmbedCompleteFonts = this.Settings.EmbedCompleteFonts;
      pdfMetafile.IsEmbedFonts = this.Settings.EmbedFonts;
      if (layouter.EditableFormFieldinEMF.Count > 0 && layouter.PreserveFormField)
        this.EditableFormField(page, layouter);
      if (this.m_wordDocument.FontSettings.PrivateFonts != null && this.m_wordDocument.FontSettings.FontStreams != null)
      {
        pdfMetafile.CustomFont = new CustomFont();
        pdfMetafile.CustomFont.EmbeddedFonts = this.CreateTempStreams(this.m_wordDocument.FontSettings.FontStreams);
        pdfMetafile.CustomFont.FontCollection = this.m_wordDocument.FontSettings.PrivateFonts;
      }
      if (this.m_settings.m_imageResolution > 0)
        pdfMetafile.ImageResolution = this.m_settings.m_imageResolution;
      else
        pdfMetafile.Quality = (long) this.m_settings.ImageQuality;
      if (this.Settings.OptimizeIdenticalImages)
      {
        pdfMetafile.OptimizeIdenticalImages = this.Settings.OptimizeIdenticalImages;
        pdfMetafile.Document = this.m_pdfDocument;
      }
      pdfMetafile.Draw(page, new RectangleF(PointF.Empty, page.Size), true);
      this.AddHyperLinks(layouter.PageResult.Pages[0].Hyperlinks);
      pdfMetafile.Dispose();
      layouter.PageResult.Pages[0].PageImage.Dispose();
      layouter.PageResult.Pages.Clear();
    }
    layouter.InitLayoutInfo();
    for (int index = 0; index < count; ++index)
    {
      this.m_currentPage = this.m_pdfDocument.Pages[index];
      this.AddBookmarkHyperlinks(DocumentLayouter.BookmarkHyperlinks);
      if (this.IsTrial)
      {
        PdfFont font = (PdfFont) new PdfTrueTypeFont(new Font("Times New Roman", 12f, FontStyle.Regular), false);
        this.m_currentPage.Graphics.DrawRectangle((PdfBrush) new PdfSolidBrush((PdfColor) Color.White), 0.0f, 0.0f, 205f, 20.65f);
        PdfBrush brush = (PdfBrush) new PdfSolidBrush((PdfColor) Color.Red);
        this.m_currentPage.Graphics.DrawString("Created by Syncfusion – DocIO library", font, brush, 6f, 4f);
      }
    }
    if (this.Settings.ExportBookmarks != ExportBookmarkType.None)
      this.AddDocumentBookmarks(DocumentLayouter.Bookmarks);
    this.AddDocumentProperties(this.m_wordDocument.BuiltinDocumentProperties);
    layouter.Close();
    if (this.m_pageSettings != null)
    {
      this.m_pageSettings.Clear();
      this.m_pageSettings = (List<WPageSetup>) null;
    }
    commentStartMarks?.Clear();
    return this.m_pdfDocument;
  }

  private Dictionary<string, Stream> CreateTempStreams(Dictionary<string, Stream> fontStreams)
  {
    Dictionary<string, Stream> tempStreams = new Dictionary<string, Stream>((IEqualityComparer<string>) System.StringComparer.CurrentCultureIgnoreCase);
    foreach (string key in fontStreams.Keys)
    {
      Stream fontStream = fontStreams[key];
      if (fontStream != null && fontStream.CanRead)
      {
        fontStream.Position = 0L;
        byte[] buffer = new byte[fontStream.Length];
        fontStream.Read(buffer, 0, (int) fontStream.Length);
        MemoryStream memoryStream = new MemoryStream(buffer, 0, (int) fontStream.Length);
        tempStreams.Add(key, (Stream) memoryStream);
      }
    }
    return tempStreams;
  }

  private PdfDocument DrawDirectWordToPDF(DocumentLayouter layouter)
  {
    this.InitPagesSettings(layouter);
    int count = layouter.Pages.Count;
    this.m_pdfDocument = this.CreateDocument();
    int autoTagsCount = 0;
    List<KeyValuePair<string, bool>> keyValuePairList = (List<KeyValuePair<string, bool>>) null;
    for (int index = 0; index < count; ++index)
    {
      PdfPage pdfPage = this.AddSection(this.PageSettings[index]).Pages.Add();
      this.m_currentPage = pdfPage;
      PdfGraphics graphics1 = pdfPage.Graphics;
      graphics1.NativeGraphics = System.Drawing.Graphics.FromImage((Image) new Bitmap(1, 1));
      graphics1.IsDirectPDF = true;
      graphics1.OptimizeIdenticalImages = this.Settings.OptimizeIdenticalImages;
      Image image = layouter.CreateImage(layouter.Pages[index].Setup, ImageType.Metafile, (MemoryStream) null);
      DocumentLayouter.PageNumber = index + 1;
      System.Drawing.Graphics graphics2 = System.Drawing.Graphics.FromImage(image);
      graphics2.PageUnit = GraphicsUnit.Point;
      PDFDrawingContext pdfDrawingContext = new PDFDrawingContext(graphics1, graphics2, GraphicsUnit.Point, this.m_pdfDocument);
      float right = this.PageSettings[index].Margins.Right;
      float num = this.PageSettings[index].PageSize.Width / (this.PageSettings[index].PageSize.Width + (260f - right));
      if (this.m_wordDocument.TrackChangesBalloonCount > 0)
        graphics1.ScaleTransform(num, num);
      if (keyValuePairList != null)
        pdfDrawingContext.m_previousLineCommentStartMarks = keyValuePairList;
      pdfDrawingContext.EmbedFonts = this.Settings.EmbedFonts;
      pdfDrawingContext.PreserveOleEquationAsBitmap = this.Settings.PreserveOleEquationAsBitmap;
      pdfDrawingContext.EmbedCompleteFonts = this.Settings.EmbedCompleteFonts;
      pdfDrawingContext.ExportBookmarks = this.Settings.ExportBookmarks;
      pdfDrawingContext.AutoTag = this.Settings.AutoTag;
      pdfDrawingContext.RecreateNestedMetafile = this.Settings.RecreateNestedMetafile;
      pdfDrawingContext.ImageQuality = this.Settings.ImageQuality;
      pdfDrawingContext.PreserveFormFields = this.Settings.PreserveFormFields;
      pdfDrawingContext.EnableComplexScript = this.Settings.AutoDetectComplexScript;
      pdfDrawingContext.PrivateFonts = this.m_wordDocument.FontSettings.PrivateFonts;
      pdfDrawingContext.FontNames = this.m_wordDocument.FontSettings.FontNames;
      pdfDrawingContext.FontStreams = this.m_wordDocument.FontSettings.FontStreams;
      pdfDrawingContext.Draw(layouter.Pages[index], ref autoTagsCount);
      pdfDrawingContext.DrawPageBorder(index, layouter.Pages);
      this.AddHyperLinks(pdfDrawingContext.Hyperlinks);
      if (pdfDrawingContext.m_previousLineCommentStartMarks != null)
        keyValuePairList = pdfDrawingContext.m_previousLineCommentStartMarks;
      graphics1.NativeGraphics.Dispose();
      graphics1.IsDirectPDF = false;
    }
    layouter.InitLayoutInfo();
    for (int index = 0; index < count; ++index)
    {
      this.m_currentPage = this.m_pdfDocument.Pages[index];
      this.AddBookmarkHyperlinks(PDFDrawingContext.BookmarkHyperlinksList);
      if (this.IsTrial)
      {
        PdfFont font = (PdfFont) new PdfTrueTypeFont(new Font("Times New Roman", 12f, FontStyle.Regular), false);
        this.m_currentPage.Graphics.DrawRectangle((PdfBrush) new PdfSolidBrush((PdfColor) Color.White), 0.0f, 0.0f, 205f, 20.65f);
        PdfBrush brush = (PdfBrush) new PdfSolidBrush((PdfColor) Color.Red);
        this.m_currentPage.Graphics.DrawString("Created by Syncfusion – DocIO library", font, brush, 6f, 4f);
      }
    }
    if (this.Settings.ExportBookmarks != ExportBookmarkType.None)
      this.AddDocumentBookmarks(PDFDrawingContext.Bookmarks);
    this.AddDocumentProperties(this.m_wordDocument.BuiltinDocumentProperties);
    layouter.Pages.Clear();
    layouter.Close();
    PDFDrawingContext.ClearFontCache();
    if (this.m_pageSettings != null)
    {
      this.m_pageSettings.Clear();
      this.m_pageSettings = (List<WPageSetup>) null;
    }
    keyValuePairList?.Clear();
    return this.m_pdfDocument;
  }

  private void ShowWarnings()
  {
    if (this.Settings.Warning == null)
      return;
    List<WarningInfo> warnings = new List<WarningInfo>();
    List<string> warningElmentNames = this.CreateWarningElmentNames();
    bool flag = this.m_wordDocument.ActualFormatType == FormatType.Rtf;
    string str = flag ? " element is not supported in Rtf to Pdf conversion" : " element is not supported in Word to Pdf conversion";
    for (int index = 0; index <= 32 /*0x20*/; ++index)
    {
      switch (index)
      {
        case 4:
        case 6:
        case 7:
        case 10:
        case 15:
        case 18:
        case 19:
        case 24:
        case 25:
        case 26:
        case 28:
        case 30:
        case 31 /*0x1F*/:
          if (this.m_wordDocument.HasElement(this.m_wordDocument.m_notSupportedElementFlag, index))
          {
            warnings.Add(new WarningInfo($"{warningElmentNames[index]}{str}", (WarningType) index));
            break;
          }
          break;
        case 5:
          if (this.m_wordDocument.HasElement(this.m_wordDocument.m_supportedElementFlag_1, 1))
          {
            warnings.Add(new WarningInfo($"{warningElmentNames[index]}{str}", WarningType.DateTime));
            break;
          }
          break;
        case 32 /*0x20*/:
          if (this.m_wordDocument.HasElement(this.m_wordDocument.m_supportedElementFlag_1, 0))
          {
            warnings.Add(new WarningInfo($"{warningElmentNames[index]}{str}", WarningType.DateTime));
            break;
          }
          break;
        default:
          if (this.m_wordDocument.HasElement(this.m_wordDocument.m_notSupportedElementFlag, index))
          {
            warnings.Add(new WarningInfo($"{warningElmentNames[index]}{str}", WarningType.DateTime));
            break;
          }
          break;
      }
    }
    if (flag)
    {
      if (this.m_wordDocument.HasElement(this.m_wordDocument.m_supportedElementFlag_1, 29))
        warnings.Add(new WarningInfo($"{"OLE Object"}{str}", WarningType.OLEObject));
      if (this.m_wordDocument.HasElement(this.m_wordDocument.m_supportedElementFlag_2, 13))
        warnings.Add(new WarningInfo($"{"Textbox "}{str}", WarningType.Textbox));
      if (this.m_wordDocument.HasElement(this.m_wordDocument.m_supportedElementFlag_1, 29))
        warnings.Add(new WarningInfo($"{"Text watermark"}{str}", WarningType.Watermark));
    }
    warningElmentNames.Clear();
    this.IsCanceled = !this.Settings.Warning.ShowWarnings(warnings);
    warnings.Clear();
  }

  private List<string> CreateWarningElmentNames()
  {
    return new List<string>()
    {
      "Abbreviated date",
      "Abbreviated day of week",
      "Abbreviated month",
      "AM/PM for current time",
      "Annotation",
      "Year short",
      "Comments",
      "Current section number",
      "Current time in hours:minutes",
      "Current Time in hours:minutes:seconds",
      "Custom shape",
      "Date and month",
      "Day of week ",
      "Day long ",
      "Day short ",
      "Group shape ",
      "Hour of current time ",
      "Hour of current time with no leading zero ",
      "Line Number",
      "Math",
      "Minute of current time with no leading zero",
      "Minute of current time",
      "Month long",
      "Month short",
      "Page number",
      "Shape",
      "Print merge helper field",
      "Seconds of current time",
      "Smart art",
      "Time in hours:minutes:seconds",
      "Track changes",
      "Word art",
      "Year long"
    };
  }
}
