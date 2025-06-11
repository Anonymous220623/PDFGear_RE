// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.Email.MailApiProvider
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace pdfeditor.Utils.Email;

internal static class MailApiProvider
{
  public static bool SendMessage(EmailMessage message)
  {
    MAPIHelper mapiHelper = new MAPIHelper();
    foreach (string email in (IEnumerable<string>) message.To)
      mapiHelper.AddRecipientTo(email);
    foreach (string email in (IEnumerable<string>) message.Cc)
      mapiHelper.AddRecipientCc(email);
    foreach (string email in (IEnumerable<string>) message.Bcc)
      mapiHelper.AddRecipientBcc(email);
    foreach (string strAttachmentFileName in (IEnumerable<string>) message.AttachmentFilePath)
      mapiHelper.AddAttachment(strAttachmentFileName);
    return mapiHelper.SendMailPopup(message.Subject, message.Body);
  }

  public static bool HasThirdPartyClient()
  {
    if (!string.IsNullOrEmpty(MailApiProvider.GetDefaultClientName()))
      return true;
    string[] names = MailApiProvider.GetNames();
    return names != null && names.Length != 0 && (names.Length != 1 || !string.Equals(names[0], "Hotmail", StringComparison.OrdinalIgnoreCase));
  }

  private static string GetDefaultClientName()
  {
    using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Clients\\Mail", false))
      return registryKey?.GetValue((string) null)?.ToString() ?? string.Empty;
  }

  private static string[] GetNames()
  {
    try
    {
      using (RegistryKey registryKey1 = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Clients\\Mail", false))
        return (registryKey1 != null ? ((IEnumerable<string>) registryKey1.GetSubKeyNames()).Where<string>((Func<string, bool>) (clientName =>
        {
          try
          {
            using (RegistryKey registryKey2 = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Clients\\Mail\\" + clientName))
              return registryKey2?.GetValue("DllPath") != null;
          }
          catch
          {
            return false;
          }
        })).ToArray<string>() : (string[]) null) ?? Array.Empty<string>();
    }
    catch
    {
    }
    return Array.Empty<string>();
  }
}
