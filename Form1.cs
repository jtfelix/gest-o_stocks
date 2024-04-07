using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;
using System.Data;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

namespace Projecto
{
    public partial class Form1 : Form
    {
        

        internal static string connString = "datasource=127.0.0.1;port=3306;username=root;password=;database=portas_e_filhos;";
        internal static MySqlConnection conn = new MySqlConnection(connString);

        //associacao tabela materiais
        BindingSource bsourceMateriais = new BindingSource();
        DataTable dtMateriais = new DataTable();

        //associacao tabela tipo de material
        BindingSource bsourceTipoMaterial = new BindingSource();
        DataTable dtTipoMaterial = new DataTable();

        //associacao tabela fornecedores
        BindingSource bsourceFornecedores = new BindingSource();    
        DataTable dtFornecedores = new DataTable();

        //associacao tabela encomendas
        BindingSource bsourceEncomendas = new BindingSource();
        DataTable dtEncomendas = new DataTable();

        //associacao tabela produto
        BindingSource bsourceProduto = new BindingSource();
        DataTable dtProduto = new DataTable();

        //associacao tabela materiais utilizados
        BindingSource bsourceUtilizados = new BindingSource();
        DataTable dtUtilizados = new DataTable();



        
        
        public Form1()
        {
            InitializeComponent();
            carregaMateriais();
            comboTipoMateriais_Materiais();
            carregaTipoMaterial();
            carregaFornecedores();
            carregaEncomendas();
            comboMaterial();
            comboFornecedores();
            carregaProdutos();
            carregaListaMateriais();
            comboPecas();
            carregaUtilizados();
        }

        //pagina tipo de material

        //Inserir Tipo de Material
        private void b_inserir_tipo_Click(object sender, EventArgs e)
        {
            string query = "INSERT INTO tipo_material (descricao) VALUES (@descricao)";
            MySqlCommand cmd = new MySqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@descricao", tb_tipo_material.Text);

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Registo Inserido com Sucesso");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { conn.Close(); }
            comboTipoMateriais_Materiais();
            carregaTipoMaterial();
            limpaTextoTipoMaterial();
        }
        //limpar texto tipo de material
        public void limpaTextoTipoMaterial()
        {
            tb_tipo_material.ResetText();
            tb_referencia_tipo.ResetText();
            tb_pesquisa_tipo.ResetText();

            b_eliminar_tipo.Enabled= false;
            b_atualizar_tipo.Enabled = false; 
        }

        //Pesquisa Tipo de Material
        private void tb_pesquisa_tipo_TextChanged_1(object sender, EventArgs e)
        {
           bsourceTipoMaterial.Filter = "[descricao] like '%" + tb_pesquisa_tipo.Text + "%'";
        }

        //Atualizar Tipo de Material
        private void b_atualizar_tipo_Click(object sender, EventArgs e)
        {
            string query = "UPDATE tipo_material SET descricao=@descricao WHERE ID=@ID";
            MySqlCommand cmd = new MySqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@ID", tb_referencia_tipo.Text);
            cmd.Parameters.AddWithValue("@descricao", tb_tipo_material.Text);

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Registo Atualizado com Sucesso");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { conn.Close(); }
            carregaTipoMaterial();
            limpaTextoTipoMaterial();
            comboTipoMateriais_Materiais();


            b_atualizar_tipo.Enabled = false;
            b_eliminar_tipo.Enabled = false;
        }

