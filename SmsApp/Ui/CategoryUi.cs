using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SmsApp.Connection;
using SmsApp.Models;

namespace SmsApp
{
    public partial class CategoryUi : Form
    {
        private string conString = " Server=ROKO\\SQLEXPRESS; Database=SmsDb;  Integrated Security=True";
        Category aCategory = new Category();
        SqlConnection con;
        private string name;
        public CategoryUi()
        {
            InitializeComponent();
           showDataGridView.DataSource = GetCategoryInfo(aCategory);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {


            aCategory.Name = nameTextBox.Text;
            try
            {


            if (Exists(Name))
            {
                MessageBox.Show("Already Exists");
                return;
            }

            if (!String.IsNullOrEmpty(nameTextBox.Text))
                {
                    con = new SqlConnection(conString);
                    string query = "Insert Into Category Values('" + aCategory.Name + "')";
                    SqlCommand command = new SqlCommand(query, con);
                    con.Open();

                    int isExecuted = command.ExecuteNonQuery();
                    if (isExecuted > 0)
                    {
                        confirmLabel.Text = " Saved Successfully";
                        confirmLabel.ForeColor = Color.Green;

                    }
                    else
                    {
                        confirmLabel.Text = " Saved Failed";
                        confirmLabel.ForeColor = Color.Red;
                    }
                    con.Close();

                }

            else
            {
                confirmLabel.Text = "Plz Enter Category Name";
                confirmLabel.ForeColor = Color.Red;
            }

           
           
           
         }

               
            
            catch (Exception exception)
            {

                MessageBox.Show(exception.Message);
            }
        }




         


        private DataTable GetCategoryInfo(Category aCategory)
        {
            con = new SqlConnection(conString);

            string query = "select * From Category ";

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
                string query = @"SELECT * FROM Category WHERE Name = '" + aCategory.Name + "'";

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

        private void showDataGridView_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int rowIndex = e.RowIndex;
            nameTextBox.Text = showDataGridView.Rows[rowIndex].Cells[1].Value.ToString();
        }
    }


}
