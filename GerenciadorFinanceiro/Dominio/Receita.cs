﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GerenciadorFinanceiro.Dominio
{
    class Receita
    {

        public Receita() { IdReceita = 0; AlunoReceita = new Aluno();
        ServicoReceita = new Servico(); Observacao = String.Empty; Frequencia = "Uma única vez"; QuantidadeParcela = 0;
        ValorTotalReceita = 0.0; ListaReceitaParcela = new List<ReceitaParcela>(); UltimoVencimento = DateTime.Now;
        }

        public int IdReceita { get; set; }

        public Aluno AlunoReceita { get; set; }

        public Servico ServicoReceita { get; set; }

        public String Observacao { get; set; }

        public String Frequencia { get; set; }

        public int QuantidadeParcela { get; set; }

        public DateTime UltimoVencimento { get; set; }

        public double ValorTotalReceita { get; set; }

        public List<ReceitaParcela> ListaReceitaParcela { get; set; }

        public DocumentoReceber Documento { get; set; }
         
    }
}
