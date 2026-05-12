using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private void button2_Click(object sender, EventArgs e)
        {
            // Example URL
            string url = "https://abasd.accessbank.az/servicedesk/customer/portal/1/create/383";

            // Open the URL in the default browser
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(url) { UseShellExecute = true });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Call method to refresh disk space information
            RefreshDiskSpace();
        }

        private void RefreshDiskSpace()
        {
            try
            {
                // Get information about the system drive
                DriveInfo driveInfo = new DriveInfo(Path.GetPathRoot(Environment.SystemDirectory));

                // Calculate the disk space usage percentage
                double totalSpace = driveInfo.TotalSize;
                double freeSpace = driveInfo.TotalFreeSpace;
                double usedSpace = totalSpace - freeSpace;
                double percentageUsed = (usedSpace / totalSpace) * 100;
                double remainingSpace = freeSpace / (1024 * 1024 * 1024); // Convert to GB
                double percentageRemaining = 100 - percentageUsed;


                // Update labels and progress bar
                //label1.Text = $"Free Space: {freeSpace / (1024 * 1024 * 1024):N2} GB";
                //label1.Text = $"Free Space: {freeSpace / (1024 * 1024 * 1024):N2} GB ({percentageUsed:N2}%)";
                label1.Text = $"Free Space: {remainingSpace:N2} GB ({percentageRemaining:N2}%)";
                label2.Text = $"Total Space: {totalSpace / (1024 * 1024 * 1024):N2} GB";
                progressBar1.Value = (int)percentageUsed;

                // Change the color of the progress bar based on remaining percentage
                if (percentageRemaining < 10)
                {
                    progressBar1.ForeColor = Color.Red;
                }
                else
                {
                    progressBar1.ForeColor = Color.Green;
                }

                // Check if free percentage is below 10%
                if (percentageRemaining < 10)
                {
                    // Trigger the DiskSpaceLow event
                    OnDiskSpaceLow();
                }
            }
            catch (Exception ex)
            {
                // Handle error
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
