// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.app_response_callback
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Patagames.Pdf;

/// <summary>
/// Displays a dialog box containing a question and an entry field for the user to reply to the question.
/// </summary>
/// <param name="pThis">Pointer to the interface structure itself</param>
/// <param name="Question">The question to be posed to the user.</param>
/// <param name="Title">The title of the dialog box.</param>
/// <param name="Default">A default value for the answer to the question. If not specified, no default value is presented.</param>
/// <param name="cLabel">A short string to appear in front of and on the same line as the edit text field.</param>
/// <param name="Password">If true, indicates that the user's response should show as asterisks (*) or bullets (?) to mask the response, which might be sensitive information. The default is false.</param>
/// <param name="buffer">A string buffer allocated by SDK, to receive the user's response.</param>
/// <param name="buflen">The length of the buffer, number of bytes. Currently, It's always be 2048.</param>
/// <returns>Number of bytes the complete user input would actually require, not including trailing zeros, regardless of the value of the length parameter or the presence of the response buffer.</returns>
/// <remarks>Required: Yes.
/// No matter on what platform, the response buffer should be always written using UTF-16LE encoding. If a response buffer is
/// present and the size of the user input exceeds the capacity of the buffer as specified by the length parameter, only the
/// first "length" bytes of the user input are to be written to the buffer.
/// </remarks>
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate int app_response_callback(
  [MarshalAs(UnmanagedType.LPStruct)] IPDF_JSPLATFORM pThis,
  [MarshalAs(UnmanagedType.LPWStr)] string Question,
  [MarshalAs(UnmanagedType.LPWStr)] string Title,
  [MarshalAs(UnmanagedType.LPWStr)] string Default,
  [MarshalAs(UnmanagedType.LPWStr)] string cLabel,
  [MarshalAs(UnmanagedType.Bool)] bool Password,
  IntPtr buffer,
  int buflen);
