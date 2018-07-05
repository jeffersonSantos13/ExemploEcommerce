using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace WebAppPI03.Models
{
    public class Validar
    {
        #region Referentes a validação dos dados inseridos
        public bool isCPFValido(string cpf)

        {

            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf, digito;
            int soma, resto;

            cpf = cpf.Trim();

            cpf = cpf.Replace(".", "").Replace("-", "");

            if (cpf.Length != 11)

                return false;

            tempCpf = cpf.Substring(0, 9);

            soma = 0;

            for (int i = 0; i < 9; i++)

                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            resto = soma % 11;

            if (resto < 2)

                resto = 0;

            else

                resto = 11 - resto;

            digito = resto.ToString();

            tempCpf = tempCpf + digito;

            soma = 0;

            for (int i = 0; i < 10; i++)

                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;

            if (resto < 2)

                resto = 0;

            else

                resto = 11 - resto;

            digito = digito + resto.ToString();

            return cpf.EndsWith(digito);

        }
        public bool isUniqueEmail(string email)
        {
            EntidadesEcommerce db = new EntidadesEcommerce();
            var existe = db.Cliente.Where(m => m.emailCliente == email).Count();
            if (existe > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public bool isUniqueCpF(string cpf)
        {
            EntidadesEcommerce db = new EntidadesEcommerce();
            var existe = db.Cliente.Where(m => m.CPFCliente == cpf).Count();
            if (existe == 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public bool ChecarAcesso(LoginViewModel login)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                EntidadesEcommerce db = new EntidadesEcommerce();
                sb.AppendFormat("SELECT emailCliente, senhaCliente, nomeCompletoCliente FROM Cliente WHERE emailCliente = '{0}' AND senhaCliente = '{1}'", login.Email, login.Senha);
                var query = db.Database.SqlQuery<LoginViewModel>(sb.ToString()).ToList();

                if (query.Count() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion
        #region Referentes a buscar dados do usuario
        [Authorize]
        public int getIDControl()
        {

            try
            {
                var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
                var claims = identity.Claims.ToList();
                return Convert.ToInt32(claims[3].Value);
            }
            catch
            {
                return 0;
            }
        }
        [Authorize]
        public static int getID()
        {

            try
            {
                var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
                var claims = identity.Claims.ToList();
                return Convert.ToInt32(claims[3].Value);
            }
            catch
            {
                return 0;
            }
        }
        [Authorize]
        public static string getEmail()
        {

            try
            {
                var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
                var claims = identity.Claims.ToList();
                return claims[0].Value;
            }
            catch
            {
                return string.Empty;
            }
        }
        [Authorize]
        public static string getName()
        {

            try
            {
                var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
                var claims = identity.Claims.ToList();
                return claims[2].Value;
            }
            catch
            {
                return string.Empty;
            }
        }
        #endregion
        #region Referentes ao crud do painel de usuario
        public void DeleteEnd(int id)
        {
            try
            {
                EntidadesEcommerce db = new EntidadesEcommerce();
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("DELETE Endereco WHERE idEndereco = '{0}'", id);
                db.Database.ExecuteSqlCommand(sb.ToString());
            }
            catch (Exception)
            {

                throw;
            }


        }
        #endregion




     
    }
}