        //Eliminar tipo de Material
        private void b_eliminar_tipo_Click(object sender, EventArgs e)
        {
            string query = "DELETE FROM tipo_material WHERE ID = @ID";
            MySqlCommand cmd = new MySqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@ID", tb_referencia_tipo.Text);
            cmd.Parameters.AddWithValue("@descricao", tb_tipo_material.Text);

            try
            {
                conn.Open(); cmd.ExecuteNonQuery();
                MessageBox.Show("Registo Eliminado com Sucesso");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { conn.Close(); }
            carregaTipoMaterial();
            limpaTextoTipoMaterial();
            comboTipoMateriais_Materiais();
            b_atualizar_tipo.Enabled = false;
            b_eliminar_tipo.Enabled = false;
        }

        //Passagem da gridview para textbox tipo de material
        private void dgv_tipo_material_DoubleClick(object sender, EventArgs e)
        {
            DataGridViewRow row = dgv_tipo_material.SelectedRows[0];
            tb_referencia_tipo.Text = row.Cells[0].Value.ToString();
            tb_tipo_material.Text = row.Cells[1].Value.ToString();

            b_atualizar_tipo.Enabled = true;
            b_eliminar_tipo.Enabled = true;
        }

        //Limpar campos pagina Tipo Material
        private void b_limpa_tipo_Click(object sender, EventArgs e)
        {
            limpaTextoTipoMaterial();
        }

        //Pagina Materiais
        public void carregaMateriais()
        {
            dtMateriais.Clear();
            MySqlDataAdapter MyDA = new MySqlDataAdapter();
            string query = "Select materiais.ID as Referencia, materiais.nome as Nome, materiais.preco as Preco, materiais.stock as Stock, materiais.id_tipo_material as RefMaterial, tipo_material.descricao as Descricao from tipo_material inner join materiais on materiais.ID_tipo_material = tipo_material.ID";
            MyDA.SelectCommand = new MySqlCommand(query, conn);
            MyDA.Fill(dtMateriais);
            bsourceMateriais.DataSource = dtMateriais;
            dgv_materiais.DataSource = bsourceMateriais;
        }
        public void comboTipoMateriais_Materiais()
        {
            //cb_tipo.Items.Clear();
            //cb_tipoMaterial.Items.Clear();
            try
            {
                string query = "SELECT ID, descricao FROM tipo_material";
                MySqlCommand cmd = new MySqlCommand(query, conn);

                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                List<Item> listTiposMaterial = new List<Item>();
                listTiposMaterial.Add(new Item());

                while (reader.Read())
                {
                    //cb_tipoMaterial.Items.Add(reader[0].ToString());
                    //cb_tipo.Items.Add(reader[0].ToString());
                    listTiposMaterial.Add(new Item() { ID = Int32.Parse(reader[0].ToString()), Value = reader[1].ToString() });
                }

                cb_tipoMaterial.DataSource = listTiposMaterial;
                cb_tipoMaterial.DisplayMember= "Value";
                cb_tipoMaterial.ValueMember = "ID";

                cb_tipo.DataSource = listTiposMaterial;
                cb_tipo.DisplayMember = "Value";
                cb_tipo.ValueMember = "ID";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        private void b_limpa_materiais_Click(object sender, EventArgs e)
        {
            limpa_texto_mate();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }
        public void limpa_texto_mate()
        {
            tb_nome_mate.ResetText();
            tb_pesquisa_mate.ResetText();
            tb_preco_mate.ResetText();
            tb_stock.ResetText();
            tb_referencia_mate.ResetText();
            cb_tipoMaterial.SelectedIndex=0;

            b_atualiza_mate.Enabled= false;
            b_eliminar_mate.Enabled= false;
        }



        //Inserir dados (Botão inserir)
        private void b_inserir_mate_Click(object sender, EventArgs e)
        {
            string query = "INSERT INTO materiais (nome, preco , stock, ID_tipo_material) VALUES (@nome , @preco, @stock, @id_tipo_material)";
            MySqlCommand cmd = new MySqlCommand(query, conn);

            int ID = ((Item)cb_tipo.SelectedItem).ID;

            cmd.Parameters.AddWithValue("@nome", tb_nome_mate.Text);
            cmd.Parameters.AddWithValue("@preco", tb_preco_mate.Text);
            cmd.Parameters.AddWithValue("@stock", tb_stock.Text);
            cmd.Parameters.AddWithValue("@id_tipo_material", ID);

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Registo Inserido com Sucesso");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { conn.Close(); }

            carregaMateriais();
            limpa_texto_mate();
            
        }

        private void dgv_materiais_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        //botão atualizar
        private void b_atualiza_mate_Click(object sender, EventArgs e)
        {
            string query = "UPDATE materiais set Nome=@nome, Preco=@preco, Stock=@stock, ID_tipo_material=@ID_tipo_material where ID = @ID";
            MySqlCommand cmd = new MySqlCommand(query, conn);

            int ID = ((Item)cb_tipo.SelectedItem).ID;

            cmd.Parameters.AddWithValue("@id", tb_referencia_mate.Text);
            cmd.Parameters.AddWithValue("@nome", tb_nome_mate.Text);
            cmd.Parameters.AddWithValue("@preco", tb_preco_mate.Text);
            cmd.Parameters.AddWithValue("@stock", tb_stock.Text);
            cmd.Parameters.AddWithValue("@id_tipo_material", ID);

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Registo Atualizado com Sucesso");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { conn.Close(); }
            carregaMateriais();
            limpa_texto_mate();

            b_atualiza_mate.Enabled = false;
            b_eliminar_mate.Enabled = false;


        }

        //passagem dados da grelha para caixa texto
        private void dgv_materiais_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            
            DataGridViewRow row = dgv_materiais.SelectedRows[0];
            tb_referencia_mate.Text = row.Cells[0].Value.ToString();
            tb_nome_mate.Text = row.Cells[1].Value.ToString();
            tb_preco_mate.Text = row.Cells[2].Value.ToString().Replace(",", ".");
            tb_stock.Text = row.Cells[3].Value.ToString();
            cb_tipoMaterial.Text = row.Cells[5].Value.ToString(); ;
            

            b_atualiza_mate.Enabled = true;
            b_eliminar_mate.Enabled = true;
        }

        //Botão eliminar registo
        private void b_eliminar_mate_Click(object sender, EventArgs e)
        {
            string query = "Delete from materiais where ID =@ID";
            MySqlCommand cmd = new MySqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@ID", tb_referencia_mate.Text);

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Registo Eliminado com Sucesso");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { conn.Close(); }

            carregaMateriais();
            limpa_texto_mate();
            comboMaterial();
            b_atualiza_mate.Enabled = false;
            b_eliminar_mate.Enabled = false;
        }

        private void tb_pesquisa_mate_TextChanged(object sender, EventArgs e)
        {
            bsourceMateriais.Filter = "[nome] like '%" + tb_pesquisa_mate.Text + "%'";
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab(tabPage1);
        }

        //pagina fornecedores
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab(tabPage2);
        }
        public void carregaTipoMaterial()
        {
            dtTipoMaterial.Clear();
            MySqlDataAdapter MyDA = new MySqlDataAdapter();
            string query = "SELECT ID as Referencia, descricao as Descricao FROM tipo_material";
            MyDA.SelectCommand = new MySqlCommand(query, conn);
            MyDA.Fill(dtTipoMaterial);
            bsourceTipoMaterial.DataSource = dtTipoMaterial;
            dgv_tipo_material.DataSource = bsourceTipoMaterial;
        }

       
        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab(tabPage4);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }           
               

       
        private void b_eliminar_tipo_Click_1(object sender, EventArgs e)
        {
            
        }
        

