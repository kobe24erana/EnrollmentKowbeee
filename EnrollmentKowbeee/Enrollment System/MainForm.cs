using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Enrollment_System
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void SubjectEntryButton_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            this.Hide();
            form1.Show();
        }

        private void SubjectScheduleEntryButton_Click(object sender, EventArgs e)
        {
            SubjectScheduleEntry subjectScheduleEntry= new SubjectScheduleEntry();
            subjectScheduleEntry.Show();
            this.Hide();
        }

        private void StudentEnrollmentEntryButton_Click(object sender, EventArgs e)
        {
            Hide();
            StudentEnrollmentEntry studentEnrollment = new StudentEnrollmentEntry();
            studentEnrollment.ShowDialog();
            Close();
        }
    }
}
