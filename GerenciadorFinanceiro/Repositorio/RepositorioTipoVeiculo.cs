﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GerenciadorFinanceiro.Repositorio
{
    class RepositorioTipoVeiculo: IRepositorio<Dominio.TipoVeiculo>
    {
        #region IRepositorio<TipoVeiculo> Members

        public void SalvarObjeto(GerenciadorFinanceiro.Dominio.TipoVeiculo objeto)
        {
            throw new NotImplementedException();
        }

        public void AtualizarObjeto(GerenciadorFinanceiro.Dominio.TipoVeiculo objeto)
        {
            throw new NotImplementedException();
        }

        public void DeletarObjeto(GerenciadorFinanceiro.Dominio.TipoVeiculo objeto)
        {
            throw new NotImplementedException();
        }

        public Dominio.TipoVeiculo BuscarObjetoPorId(int id)
        {
            throw new NotImplementedException();
        }

        public List<GerenciadorFinanceiro.Dominio.TipoVeiculo> BuscarTodos()
        {
            return new List<GerenciadorFinanceiro.Dominio.TipoVeiculo>();
        }

        #endregion
    }
}