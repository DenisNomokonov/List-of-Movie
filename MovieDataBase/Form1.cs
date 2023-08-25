using System;
using System.Data;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;



//CRUD application
namespace MovieDataBase
{
	public partial class Form1 : Form
	{
		SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["MovieDB"].ConnectionString);

		public Form1()
		{
			InitializeComponent();
		}
		int ID = 0;
		private void Form1_Load(object sender, EventArgs e)
		{
			LoadData();
		}

		private void LoadData()
		{
			try
			{
				sqlConnection.Open();
				SqlDataAdapter dataAdapter = new SqlDataAdapter(
					"SELECT * From [Movies]", sqlConnection);

				SqlCommandBuilder sqlCommandBuilder = new SqlCommandBuilder(dataAdapter);

				DataSet dataSet = new DataSet();
				dataAdapter.Fill(dataSet, "Movies");
				dataGridView1.DataSource = dataSet.Tables["Movies"];
				dataGridView1.ReadOnly = true;
				dataGridView1.AllowUserToAddRows = false;
				sqlConnection.Close();

			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void ClearData()
		{
			textBox1.Text = "";
			textBox2.Text = "";
			textBox3.Text = null;
			textBox4.Text = null;
		}


		private void button1_Click(object sender, EventArgs e)// Insert
		{
			if (textBox1.Text != "")
			{
				SqlCommand command = new SqlCommand(
				$"INSERT INTO [Movies] (Title, Genre, YearDate, Rating) VALUES (@Title, @Genre, @YearDate, @Rating)",
				sqlConnection);

				sqlConnection.Open();

				command.Parameters.AddWithValue("Title", textBox1.Text);
				command.Parameters.AddWithValue("Genre", textBox2.Text);
				if (int.TryParse(textBox3.Text, out int yeardate))
					command.Parameters.AddWithValue("YearDate", yeardate);
				else
					command.Parameters.AddWithValue("YearDate", DBNull.Value);


				if (int.TryParse(textBox4.Text, out int rating))
					command.Parameters.AddWithValue("Rating", rating);
				else
					command.Parameters.AddWithValue("Rating", DBNull.Value);




				command.ExecuteNonQuery();
				sqlConnection.Close();
				LoadData();
				ClearData();
			}
			else
			{
				MessageBox.Show("Введите название");
			}

		}

		private void buttonUpdate_Click(object sender, EventArgs e)
		{
			if (textBox1.Text != "")
			{
				SqlCommand command = new SqlCommand(
					$"UPDATE Movies SET Title = @Title, Genre = @Genre, YearDate = @YearDate, Rating = @Rating where ID = @id",
				sqlConnection);

				sqlConnection.Open();

				command.Parameters.AddWithValue("Title", textBox1.Text);
				command.Parameters.AddWithValue("Genre", textBox2.Text);

				if (int.TryParse(textBox3.Text, out int yeardate))
					command.Parameters.AddWithValue("YearDate", yeardate);
				else
					command.Parameters.AddWithValue("YearDate", DBNull.Value);


				if (int.TryParse(textBox4.Text, out int rating))
					command.Parameters.AddWithValue("Rating", rating);
				else
					command.Parameters.AddWithValue("Rating", DBNull.Value);

				command.Parameters.AddWithValue("id", ID);

				command.ExecuteNonQuery();

				sqlConnection.Close();
				LoadData();
				ClearData();
			}
			else
			{
				MessageBox.Show("Выберите запись для обновления");
			}
		}

		private void buttonDelete_Click(object sender, EventArgs e)
		{
			if (ID != 0)
			{
				SqlCommand command = new SqlCommand(
				$"DELETE [Movies] WHERE ID=@id",
				sqlConnection);

				sqlConnection.Open();

				command.Parameters.AddWithValue("id", ID);
				command.ExecuteNonQuery();
				sqlConnection.Close();
				LoadData();
				ClearData();
			}
			else
			{
				MessageBox.Show("Выберите запись для удаления");
			}
		}


		private void dataGridView1_RowHeaderMouseClick_1(object sender, DataGridViewCellMouseEventArgs e)
		{
			ID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
			textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
			textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
			textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
			textBox4.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
		}
	}
}
