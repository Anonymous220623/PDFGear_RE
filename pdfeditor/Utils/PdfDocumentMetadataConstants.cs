// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.PdfDocumentMetadataConstants
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf.Enums;
using System;
using System.Collections.Generic;

#nullable disable
namespace pdfeditor.Utils;

public static class PdfDocumentMetadataConstants
{
  private static IReadOnlyList<PdfDocumentMetadata.MetaPropertyInfo> metaProperties;

  public static IReadOnlyList<PdfDocumentMetadata.MetaPropertyInfo> MetaProperties
  {
    get
    {
      IReadOnlyList<PdfDocumentMetadata.MetaPropertyInfo> metaProperties1 = PdfDocumentMetadataConstants.metaProperties;
      if (metaProperties1 != null)
        return metaProperties1;
      List<PdfDocumentMetadata.MetaPropertyInfo> metaProperties2 = new List<PdfDocumentMetadata.MetaPropertyInfo>();
      metaProperties2.Add((PdfDocumentMetadata.MetaPropertyInfo) PdfDocumentMetadataConstants.Title);
      metaProperties2.Add((PdfDocumentMetadata.MetaPropertyInfo) PdfDocumentMetadataConstants.Description);
      metaProperties2.Add((PdfDocumentMetadata.MetaPropertyInfo) PdfDocumentMetadataConstants.Author);
      metaProperties2.Add((PdfDocumentMetadata.MetaPropertyInfo) PdfDocumentMetadataConstants.Subject);
      metaProperties2.Add((PdfDocumentMetadata.MetaPropertyInfo) PdfDocumentMetadataConstants.Keywords);
      metaProperties2.Add((PdfDocumentMetadata.MetaPropertyInfo) PdfDocumentMetadataConstants.Creator);
      metaProperties2.Add((PdfDocumentMetadata.MetaPropertyInfo) PdfDocumentMetadataConstants.Producer);
      metaProperties2.Add((PdfDocumentMetadata.MetaPropertyInfo) PdfDocumentMetadataConstants.CreationDate);
      metaProperties2.Add((PdfDocumentMetadata.MetaPropertyInfo) PdfDocumentMetadataConstants.ModificationDate);
      metaProperties2.Add((PdfDocumentMetadata.MetaPropertyInfo) PdfDocumentMetadataConstants.Trapped);
      PdfDocumentMetadataConstants.metaProperties = (IReadOnlyList<PdfDocumentMetadata.MetaPropertyInfo>) metaProperties2;
      return (IReadOnlyList<PdfDocumentMetadata.MetaPropertyInfo>) metaProperties2;
    }
  }

  public static PdfDocumentMetadata.MetaPropertyInfo<string> Title
  {
    get
    {
      PdfDocumentMetadata.MetaPropertyInfo<string> title = new PdfDocumentMetadata.MetaPropertyInfo<string>();
      title.DocumentTag = new DocumentTags?(DocumentTags.Title);
      title.InfoDictionaryKey = nameof (Title);
      title.XmpPropertyKey = "title";
      title.XmpPropertyNameSpace = "http://purl.org/dc/elements/1.1/";
      title.XmpPropertyType = PdfDocumentMetadata.XmpPropertyType.LocalizedString;
      return title;
    }
  }

  public static PdfDocumentMetadata.MetaPropertyInfo<string> Description
  {
    get
    {
      PdfDocumentMetadata.MetaPropertyInfo<string> description = new PdfDocumentMetadata.MetaPropertyInfo<string>();
      description.DocumentTag = new DocumentTags?();
      description.InfoDictionaryKey = (string) null;
      description.XmpPropertyKey = "description";
      description.XmpPropertyNameSpace = "http://purl.org/dc/elements/1.1/";
      description.XmpPropertyType = PdfDocumentMetadata.XmpPropertyType.LocalizedString;
      return description;
    }
  }

  public static PdfDocumentMetadata.MetaPropertyInfo<string[]> Author
  {
    get
    {
      PdfDocumentMetadata.MetaPropertyInfo<string[]> author = new PdfDocumentMetadata.MetaPropertyInfo<string[]>();
      author.DocumentTag = new DocumentTags?(DocumentTags.Author);
      author.InfoDictionaryKey = nameof (Author);
      author.XmpPropertyKey = "creator";
      author.XmpPropertyNameSpace = "http://purl.org/dc/elements/1.1/";
      author.XmpPropertyType = PdfDocumentMetadata.XmpPropertyType.ArrayOrdered;
      return author;
    }
  }

