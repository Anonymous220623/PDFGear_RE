// Decompiled with JetBrains decompiler
// Type: PDFLauncher.CustomControl.SaveFolderTextBox
// Assembly: PDFLauncher, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 94FE2CE7-EB91-4B8B-872D-74808A04C1E6
// Assembly location: C:\Program Files\PDFgear\PDFLauncher.exe

using Microsoft.WindowsAPICodePack.Dialogs;

#nullable disable
namespace PDFLauncher.CustomControl;

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