        //fornecedores

        //tabela fornecedores
        public void carregaFornecedores()
        {
            dtFornecedores.Clear();
            MySqlDataAdapter Myda = new MySqlDataAdapter();
            string select = "SELECT  fornecedores.ID , fornecedores.nome as Nome, fornecedores.morada as Morada, fornecedores.ID_tipo_material as RefMaterial, tipo_material.descricao as Descricao from tipo_material inner join fornecedores on fornecedores.ID_tipo_material = tipo_material.ID";
            Myda.SelectCommand=new MySqlCommand(select,conn);
            Myda.Fill(dtFornecedores);
            bsourceFornecedores.DataSource= dtFornecedores;
            dgv_fornecedores.DataSource = dtFornecedores;
        }

        //limpar campos fornecedores
        public void limpaTextoFornecedores()
        {
            tb_morada_forne.ResetText();
            tb_nome_forne.ResetText();
            tb_refeForne.ResetText();
            cb_tipo.SelectedIndex = 0;
            tb_pesquisaForne.ResetText();

            b_atualizaForne.Enabled = false;
            b_eliminaForne.Enabled = false;

        }
        private void b_limpa_forne_Click(object sender, EventArgs e)
        {
            limpaTextoFornecedores();
        }

        //passagem dados para campos
        private void dgv_fornecedores_DoubleClick(object sender, EventArgs e)
        {
            DataGridViewRow row= dgv_fornecedores.SelectedRows[0];
            tb_nome_forne.Text = row.Cells[1].Value.ToString();
            tb_morada_forne.Text = row.Cells[2].Value.ToString();
            tb_refeForne.Text = row.Cells[0].Value.ToString();
            cb_tipo.Text = row.Cells[3].Value.ToString();

            b_atualizaForne.Enabled = true;
            b_eliminaForne.Enabled = true;
        }

