// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Actions.PdfAAction
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Net.BasicTypes;
using Patagames.Pdf.Net.Wrappers;
using Patagames.Pdf.Properties;
using System;

#nullable disable
namespace Patagames.Pdf.Net.Actions;

/// <summary>Represents the additional actions in a PDF document.</summary>
public class PdfAAction : PdfWrapper
{
  private System.Collections.Generic.Dictionary<string, PdfAction> _actions = new System.Collections.Generic.Dictionary<string, PdfAction>();
  private PdfDocument _doc;

  /// <summary>
  /// Creates new instance of <see cref="T:Patagames.Pdf.Net.Actions.PdfAAction" />.
  /// </summary>
  /// <param name="document">Pdf document.</param>
  protected PdfAAction(PdfDocument document) => this._doc = document;

  /// <summary>
  /// Creates a new instance of <see cref="T:Patagames.Pdf.Net.Actions.PdfAAction" /> and initialize it with specified dictionary
  /// </summary>
  /// <param name="document">Pdf document.</param>
  /// <param name="dictionary">The dictionary or indirect dictionary</param>
  protected PdfAAction(PdfDocument document, PdfTypeBase dictionary)
    : base(dictionary)
  {
    this._doc = document;
  }

  internal PdfAction GetActionAt(string key)
  {
    if (!this.Dictionary.ContainsKey(key))
    {
      this._actions[key] = (PdfAction) null;
      return (PdfAction) null;
    }
    if (!this._actions.ContainsKey(key) || this._actions[key] == null || this.Dictionary[key].Is<PdfTypeDictionary>() && this._actions[key].Handle != this.Dictionary[key].As<PdfTypeDictionary>().Handle)
      this._actions[key] = PdfAction.FromHandle(this._doc, this.Dictionary[key].As<PdfTypeDictionary>().Handle);
    return this._actions[key];
  }

  internal void SetActionAt(string key, PdfAction action)
  {
    if (action == null && this.Dictionary.ContainsKey(key))
      this.Dictionary.Remove(key);
    else if (action != null)
    {
      if (action.Dictionary.ObjectNumber == 0 && Pdfium.FPDFOBJ_GetParentObj(action.Dictionary.Handle) != IntPtr.Zero)
        throw new ArgumentException(string.Format(Error.err0067, (object) nameof (action), (object) "object"));
      PdfIndirectList list = PdfIndirectList.FromPdfDocument(this._doc);
      list.Add((PdfTypeBase) action.Dictionary);
      this.Dictionary.SetIndirectAt(key, list, (PdfTypeBase) action.Dictionary);
    }
    this._actions[key] = action;
  }
}
