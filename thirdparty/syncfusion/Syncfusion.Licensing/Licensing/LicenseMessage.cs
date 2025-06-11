// Decompiled with JetBrains decompiler
// Type: Syncfusion.Licensing.LicenseMessage
// Assembly: Syncfusion.Licensing, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=632609b4d040f6b4
// MVID: 46253E3A-52AF-49F3-BF00-D811A33B7BC6
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Licensing.dll

using System;
using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Threading;

#nullable disable
namespace Syncfusion.Licensing;

internal class LicenseMessage
{
  private static string helpURL = string.Empty;

  public DialogResult DisplayMessage(string title, string message, string linkword)
  {
    LicenseMessageBox msgbox = new LicenseMessageBox();
    MatchCollection matchCollection = Regex.Matches(message, "https://.*?\\)");
    if (matchCollection.Count > 0)
      LicenseMessage.helpURL = matchCollection[0].ToString().Remove(matchCollection[0].Length - 1, 1);
    message = message.Replace($"help topic({LicenseMessage.helpURL})", "help topic");
    int length = message.Length;
    msgbox.Text = title;
    msgbox.BackColor = Color.White;
    msgbox.ShowInTaskbar = false;
    msgbox.ControlBox = true;
    msgbox.FormBorderStyle = FormBorderStyle.Fixed3D;
    msgbox.StartPosition = FormStartPosition.CenterScreen;
    msgbox.MaximizeBox = false;
    msgbox.MinimizeBox = false;
    msgbox.lnk_Message.Text = message;
    msgbox.lnk_Message.BackColor = Color.White;
    msgbox.lnk_Message.TextAlign = ContentAlignment.TopLeft;
    msgbox.lnk_Message.Font = new Font("Microsoft Sans Serif", 9f);
    msgbox.lnk_Message.ForeColor = Color.Black;
    msgbox.lnk_Message.AutoSize = false;
    msgbox.lnk_Message.LinkArea = new LinkArea(msgbox.lnk_Message.Text.IndexOf(linkword), linkword.Length);
    msgbox.lnk_Message.LinkClicked += new LinkLabelLinkClickedEventHandler(LicenseMessage.help_Click);
    DialogResult dialogResult = DialogResult.OK;
    if (Dispatcher.CurrentDispatcher != null)
      Dispatcher.CurrentDispatcher.BeginInvoke((Delegate) (() =>
      {
        if (msgbox == null || msgbox.IsDisposed)
          return;
        dialogResult = msgbox.ShowDialog();
        if (msgbox.lnk_Message != null)
          msgbox.lnk_Message.LinkClicked -= new LinkLabelLinkClickedEventHandler(LicenseMessage.help_Click);
        msgbox.Dispose();
      }), DispatcherPriority.Loaded);
    return dialogResult;
  }

  private static void help_Click(object sender, EventArgs e)
  {
    Process.Start(LicenseMessage.helpURL);
  }

  private delegate void Action();
}
