﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SmsApp.Models;

namespace SmsApp.Ui
{
    public partial class CompanyUi : Form
    {
        private string conString = " Server=ROKO\\SQLEXPRESS; Database=SmsDb;  Integrated Security=True"; 
        SqlConnection con;
        Company aCompany = new Company();
        public CompanyUi()
        {
            InitializeComponent();
            showDataGridView.DataSource = GetCompanyInfo(aCompany);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            aCompany.Name = nameTextBox.Text;
            
            try
            {
                if (Exists(Name))
                {
                    MessageBox.Show("Already Exists");
                    return;
                }

                if (!String.IsNullOrEmpty(nameTextBox.Text))
                {
                    bool isSaved = AddCompany(aCompany);
                    if (isSaved)
                    {
                        confirmLabel.Text = " Saved Successfully";
                        confirmLabel.ForeColor = Color.Green;


                    }
                    else
                    {
                        confirmLabel.Text = " Saved Failed";
                        confirmLabel.ForeColor = Color.Red;
                    }
                }

                
            }
            catch (Exception exception)
            {

                MessageBox.Show(exception.Message);
            }
        }

        private bool AddCompany(Company aCompany)
        {
            bool isSuccess = false;

            con = new SqlConnection(conString);
            string query = "Insert Into Company Values('" + aCompany.Name + "')";
            SqlCommand command = new SqlCommand(query, con);
            con.Open();

            int isExecuted = command.ExecuteNonQuery();
            if (isExecuted > 0)
            {
                isSuccess = true;
            }
            else
            {
                isSuccess = false;
            }
            con.Close();
            return isSuccess;
        }

        private DataTable GetCompanyInfo(Company aCompany)
        {
            con = new SqlConnection(conString);

            string query = "select * From Company ";

            SqlCommand command = new SqlCommand(query, con);

            con.Open();

            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            DataTable aTable = new DataTable();
            dataAdapter.Fill(aTable);

            con.Close();
            return aTable;

        }

        private bool Exists(string Name)
        {

            bool isExists = false;

            try
            {


                con = new SqlConnection(conString);
                string query = @"SELECT * FROM Company WHERE Name = '" + aCompany.Name + "'";

                //5
                SqlCommand sqlCommand = new SqlCommand(query, con);
                //6
                con.Open();
                //7
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                string data = "";
                if (sqlDataReader.Read())
                {
                    data = sqlDataReader["ID"].ToString();
                }

                if (!String.IsNullOrEmpty(data))
                {
                    isExists = true;
                }
                else
                {
                    isExists = false;
                }


                con.Close();


            }
            catch (Exception exception)
            {

                MessageBox.Show(exception.Message);
            }

            return isExists;
        }
       
    }
}