  public static PdfDocumentMetadata.MetaPropertyInfo<string> Subject
  {
    get
    {
      PdfDocumentMetadata.MetaPropertyInfo<string> subject = new PdfDocumentMetadata.MetaPropertyInfo<string>();
      subject.DocumentTag = new DocumentTags?(DocumentTags.Subject);
      subject.InfoDictionaryKey = nameof (Subject);
      subject.XmpPropertyKey = "description";
      subject.XmpPropertyNameSpace = "http://purl.org/dc/elements/1.1/";
      subject.XmpPropertyType = PdfDocumentMetadata.XmpPropertyType.LocalizedString;
      return subject;
    }
  }

  public static PdfDocumentMetadata.MetaPropertyInfo<string> Keywords
  {
    get
    {
      PdfDocumentMetadata.MetaPropertyInfo<string> keywords = new PdfDocumentMetadata.MetaPropertyInfo<string>();
      keywords.DocumentTag = new DocumentTags?(DocumentTags.Keywords);
      keywords.InfoDictionaryKey = nameof (Keywords);
      keywords.XmpPropertyKey = nameof (Keywords);
      keywords.XmpPropertyNameSpace = "http://ns.adobe.com/pdf/1.3/";
      keywords.XmpPropertyType = PdfDocumentMetadata.XmpPropertyType.String;
      return keywords;
    }
  }

  public static PdfDocumentMetadata.MetaPropertyInfo<string> Creator
  {
    get
    {
      PdfDocumentMetadata.MetaPropertyInfo<string> creator = new PdfDocumentMetadata.MetaPropertyInfo<string>();
      creator.DocumentTag = new DocumentTags?(DocumentTags.Creator);
      creator.InfoDictionaryKey = nameof (Creator);
      creator.XmpPropertyKey = "CreatorTool";
      creator.XmpPropertyNameSpace = "http://ns.adobe.com/xap/1.0/";
      creator.XmpPropertyType = PdfDocumentMetadata.XmpPropertyType.String;
      return creator;
    }
  }

  public static PdfDocumentMetadata.MetaPropertyInfo<string> Producer
  {
    get
    {
      PdfDocumentMetadata.MetaPropertyInfo<string> producer = new PdfDocumentMetadata.MetaPropertyInfo<string>();
      producer.DocumentTag = new DocumentTags?(DocumentTags.Producer);
      producer.InfoDictionaryKey = nameof (Producer);
      producer.XmpPropertyKey = nameof (Producer);
      producer.XmpPropertyNameSpace = "http://ns.adobe.com/pdf/1.3/";
      producer.XmpPropertyType = PdfDocumentMetadata.XmpPropertyType.String;
      return producer;
    }
  }

  public static PdfDocumentMetadata.MetaPropertyInfo<DateTimeOffset> CreationDate
  {
    get
    {
      PdfDocumentMetadata.MetaPropertyInfo<DateTimeOffset> creationDate = new PdfDocumentMetadata.MetaPropertyInfo<DateTimeOffset>();
      creationDate.DocumentTag = new DocumentTags?(DocumentTags.CreationDate);
      creationDate.InfoDictionaryKey = nameof (CreationDate);
      creationDate.XmpPropertyKey = "CreateDate";
      creationDate.XmpPropertyNameSpace = "http://ns.adobe.com/xap/1.0/";
      creationDate.XmpPropertyType = PdfDocumentMetadata.XmpPropertyType.DateTimeOffset;
      return creationDate;
    }
  }

  public static PdfDocumentMetadata.MetaPropertyInfo<DateTimeOffset> ModificationDate
  {
    get
    {
      PdfDocumentMetadata.MetaPropertyInfo<DateTimeOffset> modificationDate = new PdfDocumentMetadata.MetaPropertyInfo<DateTimeOffset>();
      modificationDate.DocumentTag = new DocumentTags?(DocumentTags.ModificationDate);
      modificationDate.InfoDictionaryKey = "ModDate";
      modificationDate.XmpPropertyKey = "ModifyDate";
      modificationDate.XmpPropertyNameSpace = "http://ns.adobe.com/xap/1.0/";
      modificationDate.XmpPropertyType = PdfDocumentMetadata.XmpPropertyType.DateTimeOffset;
      return modificationDate;
    }
  }

  public static PdfDocumentMetadata.MetaPropertyInfo<string> Trapped
  {
    get
    {
      PdfDocumentMetadata.MetaPropertyInfo<string> trapped = new PdfDocumentMetadata.MetaPropertyInfo<string>();
      trapped.DocumentTag = new DocumentTags?(DocumentTags.Trapped);
      trapped.InfoDictionaryKey = nameof (Trapped);
      trapped.XmpPropertyKey = (string) null;
      trapped.XmpPropertyNameSpace = (string) null;
      trapped.XmpPropertyType = PdfDocumentMetadata.XmpPropertyType.None;
      return trapped;
    }
  }
}
