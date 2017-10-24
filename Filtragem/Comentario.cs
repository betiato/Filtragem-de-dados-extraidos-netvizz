using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoLuiz
{
    public class Comentario
    {

        public Comentario(string textoPost, bool ehResposta, string textoComentario)
        {
            this.EhResposta = ehResposta;
            this.TextoComentario = textoComentario;
            this.TextoPost = textoPost;
            this.CaixaAlta = false;
            this.PontuacaoExcessiva = false;
            this.ContemEmoji = false;
            this.TipoIntolerancia = "";
            this.TipoIntolerancia2 = "";
        }

        public string TipoIntolerancia { get; set; }

        public string TipoIntolerancia2 { get; set; }

        public bool ContemEmoji { get; set; }

        public bool PontuacaoExcessiva { get; set; }

        public bool CaixaAlta { get; set; }

        public string TextoComentario { get; set; }

        public bool EhResposta { get; set; }

        public string TextoPost { get; set; }

    }
}