        //botão inserir dados fornecedores
        private void b_inserirForne_Click(object sender, EventArgs e)
        {
            int ID = ((Item)cb_tipo.SelectedItem).ID;
            

            string query = "INSERT INTO fornecedores (Nome, Morada, id_tipo_material) VALUES (@nome, @morada, @id_tipo_material)";
            MySqlCommand cmd= new MySqlCommand(query,conn);

            cmd.Parameters.AddWithValue("@nome", tb_nome_forne.Text);
            cmd.Parameters.AddWithValue("@morada", tb_morada_forne.Text);
            cmd.Parameters.AddWithValue("@id_tipo_material", ID);

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Registo Inserido com Sucesso");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { conn.Close(); }
            carregaFornecedores();
            limpaTextoFornecedores();
            comboFornecedores();
        }

        //atualizar fornecedores
        private void b_atualizaForne_Click(object sender, EventArgs e)
        {
            string query = "UPDATE fornecedores set nome=@nome , morada=@morada, id_tipo_material=@id_tipo_material WHERE ID=@ID";
            MySqlCommand cmd = new MySqlCommand(query, conn);

            int ID = ((Item)cb_tipo.SelectedItem).ID;

            cmd.Parameters.AddWithValue("@ID", tb_refeForne.Text);
            cmd.Parameters.AddWithValue("@nome", tb_nome_forne.Text);
            cmd.Parameters.AddWithValue("@morada", tb_morada_forne.Text);
            cmd.Parameters.AddWithValue("@id_tipo_material", ID);

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Registo Atualizado com Sucesso");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { conn.Close(); }
            carregaFornecedores();
            limpaTextoFornecedores();
            
        }

        //eliminar fornecedores
        private void b_eliminaForne_Click(object sender, EventArgs e)
        {
            string query = "DELETE FROM fornecedores WHERE ID=@ID";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@ID", tb_refeForne.Text);

            try
            {
                conn.Open(); cmd.ExecuteNonQuery();
                MessageBox.Show("Registo Eliminado com Sucesso");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { conn.Close(); }

            carregaFornecedores();
            limpaTextoFornecedores();

            b_atualizaForne.Enabled = false;
            b_eliminaForne.Enabled = false;
        }

        //pesquisa fornecedores
        private void tb_pesquisaForne_TextChanged(object sender, EventArgs e)
        {
            bsourceFornecedores.Filter = "[nome] like '%" + tb_pesquisaForne.Text + "%'";
        }

        //pagina encomendas
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab(tb_valor);
        }

        //limpar campos encomendas
        public void limpaTextoEncomendas()
        {
            cb_material.SelectedIndex= 0;
            tb_preco_enc.Text = "0";
            tb_quantidade.Text="0";
            tb_valor_enc.ResetText();
            tb_nEnc.ResetText();
            tb_pesquisa_encomenda.ResetText();
            tb_pesquisa_encomenda.Enabled= false;

            b_eliminaForne.Enabled=false;

            rb_fornecedor.Checked=false;
            rb_material.Checked=false;
            rb_nEncomenda.Checked=false;
                
        }
       
        //carregar tabela encomendas
        public void carregaEncomendas()
        {
            dtEncomendas.Clear();
            MySqlDataAdapter Myda= new MySqlDataAdapter();
            string select = "SELECT encomendas.ID as Nº, encomendas.ID_materiais as RefMaterial, materiais.nome as Material, materiais.preco as Preco, encomendas.quantidade as Quantidade, encomendas.ID_fornecedor, fornecedores.nome as Fornecedor, encomendas.valor from encomendas inner join materiais on encomendas.ID_materiais = materiais.ID inner join fornecedores on encomendas.ID_fornecedor = fornecedores.ID";
            Myda.SelectCommand = new MySqlCommand(select,conn);
            Myda.Fill(dtEncomendas);
            bsourceEncomendas.DataSource = dtEncomendas;
            dgv_encomendas.DataSource = bsourceEncomendas;
            
        }
        
