using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace windired
{
    public partial class Form1 : Form
    {
        string currentDirectory;

        private void UpdateContents(string[] files, string[] directories)
        {
            listBox1.Items.Clear();

            foreach (string f in files)
            {
                listBox1.Items.Add(new DirectoryContent(false, f));
            }

            foreach (string d in directories)
            {
                listBox1.Items.Add(new DirectoryContent(true, d));
            }

            listBox1.SetSelected(0, true);
       }
        public Form1()
        {
            InitializeComponent();
            currentDirectory = System.Environment.CurrentDirectory;

            string[] files = Directory.GetFiles(currentDirectory);
            string[] directories = Directory.GetDirectories(currentDirectory);
            UpdateContents(files, directories);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void listBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case 'j':
                    if (listBox1.SelectedIndex < listBox1.Items.Count - 1)
                    {
                        listBox1.SetSelected(listBox1.SelectedIndex + 1, true);
                    }
                    break;
                case 'k':
                    if (listBox1.SelectedIndex != 0)
                    {
                        listBox1.SetSelected(listBox1.SelectedIndex - 1, true);
                    }
                    break;
                case '\r':
                    DirectoryContent c = (DirectoryContent)listBox1.SelectedItem;
                    if (c.IsDirectory())
                    {
                        string dstDirectory = c.ToString();
                        string[] files = Directory.GetFiles(dstDirectory);
                        string[] directories = Directory.GetDirectories(dstDirectory);

                        UpdateContents(files, directories);
                        this.currentDirectory = dstDirectory;
                    }
                    else
                    {
                        ProcessStartInfo psi = new ProcessStartInfo(c.ToString());
                        Process.Start(psi);
                    }
                    break;
                case '^':
                    DirectoryInfo parentInfo = Directory.GetParent(currentDirectory);
                    if (parentInfo != null)
                    {
                        string dstDirectory = parentInfo.ToString();

                        string[] files = Directory.GetFiles(dstDirectory);
                        string[] directories = Directory.GetDirectories(dstDirectory);

                        UpdateContents(files, directories);
                        this.currentDirectory = dstDirectory.ToString();
                    }
                    break;
            }
        }
    }

    class DirectoryContent
    {
        private bool isDir;
        private string path;

        public DirectoryContent(bool isDir, string path)
        {
            this.isDir = isDir;
            this.path = path;
        }

        public override string ToString()
        {
            return path;
        }

        public bool IsDirectory()
        {
            return this.isDir;
        }
    }
}
