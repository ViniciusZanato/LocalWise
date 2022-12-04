using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace LocalWise.Models
{
    public class Funcoes
    {
        /// <summary>
        /// Gera Hash SHA1, SHA256, SHA512
        /// </summary>
        /// <param name="texto"></param>
        /// <param name="nomeHash"></param>
        /// <returns></returns>
        public static string HashTexto(string texto, string nomeHash)
        {
            HashAlgorithm algoritmo = HashAlgorithm.Create(nomeHash);
            if (algoritmo == null)
            {
                throw new ArgumentException("Nome de hash incorreto", "nomeHash");
            }
            byte[] hash = algoritmo.ComputeHash(Encoding.UTF8.GetBytes(texto));
            return Convert.ToBase64String(hash);
        }

        public class MinimumAgeAttribute : ValidationAttribute
        {
            int _minimumAge;

            public MinimumAgeAttribute(int minimumAge)
            {
                _minimumAge = minimumAge;
            }

            public override bool IsValid(object value)
            {
                DateTime date;
                if (DateTime.TryParse(value.ToString(), out date))
                {
                    return date.AddYears(_minimumAge) < DateTime.Now;
                }

                return false;
            }
        }

        public static bool CriarDiretorio()
        {
            string dir = HttpContext.Current.Request.PhysicalApplicationPath + "Upload\\";
            if (!Directory.Exists(dir))
            {
                //Caso não exista devermos criar, sempre haverá um caminho físico
                //http.... linhas acima ... pega o caminho fisico da aplicacao, ja pega onde esta salvo no meu computador
                //alem dela outra pasta chamada uploads... \unica barra-querendo que a proxima caracter seja impresso por isso usamos \\
                //verifico se o diretorio existe
                Directory.CreateDirectory(dir);
                return true;
            }
            else
                return false;
        }

        //mesma coisa do criar diretorio mas também passo o caminho fisico do arquivo, o caminho completo - a pasta completa mais o nome do arquivo
        public static bool ExcluirArquivo(string arq)
        {
            if (File.Exists(arq))
            {
                File.Delete(arq);
                return true;
            }
            else
                return false;
        }

        public static string UploadArquivo(HttpPostedFileBase flpUpload, string nome)
        {
            try
            {
                double permitido = 900;//delimitação de tamanho em kbytes - double porque trabalha com números longos, tamnho do mega quebrado
                if (flpUpload != null) //se o usuario submeteu um arquivo
                {
                    string arq = Path.GetFileName(flpUpload.FileName);//pegando o nome do rquivo
                    double tamanho = Convert.ToDouble(flpUpload.ContentLength) / 1024; //tamanho divido 1024 para chegar em kbytes 
                    string extensao = Path.GetExtension(flpUpload.FileName).ToLower();
                    string diretorio = HttpContext.Current.Request.PhysicalApplicationPath + "Upload\\" + nome; //diretorio que vou salvar, se o diretorio nao existe devera ser criado primeiro 
                    if (tamanho > permitido) //verifico se o tamanho e maior que o permitido
                        return "Tamanho Máximo permitido é de " + permitido + " kb!";
                    else if ((extensao != ".png" && extensao != ".jpg")) //se a extensao e diferente de png retorno erro 
                        return "Extensão inválida, só são permitidas .png e .jpg!";
                    else
                    {
                        flpUpload.SaveAs(diretorio); //neste momento ele salva o arquivo na minha aplicação
                        return "sucesso";
                    }
                }
                else
                    return "Erro no Upload!";
            }
            catch { return "Erro no Upload"; }
        }

        public static string EnviarEmail(string emailDestinatario, string assunto, string corpomsg)
        {
            try
            {
                //Cria o endereço de email do remetente
                MailAddress de = new MailAddress("Fatec ADS <fatecgtaads@gmail.com>");
                //Cria o endereço de email do destinatário -->
                MailAddress para = new MailAddress(emailDestinatario);
                MailMessage mensagem = new MailMessage(de, para);
                mensagem.IsBodyHtml = true;
                //Assunto do email
                mensagem.Subject = assunto;
                //Conteúdo do email
                mensagem.Body = corpomsg;
                //Prioridade E-mail
                mensagem.Priority = MailPriority.Normal;
                //Cria o objeto que envia o e-mail
                SmtpClient cliente = new SmtpClient();
                //Envia o email
                cliente.Send(mensagem);
                return "success|E-mail enviado com sucesso";
            }
            catch { return "error|Erro ao enviar e-mail"; }
        }

        public static string Codifica(string texto)
        {
            byte[] stringBase64 = new byte[texto.Length];
            stringBase64 = Encoding.UTF8.GetBytes(texto);
            string codifica = Convert.ToBase64String(stringBase64);
            return codifica;
        }

        public static string Decodifica(string texto)
        {
            var encode = new UTF8Encoding();
            var utf8Decode = encode.GetDecoder();
            byte[] stringValor = Convert.FromBase64String(texto);
            int contador = utf8Decode.GetCharCount(stringValor, 0, stringValor.Length);
            char[] decodeChar = new char[contador];
            utf8Decode.GetChars(stringValor, 0, stringValor.Length, decodeChar, 0);
            string resultado = new String(decodeChar);
            return resultado;
        }
    }
}