using System;
using System.Configuration;
using System.Drawing;
using System.Globalization;
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
            label3.Text = BuildAzerbaijaniText();
            RefreshDiskSpace();
        }

        private void RefreshDiskSpace()
        {
            int thresholdPercent = int.TryParse(ConfigurationManager.AppSettings["DiskSpaceThresholdPercent"], out int tp) ? tp : 10;
            double thresholdGB = double.TryParse(ConfigurationManager.AppSettings["DiskSpaceThresholdGB"], NumberStyles.Any, CultureInfo.InvariantCulture, out double tg) ? tg : 5.0;
            string condition = ConfigurationManager.AppSettings["DiskSpaceCondition"] ?? "OR";

            try
            {
                DriveInfo driveInfo = new DriveInfo(Path.GetPathRoot(Environment.SystemDirectory));

                double totalSpace = driveInfo.TotalSize;
                double freeSpace = driveInfo.TotalFreeSpace;
                double percentageUsed = (totalSpace - freeSpace) / totalSpace * 100;
                double remainingSpace = freeSpace / (1024 * 1024 * 1024);
                double percentageRemaining = 100 - percentageUsed;

                label1.Text = $"Free Space: {remainingSpace:N2} GB ({percentageRemaining:N2}%)";
                label2.Text = $"Total Space: {totalSpace / (1024 * 1024 * 1024):N2} GB";
                progressBar1.Value = (int)percentageUsed;

                bool belowPercent = percentageRemaining < thresholdPercent;
                bool belowGB = remainingSpace < thresholdGB;

                bool triggered;
                if (condition == "%")
                    triggered = belowPercent;
                else if (condition == "GB")
                    triggered = belowGB;
                else if (condition == "AND")
                    triggered = belowPercent && belowGB;
                else
                    triggered = belowPercent || belowGB; // OR is default

                progressBar1.ForeColor = triggered ? Color.Red : Color.Green;

                if (triggered)
                    OnDiskSpaceLow();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error occurred while refreshing disk space information: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected virtual void OnDiskSpaceLow()
        {
            DiskSpaceLow?.Invoke(this, EventArgs.Empty);
        }

        private string BuildThresholdDescription(bool english)
        {
            int thresholdPercent = int.TryParse(ConfigurationManager.AppSettings["DiskSpaceThresholdPercent"], out int tp) ? tp : 10;
            double thresholdGB = double.TryParse(ConfigurationManager.AppSettings["DiskSpaceThresholdGB"], NumberStyles.Any, CultureInfo.InvariantCulture, out double tg) ? tg : 5.0;
            string condition = ConfigurationManager.AppSettings["DiskSpaceCondition"] ?? "OR";

            if (english)
            {
                if (condition == "%")  return $"{thresholdPercent}%";
                if (condition == "GB") return $"{thresholdGB:N1} GB";
                if (condition == "AND") return $"{thresholdPercent}% and {thresholdGB:N1} GB";
                return $"{thresholdPercent}% or {thresholdGB:N1} GB";
            }
            else
            {
                if (condition == "%")  return $"{thresholdPercent}%";
                if (condition == "GB") return $"{thresholdGB:N1} GB";
                if (condition == "AND") return $"{thresholdPercent}% və {thresholdGB:N1} GB";
                return $"{thresholdPercent}% və ya {thresholdGB:N1} GB";
            }
        }

        private string BuildAzerbaijaniText()
        {
            string desc = BuildThresholdDescription(english: false);
            return $"Diqqət!\r\nSizin komputerinizin sistem diskində boş yer {desc}-dən aşağıdır.\r\nZəhmət olmasa, sistem diskinizdə (\"C\" diski) olan böyükhəcmli fayllarınızı digər disklərə (\"D\" və s.) köçürün və kömək lazım olarsa Helpdesk-ə müraciət edin.";
        }

        private string BuildEnglishText()
        {
            string desc = BuildThresholdDescription(english: true);
            return $"Attention!\r\nThe free space in your computer is below {desc}.\r\nPlease move large files from your system disk (C:) to other disks (such as D:). \r\nIf you need assistance please ask Helpdesk.";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label3.Text = BuildAzerbaijaniText();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            label3.Text = BuildEnglishText();
        }
    }
}
