namespace AutenticacaoAutorizacao.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string NomeUsuario { get; set; }
        public string Senha { get; set; }
        public string Funcao { get; set; }
    }
}
