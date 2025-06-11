// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.DebugHelper
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net.Mail;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Windows;

#nullable disable
namespace Syncfusion.Windows.Shared;

[DesignTimeVisible(false)]
public static class DebugHelper
{
  private static MemoryStream m_streamInMemoryTraceInfo = new MemoryStream(1024 /*0x0400*/);
  private static MemoryStream m_streamInMemoryDebug = new MemoryStream(1024 /*0x0400*/);
  private static TraceListener m_listenerForWPFTraces;
  private static TraceListener m_listenerForDebug;
  private static GZipStream m_streamCompressedTraceInfo;
  private static GZipStream m_streamCompressedDebugOutput;

  public static void Attach()
  {
    AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(DebugHelper.CurrentDomain_UnhandledException);
    AppDomain.CurrentDomain.ProcessExit += new EventHandler(DebugHelper.CurrentDomain_ProcessExit);
    PresentationTraceSources.Refresh();
    DebugHelper.m_streamCompressedTraceInfo = new GZipStream((Stream) DebugHelper.m_streamInMemoryTraceInfo, CompressionMode.Compress);
    DebugHelper.m_streamCompressedDebugOutput = new GZipStream((Stream) DebugHelper.m_streamInMemoryDebug, CompressionMode.Compress);
    DebugHelper.m_listenerForWPFTraces = (TraceListener) new XmlWriterTraceListener((Stream) DebugHelper.m_streamCompressedTraceInfo);
    DebugHelper.m_listenerForDebug = (TraceListener) new TimedListener((Stream) DebugHelper.m_streamCompressedDebugOutput);
    DebugHelper.AddListener(PresentationTraceSources.DataBindingSource, DebugHelper.m_listenerForWPFTraces, new SourceLevels?());
    DebugHelper.AddListener(PresentationTraceSources.MarkupSource, DebugHelper.m_listenerForWPFTraces, new SourceLevels?());
    DebugHelper.AddListener(PresentationTraceSources.FreezableSource, DebugHelper.m_listenerForWPFTraces, new SourceLevels?());
    DebugHelper.AddListener(PresentationTraceSources.RoutedEventSource, DebugHelper.m_listenerForWPFTraces, new SourceLevels?());
    DebugHelper.AddListener(PresentationTraceSources.ResourceDictionarySource, DebugHelper.m_listenerForWPFTraces, new SourceLevels?(SourceLevels.All));
    DebugHelper.AddListener(PresentationTraceSources.DependencyPropertySource, DebugHelper.m_listenerForWPFTraces, new SourceLevels?());
    DebugHelper.AddListener(PresentationTraceSources.AnimationSource, DebugHelper.m_listenerForWPFTraces, new SourceLevels?());
    Debug.Listeners.Add(DebugHelper.m_listenerForDebug);
    Trace.Listeners.Add(DebugHelper.m_listenerForDebug);
  }

  [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
  private static extern int IsThemeActive();

  [DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
  private static extern int GetCurrentThemeName(
    StringBuilder pszThemeFileName,
    int dwMaxNameChars,
    StringBuilder pszColorBuff,
    int dwMaxColorChars,
    StringBuilder pszSizeBuff,
    int cchMaxSizeChars);

  private static void AddListener(TraceSource trace, TraceListener listener, SourceLevels? level)
  {
    SourceLevels sourceLevels = (SourceLevels) ((int) level ?? 15);
    trace.Listeners.Clear();
    trace.Listeners.Add(listener);
    trace.Switch.Level = sourceLevels;
  }

  private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
  {
    DebugHelper.SaveAllAndClose((Exception) null);
  }

  private static void SaveAllAndClose(Exception exception)
  {
    DebugHelper.m_listenerForWPFTraces.Close();
    DebugHelper.m_listenerForDebug.Close();
    DebugHelper.m_streamCompressedDebugOutput.Close();
    DebugHelper.m_streamCompressedTraceInfo.Close();
    FileStream fileStream1 = new FileStream("DebugLog.txt.gz", FileMode.Create);
    FileStream fileStream2 = new FileStream("PresentationFoundationTraces.xml.gz", FileMode.Create);
    byte[] array1 = DebugHelper.m_streamInMemoryDebug.ToArray();
    fileStream1.Write(array1, 0, array1.Length);
    byte[] array2 = DebugHelper.m_streamInMemoryTraceInfo.ToArray();
    fileStream2.Write(array2, 0, array2.Length);
    fileStream1.Close();
    fileStream2.Close();
    if (exception == null)
      return;
    StringWriter writer = new StringWriter();
    writer.NewLine = "\n";
    using (TextWriterTraceListener listener = new TextWriterTraceListener((TextWriter) writer))
    {
      Debug.Listeners.Add((TraceListener) listener);
      DebugHelper.WriteExceptionInfoToDebug(exception);
      Debug.Listeners.Remove((TraceListener) listener);
    }
    DebugHelper.SendMail(writer.ToString(), "DebugLog.txt.gz", "PresentationFoundationTraces.xml.gz");
  }

  private static void WriteExceptionInfoToDebug(Exception exception)
  {
    foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
    {
      string[] strArray = assembly.FullName.Split(',');
      string str = $"[{strArray[0]}]";
      strArray[1].Remove(0, strArray[1].IndexOf('=') + 1);
      int num = assembly.GlobalAssemblyCache ? 1 : 0;
    }
    StringBuilder pszThemeFileName = new StringBuilder(260);
    StringBuilder pszColorBuff = new StringBuilder(260);
    string str1;
    if (DebugHelper.GetCurrentThemeName(pszThemeFileName, pszThemeFileName.Capacity, pszColorBuff, pszColorBuff.Capacity, (StringBuilder) null, 0) == 0)
    {
      str1 = Path.GetFileNameWithoutExtension(pszThemeFileName.ToString());
      pszColorBuff.ToString();
    }
    else
      str1 = string.Empty;
    DebugHelper.IsThemeActive();
  }

  private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
  {
    DebugHelper.SaveAllAndClose((Exception) e.ExceptionObject);
  }

  private static void SendMail(string mailBody, string pathDebugLog, string pathWPFTraces)
  {
    MailAddress from = new MailAddress("tech@syncfusion.com", $"WPF Bug Tracking [{WindowsIdentity.GetCurrent().Name}]");
    MailAddress to = new MailAddress("tech@syncfusion.com", "Daniel Jebaraj");
    Application current = Application.Current;
    Window window = current.Windows.Count > 0 ? current.Windows[0] : (Window) null;
    SmtpClient smtpClient = new SmtpClient("syncfusion.com", 25);
    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
    MailMessage message = new MailMessage(from, to);
    message.Priority = MailPriority.High;
    message.Subject = "WPF Sample Crashed: " + (window != null ? window.Title : "Unknown");
    message.Body = mailBody;
    Attachment attachment1 = new Attachment(pathDebugLog, "application/zip");
    Attachment attachment2 = new Attachment(pathWPFTraces, "application/zip");
    message.Attachments.Add(attachment1);
    message.Attachments.Add(attachment2);
    try
    {
      smtpClient.Send(message);
    }
    catch (Exception ex)
    {
      int num = (int) MessageBox.Show($"Sorry, but the mail can not be send. [Exception: {ex.Message}]");
    }
  }
}
