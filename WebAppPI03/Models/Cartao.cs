using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAppPI03.br.com.webint.www;

namespace WebAppPI03.Models
{
    public class Cartao
    {
        public bool Verificar (string nome, string numCartao, string val, string codSeg)
        {
            GatewayPagamento gp = new GatewayPagamento();
                      
            return gp.Checkout(nome, numCartao, val, codSeg);
        }
    }
}