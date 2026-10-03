using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ClienteEnvia
{
    class Program
    {
        static void Main(string[] args)
        {
            FileStream infile; //in=input, objeto da classe filestream que representa o arquivo
            String buffer = "";
            String filename = "testeXML.xml";

            int chave = 5;
            int fatorCripto = 18 + chave;

            infile = new System.IO.FileStream(filename,
                                   System.IO.FileMode.Open,
                                   System.IO.FileAccess.Read);

            for (int c = 0; c < infile.Length; ++c)
            {
                buffer += (char)infile.ReadByte(); // coloca conteúdo do arquivo em buffer
            }
            infile.Close();


            Console.WriteLine("Tamanho em bytes = " + buffer.Length);
            Console.WriteLine("Chars fragmentados de 10 em 10 bytes:");

            String bufferCriptografado = "";
            String buffer2 = "";

            for (int i = 0, j = 1; i < buffer.Length; ++i, ++j)
            {
                char caractere = buffer[i];

                // Criptografa o char
                char charCriptografado = (char)(caractere + fatorCripto);
                bufferCriptografado += charCriptografado;
                buffer2 += charCriptografado;

                if (j == 10)
                {
                    Console.WriteLine(buffer2);
                    j = 0;
                    buffer2 = "";
                }
            }


            Console.ReadKey();

            Socket socketenviar = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.IP); // conecta programa na rede
            IPEndPoint endereco = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9060); // endereço destinatário

            socketenviar.SendTo(Encoding.UTF8.GetBytes(filename), endereco); //nome, codifica string num array de bytes - string -> array de bytes
            socketenviar.SendTo(Encoding.UTF8.GetBytes(bufferCriptografado), endereco);   // conteúdo
            socketenviar.Close();
        }
    }
}
