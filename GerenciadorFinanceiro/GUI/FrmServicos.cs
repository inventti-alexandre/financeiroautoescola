﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GerenciadorFinanceiro.GUI
{
    public partial class FrmServicos : Form
    {
        public FrmServicos()
        {
            InitializeComponent();
        }
        
        private List<Dominio.Servico> _ListaServicos;
        private BindingSource _ListSource;

        private List<Dominio.TipoServico> _ListaTipoServico;
        private Dominio.Servico _Servico = new Dominio.Servico();

        #region "Controlando alterações"

        private void EnabledCampos(bool enabled)
        {
            foreach (Control ctr in this.Controls)
            {
                if (ctr is ComboBox)
                    ((ComboBox)ctr).Enabled = enabled;
                else if (ctr is TextBox)
                    ((TextBox)ctr).Enabled = enabled;
                else if (ctr is MaskedTextBox)
                    ((MaskedTextBox)ctr).Enabled = enabled;
                else if (ctr is Button)
                    ((Button)ctr).Enabled = enabled;
            }
        }

        private void CamposInterface(Status status)
        {
            TxtDescricao.Text = _Servico.Descricao;
            txtValor.Text = _Servico.Valor.ToString();            

            if (status == Status.Inserindo)
            {
                this.EnabledCampos(true);
                LblStatus.Text = "Status : Inserindo";
            }
            else if (status == Status.Editando)
            {
                this.EnabledCampos(true);
                LblStatus.Text = "Status : Editando";
            }
            else if (status == Status.Excluindo)
            {
                this.EnabledCampos(false);
                LblStatus.Text = "Status : Excluindo";
            }
            else
            {
                this.EnabledCampos(false);
                LblStatus.Text = "Status : Consultando";
            }
        }

        private void ValidaCampos()
        {
            if (TxtDescricao.Text.Trim() == string.Empty)
                throw new Exception("O campo descrição está em branco.");
            double valor;
            if (!double.TryParse(txtValor.Text, out valor))
                throw new Exception("O campo valor tem um número inválido.");
            if (cmbTipoServico.SelectedItem == null)
                throw new Exception("Por favor escolha um tipo de serviço.");
            _Servico.Tipo = (Dominio.TipoServico)cmbTipoServico.SelectedItem;
            _Servico.Descricao = TxtDescricao.Text;                        
            _Servico.Valor = valor;
            _Servico.Observacao = txtObservacao.Text;
        }

        private void BuscarTodosOsServicos()
        {
            _ListaServicos = new Repositorio.RepositorioServico().BuscarTodos();
            this.ctrNavigator1.DataSource = _ListaServicos;
            _ListSource = new BindingSource(_ListaServicos, "");
            DGServicos.DataSource = _ListSource;
        }

        private void BuscarTiposDeServicos()
        {
            _ListaTipoServico = new Repositorio.RepositorioTipoServico().BuscarTodos();
            cmbTipoServico.DataSource = _ListaTipoServico;
            cmbTipoServico.DisplayMember = "Descricao";
            cmbTipoServico.ValueMember = "IdTipoServico";
        }
        #endregion

        #region "Eventos formulario"

        private void FrmServicos_Load(object sender, EventArgs e)
        {
            try
            {
                this.BuscarTodosOsServicos();
                BuscarTiposDeServicos();
                this.CamposInterface(Status.Consultando);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }            
        }



        #endregion 

        private void ctrNavigator1_CancelarAcao()
        {
            if (DGServicos.SelectedRows.Count > 0)
                _Servico = (Dominio.Servico)DGServicos.SelectedRows[0].DataBoundItem;
            this.CamposInterface(Status.Consultando);
        }

        private void ctrNavigator1_EditarRegistro(object objEditar)
        {
            this.CamposInterface(Status.Editando);
        }

        private void ctrNavigator1_EventoNovo()
        {
            _Servico = null;
            _Servico = new Dominio.Servico();
            this.CamposInterface(Status.Inserindo);
        }

        private void ctrNavigator1_ExcluirRegistro(object objExcluir)
        {
            if (MessageBox.Show("Deseja excluir o registro.", "Atenção!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.CamposInterface(Status.Excluindo);
                try
                {
                    new Repositorio.RepositorioServico().DeletarObjeto((Dominio.Servico)objExcluir);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Atenção!");
                }
            }
        }

        private void ctrNavigator1_MudaRegistroSelecionado(object objetoAtual)
        {
            DGServicos.Rows[ctrNavigator1.Indice].Selected = true;
        }

        private void ctrNavigator1_SalvarRegistro(object objSalvar)
        {
            try
            {
                this.ValidaCampos();
                if (_Servico.IdServico == 0)
                {
                    new Repositorio.RepositorioServico().SalvarObjeto(_Servico);
                    _ListaServicos.Add(_Servico);
                }
                else
                    new Repositorio.RepositorioServico().AtualizarObjeto(_Servico);

                _ListSource.ResetBindings(true);
                MessageBox.Show("Registro salvo com sucesso.", "Atenção.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Atenção.", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnNovoTipoServico_Click(object sender, EventArgs e)
        {
            FrmTipoServico frm = new FrmTipoServico();
            frm.ShowDialog();
            frm.Dispose();
            this.BuscarTiposDeServicos();
        }

        private void DGServicos_SelectionChanged(object sender, EventArgs e)
        {
            if (this.DGServicos.Rows.Count > 0)
            {
                if (DGServicos.SelectedRows.Count > 0)
                {
                    _Servico = (Dominio.Servico)DGServicos.SelectedRows[0].DataBoundItem;
                    if (ctrNavigator1.DataSource != null)
                        ctrNavigator1.Indice = DGServicos.SelectedRows[0].Index;
                }
            }
            this.CamposInterface(Status.Consultando);
        }
    }
}
