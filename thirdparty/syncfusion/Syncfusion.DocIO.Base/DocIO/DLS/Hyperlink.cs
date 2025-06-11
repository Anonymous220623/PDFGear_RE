// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.Hyperlink
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class Hyperlink
{
  private WField m_hyperlink;
  private HyperlinkType m_type;
  private string m_filePath;
  private string m_uriPath;
  private string m_bookmark;
  private string m_textToDisplay;
  private WPicture m_picToDisplay;
  private string m_localReference;

  public string FilePath
  {
    get => this.m_filePath == null ? (string) null : this.m_filePath.Replace("\"", string.Empty);
    set => this.SetFilePathValue(value);
  }

  public string Uri
  {
    get => this.m_uriPath == null ? (string) null : this.m_uriPath.Replace("\"", string.Empty);
    set
    {
      this.SetUriValue(value);
      this.m_uriPath = value;
    }
  }

  public string BookmarkName
  {
    get => this.m_bookmark != null ? this.m_bookmark.Replace("\"", string.Empty) : (string) null;
    set
    {
      this.SetBookmarkNameValue(value);
      this.m_bookmark = value;
    }
  }

  public HyperlinkType Type
  {
    get => this.m_type;
    set
    {
      this.m_type = value;
      this.UpdateType();
    }
  }

  public string TextToDisplay
  {
    get => this.m_textToDisplay;
    set
    {
      this.m_textToDisplay = value;
      this.m_hyperlink.UpdateFieldResult(this.m_textToDisplay, true);
    }
  }

  public WPicture PictureToDisplay
  {
    get => this.m_picToDisplay;
    set
    {
      this.m_picToDisplay = value;
      this.SetImageToDisplay();
    }
  }

  internal WField Field => this.m_hyperlink;

  public string LocalReference
  {
    get => this.m_localReference;
    set
    {
      this.SetLocalReferenceValue(value);
      this.m_localReference = value;
    }
  }

  public Hyperlink(WField hyperlink)
  {
    this.CheckHyperlink(hyperlink);
    this.m_hyperlink = hyperlink;
    this.Parse();
  }

  private void CheckHyperlink(WField field)
  {
    if (field == null)
      throw new ArgumentException("Argument is not a field", "hyperlink");
    if (field.FieldType != FieldType.FieldHyperlink)
      throw new ArgumentException("Argument is not a hyperlink", "hyperlink");
  }

  private void Parse()
  {
    string fieldValue = this.m_hyperlink.FieldValue;
    if (fieldValue == null || fieldValue == string.Empty)
      return;
    if (this.StartsWithExt(fieldValue, "\"http"))
    {
      this.m_type = HyperlinkType.WebLink;
      this.m_uriPath = fieldValue;
      if (!string.IsNullOrEmpty(this.m_hyperlink.LocalReference))
        this.m_localReference = this.m_hyperlink.LocalReference;
    }
    else if (this.StartsWithExt(fieldValue, "\"mailto"))
    {
      this.m_type = HyperlinkType.EMailLink;
      this.m_uriPath = fieldValue;
    }
    else if (this.m_hyperlink.IsLocal || this.m_hyperlink.FormattingString.IndexOf("l") != -1)
    {
      this.m_type = HyperlinkType.Bookmark;
      if (this.m_hyperlink.LocalReference != null && this.m_hyperlink.LocalReference != string.Empty)
      {
        this.m_bookmark = this.m_hyperlink.LocalReference;
        this.m_filePath = fieldValue;
      }
      else
        this.m_bookmark = fieldValue;
    }
    else
    {
      this.m_type = HyperlinkType.FileLink;
      this.m_filePath = fieldValue;
      if (!string.IsNullOrEmpty(this.m_hyperlink.LocalReference))
        this.m_localReference = this.m_hyperlink.LocalReference;
    }
    this.UpdateTextToDisplay();
    this.SetHyperlinkFieldCode();
  }

  private void UpdateTextToDisplay()
  {
    if (!(this.m_hyperlink.Owner is WParagraph))
      return;
    WParagraph wparagraph = this.m_hyperlink.OwnerParagraph;
    int index = this.m_hyperlink.FieldSeparator != null ? this.m_hyperlink.FieldSeparator.Index + 1 : -1;
    if (index == -1 || wparagraph.Items.Count <= index)
      return;
    for (ParagraphItem paragraphItem1 = wparagraph.Items[index]; !(paragraphItem1 is WFieldMark); paragraphItem1 = wparagraph.Items[index])
    {
      ParagraphItem paragraphItem2 = wparagraph.Items[index];
      switch (paragraphItem2)
      {
        case WTextRange _ when !(paragraphItem2 is WField):
          this.m_textToDisplay += (paragraphItem2 as WTextRange).Text;
          break;
        case WPicture _:
          this.m_picToDisplay = paragraphItem2 as WPicture;
          break;
      }
      ++index;
      if (wparagraph.Items.Count <= index)
      {
        if (wparagraph.NextSibling == null || !(wparagraph.NextSibling is WParagraph))
          break;
        wparagraph = wparagraph.NextSibling as WParagraph;
        while (wparagraph.NextSibling != null && wparagraph.NextSibling is WParagraph && wparagraph.Items.Count == 0)
          wparagraph = wparagraph.NextSibling as WParagraph;
        index = !(wparagraph.LastItem is WField) ? 0 : 1;
      }
    }
  }

  private void SetImageToDisplay()
  {
    if (!(this.m_hyperlink.Owner is WParagraph) || this.Field.FieldSeparator == null || this.Field.FieldSeparator.OwnerParagraph == null)
      return;
    this.Field.RemoveFieldResult();
    this.Field.FieldSeparator.OwnerParagraph.Items.Insert(this.Field.FieldSeparator.Index + 1, (IEntity) this.m_picToDisplay);
  }

  private int FindHyperlinkText(ref WParagraph ownerPara)
  {
    Entity entity = (Entity) this.m_hyperlink;
    int num = 0;
    for (; entity.NextSibling != null; entity = entity.NextSibling as Entity)
    {
      if (entity is WFieldMark && (entity as WFieldMark).Type == FieldMarkType.FieldSeparator && entity.NextSibling is WFieldMark && (entity.NextSibling as WFieldMark).Type == FieldMarkType.FieldEnd)
        return -1;
      if (entity.NextSibling is InlineShapeObject || entity.NextSibling is WFieldMark)
        ++num;
      else if (entity.NextSibling is WTextRange || entity.NextSibling is WPicture)
      {
        ++num;
        break;
      }
      if (num > 3)
      {
        num = -1;
        break;
      }
    }
    int index = this.m_hyperlink.GetIndexInOwnerCollection() + num;
    if (ownerPara.Items.Count <= index)
    {
      if (ownerPara.NextSibling == null || ownerPara == null)
        return -1;
      index = !(ownerPara.LastItem is WField) ? 0 : 1;
      ownerPara = ownerPara.NextSibling as WParagraph;
    }
    switch (ownerPara.Items[index])
    {
      case WTextRange _:
      case WPicture _:
        return index;
      default:
        return -1;
    }
  }

  private void UpdateType()
  {
    if (this.m_type == HyperlinkType.Bookmark)
    {
      this.m_hyperlink.IsLocal = true;
    }
    else
    {
      this.m_hyperlink.LocalReference = (string) null;
      this.m_hyperlink.m_formattingString = string.Empty;
    }
  }

  private void SetUriValue(string uri)
  {
    if (this.m_type != HyperlinkType.WebLink && this.m_type != HyperlinkType.EMailLink)
      throw new ArgumentException("Uri can be set only for \"WebLink\" or \"EMailLink\" types of hyperlink");
    uri = this.CheckUri(uri);
    uri = this.CheckValue(uri);
    this.m_hyperlink.m_fieldValue = uri;
    this.SetHyperlinkFieldCode();
  }

  private void SetBookmarkNameValue(string name)
  {
    if (this.m_type != HyperlinkType.Bookmark)
      throw new ArgumentException("Bookmark name can be set only for \"Bookmark\" type of hyperlink");
    name = this.CheckValue(name);
    this.m_hyperlink.m_fieldValue = name;
    this.SetHyperlinkFieldCode();
  }

  private void SetLocalReferenceValue(string localReference)
  {
    if (this.m_type != HyperlinkType.WebLink && this.m_type != HyperlinkType.FileLink)
      throw new ArgumentException("Local reference can be set only for \"FileLink\" or \"WebLink\" type of hyperlink");
    this.m_hyperlink.LocalReference = localReference;
    this.SetHyperlinkFieldCode();
  }

  private void SetFilePathValue(string filePath)
  {
    if (this.m_type != HyperlinkType.FileLink && this.m_type != HyperlinkType.Bookmark)
      throw new ArgumentException("File path can be set only for \"FileLink\" or \"Bookmark\" type of hyperlink");
    filePath = this.CheckPath(filePath);
    filePath = this.CheckValue(filePath);
    this.m_hyperlink.m_fieldValue = filePath;
    if (this.m_type == HyperlinkType.Bookmark)
      this.m_hyperlink.LocalReference = this.m_bookmark != null && !(this.m_bookmark == string.Empty) ? this.CheckValue(this.m_bookmark) : throw new ArgumentException("Bookmark name can't be null or empty. Bookmark name must be set before file path.");
    this.m_filePath = filePath;
    this.SetHyperlinkFieldCode();
  }

  private string CheckValue(string value)
  {
    if (!this.StartsWithExt(value, "\""))
      value = "\"" + value;
    if (!value.EndsWith("\""))
      value += "\"";
    return value;
  }

  private string CheckPath(string path)
  {
    char[] chArray = new char[1]{ '\\' };
    string[] strArray = path.Split(chArray);
    path = string.Empty;
    int index = 0;
    for (int length = strArray.Length; index < length; ++index)
    {
      if (strArray[index] != string.Empty)
      {
        path += strArray[index];
        if (index < length - 1)
          path += "\\\\";
      }
    }
    return path;
  }

  private string CheckUri(string uri)
  {
    uri = uri.Replace("\"", string.Empty);
    if (this.m_type == HyperlinkType.WebLink)
    {
      if (!uri.Contains("http://") && !uri.Contains("https://"))
      {
        if (this.StartsWithExt(uri.ToLower(), "www."))
          uri = "http://" + uri;
        else if (uri.Contains("@") && !this.StartsWithExt(uri.ToLower(), "mailto:"))
          uri = "mailto:" + uri;
      }
    }
    else if (!this.StartsWithExt(uri.ToLower(), "mailto:"))
      uri = "mailto:" + uri;
    return uri;
  }

  internal void SetHyperlinkFieldCode()
  {
    string empty = string.Empty;
    string str1;
    if (this.Type == HyperlinkType.Bookmark || this.m_hyperlink.IsLocal)
    {
      if (string.IsNullOrEmpty(this.m_hyperlink.LocalReference))
        str1 = "HYPERLINK \\l " + this.m_hyperlink.FieldValue;
      else
        str1 = $"HYPERLINK {this.m_hyperlink.FieldValue} \\l \"{this.m_hyperlink.LocalReference}\"";
    }
    else
      str1 = $"HYPERLINK {this.m_hyperlink.FieldValue} {this.m_hyperlink.LocalReference}";
    if (!string.IsNullOrEmpty(this.m_hyperlink.ScreenTip))
    {
      string str2 = $"\\o \"{this.m_hyperlink.ScreenTip}\"";
      str1 += str2;
      this.m_hyperlink.m_formattingString += str2;
    }
    if (this.m_hyperlink.Owner == null && !this.m_hyperlink.Document.IsInternalManipulation())
    {
      this.m_hyperlink.m_detachedFieldCode = str1;
    }
    else
    {
      if (!(this.m_hyperlink.NextSibling is WTextRange))
        return;
      (this.m_hyperlink.NextSibling as WTextRange).Text = str1;
    }
  }

  internal bool StartsWithExt(string text, string value) => text.StartsWithExt(value);
}
