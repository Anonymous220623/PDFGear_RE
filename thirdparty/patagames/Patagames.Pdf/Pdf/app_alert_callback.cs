// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.app_alert_callback
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using System.Runtime.InteropServices;

#nullable disable
namespace Patagames.Pdf;

/// <summary>pop up a dialog to show warning or hint.</summary>
/// <param name="pThis">Pointer to the interface structure itself</param>
/// <param name="Msg">A string containing the message to be displayed.</param>
/// <param name="Title">The title of the dialog.</param>
/// <param name="Type">The type of button group. 0-OK(default); 1-OK,Cancel; 2-Yes,NO; 3-Yes, NO, Cancel.</param>
/// <param name="Icon">The icon type</param>
/// <returns>The return value could be the folowing type: 1-OK; 2-Cancel; 3-NO; 4-Yes;</returns>
/// <remarks>Required: Yes.</remarks>
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate DialogResults app_alert_callback(
  [MarshalAs(UnmanagedType.LPStruct)] IPDF_JSPLATFORM pThis,
  [MarshalAs(UnmanagedType.LPWStr)] string Msg,
  [MarshalAs(UnmanagedType.LPWStr)] string Title,
  ButtonTypes Type,
  IconTypes Icon);
