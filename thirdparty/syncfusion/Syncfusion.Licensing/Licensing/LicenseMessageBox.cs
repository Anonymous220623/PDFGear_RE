// Decompiled with JetBrains decompiler
// Type: Syncfusion.Licensing.LicenseMessageBox
// Assembly: Syncfusion.Licensing, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=632609b4d040f6b4
// MVID: 46253E3A-52AF-49F3-BF00-D811A33B7BC6
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Licensing.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Syncfusion.Licensing;

[EditorBrowsable(EditorBrowsableState.Never)]
public class LicenseMessageBox : Form
{
  private IContainer components;
  private Button btn_Close;
  public LinkLabel lnk_Message;

  public LicenseMessageBox() => this.InitializeComponent();

  private void btn_Close_Click(object sender, EventArgs e) => this.Close();

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (LicenseMessageBox));
    this.btn_Close = new Button();
    this.lnk_Message = new LinkLabel();
    this.SuspendLayout();
    this.btn_Close.Location = new Point(497, 155);
    this.btn_Close.Name = "btn_Close";
    this.btn_Close.Size = new Size(101, 39);
    this.btn_Close.TabIndex = 0;
    this.btn_Close.Text = "Close";
    this.btn_Close.UseVisualStyleBackColor = true;
    this.btn_Close.Click += new EventHandler(this.btn_Close_Click);
    this.lnk_Message.Location = new Point(23, 19);
    this.lnk_Message.Name = "lnk_Message";
    this.lnk_Message.Size = new Size(575, 131);
    this.lnk_Message.TabIndex = 1;
    this.lnk_Message.TabStop = true;
    this.lnk_Message.Text = "Message";
    this.AutoScaleDimensions = new SizeF(9f, 20f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.BackColor = Color.White;
    this.ClientSize = new Size(624, 204);
    this.ControlBox = false;
    this.Controls.Add((Control) this.lnk_Message);
    this.Controls.Add((Control) this.btn_Close);
    this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
    this.Name = nameof (LicenseMessageBox);
    this.Text = "Syncfusion License";
    this.ResumeLayout(false);
  }
}
