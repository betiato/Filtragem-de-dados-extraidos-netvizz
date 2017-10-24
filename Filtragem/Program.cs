using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoLuiz
{
    class Program
    {

        private static List<Comentario> ListaOriginal = new List<Comentario>();

        private static List<Comentario> ListaComentarios = new List<Comentario>();

        private static List<string> ListaMisoginia = new List<string>();
        private static List<string> ListaPoliticoPartidario = new List<string>();
        private static List<string> ListaHomofobia = new List<string>();
        private static List<string> ListaRacismo = new List<string>();
        private static List<string> ListaClasseSocial= new List<string>();
        private static List<string> ListaXenofobia = new List<string>();
        private static List<string> ListaDeficienciaFisica = new List<string>();
        private static List<string> ListaIdadeGeracao = new List<string>();
        private static List<string> ListaReligiosa = new List<string>();
        private static List<string> ListaAparencia = new List<string>();

        private static int index = 0;

        static void Main(string[] args)
        {
            InicializaListasFiltro();

            System.IO.Directory.CreateDirectory("./Resultados");

            try
            {
                foreach (string file in Directory.EnumerateFiles(@"./Dados", "*.tab"))
                {
                    var nameFile = file.Replace("./Dados\\", "").Replace(".tab", "");

                    //

                    //

                    Console.WriteLine("Filtrando arquivo: " + nameFile);

                    ListaOriginal = new List<Comentario>();
                    ListaComentarios = new List<Comentario>();

                    var encodingEntrada = GetEncoding(nameFile);

                    if (encodingEntrada == Encoding.ASCII)
                    {
                      //ConvertFileEncoding(@"./Dados/" + nameFile + ".csv",@"./Dados", encodingEntrada, Encoding.UTF8);
                    }

                    using (StreamReader sr = new StreamReader(file))
                    {
                        var initial = sr.ReadLine();


                        while (!sr.EndOfStream)
                        {
                            //var texto = sr.ReadLine();
                            //var aaa =  ding.Unicode.GetBytes(texto);

                            var split = sr.ReadLine().Split('\t');

                            if (split.Length > 2)
                            {
                                if (!string.IsNullOrEmpty(split.ToString()))
                                {
                                    try
                                    {
                                        var comentario = new Comentario(split[3], Convert.ToBoolean(Convert.ToInt16(split[7])), split[8]);
                                        ListaOriginal.Add(comentario);
                                        //Console.WriteLine("Processando linha " + (index + 1));
                                        index++;
                                    }
                                    catch (Exception e)
                                    {

                                    }


                                }
                            }
                        }

                        //Preenche coluna TipoIntolerancia
                        FiltraLista();

                        //Preenche coluna TipoIntolerancia2
                        FiltraListaSecundario();

                        VerificaCaixaAlta();

                        VerificaEmojis();

                        VerificaPontuacaoExcessiva();

                        var teste = ListaComentarios.Where(x => !string.IsNullOrEmpty(x.TipoIntolerancia2)).ToList();

                        //Imprime arquivo de resultado
                        using (System.IO.StreamWriter fileOutput = new System.IO.StreamWriter(File.Open("./Resultados/Resultado " + nameFile + ".tab", FileMode.Create)))
                        {
                            fileOutput.WriteLine("CODIFICADOR\tFAN PAGE\tPOST\tCOMENTÁRIO\tRESP_COMENT\tCOMP_ENQ\tTIPO\tTIPO2\tFORMA_DISC\tTRANSMISSÃO\tFORMA_INT\tUSUÁRIO\tCAIXA ALTA\tEMOJI\tPONT_EXC");

                            foreach (var comentario in ListaComentarios)
                            {
                                fileOutput.WriteLine("\t" + "\t" + comentario.TextoPost + "\t" + comentario.TextoComentario + "\t" + Convert.ToInt16(comentario.EhResposta) + "\t" + "\t" + comentario.TipoIntolerancia + "\t" + comentario.TipoIntolerancia2 + "\t" + "\t" + "\t" + "\t" + "\t" + Convert.ToInt16(comentario.CaixaAlta) + "\t" + Convert.ToInt16(comentario.ContemEmoji) + "\t" + Convert.ToInt16(comentario.PontuacaoExcessiva));
                            }
                        }
                    }

                    index = 0;

                }

                Console.WriteLine("Todos arquivos foram filtrados com sucesso!");

            }
            catch (FileNotFoundException er)
            {
                Console.WriteLine(er.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + " - Erro na linha " + (index + 2).ToString());
            }


            Console.Read();

            //try
            //{   // Open the text file using a stream reader.
            //    using (StreamReader sr = new StreamReader(arquivoEntrada + ".csv"))
            //    {
            //        var initial = sr.ReadLine();

            //        while (!sr.EndOfStream)
            //        {
            //            var split = sr.ReadLine().Split(';');

            //            if(split.Length > 2)
            //            {
            //                if (!string.IsNullOrEmpty(split.ToString()))
            //                {
            //                    var comentario = new Comentario(split[0], Convert.ToBoolean(Convert.ToInt16(split[1])), split[2]);
            //                    ListaOriginal.Add(comentario);
            //                    Console.WriteLine("Processando linha " + (index + 1));
            //                    index++;
            //                }
            //            }
            //        }

            //        FiltraLista();

            //        VerificaCaixaAlta();

            //        VerificaEmojis();

            //        VerificaPontuacaoExcessiva();

            //        using (System.IO.StreamWriter file = new System.IO.StreamWriter(File.Open("./Resultado.csv", FileMode.Create), Encoding.UTF8))
            //        {
            //            file.WriteLine("CODIFICADOR;FAN PAGE;POST;COMENTÁRIO;RESP_COMENT;COMP_ENQ;TIPO;TIPO2;FORMA_DISC;TRANSMISSÃO;FORMA_INT;USUÁRIO;CAIXA ALTA;EMOJI;PONT_EXC");

            //            foreach (var comentario in ListaComentarios)
            //            {
            //                file.WriteLine(";" + ";" + comentario.TextoPost + ";" + comentario.TextoComentario + ";" + Convert.ToInt16(comentario.EhResposta) + ";" + ";" + comentario.TipoIntolerancia + ";" + ";" + ";" + ";" + ";" + ";" + Convert.ToInt16(comentario.CaixaAlta) + ";" + Convert.ToInt16(comentario.ContemEmoji) + ";" + Convert.ToInt16(comentario.PontuacaoExcessiva));
            //            }
            //        }

            //    }

            //    Console.WriteLine("Acabou");

            //}
            //catch (FileNotFoundException er)
            //{
            //    Console.WriteLine(er.Message);
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message + " - Erro na linha " + (index+2).ToString());
            //}


            //Console.Read();
        }

        /*private static void FiltraLista()
        {

            foreach (var comentario in ListaOriginal)
            {

                if (VerificaOcorrencia(comentario.TextoComentario, TiposPalavrasChave.PoliticoPartidario))
                {
                    comentario.TipoIntolerancia = "Político/partidário";
                    ListaComentarios.Add(comentario);
                }


                if (VerificaOcorrencia(comentario.TextoComentario, TiposPalavrasChave.Misoginia))
                {
                    comentario.TipoIntolerancia = "Misoginia";
                    ListaComentarios.Add(comentario);
                }


                if (VerificaOcorrencia(comentario.TextoComentario, TiposPalavrasChave.Homofobia))
                {
                    comentario.TipoIntolerancia = "Homofobia";
                    ListaComentarios.Add(comentario);
                }


                if (VerificaOcorrencia(comentario.TextoComentario, TiposPalavrasChave.Racismo))
                {
                    comentario.TipoIntolerancia = "Racismo";
                    ListaComentarios.Add(comentario);
                }


                if (VerificaOcorrencia(comentario.TextoComentario, TiposPalavrasChave.ClasseSocial))
                {
                    comentario.TipoIntolerancia = "Classe social";
                    ListaComentarios.Add(comentario);
                }


                if (VerificaOcorrencia(comentario.TextoComentario, TiposPalavrasChave.Xenofobia))
                {
                    comentario.TipoIntolerancia = "Xenofobia";
                    ListaComentarios.Add(comentario);
                }


                if (VerificaOcorrencia(comentario.TextoComentario, TiposPalavrasChave.DeficienciaFisica))
                {
                    comentario.TipoIntolerancia = "Deficiência física";
                    ListaComentarios.Add(comentario);
                }



                if (VerificaOcorrencia(comentario.TextoComentario, TiposPalavrasChave.IdadeGeracao))
                {
                    comentario.TipoIntolerancia = "Idade/Geração";
                    ListaComentarios.Add(comentario);
                }


                if (VerificaOcorrencia(comentario.TextoComentario, TiposPalavrasChave.Religiosa))
                {
                    comentario.TipoIntolerancia = "Religiosa";
                    ListaComentarios.Add(comentario);
                }


                if (VerificaOcorrencia(comentario.TextoComentario, TiposPalavrasChave.Aparencia))
                {
                    comentario.TipoIntolerancia = "Aparência";
                    ListaComentarios.Add(comentario);
                }


            }

        }*/

        private static void FiltraLista()
        {

            foreach (var comentario in ListaOriginal)
            {

                foreach (var odio in ListaPoliticoPartidario)
                {
                    if (comentario.TextoComentario.Contains(odio))
                    {
                        comentario.TipoIntolerancia = "Político/partidário";
                        ListaComentarios.Add(comentario);
                        break;
                    }

                }

                foreach (var misoginia in ListaMisoginia)
                {
                    if (comentario.TextoComentario.Contains(misoginia))
                    {
                        comentario.TipoIntolerancia = "Misoginia";
                        ListaComentarios.Add(comentario);
                        break;
                    }
                }

                foreach (var misoginia in ListaHomofobia)
                {
                    if (comentario.TextoComentario.Contains(misoginia))
                    {
                        comentario.TipoIntolerancia = "Homofobia";
                        ListaComentarios.Add(comentario);
                        break;
                    }
                }

                foreach (var misoginia in ListaRacismo)
                {
                    if (comentario.TextoComentario.Contains(misoginia))
                    {
                        comentario.TipoIntolerancia = "Racismo";
                        ListaComentarios.Add(comentario);
                        break;
                    }
                }

                foreach (var misoginia in ListaClasseSocial)
                {
                    if (comentario.TextoComentario.Contains(misoginia))
                    {
                        comentario.TipoIntolerancia = "Classe social";
                        ListaComentarios.Add(comentario);
                        break;
                    }
                }

                foreach (var misoginia in ListaXenofobia)
                {
                    if (comentario.TextoComentario.Contains(misoginia))
                    {
                        comentario.TipoIntolerancia = "Xenofobia";
                        ListaComentarios.Add(comentario);
                        break;
                    }
                }

                foreach (var misoginia in ListaDeficienciaFisica)
                {
                    if (comentario.TextoComentario.Contains(misoginia))
                    {
                        comentario.TipoIntolerancia = "Deficiência física";
                        ListaComentarios.Add(comentario);
                        break;
                    }
                }

                foreach (var misoginia in ListaIdadeGeracao)
                {
                    if (comentario.TextoComentario.Contains(misoginia))
                    {
                        comentario.TipoIntolerancia = "Idade/Geração";
                        ListaComentarios.Add(comentario);
                        break;
                    }
                }

                foreach (var misoginia in ListaReligiosa)
                {
                    if (comentario.TextoComentario.Contains(misoginia))
                    {
                        comentario.TipoIntolerancia = "Religiosa";
                        ListaComentarios.Add(comentario);
                        break;
                    }
                }

                foreach (var misoginia in ListaAparencia)
                {
                    if (comentario.TextoComentario.Contains(misoginia))
                    {
                        comentario.TipoIntolerancia = "Aparência";
                        ListaComentarios.Add(comentario);
                        break;
                    }
                }


            }

        }

        private static void FiltraListaSecundario()
        {

            foreach (var comentario in ListaComentarios)
            {

                if(comentario.TipoIntolerancia != "Político/partidário")
                {
                    foreach (var odio in ListaPoliticoPartidario)
                    {
                        if (comentario.TextoComentario.Contains(odio))
                        {
                            if(!comentario.TipoIntolerancia2.Contains("Político/partidário"))
                                comentario.TipoIntolerancia2 += comentario.TipoIntolerancia2.Contains(",") ? ",Político/partidário" : "Político/partidário";
                        }
                            
                    }
                }

                if (comentario.TipoIntolerancia != "Misoginia")
                {
                    foreach (var misoginia in ListaMisoginia)
                    {
                        if (comentario.TextoComentario.Contains(misoginia))
                        {
                            if (!comentario.TipoIntolerancia2.Contains("Misoginia"))
                                comentario.TipoIntolerancia2 += comentario.TipoIntolerancia2.Contains(",") ? ",Misoginia" : "Misoginia";
                        }
                    }
                }

                if (comentario.TipoIntolerancia != "Homofobia")
                {
                    foreach (var misoginia in ListaHomofobia)
                    {
                        if (comentario.TextoComentario.Contains(misoginia))
                        {
                            if (!comentario.TipoIntolerancia2.Contains("Homofobia"))
                                comentario.TipoIntolerancia2 += comentario.TipoIntolerancia2.Contains(",") ? ",Homofobia" : "Homofobia";
                        }
                    }
                }

                if (comentario.TipoIntolerancia != "Racismo")
                {
                    foreach (var misoginia in ListaRacismo)
                    {
                        if (comentario.TextoComentario.Contains(misoginia))
                        {
                            if (!comentario.TipoIntolerancia2.Contains("Racismo"))
                                comentario.TipoIntolerancia2 += comentario.TipoIntolerancia2.Contains(",") ? ",Racismo" : "Racismo";
                        }
                    }
                }

                if (comentario.TipoIntolerancia != "Classe social")
                {
                    foreach (var misoginia in ListaClasseSocial)
                    {
                        if (comentario.TextoComentario.Contains(misoginia))
                        {
                            if (!comentario.TipoIntolerancia2.Contains("Classe social"))
                                comentario.TipoIntolerancia2 += comentario.TipoIntolerancia2.Contains(",") ? ",Classe social" : "Classe social";
                        }
                    }
                }

                if (comentario.TipoIntolerancia != "Xenofobia")
                {
                    foreach (var misoginia in ListaXenofobia)
                    {
                        if (comentario.TextoComentario.Contains(misoginia))
                        {
                            if (!comentario.TipoIntolerancia2.Contains("Xenofobia"))
                                comentario.TipoIntolerancia2 += comentario.TipoIntolerancia2.Contains(",") ? ",Xenofobia" : "Xenofobia";
                        }
                    }
                }

                if (comentario.TipoIntolerancia != "Deficiência física")
                {
                    foreach (var misoginia in ListaDeficienciaFisica)
                    {
                        if (comentario.TextoComentario.Contains(misoginia))
                        {
                            if (!comentario.TipoIntolerancia2.Contains("Deficiência física"))
                                comentario.TipoIntolerancia2 += comentario.TipoIntolerancia2.Contains(",") ? ",Deficiência física" : "Deficiência física";
                        }
                    }
                }

                if (comentario.TipoIntolerancia != "Idade/Geração")
                {
                    foreach (var misoginia in ListaIdadeGeracao)
                    {
                        if (comentario.TextoComentario.Contains(misoginia))
                        {
                            if (!comentario.TipoIntolerancia2.Contains("Idade/Geração"))
                                comentario.TipoIntolerancia2 += comentario.TipoIntolerancia2.Contains(",") ? ",Idade/Geração" : "Idade/Geração";
                        }
                    }
                }

                if (comentario.TipoIntolerancia != "Religiosa")
                {
                    foreach (var misoginia in ListaReligiosa)
                    {
                        if (comentario.TextoComentario.Contains(misoginia))
                        {
                            if (!comentario.TipoIntolerancia2.Contains("Religiosa"))
                                comentario.TipoIntolerancia2 += comentario.TipoIntolerancia2.Contains(",") ? ",Religiosa" : "Religiosa";
                        }
                    }
                }

                if (comentario.TipoIntolerancia != "Aparência")
                {
                    foreach (var misoginia in ListaAparencia)
                    {
                        if (comentario.TextoComentario.Contains(misoginia))
                        {
                            if (!comentario.TipoIntolerancia2.Contains("Aparência"))
                                comentario.TipoIntolerancia2 += comentario.TipoIntolerancia2.Contains(",") ? ",Aparência" : "Aparência";
                        }
                    }
                }
                    


            }

        }

        private static void VerificaPontuacaoExcessiva()
        {
            foreach (var comentario in ListaComentarios)
            {
                if (comentario.TextoComentario.Contains("!!!")||
                    comentario.TextoComentario.Contains("???")||
                    comentario.TextoComentario.Contains("@@@") ||
                    comentario.TextoComentario.Contains("....") ||
                    comentario.TextoComentario.Contains(",,,") ||
                    comentario.TextoComentario.Contains("***") 
                    )
                    comentario.PontuacaoExcessiva = true;
            }

        }

        private static void VerificaEmojis()
        {

            var listaEmojis = "😀	😃	😊	☺️	😉	😍	😘	😚	😜	😝	😳	😬	😔	😌	😒	😞	😣	😢	😂	😭	😪	😥	😰	😓	😔	😣	😨	😱	😠	😡	😁	😖	😌	😷	😣	😵	😈	😏	👲	👳	👮	👷	💂	👶	👦	👧	👨	👩	👴	👵	👰	👼	👸	🐶	🐺	🐱	🐭	🐹	🐰	🐸	🐯	🐨	🐻	🐷	🐮	🐗	🐵	🐒	🐴	🐑	🐘	🐧	🐦	🐤	🐔	🐍	🐲	🐙	🐚	🐠	🐟	🐬	🐳	🐎	🐡	🐫	🐩	👣	💛	💙	💜	💚	❤️	💔	💗	💓	💖	💞	💘	💌	💋	💍	💎	🎩	👑	👒	👟	👡	👠	👢	👕	👔	👗	👘	👙	🌂	💄	💼	👜	🎀	🎍	💝	🎎	🎒	🎓	🎏	🎐	🎃	👻	🎅	🎄	🎁	🎉	🎈	🎌	🎥	📷	📼	💿	📀	💽	💾	🖥	📱	☎️	📞	📠	📺	📻	🔔	📣	📢	🔓	🔒	🔏	🔐	🔑	🔎	💡	🔍	🛁	🚽	🔨	🚬	💣	🔫	💊	💉	💰	💴	💵	📲	☕	🍵	🍶	🍺	🍻	🍸	🍴	🍔	🍟	🍝	🍛	🍱	🍣	🍙	🍘	🍚	🍜	🍲	🍢	🍡	🍳	🍞	🍦	🍧	🎂	🍎	🍊	🍉	🍓	🍆	🍅	⛵	🚣	🚀	✈️	💺	🚉	🚄	🚅	🚃	🚌	🚗	🚘	🚕	🚚	🚓	🚒	🚑	🚲	💈	🚏	🎫	🚥	⚠️	🚧	🔰	⛽	♨	🎭	🏠	🏡	🏢	🏦	🏤	🏥	🏛	🏪	🏩	🏨	💒	⛪	🏬	🌇	🌆	🏯	🏰	⛺	🏭	🗼	🗻	🌄	🌅	🌃	🗽	🎡	⛲	🎢	🚢	💐	🌸	🌷	🍀	🌹	🌻	🌺	🍁	🍃	🍂	🌾	🌵	🌴	🌱	🎨	🎬	🎤	🎧	🎶	🎵	🎶	🎺	🎷	🎻	🌙	⭐	☀️	☁️	⚡	☔	⛄	🌀	🌈	🌊	✉️	📩	📨	📫	📪	📬	📭	📮	📝	✂	📖	👾	🀄	🎯	🏈	🏀	⚽	⚾	🎾	🎱	⛳	🏁	🏆	🎿	⬆️	⬇️	⬅️	➡️	↗️	↖️	↘️	↙️	⤵️	⤴️	🆙	🆒	📶	🎞	🈁	🈯	🈳	🈲	㊙️	🈹	🈺	🈶	🈚	🚻	🚹	🚺	🚼	🚾	♿	🚭	🈷️	🈶	🈂️	㊙️	㊗️	🔞	⛔	*️⃣	❎	✳️	💟	🆚	🔕	📴	💠	➿	🏧	💲	❌	❗	❓	❕	❔	⭕	❌	♠️	♥️	♣️	♦️	〽️	🔱	◼	◻	◾	◽	▪	▫	🔲	🔳	⚫	⚪	🔴	🔵	⬜	⬛	🔶	🔷	🔸	🔹	👎	👌	💀	👽	💩	👍	🔥	✨	🌟	💢	💦	💧	💤	💨	👂	👀	👃	😝	👄	🙌	📡	👏	💪";

            var splitEmojis = listaEmojis.Split('	');

            foreach (var comentario in ListaComentarios)
            {

                for (int i = 0; i < splitEmojis.Length; i++)
                {
                    if (comentario.TextoComentario.Contains(splitEmojis.ElementAt(i)))
                    {
                        comentario.ContemEmoji = true;
                    }
                }
            }

            var asdas = ListaComentarios.Where(x => x.ContemEmoji == true).ToList();

        }

        private static void VerificaCaixaAlta()
        {

            foreach (var comentario in ListaComentarios)
            {

                for (int i = 0; i < comentario.TextoComentario.Length; i++)
                {
                    if (Char.IsUpper(comentario.TextoComentario[i]) && i < (comentario.TextoComentario.Length - 2))
                    {

                        if (Char.IsUpper(comentario.TextoComentario[i+1]))
                        {
                            if (Char.IsUpper(comentario.TextoComentario[i+2]))
                            {
                                comentario.CaixaAlta = true;
                            }
                        }
                    }
                }
            }

        }

        private static bool IsAllUpper(string input)
        {

            for (int i = 0; i < input.Length; i++)
            {
                if ((Char.IsDigit(input[i]) || (Char.IsLetter(input[i]) && !Char.IsUpper(input[i]))))
                    return false;
            }
            return true;
        }

        private static void InicializaListasFiltro()
        {
            try
            {

                using (StreamReader sr = new StreamReader("./Palavras chave/Aparencia.txt"))
                {
                    var text = sr.ReadToEnd();
                    var split = text.Split('/');
                    foreach (var item in split)
                    {
                        ListaAparencia.Add(item);
                    }
                }


                using (StreamReader sr = new StreamReader("./Palavras chave/Classe social.txt"))
                {
                    var text = sr.ReadToEnd();
                    var split = text.Split('/');
                    foreach (var item in split)
                    {
                        ListaClasseSocial.Add(item);
                    }
                }

                using (StreamReader sr = new StreamReader("./Palavras chave/Deficiencia fisica.txt"))
                {
                    var text = sr.ReadToEnd();
                    var split = text.Split('/');
                    foreach (var item in split)
                    {
                        ListaDeficienciaFisica.Add(item);
                    }
                }


                using (StreamReader sr = new StreamReader("./Palavras chave/Homofobia.txt"))
                {
                    var text = sr.ReadToEnd();
                    var split = text.Split('/');
                    foreach (var item in split)
                    {
                        ListaHomofobia.Add(item);
                    }
                }


                using (StreamReader sr = new StreamReader("./Palavras chave/Idade geracao.txt"))
                {
                    var text = sr.ReadToEnd();
                    var split = text.Split('/');
                    foreach (var item in split)
                    {
                        ListaIdadeGeracao.Add(item);
                    }
                }


                using (StreamReader sr = new StreamReader("./Palavras chave/Misoginia.txt"))
                {
                    var text = sr.ReadToEnd();
                    var split = text.Split('/');
                    foreach (var item in split)
                    {
                        ListaMisoginia.Add(item);
                    }
                }


                using (StreamReader sr = new StreamReader("./Palavras chave/Politico partidaria.txt"))
                {
                    var text = sr.ReadToEnd();
                    var split = text.Split('/');
                    foreach (var item in split)
                    {
                        ListaPoliticoPartidario.Add(item);
                    }
                }


                using (StreamReader sr = new StreamReader("./Palavras chave/Racismo.txt"))
                {
                    var text = sr.ReadToEnd();
                    var split = text.Split('/');
                    foreach (var item in split)
                    {
                        ListaRacismo.Add(item);
                    }
                }


                using (StreamReader sr = new StreamReader("./Palavras chave/Religiosa.txt"))
                {
                    var text = sr.ReadToEnd();
                    var split = text.Split('/');
                    foreach (var item in split)
                    {
                        ListaReligiosa.Add(item);
                    }
                }


                using (StreamReader sr = new StreamReader("./Palavras chave/Xenofobia.txt"))
                {
                    var text = sr.ReadToEnd();
                    var split = text.Split('/');
                    foreach (var item in split)
                    {
                        ListaXenofobia.Add(item);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }



            //ListaMisoginia.Add("vadia");
            //ListaMisoginia.Add("safada");
            //ListaMisoginia.Add("mal comid");
            //ListaMisoginia.Add("coisa de mulherzinha");
            //ListaMisoginia.Add("mulher da relação");
            //ListaMisoginia.Add("falta de rola");
            //ListaMisoginia.Add("feminismo começa");
            //ListaMisoginia.Add("falta de pica");
            //ListaMisoginia.Add("cara de puta");
            //ListaMisoginia.Add("odeio mulher");
            //ListaMisoginia.Add("feminazi");
            //ListaMisoginia.Add("tinha que ser mulher");
            //ListaMisoginia.Add("Dilmanta");


            //ListaOdioPartidario.Add("petralha safad");
            //ListaOdioPartidario.Add("coxinha burr");
            //ListaOdioPartidario.Add("comunista safad");
            //ListaOdioPartidario.Add("coxinha fascista");
            //ListaOdioPartidario.Add("comunista");
            //ListaOdioPartidario.Add("ladrão");
            //ListaOdioPartidario.Add("bolsa esmola");
            //ListaOdioPartidario.Add("ameaça");
            //ListaOdioPartidario.Add("petista");
            //ListaOdioPartidario.Add("bolsa");
            //ListaOdioPartidario.Add("compra voto");
            //ListaOdioPartidario.Add("petista vagabund");
            //ListaOdioPartidario.Add("gangue petista");
            //ListaOdioPartidario.Add("temer vampiro");
            //ListaOdioPartidario.Add("elite golpista");
            //ListaOdioPartidario.Add("esquerda caviar");
            //ListaOdioPartidario.Add("petralha");


        }

        public static Encoding GetEncoding(string filename)
        {
            // Read the BOM
            var bom = new byte[4];
            using (var file = new FileStream(@"./Dados/"+filename+".tab", FileMode.Open, FileAccess.Read))
            {
                file.Read(bom, 0, 4);
            }

            // Analyze the BOM
            if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76) return Encoding.UTF7;
            if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) return Encoding.UTF8;
            if (bom[0] == 0xff && bom[1] == 0xfe) return Encoding.Unicode; //UTF-16LE
            if (bom[0] == 0xfe && bom[1] == 0xff) return Encoding.BigEndianUnicode; //UTF-16BE
            if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) return Encoding.UTF32;
            return Encoding.ASCII;
        }

        public static void ConvertFileEncoding(String sourcePath, String destPath, Encoding sourceEncoding, Encoding destEncoding)
        {
            // If the destination’s parent doesn’t exist, create it.
            String parent = Path.GetDirectoryName(Path.GetFullPath(destPath));

            if (!Directory.Exists(parent))
            {
                Directory.CreateDirectory(parent);
            }

            // If the source and destination encodings are the same, just copy the file.
            if (sourceEncoding == destEncoding)
            {
                File.Copy(sourcePath, destPath, true);
                return;
            }

            // Convert the file.

            String tempName = null;

            try
            {
                tempName = Path.GetTempFileName();
                using (StreamReader sr = new StreamReader(sourcePath, sourceEncoding, false))
                {
                    using (StreamWriter sw = new StreamWriter(tempName, false, destEncoding))
                    {
                        int charsRead;

                        char[] buffer = new char[128 * 1024];

                        while ((charsRead = sr.ReadBlock(buffer, 0, buffer.Length)) > 0)
                        {
                            sw.Write(buffer, 0, charsRead);
                        }
                    }
                }

                File.Delete(sourcePath);
                File.Move(tempName, sourcePath);
            }
            catch (Exception e)
            {

            }
            finally
            {
                File.Delete(tempName);
            }
        }


        private static bool VerificaOcorrencia(string comentario, TiposPalavrasChave tipo)
        {
            var split = comentario.Split(' ');

            switch (tipo)
            {
                case TiposPalavrasChave.PoliticoPartidario:

                    foreach (var palavra in split)
                    {
                        if(ListaPoliticoPartidario.Any(x => x.Equals(palavra)))
                        {
                            return true;
                        }
                    }

                    break;


                case TiposPalavrasChave.Misoginia:

                    foreach (var palavra in split)
                    {
                        if (ListaMisoginia.Any(x => x.Equals(palavra)))
                        {
                            return true;
                        }
                    }

                    break;


                case TiposPalavrasChave.Homofobia:
                    foreach (var palavra in split)
                    {
                        if (ListaHomofobia.Any(x => x.Equals(palavra)))
                        {
                            return true;
                        }
                    }
                    break;


                case TiposPalavrasChave.Racismo:
                    foreach (var palavra in split)
                    {
                        if (ListaRacismo.Any(x => x.Equals(palavra)))
                        {
                            return true;
                        }
                    }
                    break;


                case TiposPalavrasChave.ClasseSocial:
                    foreach (var palavra in split)
                    {
                        if (ListaClasseSocial.Any(x => x.Equals(palavra)))
                        {
                            return true;
                        }
                    }
                    break;


                case TiposPalavrasChave.DeficienciaFisica:
                    foreach (var palavra in split)
                    {
                        if (ListaDeficienciaFisica.Any(x => x.Equals(palavra)))
                        {
                            return true;
                        }
                    }
                    break;


                case TiposPalavrasChave.IdadeGeracao:
                    foreach (var palavra in split)
                    {
                        if (ListaIdadeGeracao.Any(x => x.Equals(palavra)))
                        {
                            return true;
                        }
                    }
                    break;


                case TiposPalavrasChave.Religiosa:
                    foreach (var palavra in split)
                    {
                        if (ListaReligiosa.Any(x => x.Equals(palavra)))
                        {
                            return true;
                        }
                    }
                    break;


                case TiposPalavrasChave.Aparencia:
                    foreach (var palavra in split)
                    {
                        if (ListaAparencia.Any(x => x.Equals(palavra)))
                        {
                            return true;
                        }
                    }
                    break;

                case TiposPalavrasChave.Xenofobia:
                    foreach (var palavra in split)
                    {
                        if (ListaXenofobia.Any(x => x.Equals(palavra)))
                        {
                            return true;
                        }
                    }
                    break;


                default:
                    break;
            }    


            return false;

        }


    }



    enum TiposPalavrasChave
    {
        PoliticoPartidario,
        Misoginia,
        Homofobia,
        Racismo,
        ClasseSocial,
        DeficienciaFisica,
        IdadeGeracao,
        Religiosa,
        Aparencia,
        Xenofobia

    }

}