        //carregar comboBox material
        public void comboMaterial()
        {
            try
            {
                string query = "SELECT ID, nome FROM materiais";
                MySqlCommand cmd = new MySqlCommand(query, conn);

                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                List<Item> listMaterial = new List<Item>();

                listMaterial.Add(new Item());

                while (reader.Read())
                {
                    
                    listMaterial.Add(new Item() { ID = Int32.Parse(reader[0].ToString()), Value = reader[1].ToString() });
                }

                conn.Close();

                cb_material.DataSource = listMaterial;
                cb_material.DisplayMember = "Value";
                cb_material.ValueMember = "ID";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        //CARREGAR PRECO
        private void cb_material_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int ID = ((Item)cb_material.SelectedItem).ID;

                string query = "SELECT preco FROM materiais WHERE ID = @ID";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", ID);

                if(conn.State != ConnectionState.Open)
                    conn.Open();

                cmd.ExecuteNonQuery();
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    tb_preco_enc.Text = reader[0].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show (ex.Message);
            }
            finally
            {
                conn.Close();
            }
            
        }

        //carrega comboBox fornecedores
        public void comboFornecedores()
        {
            try
            {
                string query = "SELECT ID, nome FROM fornecedores";
                MySqlCommand cmd = new MySqlCommand(query, conn);

                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                List<Item> listFornecedores = new List<Item>();
                listFornecedores.Add(new Item());

                while (reader.Read())
                {

                    listFornecedores.Add(new Item() { ID = Int32.Parse(reader[0].ToString()), Value = reader[1].ToString() });
                }

                cb_fornecedor.DataSource = listFornecedores;
                cb_fornecedor.DisplayMember = "Value";
                cb_fornecedor.ValueMember = "ID";

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        //Calculo valor encomenda
        public void calculoValor()
        {
            double preco;
            double quantidade;
            double valor;

            if (tb_quantidade.Text == "" || tb_preco_enc.Text == "")
            {
                tb_valor_enc.Text = "0";
                return;
            }

            preco = Convert.ToDouble(tb_preco_enc.Text);
            quantidade = Convert.ToDouble(tb_quantidade.Text);
            valor = Math.Round((quantidade * preco), 2);
            tb_valor_enc.Text = valor.ToString();
        }

        private void tb_quantidade_TextChanged(object sender, EventArgs e)
        {
            calculoValor();
        }

        private void tb_preco_enc_TextChanged(object sender, EventArgs e)
        {
            calculoValor();
        }

        //Realizar encomenda
        private void b_realizar_enc_Click(object sender, EventArgs e)
        {
            string query = "INSERT INTO encomendas (ID_materiais, ID_fornecedor, quantidade, valor) VALUES (@ID_materiais, @ID_fornecedor, @quantidade, @valor)";
            MySqlCommand cmd = new MySqlCommand(query,conn);

            int ID_mateiral = ((Item)cb_material.SelectedItem).ID;
            int ID_fornecedor = ((Item)cb_fornecedor.SelectedItem).ID;

            cmd.Parameters.AddWithValue("@ID_materiais", ID_mateiral);
            cmd.Parameters.AddWithValue("@ID_fornecedor", ID_fornecedor);
            cmd.Parameters.AddWithValue("@quantidade", tb_quantidade.Text);
            cmd.Parameters.AddWithValue("@valor", tb_valor_enc.Text);

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Registo Inserido com Sucesso");
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message);
            }
            finally { conn.Close(); }

            string queryStock = "UPDATE materiais SET stock = stock + @stock WHERE ID=@ID";
            MySqlCommand cmdStock = new MySqlCommand(queryStock,conn);

            cmdStock.Parameters.AddWithValue("@ID", ID_mateiral);
            cmdStock.Parameters.AddWithValue("@stock", tb_quantidade.Text);

            try
            {
                conn.Open(); 
                cmdStock.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show (ex.Message);
            }
            finally { conn.Close(); }

            carregaEncomendas();
            limpaTextoEncomendas();
            carregaMateriais();
        }

