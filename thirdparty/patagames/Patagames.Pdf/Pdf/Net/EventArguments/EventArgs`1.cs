// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.EventArguments.EventArgs`1
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;

#nullable disable
namespace Patagames.Pdf.Net.EventArguments;

/// <summary>
/// Represents the class that contain event data, and provides a value to use for various events.
/// </summary>
/// <typeparam name="T">The type of the property of the class.</typeparam>
public class EventArgs<T> : EventArgs
{
  /// <summary>
  /// Gets or sets a value, which depend on the type of event
  /// </summary>
  public T Value { get; set; }

  /// <summary>Construct EventArgs object</summary>
  /// <param name="value">The value with which object will be initialized</param>
  public EventArgs(T value) => this.Value = value;
}
