﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GerenciadorFinanceiro.Dominio
{
    class Servico
    {
        public Servico()
        {
            Observacao = string.Empty;
        }
                
        public int IdServico { get; set; }

        public String Descricao { get; set; }

        public Double Valor { get; set; }

        public String Observacao { get; set; }

        public TipoServico Tipo { get; set; }

    }
}
