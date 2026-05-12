using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Windows.Forms;


namespace Diskspace_Notification
{
    public partial class Form1 : Form
    {
        // Define an event handler for DiskSpaceLow event
        public event EventHandler DiskSpaceLow;

        public Form1()
        {
            InitializeComponent();
            this.TopMost = true;

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            RefreshDiskSpace();
        }

        private void RefreshDiskSpace()
        {
            int threshold = int.TryParse(ConfigurationManager.AppSettings["DiskSpaceThresholdPercent"], out int t) ? t : 10;

            try
            {
                DriveInfo driveInfo = new DriveInfo(Path.GetPathRoot(Environment.SystemDirectory));

                double totalSpace = driveInfo.TotalSize;
                double freeSpace = driveInfo.TotalFreeSpace;
                double usedSpace = totalSpace - freeSpace;
                double percentageUsed = (usedSpace / totalSpace) * 100;
                double remainingSpace = freeSpace / (1024 * 1024 * 1024);
                double percentageRemaining = 100 - percentageUsed;

                label1.Text = $"Free Space: {remainingSpace:N2} GB ({percentageRemaining:N2}%)";
                label2.Text = $"Total Space: {totalSpace / (1024 * 1024 * 1024):N2} GB";
                progressBar1.Value = (int)percentageUsed;

                progressBar1.ForeColor = percentageRemaining < threshold ? Color.Red : Color.Green;

                if (percentageRemaining < threshold)
                    OnDiskSpaceLow();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error occurred while refreshing disk space information: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Raise the DiskSpaceLow event
        protected virtual void OnDiskSpaceLow()
        {
            DiskSpaceLow?.Invoke(this, EventArgs.Empty);
        }
        private string label3OriginalText = "Diqqət!\r\nSizin komputerinizin sistem diskində boş yer 10%-dən aşağıdır.\r\nZəhmət olmasa, sistem diskinizdə (\"C\" diski) olan böyükhəcmli fayllarınızı digər disklərə (\"D\" və s.) köçürün və kömək lazım olarsa Helpdesk-ə müraciət edin.";
        private string label3EnglishText = "Attention!\r\nThe free space in your computer is below 10%.\r\nPlease move large files from your system disk (C:) to other disks (such as D:). \r\nIf you need assistance please ask Helpdesk.";
        private void button3_Click(object sender, EventArgs e)
        {
            label3.Text = label3OriginalText; // Set label3 text to original language
        }

        private void button4_Click(object sender, EventArgs e)
        {
            label3.Text = label3EnglishText; // Set label3 text to English
        }
    }
}