        //eliminar encomenda
        private void b_elimina_enc_Click(object sender, EventArgs e)
        {
            string query = "DELETE FROM encomendas WHERE ID=@ID";
            MySqlCommand cmd= new MySqlCommand(query,conn);

            cmd.Parameters.AddWithValue("@ID", tb_nEnc.Text);

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Encomenda Eliminada com Sucesso");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { conn.Close(); }

            string queryStock = "UPDATE materiais set stock = stock - @stock WHERE ID=@ID";
            MySqlCommand cmdStock = new MySqlCommand(queryStock,conn);

            try
            {
                conn.Open();
                cmdStock.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { conn.Close(); }

            carregaEncomendas();
            limpaTextoEncomendas();
            carregaMateriais();
        }

        //atualizar encoemdna
        private void b_atuliza_enc_Click(object sender, EventArgs e)
        {
            limpaTextoEncomendas();
            
        }

        //passagem dados para campos
        private void dgv_encomendas_DoubleClick(object sender, EventArgs e)
        {
            DataGridViewRow row = dgv_encomendas.SelectedRows[0];
            cb_material.Text = row.Cells[2].Value.ToString();
            tb_preco_enc.Text = row.Cells[3].Value.ToString();
            tb_quantidade.Text = row.Cells[4].Value.ToString();
            tb_nEnc.Text = row.Cells[0].Value.ToString();
            cb_fornecedor.Text = row.Cells[6].Value.ToString();
            tb_valor_enc.Text = row.Cells[7].Value.ToString();

            b_elimina_enc.Enabled = true;
        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {            
        }

        //pesquisa 
        private void rb_material_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_material.Checked==true) 
                tb_pesquisa_encomenda.Enabled = true;
        }

        private void rb_nEncomenda_CheckedChanged(object sender, EventArgs e)
        {
            if(rb_nEncomenda.Checked==true)
                tb_pesquisa_encomenda.Enabled = true;
        }

