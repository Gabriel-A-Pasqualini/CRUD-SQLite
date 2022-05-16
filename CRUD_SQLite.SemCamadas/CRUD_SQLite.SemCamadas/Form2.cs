using System;
using System.Data.SQLite;


namespace CRUD_SQLite.SemCamadas
{
    public partial class Form2 : Form
    {
        Cliente _cliente;
        string operacao = "";
        string connectionString = @"Data Source=C:\Users\gabri\Documents\dev\SQLite\start.s3db";

        public Form2(Cliente cliente)
        {
            InitializeComponent();
            _cliente = cliente;
        }
        

        public int IncluirDados(Cliente cliente)
        {
            int resultado = -1;
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "INSERT INTO Clientes(nome,email,idade) VALUES (@nome,@email,@idade)";
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@nome", cliente.nome);
                    cmd.Parameters.AddWithValue("@email", cliente.email);
                    cmd.Parameters.AddWithValue("@idade", cliente.idade);
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

        public int AtualizaDados(Cliente cliente)
        {
            int resultado = -1;
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "UPDATE Clientes SET nome=@nome, email=@email, idade=@idade WHERE Id = @id";
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@Id", cliente.Id);
                    cmd.Parameters.AddWithValue("@nome", cliente.nome);
                    cmd.Parameters.AddWithValue("@email", cliente.email);
                    cmd.Parameters.AddWithValue("@idade", cliente.idade);
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

        private Boolean ValidaDados()
        {
            bool retorno = true;
            if (txtNome.Text == string.Empty)
                retorno = false;
            if (txtEmail.Text == string.Empty)
                retorno = false;
            if (txtIdade.Text == string.Empty)
                retorno = false;
            return retorno;
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (ValidaDados())
            {
                Cliente cliente = new Cliente();
                cliente.Id = _cliente?.Id ?? 0;
                cliente.nome = txtNome.Text;
                cliente.email = txtEmail.Text;
                cliente.idade = Convert.ToInt32(txtIdade.Text);
                
                try
                {
                    if ((_cliente?.Id ?? 0) == 0)
                    {
                        if (IncluirDados(cliente) > 0)
                        {
                            MessageBox.Show("Dados incluídos com sucesso.");
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Os dados não foram incluídos.");
                            this.Close();
                        }
                    }
                    else
                    {
                        
                        if (AtualizaDados(cliente) > 0)
                        {   
                            MessageBox.Show("Dados atualizados com sucesso.");
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Os dados não foram atualizados.");
                            this.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro : " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Dados inválidos.");
            }
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            if (_cliente == null)
            {
                txtNome.Focus();
                operacao = "incluir";
            }
            else
            {
                SQLiteConnection conn = new SQLiteConnection(connectionString);

                conn.Open();
                string sqlNome = "SELECT nome FROM Clientes WHERE nome='" + _cliente.nome + "'";
                string sqlEmail = "SELECT email FROM Clientes WHERE email='" + _cliente.email + "'";
                string sqlIdade = "SELECT idade FROM Clientes WHERE idade='" + _cliente.idade + "'";

                using var cmdNome = new SQLiteCommand(sqlNome, conn);
                using var cmdEmail = new SQLiteCommand(sqlEmail, conn);
                using var cmdIdade = new SQLiteCommand(sqlIdade, conn);

                string nome = cmdNome.ExecuteScalar().ToString();
                string email = cmdEmail.ExecuteScalar().ToString();
                string idade = cmdIdade.ExecuteScalar().ToString();

                operacao = "alterar";
                txtNome.Text = nome;
                txtEmail.Text = email;
                txtIdade.Text = idade;
                //txtNome.Text = _cliente.nome;
                //txtEmail.Text = _cliente.email;  
                //txtIdade.Text = _cliente.idade.ToString();
            }

        }
    }
}
