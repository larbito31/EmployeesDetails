using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.IO;

namespace EmployeesDetails
{
    public partial class Form1 : Form
    {
        public SqlConnection con = new SqlConnection(@"Data Source=.;Initial Catalog=employee_db;Integrated Security=True");
        public SqlCommand cmd;
        public DataTable dt = new DataTable();
        public SqlDataReader dr;
        public OpenFileDialog op = new OpenFileDialog();

        public Form1()
        {
            InitializeComponent();
            LoadData();   // load data and refresh 
        }

        void LoadData()   // function for load data and refresh  
        {
            dt.Clear();
            cmd = new SqlCommand("SELECT * FROM employee_details", con);
            con.Open();
           dr = cmd.ExecuteReader();
            dt.Load(dr);
            //Column HeaderText design
            dataGridView1.DataSource = dt;
            dataGridView1.Columns[0].HeaderText = "ID";
            dataGridView1.Columns[0].Width = 45;

            dataGridView1.Columns[1].HeaderText = "Name";
            dataGridView1.Columns[1].Width = 150;

            dataGridView1.Columns[2].HeaderText = "Phone";
            dataGridView1.Columns[2].Width = 150;

            dataGridView1.Columns[3].HeaderText = "Email";
            dataGridView1.Columns[3].Width = 200;

            dataGridView1.Columns[4].HeaderText = "Address";
            dataGridView1.Columns[4].Width = 200;

            ((DataGridViewImageColumn)dataGridView1.Columns[5]).ImageLayout = DataGridViewImageCellLayout.Stretch; //image column 

            con.Close();

        }

