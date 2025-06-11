// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.ActionTypes
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System.ComponentModel;

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>A type of action associated with bookmark</summary>
public enum ActionTypes : ulong
{
  /// <summary>Unsupported action type</summary>
  [Description("Unknown")] Unknown,
  /// <summary>Go to a destination within current document.</summary>
  [Description("GoTo")] CurrentDoc,
  /// <summary>Go to a destination within another document</summary>
  [Description("GoToR")] ExternalDoc,
  /// <summary>
  /// Universal Resource Identifier, including web pages and other Internet based resources
  /// </summary>
  [Description("URI")] Uri,
  /// <summary>Launch an application or open a file</summary>
  [Description("Launch")] Application,
  /// <summary>Begin reading an article thread.</summary>
  [Description("Thread")] Thread,
  /// <summary>Go to a destination in an embedded file.</summary>
  [Description("GoToE")] EmbeddedDoc,
  /// <summary>Play a sound.</summary>
  [Description("Sound")] Sound,
  /// <summary>Play a movie.</summary>
  [Description("Movie")] Movie,
  /// <summary>Set an annotation’s Hidden flag.</summary>
  [Description("Hide")] Hide,
  /// <summary>
  /// Execute an action predefined by the viewer application.
  /// </summary>
  [Description("Named")] Named,
  /// <summary>Send data to a uniform resource locator.</summary>
  [Description("SubmitForm")] SubmitForm,
  /// <summary>Set fields to their default values.</summary>
  [Description("ResetForm")] ResetForm,
  /// <summary>Import field values from a file.</summary>
  [Description("ImportData")] ImportData,
  /// <summary>Execute a JavaScript script.</summary>
  [Description("JavaScript")] JavaScript,
  /// <summary>Set the states of optional content groups.</summary>
  [Description("SetOCGState")] SetOCGState,
  /// <summary>Controls the playing of multimedia content.</summary>
  [Description("Rendition")] Rendition,
  /// <summary>
  /// Updates the display of a document, using a transition dictionary.
  /// </summary>
  [Description("Trans")] Transition,
  /// <summary>Set the current view of a 3D annotation</summary>
  [Description("GoTo3DView")] GoTo3DView,
}
