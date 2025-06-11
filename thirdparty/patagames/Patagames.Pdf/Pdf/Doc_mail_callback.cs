// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Doc_mail_callback
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System.Runtime.InteropServices;

#nullable disable
namespace Patagames.Pdf;

/// <summary>
/// Mails the data buffer as an attachment to all recipients, with or without user interaction.
/// </summary>
/// <param name="pThis">Pointer to the interface structure itself</param>
/// <param name="mailData">Pointer to the data buffer to be sent.Can be NULL.</param>
/// <param name="length">The size,in bytes, of the buffer pointed by mailData parameter.Can be 0.</param>
/// <param name="bUI">If true, the rest of the parameters are used in a compose-new-message window that is displayed to the user. If false, the cTo parameter is required and all others are optional.</param>
/// <param name="To">A semicolon-delimited list of recipients for the message.</param>
/// <param name="Subject">The subject of the message. The length limit is 64 KB.</param>
/// <param name="Cc">A semicolon-delimited list of CC recipients for the message.</param>
/// <param name="Bcc">A semicolon-delimited list of BCC recipients for the message.</param>
/// <param name="Msg">The content of the message. The length limit is 64 KB.</param>
/// <remarks>Required: Yes. If the parameter mailData is NULL or length is 0, the current document will be mailed as an attachment to all recipients.</remarks>
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate void Doc_mail_callback(
  [MarshalAs(UnmanagedType.LPStruct)] IPDF_JSPLATFORM pThis,
  [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] byte[] mailData,
  int length,
  [MarshalAs(UnmanagedType.Bool)] bool bUI,
  [MarshalAs(UnmanagedType.LPStr)] string To,
  [MarshalAs(UnmanagedType.LPStr)] string Subject,
  [MarshalAs(UnmanagedType.LPStr)] string Cc,
  [MarshalAs(UnmanagedType.LPStr)] string Bcc,
  [MarshalAs(UnmanagedType.LPStr)] string Msg);
