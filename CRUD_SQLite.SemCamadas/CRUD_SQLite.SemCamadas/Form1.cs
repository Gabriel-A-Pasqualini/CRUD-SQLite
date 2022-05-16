using System.Data;
using System.Data.SQLite;

namespace CRUD_SQLite.SemCamadas
{
    public partial class Form1 : Form
    {
        string connectionString = @"Data Source=C:\Users\gabri\Documents\dev\SQLite\start.s3db";
        public Form1()
        {
            InitializeComponent();
        }

        private void CarregaDados()
        {
            DataTable dt = new DataTable();
            SQLiteConnection conn = null;
            String sql = "select * from Clientes";
            try
            {
                conn = new SQLiteConnection(connectionString);
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, connectionString);
                da.Fill(dt);
                dgvClientes.DataSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro :" + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        private object LeDados1<T1, T2>(string v)
        {
            throw new NotImplementedException();
        }

        private void btnIncluir_Click(object sender, EventArgs e)
        {
            try
            {
                Cliente _cliente = null;
                Form2 frm2 = new Form2(_cliente);
                frm2.ShowDialog();
                CarregaDados();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro : " + ex.Message);
            }
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            try
            {
                Form2 frm2 = new Form2(GetDadosDoGrid());
                frm2.ShowDialog();
                CarregaDados();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro : " + ex.Message);
            }
        }

        public Cliente GetDadosDoGrid()
        {
            try
            {
                int linha;
                linha = dgvClientes.CurrentRow.Index;
                Cliente cliente = new Cliente();
                cliente.Id = Convert.ToInt32(dgvClientes[0, linha].Value);
                cliente.nome = dgvClientes[1, linha].Value.ToString();
                cliente.email = dgvClientes[2, linha].Value.ToString();
                cliente.idade = Convert.ToInt32(dgvClientes[3, linha].Value);
                return cliente;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int DeletaDados(Cliente cliente)
        {
            int resultado = -1;
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "DELETE FROM Clientes WHERE Id = @Id";
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@Id", cliente.Id);
                    try
                    {
                        resultado = cmd.ExecuteNonQuery();
                    }
                    catch (SQLiteException ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            return resultado;
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            dgvClientes.DataSource = ProcurarDados();
        }

        private DataTable ProcurarDados()
        {
            string sql = "SELECT id, nome, email, idade from Clientes WHERE nome LIKE  '" + textBox1.Text + "%'";
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (SQLiteDataAdapter da = new SQLiteDataAdapter(sql, conn))
                {
                    try
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        return dt;
                    }
                    catch (SQLiteException ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExcluir_Click_1(object sender, EventArgs e)
        {
            try
            {
                DialogResult response = MessageBox.Show("Deseja deletar este registro ?", "Deletar linha",
                      MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (response == DialogResult.Yes)
                {
                    DeletaDados(GetDadosDoGrid());
                    CarregaDados();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro : " + ex.Message);
            }
        }
    }
}
