﻿using System;
using System.Net.Mail;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GerenciadorFinanceiro.GUI
{
    public partial class FrmEmail : Form
    {

        List<Attachment> _ListaAnexos = new List<Attachment>();

        public FrmEmail()
        {
            InitializeComponent();
        }

        private void BtnAnexar_Click(object sender, EventArgs e)
        {
            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
                for (int i = 0; i < OpenFile.FileNames.Length; i++)
                {
                    ListAnexos.Items.Add(OpenFile.FileNames[i]);
                }
            }
        }

        private void BtnRemoverAnexo_Click(object sender, EventArgs e)
        {
            if (ListAnexos.SelectedIndex > -1)
            {
                ListAnexos.Items.RemoveAt(ListAnexos.SelectedIndex);
                ListAnexos.Refresh();
            }
        }

        private void BtnEnviarEmail_Click(object sender, EventArgs e)
        {
            try
            {
                this.EnviarEmail();
                MessageBox.Show("Email enviado com sucesso.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro");
            }
        }

        private void EnviarEmail()
        {
            if (new Servicos.Email().ValidaEmail(TxtDestinatario.Text.Trim()) == false)
                throw new Exception("Email inválido.");
            this.AdicionarAnexos();
            new Servicos.Email().EnviarEmail(Properties.Settings.Default.Email, Properties.Settings.Default.Remetente, TxtDestinatario.Text,
                                                    TxtAssunto.Text, TxtMensagem.Text, Properties.Settings.Default.ServidorSMTP, Properties.Settings.Default.SenhaAutenticar, _ListaAnexos);
        }

        private void AdicionarAnexos()
        {
            for (int j = 0; j < ListAnexos.Items.Count; j++)
            {
                try
                {
                    _ListaAnexos.Add(new Attachment((string)ListAnexos.Items[j]));
                }
                catch (Exception ex)
                {
                    throw new Exception("Não foi possível adicionar todos os anexos. ", ex);
                }
            }
        }
    }
}
