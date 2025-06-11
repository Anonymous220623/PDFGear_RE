// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.SaveFolderTextBox
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Microsoft.WindowsAPICodePack.Dialogs;

#nullable disable
namespace pdfeditor.Controls;

public class SaveFolderTextBox : PathTextBox
{
  protected override object CreateDialog(string initialDirectory, string filename)
  {
    CommonOpenFileDialog dialog = new CommonOpenFileDialog();
    dialog.IsFolderPicker = true;
    dialog.InitialDirectory = initialDirectory;
    return (object) dialog;
  }
}