        private void rb_fornecedor_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_fornecedor.Checked==true)
                tb_pesquisa_encomenda.Enabled = true;
        }

        private void tb_pesquisa_encomenda_TextChanged(object sender, EventArgs e)
        {
            

            if (rb_fornecedor.Checked == true)
                bsourceEncomendas.Filter = "[fornecedor] like '%" + tb_pesquisa_encomenda.Text + "%'";

            if (rb_material.Checked==true)
                bsourceEncomendas.Filter = "[material] like '%" + tb_pesquisa_encomenda.Text + "%'";

            if (rb_nEncomenda.Checked == true)
            {
                bsourceEncomendas.Filter = "[Nº] = " + tb_pesquisa_encomenda.Text;                
            }
        }

        //pagina produto

        public void carregaProdutos()
        {
            dtProduto.Clear();
            MySqlDataAdapter Myda = new MySqlDataAdapter();
            string select = "SELECT ID as Referencia, nome as Produto, preco_venda as Preco, tempo_producao as Tempo FROM produtos";
            Myda.SelectCommand= new MySqlCommand(select,conn);
            Myda.Fill(dtProduto);
            bsourceProduto.DataSource = dtProduto;
            dgv_produtos.DataSource = bsourceProduto;
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab(tabPage3);
        }
       
        //limpar texto produto
        public void limpaTextoProdutos()
        {
            tb_nome_peca.ResetText();
            tb_refe_peca.ResetText();
            tb_preco_venda.ResetText();
            tb_tempo.ResetText();

            b_eliminar_peca.Enabled = false;
            b_atualizar_peca.Enabled = false;
        }

        //inserir produtos
        private void b_inserir_peca_Click(object sender, EventArgs e)
        {
            string query = "INSERT INTO produtos (nome, preco_venda, tempo_producao) VALUES (@nome, @preco_venda, @tempo_producao)";
            MySqlCommand cmd = new MySqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@nome", tb_nome_peca.Text);
            cmd.Parameters.AddWithValue("@preco_venda", tb_preco_venda.Text);
            cmd.Parameters.AddWithValue("@tempo_producao", tb_tempo.Text);

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Produto Registado com Sucesso");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { conn.Close(); }

            carregaProdutos();
            limpaTextoProdutos();
        }

        private void b_limpar_peca_Click(object sender, EventArgs e)
        {
            limpaTextoProdutos();
        }

        private void dgv_produtos_DoubleClick(object sender, EventArgs e)
        {
            DataGridViewRow row = dgv_produtos.SelectedRows[0];
            tb_refe_peca.Text = row.Cells[0].Value.ToString();
            tb_nome_peca.Text = row.Cells[1].Value.ToString();
            tb_preco_venda.Text = row.Cells[2].Value.ToString();
            tb_tempo.Text = row.Cells[3].Value.ToString();

            b_eliminar_peca.Enabled = true;
            b_atualizar_peca.Enabled=true;
        }

        private void b_atualizar_peca_Click(object sender, EventArgs e)
        {
            string query = "UPDATE produtos set nome=@nome, preco_venda=@preco_venda, tempo_producao=@tempo_producao WHERE ID=@ID";
            MySqlCommand cmd = new MySqlCommand(query,conn);

            cmd.Parameters.AddWithValue("@ID", tb_refe_peca.Text);
            cmd.Parameters.AddWithValue("@nome", tb_nome_peca.Text);
            cmd.Parameters.AddWithValue("@preco_venda", tb_preco_venda.Text);
            cmd.Parameters.AddWithValue("@tempo_producao", tb_tempo.Text);

            try
            { 
                conn.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Peça atualizada com sucesso");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { conn.Close(); }

            carregaProdutos();
            limpaTextoProdutos();
        }

        private void b_eliminar_peca_Click(object sender, EventArgs e)
        {
            string query = "DELETE FROM produtos WHERE ID=@ID";
            MySqlCommand cmd = new MySqlCommand(query,conn);

            cmd.Parameters.AddWithValue("@ID", tb_refe_peca.Text);

            try
            {
                conn.Open(); cmd.ExecuteNonQuery();
                MessageBox.Show("Peça Eliminada com Sucesso");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { conn.Close(); }
            
            carregaProdutos();
            limpaTextoProdutos();
        }

        private void tb_pesquisa_peca_TextChanged(object sender, EventArgs e)
        {
            bsourceProduto.Filter = "[produto] like '%" + tb_pesquisa_peca.Text + "%'";
        }

        private void label28_Click(object sender, EventArgs e)
        {

        }

        //pagina materiais utilizados
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab(tabPage5);
        }

        public void limpaTextoUtilizados()
        {
            cb_pecas.SelectedIndex = 0;
            tb_refe_peca.ResetText();
            rb_peca.Checked = false;
            rb_mate_utilizado.Checked = false;

            lb_utilizados.DataSource = null;

            //lb_utilizados.Items.Clear();
            tb_quantidade_utilizado.ResetText();
            tb_pesquisa_utilizados.ResetText();
            tb_pesquisa_utilizados.BackColor = Color.Silver;
            tb_pesquisa_utilizados.Enabled = false;
            b_eliminar_utilizados.Enabled=false;
        }
        public void comboPecas()
        {
            try
            {
                string query = "SELECT ID, nome FROM produtos";
                MySqlCommand cmd = new MySqlCommand(query, conn);

                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                List<Item> listPecas = new List<Item>();
                listPecas.Add(new Item());


                while (reader.Read())
                {

                    listPecas.Add(new Item() { ID = Int32.Parse(reader[0].ToString()), Value = reader[1].ToString() });
                }

                cb_pecas.DataSource = listPecas;
                cb_pecas.DisplayMember = "Value";
                cb_pecas.ValueMember = "ID";

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        public void carregaUtilizados()
        {
            dtUtilizados.Clear();
            MySqlDataAdapter MyDA = new MySqlDataAdapter();
            string select = "SELECT materiais_produtos.ID as Referencia, materiais_produtos.ID_produto, produtos.nome as Peca, materiais_produtos.ID_materiais, materiais.nome as Material, materiais_produtos.quantidade_material as Quantiade FROM materiais_produtos INNER JOIN materiais on materiais_produtos.ID_materiais = materiais.ID INNER JOIN produtos on materiais_produtos.ID_produto = produtos.ID";
            MyDA.SelectCommand= new MySqlCommand(select, conn);
            MyDA.Fill(dtUtilizados);
            bsourceUtilizados.DataSource = dtUtilizados;
            dgv_utilizados.DataSource = bsourceUtilizados;
        }
        public void carregaListaMateriais()
        {
            try
            {
                string query = "SELECT ID, nome FROM materiais";
                MySqlCommand cmd = new MySqlCommand(query, conn);

                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                List<Item> listMateUtilizados = new List<Item>();


                while (reader.Read())
                {

                    listMateUtilizados.Add(new Item() { ID = Int32.Parse(reader[0].ToString()), Value = reader[1].ToString() });
                }

                lb_materiais.DataSource = listMateUtilizados;
                lb_materiais.DisplayMember = "Value";
                lb_materiais.ValueMember = "ID";

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            
        }

        public void b_adicionar_Click(object sender, EventArgs e)
        {
            List<Item> itens = new List<Item>();
            
            int ID_material = ((Item)lb_materiais.SelectedItem).ID;
            string material = ((Item)lb_materiais.SelectedItem).Value;

            itens.Add(
                new Item {
                    ID = ID_material,
                    Value = tb_quantidade_utilizado.Text + "x " + material
                }
                );           

            lb_utilizados.DataSource= itens;
            lb_utilizados.DisplayMember= "Value";
            lb_utilizados.ValueMember= "ID";
            tb_quantidade_utilizado.ResetText();
           
        }

        private void b_inserir_utilizados_Click(object sender, EventArgs e)
        {
            
            foreach (Item item in lb_utilizados.Items)
            {
                int ID_material = item.ID;
                string quantidade = item.ToString().Split("x")[0].Trim();

                    
                    string query = "INSERT INTO materiais_produtos (ID_produto, ID_materiais, quantidade_material) VALUES (@ID_produto, @ID_materiais, @quantidade_material)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);


                    int ID_peca = ((Item)cb_pecas.SelectedItem).ID;

                    cmd.Parameters.AddWithValue("@ID_produto", ID_peca);
                    cmd.Parameters.AddWithValue("@ID_materiais", ID_material);
                    cmd.Parameters.AddWithValue("@quantidade_material", quantidade);
                    
                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally { conn.Close(); }

                    string queryStock = "UPDATE materiais SET stock = stock - @stock WHERE ID=@ID";
                    MySqlCommand cmdStock = new MySqlCommand(queryStock, conn);

                    cmdStock.Parameters.AddWithValue("@ID", ID_material);
                    cmdStock.Parameters.AddWithValue("@stock", quantidade);

                    try
                    {
                        conn.Open();
                        cmdStock.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    { 
                        MessageBox.Show(ex.Message);
                    }
                    finally { conn.Close(); }
                
            }
            carregaMateriais();
            carregaUtilizados();
            limpaTextoUtilizados();
        }

        private void rb_peca_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_peca.Checked == true)
            {
                tb_pesquisa_utilizados.Enabled = true;
                tb_pesquisa_utilizados.BackColor= Color.White;
            }
        }

        private void rb_mate_utilizado_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_mate_utilizado.Checked == true)
            {
                tb_pesquisa_utilizados.Enabled = true;
                tb_pesquisa_utilizados.BackColor = Color.White;
            }
        }

        private void tb_pesquisa_utilizados_TextChanged(object sender, EventArgs e)
        {
            if (rb_peca.Checked == true)
                bsourceUtilizados.Filter = "[peca] like '%" + tb_pesquisa_utilizados.Text + "%'";

            if (rb_mate_utilizado.Checked == true)
                bsourceUtilizados.Filter = "[material] like '%" + tb_pesquisa_utilizados.Text + "%'";
        }

        private void b_limpa_utilizados_Click(object sender, EventArgs e)
        {
            limpaTextoUtilizados();
        }

        private void b_eliminar_utilizados_Click(object sender, EventArgs e)
        {
            string query = "DELETE FROM materiais_produtos WHERE ID=@ID";
            MySqlCommand cmd= new MySqlCommand(query,conn);

            cmd.Parameters.AddWithValue("@ID", dgv_utilizados.SelectedCells[0].Value);

            try 
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Registo eliminado com sucesso");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            string queryStock = "UPDATE materiais SET stock = stock + @stock WHERE ID=@ID";
            MySqlCommand cmdStock = new MySqlCommand(queryStock,conn);

            cmdStock.Parameters.AddWithValue("@ID", dgv_utilizados.SelectedCells[3].Value.ToString());
            cmdStock.Parameters.AddWithValue("@stock", dgv_utilizados.SelectedCells[5].Value.ToString());

            try
            {
                conn.Open();
                cmdStock.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            carregaMateriais();
            carregaUtilizados();
            limpaTextoUtilizados();
        }
        

        private void dgv_utilizados_DoubleClick(object sender, EventArgs e)
        {
            lb_utilizados.Items.Clear();
            DataGridViewRow row = dgv_utilizados.SelectedRows[0];
            tb_refe_peca.Text = row.Cells[1].ToString();
            cb_pecas.Text= row.Cells[2].Value.ToString();
            lb_utilizados.Items.Add(row.Cells[4].Value.ToString() + "--" + row.Cells[5].Value.ToString() + "x");
            b_eliminar_utilizados.Enabled = true;
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            DialogResult escolha =MessageBox.Show ("Tem a certeza que pretende sair?", "Menssagem", MessageBoxButtons.YesNo);
            if (escolha == DialogResult.Yes)
                Environment.Exit(0);
            else
                conn.Close();
        }
    }
}