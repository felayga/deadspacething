using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;

namespace dead_space_config
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// Yay, http://www.pinvoke.net/default.aspx/user32.mapvirtualkey
        /// </summary>
        [DllImport("user32.dll")]
        private static extern int MapVirtualKey(uint uCode, uint uMapType);

        const uint MAPVK_VK_TO_VSC = 0x00;
        const uint MAPVK_VSC_TO_VK = 0x01;
        const uint MAPVK_VK_TO_CHAR = 0x02;
        const uint MAPVK_VSC_TO_VK_EX = 0x03;
        const uint MAPVK_VK_TO_VSC_EX = 0x04;

        List<KeyValuePair<string, int>>[] settings;
        byte[] file;

        public Form1()
        {
            InitializeComponent();
            settings = new List<KeyValuePair<string,int>>[3];

            settings[0] = new List<KeyValuePair<string,int>>();
            settings[0].Add(new KeyValuePair<string, int>("Move Forward", 0x14C));
            settings[0].Add(new KeyValuePair<string, int>("Move Back", 0xAC));
            settings[0].Add(new KeyValuePair<string, int>("Move Left", 0x5C));
            settings[0].Add(new KeyValuePair<string, int>("Move Right", 0x23C));
            settings[0].Add(new KeyValuePair<string, int>("RIG", 0x250));
            settings[0].Add(new KeyValuePair<string, int>("Activate", 0x70));
            settings[0].Add(new KeyValuePair<string, int>("Sprint", 0x264));
            settings[0].Add(new KeyValuePair<string, int>("Punch", 0x1B0));
            settings[0].Add(new KeyValuePair<string, int>("Foot Stomp", 0xD4));
            settings[0].Add(new KeyValuePair<string, int>("Weapon Slot Up", 0x1D8));
            settings[0].Add(new KeyValuePair<string, int>("Weapon Slot Down", 0x19C));
            settings[0].Add(new KeyValuePair<string, int>("Weapon Slot Left", 0x84));
            settings[0].Add(new KeyValuePair<string, int>("Weapon Slot Right", 0x28C));
            settings[0].Add(new KeyValuePair<string, int>("Breadcrumbs", 0x160));
            settings[0].Add(new KeyValuePair<string, int>("Quick Health Pack", 0x138));
            settings[0].Add(new KeyValuePair<string, int>("Map Hotkey", 0x228));
            settings[0].Add(new KeyValuePair<string, int>("Inventory Hotkey", 0x278));
            settings[0].Add(new KeyValuePair<string, int>("Missions Hotkey", 0x34));
            settings[0].Add(new KeyValuePair<string, int>("Log Database Hotkey", 0x214));
            settings[0].Add(new KeyValuePair<string, int>("Cancel (logs/menus)", 0x200));
            settings[0].Add(new KeyValuePair<string, int>("Pause Menu", 0));

            settings[1] = new List<KeyValuePair<string, int>>();
            settings[1].Add(new KeyValuePair<string, int>("Aim Mode", 0x1EC));
            settings[1].Add(new KeyValuePair<string, int>("Aim Mode (toggle)", 0x2A0));
            settings[1].Add(new KeyValuePair<string, int>("Reload", 0xE8));
            settings[1].Add(new KeyValuePair<string, int>("Fire", 0xC0));
            settings[1].Add(new KeyValuePair<string, int>("Secondary", 0xFC));
            settings[1].Add(new KeyValuePair<string, int>("TK Module", 0x188));
            settings[1].Add(new KeyValuePair<string, int>("Stasis Gun", 0x124));
            settings[1].Add(new KeyValuePair<string, int>("Zero-G Jump", 0x20));

            settings[2] = new List<KeyValuePair<string,int>>();
            settings[2].Add(new KeyValuePair<string, int>("Mouse Modifier", 0x98));
            settings[2].Add(new KeyValuePair<string, int>("Cursor Left", 0));
            settings[2].Add(new KeyValuePair<string, int>("Cursor Right", 0));
            settings[2].Add(new KeyValuePair<string, int>("Cursor Up", 0));
            settings[2].Add(new KeyValuePair<string, int>("Cursor Down", 0));
            settings[2].Add(new KeyValuePair<string, int>("Inventory", 0));
            settings[2].Add(new KeyValuePair<string, int>("Map", 0));
            settings[2].Add(new KeyValuePair<string, int>("Mission", 0));
            settings[2].Add(new KeyValuePair<string, int>("Log Database", 0));
            settings[2].Add(new KeyValuePair<string, int>("Select", 0));
            settings[2].Add(new KeyValuePair<string, int>("Back", 0x200));

            // +1 = alteratekey
            // +8 = canmouse
            // +9 = mousebuton

            file = null;// File.ReadAllBytes(@"C:\Users\HELLO\AppData\Local\Electronic Arts\Dead Space\controls.rmp");

            settingscrap(this.tableLayoutPanel1, settings[0], file);
            settingscrap(this.tableLayoutPanel2, settings[1], file);
            settingscrap(this.tableLayoutPanel3, settings[2], file);

            this.tableLayoutPanel5.Dock = DockStyle.None;
            this.tableLayoutPanel5.Width = 40960;
            this.tableLayoutPanel5.Dock = DockStyle.Fill;

            this.tableLayoutPanel6.Dock = DockStyle.None;
            this.tableLayoutPanel6.Width = 40960;
            this.tableLayoutPanel6.Dock = DockStyle.Fill;

            this.tableLayoutPanel7.Dock = DockStyle.None;
            this.tableLayoutPanel7.Width = 40960;
            this.tableLayoutPanel7.Dock = DockStyle.Fill;

            this.button1.Click += new EventHandler(button1_Click);
            base.KeyPreview = true;

            this.label11.MouseUp += new MouseEventHandler(label11_MouseUp);
            this.button2.Click += new EventHandler(button2_Click);
        }

        private void settingscrap(TableLayoutPanel tablelayoutpanel, List<KeyValuePair<string, int>> settings, byte[] file)
        {
            tablelayoutpanel.SuspendLayout();

            if (tablelayoutpanel.RowCount == settings.Count + 2)
            {
                foreach (Control control in tablelayoutpanel.Controls)
                {
                    if (!(control is Button)) continue;

                    Button buton = (Button)control;

                    int index = (int)buton.Tag;
                    if (index < 1) continue;

                    if (tablelayoutpanel.GetColumn(buton) < 3)
                    {
                        if (file != null)
                        {
                            string test = ((char)MapVirtualKey((uint)MapVirtualKey(file[index], MAPVK_VSC_TO_VK), MAPVK_VK_TO_CHAR)).ToString().Trim();
                            if (!string.IsNullOrEmpty(test) && test != "\0")
                            {
                                buton.Text = test;
                            }
                            else
                            {
                                buton.Text = ((Keys)MapVirtualKey(file[index], MAPVK_VSC_TO_VK)).ToString();
                            }
                        }
                        else
                        {
                            buton.Text = null;
                        }
                    }
                    else
                    {
                        if (file != null)
                        {
                            byte setting = file[index];
                            if (setting > 0)
                            {
                                setting = file[index + 1];
                                switch (setting)
                                {
                                    case 0:
                                        buton.Text = "LEFT BUTON";
                                        break;
                                    case 1:
                                        buton.Text = "RIGHT BUTON";
                                        break;
                                    case 2:
                                        buton.Text = "MIDDLE BUTON";
                                        break;
                                    case 255:
                                        buton.Text = "<none>";
                                        break;
                                    default:
                                        buton.Text = "BUTON #" + setting;
                                        break;
                                }
                                buton.Enabled = true;
                            }
                            else
                            {
                                buton.Text = "NO.";
                                buton.Enabled = false;
                            }
                        }
                        else
                        {
                            buton.Text = null;
                        }
                    }
                }
            }
            else
            {
                tablelayoutpanel.RowCount = settings.Count + 2;

                for (int n = 0; n < settings.Count; n++)
                {
                    int row = tablelayoutpanel.RowStyles.Count;

                    Label label = new Label();
                    label.Margin = new Padding(0);
                    label.Dock = DockStyle.Fill;
                    label.TextAlign = ContentAlignment.MiddleLeft;
                    label.Text = settings[n].Key;

                    tablelayoutpanel.Controls.Add(label, 0, row);

                    int settingspos = settings[n].Value;

                    Button firsty = new Button();
                    firsty.Margin = new Padding(0);
                    firsty.Dock = DockStyle.Fill;
                    firsty.Tag = settingspos;

                    if (settingspos > 0)
                    {
                        firsty.Click += new EventHandler(firsty_Click);
                    }
                    else
                    {
                        firsty.Tag = 0;
                        firsty.Text = "NO.";
                        firsty.Enabled = false;
                    }

                    tablelayoutpanel.Controls.Add(firsty, 1, row);

                    Button secondy = new Button();
                    secondy.Margin = new Padding(0);
                    secondy.Dock = DockStyle.Fill;
                    secondy.Tag = settingspos + 1;

                    if (settingspos > 0)
                    {
                        secondy.Click += new EventHandler(firsty_Click);
                    }
                    else
                    {
                        secondy.Tag = 0;
                        secondy.Text = "NO.";
                        secondy.Enabled = false;
                    }

                    tablelayoutpanel.Controls.Add(secondy, 2, row);


                    Button mousey = new Button();
                    mousey.Margin = new Padding(0);
                    mousey.Dock = DockStyle.Fill;
                    mousey.Tag = settingspos + 8;

                    if (settingspos > 0)
                    {
                        mousey.Click += new EventHandler(mousey_Click);
                    }
                    else
                    {
                        mousey.Tag = 0;
                        mousey.Text = "NO.";
                        mousey.Enabled = false;
                    }

                    tablelayoutpanel.Controls.Add(mousey, 3, row);

                    tablelayoutpanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                }
            }

            tablelayoutpanel.ResumeLayout();
        }

        void button1_Click(object sender, EventArgs e)
        {
            keyboard_clicked = null;
            mouse_clicked = null;

            this.label1.ForeColor = SystemColors.ControlText;
            this.label1.BackColor = SystemColors.Control;
            this.label1.Enabled = false;

            this.label11.ForeColor = SystemColors.ControlText;
            this.label11.BackColor = SystemColors.Control;
            this.label11.Enabled = false;

            this.button1.Enabled = false;
            this.button2.Enabled = false;
            this.tabControl1.Enabled = true;
        }

        void button2_Click(object sender, EventArgs e)
        {
            if (keyboard_clicked != null)
            {
                int index = (int)keyboard_clicked.Tag;
                file[index] = 0;
                keyboard_clicked.Text = "<none>";
            }
            if (mouse_clicked != null)
            {
                int index = (int)mouse_clicked.Tag;
                file[index] = 1;
                file[index + 1] = 255;
                mouse_clicked.Text = "<none>";
            }

            button1_Click(sender, e);
        }

        Button keyboard_clicked;
        void firsty_Click(object sender, EventArgs e)
        {
            keyboard_clicked = (Button)sender;

            this.tabControl1.Enabled = false;

            this.label1.ForeColor = SystemColors.HighlightText;
            this.label1.BackColor = SystemColors.Highlight;

            this.button1.Enabled = true;
            this.button2.Enabled = true;
            this.label1.Enabled = true;
        }

        protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            if (keyboard_clicked == null)
            {
                base.OnPreviewKeyDown(e);
                return;
            }

            int index = (int)keyboard_clicked.Tag;

            file[index] = (byte)MapVirtualKey((uint)e.KeyCode, MAPVK_VK_TO_VSC);
            string test = ((char)MapVirtualKey((uint)MapVirtualKey(file[index], MAPVK_VSC_TO_VK), MAPVK_VK_TO_CHAR)).ToString().Trim();
            if (!string.IsNullOrEmpty(test) && test != "\0")
            {
                keyboard_clicked.Text = test;
            }
            else
            {
                keyboard_clicked.Text = ((Keys)MapVirtualKey(file[index], MAPVK_VSC_TO_VK)).ToString();
            }

            button1_Click(this, e);
        }

        Button mouse_clicked;
        void mousey_Click(object sender, EventArgs e)
        {
            mouse_clicked = (Button)sender;

            this.tabControl1.Enabled = false;

            this.label11.ForeColor = SystemColors.HighlightText;
            this.label11.BackColor = SystemColors.Highlight;

            this.button1.Enabled = true;
            this.button2.Enabled = true;
            this.label11.Enabled = true;
        }

        void label11_MouseUp(object sender, MouseEventArgs e)
        {
            int buton = -1;
            switch (e.Button)
            {
                case System.Windows.Forms.MouseButtons.Left:
                    buton = 0;
                    break;
                case System.Windows.Forms.MouseButtons.Right:
                    buton = 1;
                    break;
                case System.Windows.Forms.MouseButtons.Middle:
                    buton = 2;
                    break;
                case System.Windows.Forms.MouseButtons.XButton1:
                    buton = 3;
                    break;
                case System.Windows.Forms.MouseButtons.XButton2:
                    buton = 4;
                    break;
            }
            if (buton >= 0)
            {
                int index = (int)mouse_clicked.Tag;
                file[index] = 1;
                file[index + 1] = (byte)buton;
                switch (buton)
                {
                    case 0:
                        mouse_clicked.Text = "LEFT BUTON";
                        break;
                    case 1:
                        mouse_clicked.Text = "RIGHT BUTON";
                        break;
                    case 2:
                        mouse_clicked.Text = "MIDDLE BUTON";
                        break;
                    default:
                        mouse_clicked.Text = "BUTON #" + buton;
                        break;
                }

                button1_Click(sender, e);
            }
            else
            {
                button2_Click(sender, e);
            }
        }

        private void oPENDEADSPACETHINGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.RestoreDirectory = true;
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Electronic Arts\Dead Space";
            dialog.Filter = "RUMP-BASED FILE|*.rmp";

            if (dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            try
            {
                using (FileStream _file = new FileStream(dialog.FileName, FileMode.Open, FileAccess.Read))
                {
                    file = new byte[0x2B1];
                    if (_file.Read(file, 0, 0x2B0) != 0x2B0)
                    {
                        if (MessageBox.Show("THAT DON'T LOOK LIKE A DEAD SPACE THING." + Environment.NewLine + "DO IT ANYWAY?", "HEY THEN", MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK)
                        {
                            file = null;
                        }
                    }
                }

                this.tabControl1.Enabled = file != null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Your computer is whining." + Environment.NewLine + Environment.NewLine + ex.Message);
            }

            settingscrap(this.tableLayoutPanel1, settings[0], file);
            settingscrap(this.tableLayoutPanel2, settings[1], file);
            settingscrap(this.tableLayoutPanel3, settings[2], file);
        }

        private void sAVEDEAFDSPACETHINGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.RestoreDirectory = true;
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Electronic Arts\Dead Space";
            dialog.Filter = "RUMP-BASED FILE|*.rmp";

            if (dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            try
            {
                using (FileStream _file = new FileStream(dialog.FileName, FileMode.Create, FileAccess.Write))
                {
                    _file.Write(file, 0, file.Length - 1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Your computer is whining." + Environment.NewLine + Environment.NewLine + ex.Message);
            }
        }

    }
}
