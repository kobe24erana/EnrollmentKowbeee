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
    public partial class StudentEnrollmentEntry : Form
    {
        public StudentEnrollmentEntry()
        {
            InitializeComponent();
        }

        private void StudentEnrollmentEntry_Load(object sender, EventArgs e)
        {
            
        }
        int totalUnits = 0;
        bool search = false;
        bool searching = false;
        bool outcome = false;
        bool add = false;
        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Appsdev\ERANA_KOBE.accdb";
        private void IDNumberTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                OleDbConnection thisConnection = new OleDbConnection(connectionString);
                thisConnection.Open();
                OleDbCommand thisCommand = thisConnection.CreateCommand();

                string sql = "SELECT * FROM STUDENTFILE";
                thisCommand.CommandText = sql;

                OleDbDataReader thisDataReader = thisCommand.ExecuteReader();

                bool found = false;

                string name = "";
                string course = "";
                string year = "";

                while (thisDataReader.Read())
                {
                    if (thisDataReader["STFSTUDIDNO"].ToString().ToLower() == IDNumberTextBox.Text.ToLower())
                    {
                        if (thisDataReader["STFSTUDMNAME"].ToString() == "")
                        {
                            name = thisDataReader["STFSTUDFNAME"].ToString() + " " + thisDataReader["STFSTUDLNAME"].ToString();
                        }
                        else
                        {
                            name = thisDataReader["STFSTUDLNAME"].ToString() + " " +
                               thisDataReader["STFSTUDFNAME"].ToString() + " " +
                               thisDataReader["STFSTUDMNAME"].ToString().Substring(0, 1);
                        }

                        course = thisDataReader["STFSTUDCOURSE"].ToString();
                        year = thisDataReader["STFSTUDYEAR"].ToString();
                        found = true;
                        outcome = true;
                        search = true;
                        searching = true;
                        break;


                    }
                    /*if (thisDataReader["SFSTUDIDNO"].ToString().Trim().ToUpper() == IDNoTextBox.Text.Trim().ToUpper())
                    {
                        found = true;

                        name = thisDataReader["SFSTUDLNAME"].ToString() + " " +
                               thisDataReader["SFSTUDFNAME"].ToString() + " " +
                               thisDataReader["SFSTUDMNAME"].ToString().Substring(0, 1);
                        course = thisDataReader["SFSTUDCOURSE"].ToString();
                        year = thisDataReader["STFSTUDYEAR"].ToString();
                        break;

                    }*/

                }


                if (found == false)
                    MessageBox.Show("Subject Code Not Found");
                else
                {
                    NameLabel.Text = name;
                    CourseLabel.Text = course;
                    YearLabel.Text = year;
                }
            }
        }

        private void EDPCodeTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                try
                {

                    OleDbConnection thisConnection = new OleDbConnection(connectionString);
                    thisConnection.Open();

                    string commandText = "SELECT * FROM ENROLLMENTDETAILFILE";
                    OleDbCommand thisCommand5 = thisConnection.CreateCommand();
                    thisCommand5.CommandText = commandText;

                    OleDbDataReader thisReader5 = thisCommand5.ExecuteReader();

                    bool alreadyAdded = false;

                    while (thisReader5.Read())
                    {
                        if (thisReader5["ENRDFSTUDID"].ToString() == IDNumberTextBox.Text)
                        {
                            alreadyAdded = true;
                            break;

                        }
                        else
                        {
                            continue;
                        }
                    }



                    if (alreadyAdded == false)
                    {
                        if (searching == true)
                        {
                            if (IDNumberTextBox.Text != "ID Number")
                            {
                                if (EDPCodeTextBox.Text != "EDP Code")
                                {
                                    bool duplicate = false;
                                    bool result = false;
                                    bool conflict = false;
                                    OleDbConnection thisConnection1 = new OleDbConnection(connectionString);
                                    thisConnection1.Open();

                                    string commandText1 = "SELECT * FROM SUBJECTSCHEDFILE";
                                    OleDbCommand thisCommand = thisConnection.CreateCommand();
                                    thisCommand.CommandText = commandText1;

                                    OleDbDataReader thisReader = thisCommand.ExecuteReader();
                                    while (thisReader.Read())
                                    {
                                        if (thisReader["SFEDPCODE"].ToString().Trim() == EDPCodeTextBox.Text)
                                        {
                                            OleDbConnection thisConnection2 = new OleDbConnection(connectionString);
                                            thisConnection2.Open();

                                            string commandText2 = "SELECT * FROM SUBJECTFILE";
                                            OleDbCommand thisCommand2 = thisConnection2.CreateCommand();
                                            thisCommand2.CommandText = commandText2;

                                            OleDbDataReader thisReader2 = thisCommand2.ExecuteReader();


                                            for (int size = 0; size < SubjectDataGridView.Rows.Count; size++)
                                            {
                                                if (SubjectDataGridView.Rows[size].Cells[0].Value.ToString().Trim() == EDPCodeTextBox.Text.Trim())
                                                {
                                                    duplicate = true;
                                                    result = true;
                                                }
                                            }
                                            if (duplicate == false)
                                            {
                                                while (thisReader2.Read())
                                                {
                                                    if (thisReader["SFSUBJCODE"].ToString().Trim() == thisReader2["SFSUBJCODE"].ToString().Trim())
                                                    {
                                                        for (int size = 0; size < SubjectDataGridView.Rows.Count; size++)
                                                        {
                                                            SubjectDataGridView.Rows[size].DefaultCellStyle.BackColor = Color.White;
                                                            DateTime startTime = DateTime.Parse(thisReader["SFSTARTTIME"].ToString());
                                                            DateTime endTime = DateTime.Parse(thisReader["SFENDTIME"].ToString());

                                                            DateTime start = DateTime.Parse(SubjectDataGridView.Rows[size].Cells[2].Value.ToString());
                                                            DateTime end = DateTime.Parse(SubjectDataGridView.Rows[size].Cells[3].Value.ToString());

                                                            if (thisReader2["SFDAYS"].ToString().Trim().Contains('M') && (SubjectDataGridView.Rows[size].Cells[4].ToString().Contains("M")))
                                                            {
                                                                if ((((start >= startTime && start < endTime) || (end > startTime && end <= endTime)) || (start < startTime && end > endTime)))
                                                                {
                                                                    SubjectDataGridView.Rows[size].DefaultCellStyle.BackColor = Color.Firebrick;
                                                                    conflict = true;
                                                                    break;
                                                                }
                                                            }
                                                            else if (thisReader2["SFDAYS"].ToString().Trim().Contains('T') && (SubjectDataGridView.Rows[size].Cells[4].ToString().Contains("T")))
                                                            {
                                                                if ((((start >= startTime && start < endTime) || (end > startTime && end <= endTime)) || (start < startTime && end > endTime)))
                                                                {
                                                                    SubjectDataGridView.Rows[size].DefaultCellStyle.BackColor = Color.Firebrick;
                                                                    conflict = true;
                                                                    break;
                                                                }
                                                            }

                                                            else if (thisReader2["SFDAYS"].ToString().Trim().Contains('W') && (SubjectDataGridView.Rows[size].Cells[4].ToString().Contains("W")))
                                                            {
                                                                if ((((start >= startTime && start < endTime) || (end > startTime && end <= endTime)) || (start < startTime && end > endTime)))
                                                                {
                                                                    SubjectDataGridView.Rows[size].DefaultCellStyle.BackColor = Color.Firebrick;
                                                                    conflict = true;
                                                                    break;
                                                                }
                                                            }

                                                            else if (thisReader2["SFDAYS"].ToString().Trim().Contains("TH") && (SubjectDataGridView.Rows[size].Cells[4].ToString().Contains("TH")))
                                                            {
                                                                if ((((start >= startTime && start < endTime) || (end > startTime && end <= endTime)) || (start < startTime && end > endTime)))
                                                                {
                                                                    SubjectDataGridView.Rows[size].DefaultCellStyle.BackColor = Color.Firebrick;
                                                                    conflict = true;
                                                                    break;
                                                                }
                                                            }

                                                            else if (thisReader2["SFDAYS"].ToString().Trim().Contains('F') && (SubjectDataGridView.Rows[size].Cells[4].ToString().Contains("F")))
                                                            {
                                                                if ((((start >= startTime && start < endTime) || (end > startTime && end <= endTime)) || (start < startTime && end > endTime)))
                                                                {
                                                                    SubjectDataGridView.Rows[size].DefaultCellStyle.BackColor = Color.Firebrick;
                                                                    conflict = true;
                                                                    break;
                                                                }
                                                            }
                                                            else if (thisReader2["SFDAYS"].ToString().Trim().Contains('S') && (SubjectDataGridView.Rows[size].Cells[4].ToString().Contains("S")))
                                                            {
                                                                if ((((start >= startTime && start < endTime) || (end > startTime && end <= endTime)) || (start < startTime && end > endTime)))
                                                                {
                                                                    SubjectDataGridView.Rows[size].DefaultCellStyle.BackColor = Color.Firebrick;
                                                                    conflict = true;
                                                                    break;
                                                                }
                                                            }

                                                            else if (thisReader2["SFDAYS"].ToString().Trim().Contains("TTH") && (SubjectDataGridView.Rows[size].Cells[4].ToString().Contains("TTH")))
                                                            {
                                                                if (((start >= startTime && start < endTime) || (end > startTime && end <= endTime)))
                                                                {
                                                                    SubjectDataGridView.Rows[size].DefaultCellStyle.BackColor = Color.Firebrick;
                                                                    conflict = true;
                                                                    break;
                                                                }
                                                            }

                                                            else if (thisReader2["SFDAYS"].ToString().Trim().Contains("MWF") && (SubjectDataGridView.Rows[size].Cells[4].ToString().Contains("MWF")))
                                                            {
                                                                if (((start >= startTime && start < endTime) || (end > startTime && end <= endTime)))
                                                                {
                                                                    SubjectDataGridView.Rows[size].DefaultCellStyle.BackColor = Color.Firebrick;
                                                                    conflict = true;
                                                                    break;
                                                                }
                                                            }

                                                            else
                                                            {
                                                                continue;
                                                            }
                                                        }

                                                        if (conflict == false)
                                                        {
                                                            int row = SubjectDataGridView.Rows.Add();

                                                            SubjectDataGridView.Rows[row].Cells["EDPCodeCol"].Value = thisReader["SFEDPCODE"].ToString();
                                                            SubjectDataGridView.Rows[row].Cells["SubjectCodeCol"].Value = thisReader["SFSUBJCODE"].ToString();
                                                            SubjectDataGridView.Rows[row].Cells["StartTimeCol"].Value = thisReader["SFSTARTTIME"].ToString();
                                                            SubjectDataGridView.Rows[row].Cells["EndTimeCol"].Value = thisReader["SFENDTIME"].ToString(); ;
                                                            SubjectDataGridView.Rows[row].Cells["DaysCol"].Value = thisReader["SFDAYS"].ToString();
                                                            SubjectDataGridView.Rows[row].Cells["RoomCol"].Value = thisReader["SFROOM"].ToString();
                                                            SubjectDataGridView.Rows[row].Cells["UnitsCol"].Value = thisReader2["SFSUBJUNITS"].ToString();

                                                            totalUnits += Convert.ToInt16(thisReader2["SFSUBJUNITS"]);
                                                            TotalUnitsLabel.Text = totalUnits.ToString();
                                                            String schoolYear = thisReader["SFSCHOOLYEAR"].ToString();
                                                            IDNumberTextBox.Enabled = false;
                                                            add = true;
                                                            result = true;
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            MessageBox.Show("Time Conflict!", "Information");
                                                        }

                                                    }
                                                    else
                                                    {
                                                        continue;
                                                    }

                                                }
                                            }
                                            else
                                            {
                                                MessageBox.Show("Already Added!", "Message");
                                            }
                                        }
                                        else
                                        {
                                            continue;
                                        }
                                    }
                                    if (result == false)
                                    {
                                        MessageBox.Show("No subject has been added.", "Information");
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Enter your EDP Code", "Message");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Please Enter your ID Number", "Message");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Initial Student Search!", "Message");
                        }
                    }
                    else
                    {
                        MessageBox.Show("This Student Cannot Have Subjects Updated or Added!", "Message");
                    }
                }
                catch (Exception x)
                {
                    MessageBox.Show(x.ToString(), "Error");
                }

            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            bool added = false;
            try
            {
                if (search == true)
                {
                    if (add == true)
                    {
                        if (EncodedByTextBox.Text != "Encoder's Name")
                        {


                            for (int size = 0; size < SubjectDataGridView.Rows.Count; size++)
                            {
                                string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Kent Albores\OneDrive\Desktop\AppsDev\Enrollment-System\Albores.accdb";
                                OleDbConnection thisConnection2 = new OleDbConnection(connectionString);
                                string sql2 = "Select * from ENROLLMENTDETAILFILE";
                                OleDbDataAdapter thisAdapter2 = new OleDbDataAdapter(sql2, thisConnection2);
                                OleDbCommandBuilder thisBuilder2 = new OleDbCommandBuilder(thisAdapter2);
                                DataSet thisDataSet2 = new DataSet();
                                thisAdapter2.Fill(thisDataSet2, "ENROLLMENTDETAILFILE");

                                DataRow thisRow2 = thisDataSet2.Tables["ENROLLMENTDETAILFILE"].NewRow();
                                thisRow2["ENRDFSTUDID"] = Convert.ToInt32(IDNumberTextBox.Text);
                                thisRow2["ENRDFSTUDSUBJCODE"] = SubjectDataGridView.Rows[size].Cells[1].Value.ToString();
                                thisRow2["ENRDFSTUDEDPCODE"] = SubjectDataGridView.Rows[size].Cells[0].Value.ToString();

                                thisDataSet2.Tables["ENROLLMENTDETAILFILE"].Rows.Add(thisRow2);
                                thisAdapter2.Update(thisDataSet2, "ENROLLMENTDETAILFILE");


                                OleDbConnection thisConnection3 = new OleDbConnection(connectionString);
                                thisConnection3.Open();
                                string commandText3 = "SELECT * FROM SUBJECTSCHEDFILE";
                                OleDbCommand thisCommand3 = thisConnection3.CreateCommand();
                                thisCommand3.CommandText = commandText3;

                                OleDbDataReader thisReader3 = thisCommand3.ExecuteReader();
                                int classSize = 0;
                                while (thisReader3.Read())
                                {
                                    if (thisReader3["SSFCLASSSIZE"].ToString() != thisReader3["SSFMAXSIZE"].ToString())
                                    {
                                        if (thisReader3["SSFEDPCODE"].ToString() == SubjectDataGridView.Rows[size].Cells[0].Value.ToString())
                                        {
                                            if (thisReader3["SSFCLASSSIZE"].ToString() == "")
                                            {

                                                OleDbConnection connection = new OleDbConnection(connectionString);
                                                connection.Open();
                                                OleDbCommand command = new OleDbCommand();
                                                command.Connection = connection;
                                                string query = "update SUBJECTSCHEDFILE set SSFCLASSSIZE=" + 1 + " where SSFEDPCODE='" + thisReader3["SSFEDPCODE"].ToString() + "'";
                                                command.CommandText = query;

                                                command.ExecuteNonQuery();
                                                connection.Close();

                                                added = true;
                                            }
                                            else
                                            {
                                                classSize = Convert.ToInt32(thisReader3["SSFCLASSSIZE"].ToString()) + 1;
                                                OleDbConnection connection = new OleDbConnection(connectionString);
                                                connection.Open();
                                                OleDbCommand command = new OleDbCommand();
                                                command.Connection = connection;
                                                string query = "update SUBJECTSCHEDFILE set SSFCLASSSIZE=" + classSize + " where SSFEDPCODE='" + thisReader3["SSFEDPCODE"].ToString() + "'";
                                                command.CommandText = query;

                                                command.ExecuteNonQuery();
                                                connection.Close();
                                                added = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Already Full!", "Message");
                                    }
                                }
                            }

                            if (added == true)
                            {

                                //Open Header File
                                OleDbConnection thisConnection = new OleDbConnection(connectionString);
                                string sql = "Select * from ENROLLMENTHEADERFILE";
                                OleDbDataAdapter thisAdapter = new OleDbDataAdapter(sql, thisConnection);
                                OleDbCommandBuilder thisBuilder = new OleDbCommandBuilder(thisAdapter);
                                DataSet thisDataSet = new DataSet();
                                thisAdapter.Fill(thisDataSet, "ENROLLMENTHEADERFILE");

                                DataRow thisRow = thisDataSet.Tables["ENROLLMENTHEADERFILE"].NewRow();

                                //To Insert
                                thisRow["ENRHFSTUDID"] = Convert.ToInt32(IDNumberTextBox.Text);
                                thisRow["ENRHFSTUDDATEENROLL"] = DatePicker.Value.ToString();
                                thisRow["ENRHFSTUDSCHLYR"] = YearLabel.Text;
                                thisRow["ENRHFSTUDENCODER"] = EncodedByTextBox.Text;
                                thisRow["ENRHFSTUDTOTALUNITS"] = TotalUnitsLabel.Text;

                                thisDataSet.Tables["ENROLLMENTHEADERFILE"].Rows.Add(thisRow);
                                thisAdapter.Update(thisDataSet, "ENROLLMENTHEADERFILE");

                                MessageBox.Show("Entries Recorded", "Message");


                                IDNumberTextBox.Enabled = true;
                                IDNumberTextBox.Text = "";
                                NameLabel.Text = string.Empty;
                                CourseLabel.Text = "";
                                YearLabel.Text = null;
                                EDPCodeTextBox.Text = "";
                                EDPCodeTextBox.ForeColor = Color.SlateGray;
                                EncodedByTextBox.Text = "--Encoded BY--";
                                EncodedByTextBox.ForeColor = Color.SlateGray;
                                TotalUnitsLabel.Text = null;

                                YearLabel.Text = null;
                                SubjectDataGridView.Rows.Clear();
                            }


                        }
                        else
                        {
                            MessageBox.Show("Please Enter the Encoders Name", "Message");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Enroll a subject first!", "Message");
                    }

                }
                else
                {
                    MessageBox.Show("Before enrolling in subjects, kindly add students!", "Message");
                }
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message, "Error");
            }
        }
    }
}

