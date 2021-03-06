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
    public partial class FrmTipoServico : Form
    {
        public FrmTipoServico()
        {
            InitializeComponent();
            _TipoServico = new GerenciadorFinanceiro.Dominio.TipoServico();
        }

        private Dominio.TipoServico _TipoServico;

        private BindingSource _ListSource;
        private List<Dominio.TipoServico> _ListaTipoServicos;
        
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
            TxtDescricao.Text = _TipoServico.Descricao;

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
            _TipoServico.Descricao = TxtDescricao.Text;    
        }

        #endregion

        private void ctrNavigator1_EditarRegistro(object objEditar)
        {
            if(objEditar !=null)
                _TipoServico = (Dominio.TipoServico)objEditar;
            this.CamposInterface(Status.Editando);
        }

        private void ctrNavigator1_EventoNovo()
        {
            _TipoServico = new GerenciadorFinanceiro.Dominio.TipoServico();
            this.CamposInterface(Status.Inserindo);
        }

        private void ctrNavigator1_ExcluirRegistro(object objExcluir)
        {
            if (MessageBox.Show("Deseja excluir o registro.", "Atenção!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if(objExcluir!=null)
                    _TipoServico = (Dominio.TipoServico)objExcluir;
                this.CamposInterface(Status.Excluindo);
                try
                {
                    new Repositorio.RepositorioTipoServico().DeletarObjeto(_TipoServico);
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
                if(objSalvar!=null)
                    _TipoServico = (Dominio.TipoServico)objSalvar;
                ValidaCampos();
                if (_TipoServico.IdTipoServico == 0)
                {
                    new Repositorio.RepositorioTipoServico().SalvarObjeto(_TipoServico);
                    _ListaTipoServicos.Add(_TipoServico);
                }
                else
                    new Repositorio.RepositorioTipoServico().AtualizarObjeto(_TipoServico);
                _ListSource.ResetBindings(true);
                MessageBox.Show("Registro salvo com suceso!", "OK");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Atenção!");
            }
        }

        private void FrmTipoServico_Load(object sender, EventArgs e)
        {
            try
            {                
                _ListaTipoServicos = new Repositorio.RepositorioTipoServico().BuscarTodos();
                _ListSource = new BindingSource(_ListaTipoServicos, "");
                DGServicos.DataSource = _ListSource;
                this.CamposInterface(Status.Consultando);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }            
        }

        private void ctrNavigator1_CancelarAcao()
        {
            if (DGServicos.SelectedRows.Count > 0)
                _TipoServico = (Dominio.TipoServico)DGServicos.SelectedRows[0].DataBoundItem;
            this.CamposInterface(Status.Consultando);
        }

        private void DGServicos_SelectionChanged(object sender, EventArgs e)
        {
            if (this.DGServicos.Rows.Count > 0)
            {
                if (DGServicos.SelectedRows.Count > 0)
                {
                    _TipoServico = (Dominio.TipoServico)DGServicos.SelectedRows[0].DataBoundItem;
                    if (ctrNavigator1.DataSource != null)
                        ctrNavigator1.Indice = DGServicos.SelectedRows[0].Index;
                }
            }
            this.CamposInterface(Status.Consultando);
        }
    }
}
