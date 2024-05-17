using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Enrollment_System
{
    public partial class SubjectScheduleEntry : Form
    {
        public SubjectScheduleEntry()
        {
            InitializeComponent();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Appsdev\ERANA_KOBE.accdb";
        bool subjectSearch = false;
        private void SaveButton_Click(object sender, EventArgs e)
        {
            bool conflict = false;
            if (subjectSearch == true)
            {
                try
                {


                    OleDbConnection thisConnection2 = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Appsdev\ERANA_KOBE.accdb");
                    thisConnection2.Open();

                    string commandText2 = "SELECT * FROM SUBJECTSCHEDFILE";
                    OleDbCommand thisCommand2 = thisConnection2.CreateCommand();
                    thisCommand2.CommandText = commandText2;

                    OleDbDataReader thisReader2 = thisCommand2.ExecuteReader();


                    if (conflict == false)
                    {
                        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Appsdev\ERANA_KOBE.accdb";
                        OleDbConnection thisConnection = new OleDbConnection(connectionString);
                        thisConnection.Open();

                        string commandText = "SELECT * FROM SUBJECTSCHEDFILE";
                        OleDbDataAdapter thisAdapter = new OleDbDataAdapter(commandText, thisConnection);
                        OleDbCommandBuilder thisCommandBuilder = new OleDbCommandBuilder(thisAdapter);
                        DataSet thisDataSet = new DataSet();
                        thisAdapter.Fill(thisDataSet, "SUBJECTSCHEDFILE");

                        //setup primary key
                        DataColumn[] keys = new DataColumn[2];// DataColumn array is named keys
                                                              // assgin the first element of the keys arrray, keys[0] to the ProductID column in Product tabel.
                        keys[0] = thisDataSet.Tables["SUBJECTSCHEDFILE"].Columns["SFEDPCODE"];

                        // assign the array keys to the PrimaryKey property of the OrderDetails DataTable object.
                        thisDataSet.Tables["SUBJECTSCHEDFILE"].PrimaryKey = keys;

                        //values to be searched
                        String[] valuesToSearch = new string[1];
                        valuesToSearch[0] = SubjectEDPCodeTextBox.Text;

                        DataRow findRow = thisDataSet.Tables["SUBJECTSCHEDFILE"].Rows.Find(valuesToSearch);

                        DataRow thisRow = thisDataSet.Tables["SUBJECTSCHEDFILE"].NewRow();
                        thisRow["SFEDPCODE"] = SubjectEDPCodeTextBox.Text;
                        thisRow["SFSUBJCODE"] = SubjectCodeTextBox.Text;
                        thisRow["SFSTARTTIME"] = TimeStartPicker.Value.ToString("hh:mm tt");
                        thisRow["SFENDTIME"] = TimeEndPicker.Value.ToString("hh:mm tt");
                        thisRow["SFDAYS"] = DaysTextBox.Text;
                        thisRow["SFROOM"] = RoomTextBox.Text;
                        thisRow["SFSECTION"] = SectionTextBox.Text;
                        thisRow["SFSCHOOLYEAR"] = SchoolYearTextBox.Text;


                        thisDataSet.Tables["SUBJECTSCHEDFILE"].Rows.Add(thisRow);
                        thisAdapter.Update(thisDataSet, "SUBJECTSCHEDFILE");
                        MessageBox.Show("Schedule Recorded!", "Subject Schedule");


                    }
                    else
                    {
                        MessageBox.Show("Schedule Conflict!", "Error");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                }
            }
            else
            {
                MessageBox.Show("Search Subject First", "Information Message");
            }
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            StudentEnrollmentEntry entry = new StudentEnrollmentEntry();
            entry.Show();
            this.Hide();
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }

        private void SubjectCodeTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                try
                {



                    OleDbConnection thisConnection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Appsdev\ERANA_KOBE.accdb");
                    thisConnection.Open();

                    string commandText = "SELECT * FROM SUBJECTFILE";
                    OleDbCommand thisCommand = thisConnection.CreateCommand();
                    thisCommand.CommandText = commandText;

                    OleDbDataReader thisReader = thisCommand.ExecuteReader();
                    while (thisReader.Read())
                    {
                        if (thisReader["SFSUBJCODE"].ToString().ToLower() == SubjectCodeTextBox.Text.ToLower())
                        {
                            DescriptionLabel.Text = thisReader["SFSUBJDESC"].ToString();
                            subjectSearch = true;
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Information Message");
                }
            }
        }
    }
}
