using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaCadastro
{
    public partial class Form1 : Form
    {
        List<Pessoa> pessoas;

        public Form1()
        {
            InitializeComponent();

            pessoas = new List<Pessoa>();

            // Configurar o ComboBox para não permitir digitação
            comboEstadoCivil.DropDownStyle = ComboBoxStyle.DropDownList;

            comboEstadoCivil.Items.Add("Casado(a)");
            comboEstadoCivil.Items.Add("Solteiro(a)");
            comboEstadoCivil.Items.Add("União Estável");
            comboEstadoCivil.Items.Add("Divorciado(a)");
            comboEstadoCivil.Items.Add("Viúvo(a)");
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            int index = -1;

            foreach (Pessoa pessoa in pessoas)
            {
                if (pessoa.Nome == txtNome.Text)
                {
                    index = pessoas.IndexOf(pessoa);
                }
            }

            if (txtNome.Text == "")
            {
                MessageBox.Show("Preencha o campo nome.");
                txtNome.Focus();
                return;
            }

            DateTime dataSelecionada = txtData.Value; // Obtém a data selecionada no DateTimePicker
            DateTime dataAtual = DateTime.Today; // Obtém a data atual

            int idade = dataAtual.Year - dataSelecionada.Year;

            // Ajusta a idade se o aniversário ainda não foi atingido neste ano
            if (dataSelecionada > dataAtual.AddYears(-idade))
            {
                idade--;
            }

            if (idade < 18)
            {
                MessageBox.Show("A idade deve ser superior a 18 anos.");
                txtNome.Focus(); // Retorna o foco para o campo de nome
                return;
            }

            if (comboEstadoCivil.SelectedIndex == -1)
            {
                MessageBox.Show("Preencha o campo estado civil.");
                comboEstadoCivil.Focus();
                return;
            }

            if (!txtFone.MaskFull)
            {
                MessageBox.Show("Preencha o campo telefone.");
                txtFone.Focus();
                return;
            }

            char sexo = '0';

            if (!radioMasculino.Checked && !radioFeminino.Checked && !radioOutro.Checked)
            {
                MessageBox.Show("Por favor, selecione o sexo.");
                return;
            }
            else if (radioMasculino.Checked)
            {
                sexo = 'M';
            }

            else if (radioFeminino.Checked)
            {
                sexo = 'F';
            }

            else
            {
                sexo = 'O';
            }

            Pessoa p = new Pessoa();
            p.Nome = txtNome.Text;
            p.DataNasc = txtData.Text;
            p.EstadoCivil = comboEstadoCivil.SelectedItem.ToString();
            p.Telefone = txtFone.Text;
            p.CasaPropria = checkCasa.Checked;
            p.Veiculo = checkVeiculo.Checked;
            p.Sexo = sexo;

            if (index < 0)
            {
                pessoas.Add(p);
                Listar();
                MessageBox.Show("Cadastro concluído com sucesso!");
            }
            else
            {
                pessoas[index] = p;
                Listar();
                MessageBox.Show("Alteração concluída com sucesso!");
            }

            btnLimpar_Click(btnLimpar, EventArgs.Empty);
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            int indice = lista.SelectedIndex;

            // Verificar se algum item está selecionado
            if (indice == -1)
            {
                MessageBox.Show("Selecione um usuário para excluir.");
                return;
            }

            // Obter o nome do usuário selecionado
            string nomeUsuario = pessoas[indice].Nome;

            // Exibir o MessageBox de confirmação com o nome do usuário
            DialogResult resultado = MessageBox.Show(
                $"Tem certeza de que deseja excluir o usuário {nomeUsuario}?",
                "Confirmar Exclusão",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            // Se o usuário clicar em "Sim", exclui o item
            if (resultado == DialogResult.Yes)
            {
                pessoas.RemoveAt(indice);
                Listar();  // Atualiza a lista
                MessageBox.Show("Usuário excluído com sucesso!");
            }
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            txtNome.Text = "";
            txtData.Text = "";
            comboEstadoCivil.SelectedIndex = -1;
            txtFone.Text = "";
            checkCasa.Checked = false;
            checkVeiculo.Checked = false;
            radioMasculino.Checked = false;
            radioFeminino.Checked = false;
            radioOutro.Checked = false;
            txtNome.Focus();
        }

        private void Listar()
        {
            lista.Items.Clear();

            foreach (Pessoa p in pessoas)
            {
                lista.Items.Add(p.Nome);
            }
        }

        private void lista_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int indice = lista.SelectedIndex;
            Pessoa p = pessoas[indice];

            txtNome.Text = p.Nome;
            txtData.Text = p.DataNasc;
            comboEstadoCivil.SelectedItem = p.EstadoCivil;
            txtFone.Text = p.Telefone;
            checkCasa.Checked = p.CasaPropria;
            checkVeiculo.Checked = p.Veiculo;

            switch(p.Sexo)
            {
                case 'M':
                    radioMasculino.Checked = true;
                    break;
                case 'F':
                    radioFeminino.Checked = true;
                    break;
                case 'O':
                    radioOutro.Checked = true;
                    break;
            }
        }
    }
}