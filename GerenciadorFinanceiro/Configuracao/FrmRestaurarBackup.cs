﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GerenciadorFinanceiro.Configuracao
{
    public partial class FrmRestaurarBackup : Form
    {
        public FrmRestaurarBackup()
        {
            InitializeComponent();
        }

        private void BtnRealizarBackupAgora_Click(object sender, EventArgs e)
        {        
            if (MessageBox.Show("Tem certeza que deseja restaurar o sistema da backup Selecionado ?  ","Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                String restaurar = System.IO.Path.Combine(Properties.Settings.Default.LocalBackupDB, CmbListaBackups.Text);
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                new Servicos.Backup().RestaurarBancoDados(restaurar);
                this.Cursor = System.Windows.Forms.Cursors.Default;
                this.Close();
            }
        }

        private void FrmRestaurarBackup_Load(object sender, EventArgs e)
        {
            CmbListaBackups.Items.Clear();
            //StringComparer ordenar = StringComparer.CurrentCultureIgnoreCase;
            //Array.Sort(registrosBackup, ordenar);
            String[] registrosBackup = new Servicos.Backup().ListarTodosOsBackup();
            Array.Sort(registrosBackup, new System.Comparison<string>(OrdenarPorData));

            for (int i = 0; i < registrosBackup.Length; i++)
                CmbListaBackups.Items.Add(registrosBackup[i].Substring(registrosBackup[i].Length - 17, 17));
            if (CmbListaBackups.Items.Count > 0)
                CmbListaBackups.SelectedIndex = 0;
            else
                MessageBox.Show("Nenhum registro de backup encontrado. ", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private int OrdenarPorData(string data1, string data2)
        {
            DateTime dataum = DateTime.Parse(data1.Substring(data1.Length -10, 10));
            DateTime datadois = DateTime.Parse(data2.Substring(data2.Length - 10, 10));
            if (dataum > datadois)
                return -1;
            else if (datadois> dataum)
                return 1;  
            else
                return 0;
        }

    }
}
