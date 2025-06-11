// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.app_beep_callback
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using System.Runtime.InteropServices;

#nullable disable
namespace Patagames.Pdf;

/// <summary>Causes the system to play a sound.</summary>
/// <param name="pThis">Pointer to the interface structure itself</param>
/// <param name="nType">The sound type. 0 - Error; 1 - Warning; 2 - Question; 3 - Status; 4 - Default (default value)</param>
/// <remarks>Required: Yes.</remarks>
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate void app_beep_callback([MarshalAs(UnmanagedType.LPStruct)] IPDF_JSPLATFORM pThis, BeepTypes nType);