        void clear_data()
        {
            txt_phone.Clear();
            txt_email.Clear();
            txt_name.Clear();
            txt_address.Clear();
            pictureBox1.Image = Properties.Resources.dad;
            



        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(op.FileName == "")
            {
                
                String insertQ = "INSERT INTO employee_details (employee_id, employee_name, employee_phone, employee_email, employee_address) " +
                    "VALUES (@employee_id, @employee_name, @employee_phone, @employee_email, @employee_address)";
                
                SqlCommand cmd = new SqlCommand(insertQ, con);
                cmd.Parameters.AddWithValue("@employee_id", dt.Rows.Count + 1);
             
                cmd.Parameters.AddWithValue("@employee_name", txt_name.Text);
                cmd.Parameters.AddWithValue("@employee_phone", txt_phone.Text);
                cmd.Parameters.AddWithValue("@employee_email", txt_email.Text);
                cmd.Parameters.AddWithValue("@employee_address", txt_address.Text);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                
                LoadData();
                MessageBox.Show("Successfully added!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);



            } else
            {
                String insertQ = "INSERT INTO employee_details (employee_id, employee_name, employee_phone, employee_email, employee_address,employee_picture) " +
                  "VALUES (@employee_id, @employee_name, @employee_phone, @employee_email, @employee_address, @employee_picture)";

                SqlCommand cmd = new SqlCommand(insertQ, con);
                cmd.Parameters.Add("@employee_picture", SqlDbType.Image).Value = File.ReadAllBytes(op.FileName);
                cmd.Parameters.AddWithValue("@employee_id", dt.Rows.Count + 1);
                cmd.Parameters.AddWithValue("@employee_name", txt_name.Text);
                cmd.Parameters.AddWithValue("@employee_phone", txt_phone.Text);
                cmd.Parameters.AddWithValue("@employee_email", txt_email.Text);
                cmd.Parameters.AddWithValue("@employee_address", txt_address.Text);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                LoadData();
                
                MessageBox.Show("Successfully added!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

            try
            {
                // add image ( openfile) 
                op.Filter = "Image files(*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png;";
                op.Multiselect = false;
                if (op.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image = Image.FromFile(op.FileName);
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (op.FileName == "")
            {

                String insertQ = "UPDATE employee_details SET employee_name= @employee_name, employee_phone= @employee_phone, employee_email= @employee_email, employee_address=@employee_address where employee_id = @ID";
                  

                SqlCommand cmd = new SqlCommand(insertQ, con);
               
                cmd.Parameters.AddWithValue("@employee_name", txt_name.Text);
                cmd.Parameters.AddWithValue("@employee_phone", txt_phone.Text);
                cmd.Parameters.AddWithValue("@employee_email", txt_email.Text);
                cmd.Parameters.AddWithValue("@employee_address", txt_address.Text);
                cmd.Parameters.AddWithValue("@ID", dataGridView1.SelectedRows[0].Cells[0].Value);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                LoadData();
                MessageBox.Show("Successfully Updated!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);



            }
            else
            {
                String insertQ = "UPDATE employee_details SET employee_name= @employee_name, employee_phone= @employee_phone, employee_email= @employee_email, employee_address=@employee_address, employee_picture= @employee_picture where employee_id = @ID";

                SqlCommand cmd = new SqlCommand(insertQ, con);
                cmd.Parameters.Add("@employee_picture", SqlDbType.Image).Value = File.ReadAllBytes(op.FileName);
                cmd.Parameters.AddWithValue("@employee_name", txt_name.Text);
                cmd.Parameters.AddWithValue("@employee_phone", txt_phone.Text);
                cmd.Parameters.AddWithValue("@employee_email", txt_email.Text);
                cmd.Parameters.AddWithValue("@employee_address", txt_address.Text);
                cmd.Parameters.AddWithValue("@ID", dataGridView1.SelectedRows[0].Cells[0].Value);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                LoadData();
                MessageBox.Show("Successfully Updated!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void txt_phone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void dataGridView1_SizeChanged(object sender, EventArgs e)
        {
            
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                txt_name.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                txt_phone.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                txt_email.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                txt_address.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
                MemoryStream ms = new MemoryStream((byte[])dataGridView1.SelectedRows[0].Cells[5].Value);
                pictureBox1.Image = Image.FromStream(ms);

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult dialogr = MessageBox.Show("Are you Sure delete this row?", "Warning !!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (dialogr == DialogResult.Yes )
            {
                
                cmd = new SqlCommand("DELETE FROM employee_details where employee_id = " + dataGridView1.SelectedRows[0].Cells[0].Value, con);
                
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                LoadData();


                // reorder the ID from 1
                for (int i=0; i<=dt.Rows.Count - 1 ; i++)
                {
                   
                    
                    cmd  = new SqlCommand("update employee_details set employee_id =" + (i+1) + " where employee_id = "  +
                        (dt.Rows[i].ItemArray[0]) + "", con);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    LoadData();
                  

                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            clear_data();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            clear_data();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            BindingContext[dt].Position = 0;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            BindingContext[dt].Position = dt.Rows.Count - 1;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            BindingContext[dt].Position += 1;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            BindingContext[dt].Position -= 1;
        }

        private void txt_search_TextChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                dt.Clear();
                cmd = new SqlCommand("SELECT * FROM employee_details where employee_name like '%" + txt_search.Text+ "%'", con);
                con.Open();
                dr = cmd.ExecuteReader();
                dt.Load(dr);
                con.Close();
            } 
            else if (radioButton2.Checked == true)
            {
                dt.Clear();
                cmd = new SqlCommand("SELECT * FROM employee_details where employee_email like '%" + txt_search.Text + "%'", con);
                con.Open();
                dr = cmd.ExecuteReader();
                dt.Load(dr);
                con.Close();
            }
            else if (radioButton3.Checked == true)
            {
                dt.Clear();
                cmd = new SqlCommand("SELECT * FROM employee_details where employee_address like '%" + txt_search.Text + "%'", con);
                con.Open();
                dr = cmd.ExecuteReader();
                dt.Load(dr);
                con.Close();
            }
            else if (radioButton4.Checked == true)
            {
                dt.Clear();
                cmd = new SqlCommand("SELECT * FROM employee_details where employee_phone like '%" + txt_search.Text + "%'", con);
                con.Open();
                dr = cmd.ExecuteReader();
                dt.Load(dr);
                con.Close();
            } else
            {
                MessageBox.Show("Please choose Search By !!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt_search.Clear();
            }
        }
    }
}
