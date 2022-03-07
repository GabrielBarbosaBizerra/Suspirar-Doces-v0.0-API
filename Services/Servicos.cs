using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BackEnd.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BackEnd.Services
{
    public class Servicos
    {

        public Servicos()
        {
        }

    

        public static string EncriptarSenhas(string senha)
        {
            HashAlgorithm sha = new SHA1CryptoServiceProvider();
            byte[] senhaEncriptada = sha.ComputeHash(Encoding.UTF8.GetBytes(senha));
            StringBuilder stringBuilder = new StringBuilder();
            foreach(var caractere in senhaEncriptada)
            {
                stringBuilder.Append(caractere.ToString("X2"));
            }
            return stringBuilder.ToString();
        }


    }
}