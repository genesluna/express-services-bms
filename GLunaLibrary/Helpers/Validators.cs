using System;
using System.Text.RegularExpressions;

namespace GLunaLibrary.Helpers
{
    public static class Validators
    {
        public static bool CPFCNPJ(string cpfcnpj, bool empty)
        {
            string OnlyNumber = Regex.Replace(cpfcnpj, "[^0-9]", string.Empty);

            if (string.IsNullOrEmpty(OnlyNumber))
                return empty;
            else
            {
                int[] d = new int[14];
                int[] v = new int[2];
                int j, i, sum;
                string Sequency;

                //verificando se todos os numeros são iguais
                if (new string(OnlyNumber[0], OnlyNumber.Length) == OnlyNumber) return false;

                // se a quantidade de dígitos numérios for igual a 11
                // iremos verificar como CPF
                if (OnlyNumber.Length == 11)
                {
                    for (i = 0; i <= 10; i++) d[i] = Convert.ToInt32(OnlyNumber.Substring(i, 1));
                    for (i = 0; i <= 1; i++)
                    {
                        sum = 0;
                        for (j = 0; j <= 8 + i; j++) sum += d[j] * (10 + i - j);

                        v[i] = (sum * 10) % 11;
                        if (v[i] == 10) v[i] = 0;
                    }
                    return (v[0] == d[9] & v[1] == d[10]);
                }
                // se a quantidade de dígitos numérios for igual a 14
                // iremos verificar como CNPJ
                else if (OnlyNumber.Length == 14)
                {
                    Sequency = "6543298765432";
                    for (i = 0; i <= 13; i++) d[i] = Convert.ToInt32(OnlyNumber.Substring(i, 1));
                    for (i = 0; i <= 1; i++)
                    {
                        sum = 0;
                        for (j = 0; j <= 11 + i; j++)
                            sum += d[j] * Convert.ToInt32(Sequency.Substring(j + 1 - i, 1));

                        v[i] = (sum * 10) % 11;
                        if (v[i] == 10) v[i] = 0;
                    }
                    return (v[0] == d[12] & v[1] == d[13]);
                }
                // CPF ou CNPJ inválido se
                // a quantidade de dígitos numérios for diferente de 11 e 14
                else return false;
            }
        }

        public static bool Email(string email)
        {
            return Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
        }
    }
}