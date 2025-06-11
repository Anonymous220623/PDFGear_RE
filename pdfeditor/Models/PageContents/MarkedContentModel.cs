// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.PageContents.MarkedContentModel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.BasicTypes;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace pdfeditor.Models.PageContents;

public class MarkedContentModel : IEquatable<MarkedContentModel>
{
  public MarkedContentModel(
    string tag,
    Dictionary<string, PdfTypeBase> parameters,
    PropertyListTypes paramType,
    bool hasMCID)
  {
    this.Tag = tag;
    this.Parameters = parameters;
    this.ParamType = paramType;
    this.HasMCID = hasMCID;
  }

  public string Tag { get; }

  public Dictionary<string, PdfTypeBase> Parameters { get; }

  public PropertyListTypes ParamType { get; }

  public bool HasMCID { get; }

  public PdfMarkedContent ToMarkedContent()
  {
    PdfTypeDictionary parameters = PdfTypeDictionary.Create();
    if (this.Parameters != null && this.Parameters.Count > 0)
    {
      foreach (KeyValuePair<string, PdfTypeBase> parameter in this.Parameters)
        parameters[parameter.Key] = parameter.Value;
    }
    return new PdfMarkedContent(this.Tag, this.HasMCID, this.ParamType, parameters);
  }

  public static MarkedContentModel Create(PdfMarkedContent markedContent)
  {
    if (markedContent == null)
      return (MarkedContentModel) null;
    PdfTypeDictionary parameters = markedContent.Parameters;
    Dictionary<string, PdfTypeBase> dictionary = parameters != null ? parameters.ToDictionary<KeyValuePair<string, PdfTypeBase>, string, PdfTypeBase>((Func<KeyValuePair<string, PdfTypeBase>, string>) (c => c.Key), (Func<KeyValuePair<string, PdfTypeBase>, PdfTypeBase>) (c => c.Value.Clone())) : (Dictionary<string, PdfTypeBase>) null;
    return new MarkedContentModel(markedContent.Tag, dictionary, markedContent.ParamType, markedContent.HasMCID);
  }

  public bool Equals(MarkedContentModel other)
  {
    if ((other == null || !(this.Tag == other.Tag) || this.ParamType != other.ParamType ? 0 : (this.HasMCID == other.HasMCID ? 1 : 0)) == 0)
      return false;
    if (this.Parameters == null && other.Parameters == null)
      return true;
    return this.Parameters != null && other.Parameters != null && this.Parameters.Count == other.Parameters.Count && this.Parameters.Select<KeyValuePair<string, PdfTypeBase>, (string, PdfTypeBase)>((Func<KeyValuePair<string, PdfTypeBase>, (string, PdfTypeBase)>) (c => (c.Key, c.Value))).SequenceEqual<(string, PdfTypeBase)>(other.Parameters.Select<KeyValuePair<string, PdfTypeBase>, (string, PdfTypeBase)>((Func<KeyValuePair<string, PdfTypeBase>, (string, PdfTypeBase)>) (c => (c.Key, c.Value))));
  }

  public override bool Equals(object obj) => obj is MarkedContentModel other && this.Equals(other);

  public override int GetHashCode()
  {
    return HashCode.Combine<string, Dictionary<string, PdfTypeBase>, PropertyListTypes, bool>(this.Tag, this.Parameters, this.ParamType, this.HasMCID);
  }
}
