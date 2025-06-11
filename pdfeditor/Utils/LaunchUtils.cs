// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.LaunchUtils
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf.Net;
using System;
using System.Windows;

#nullable disable
namespace pdfeditor.Utils;

internal static class LaunchUtils
{
  private static string action;

  public static void Initialize(StartupEventArgs e)
  {
    int num = Array.IndexOf<string>(e.Args, "-action");
    if (num == -1 || e.Args.Length <= num + 1)
      return;
    LaunchUtils.action = e.Args[num + 1];
  }

  public static void OnDocumentLoaded(PdfDocument document)
  {
    if (string.IsNullOrEmpty(LaunchUtils.action))
      return;
    string action = LaunchUtils.action;
    LaunchUtils.action = (string) null;
    LaunchActionInvokedEventHandler launchActionInvoked = LaunchUtils.LaunchActionInvoked;
    if (launchActionInvoked == null)
      return;
    launchActionInvoked(document, new LaunchActionInvokedEventArgs(action));
  }

  public static void DoLaunchAction()
  {
    if (!(LaunchUtils.action == "new:CreatedFile"))
      return;
    string action = LaunchUtils.action;
    LaunchUtils.action = (string) null;
    LaunchActionInvokedEventHandler launchActionInvoked = LaunchUtils.LaunchActionInvoked;
    if (launchActionInvoked == null)
      return;
    launchActionInvoked((PdfDocument) null, new LaunchActionInvokedEventArgs(action));
  }

  public static event LaunchActionInvokedEventHandler LaunchActionInvoked;
}
