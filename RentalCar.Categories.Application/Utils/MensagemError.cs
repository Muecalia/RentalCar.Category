namespace RentalCar.Categories.Application.Utils
{
    public class MensagemError
    {
        //NOTFOUND
        public static string NotFound(string entidade)
        {
            return $"{entidade} não encontrada";
        }
        public static string NotFound(string entidade, string key)
        {
            return $"{entidade} com a chave {key} não encontrada";
        }
        public static string NotFoundEmail(string entidade, string email)
        {
            return $"{entidade} com o e-mail {email} não encontrado";
        }
        //public static string NotFound(string entidade, string mensagem)
        //{
        //    return $"{entidade} não encontrada. Mensagem: {mensagem}";
        //}

        //LISTAR
        public static string CarregamentoSucesso(string entidade)
        {
            return $"Dados do(a) {entidade} carregado com sucesso";
        }
        public static string CarregamentoSucesso(string entidade, int size)
        {
            return $"Dados do(a) {entidade} carregados com sucesso. Total Itens: {size}";
        }
        public static string CarregamentoErro(string entidade)
        {
            return $"Erro ao carregar os dados do(a) {entidade}";
        }
        public static string CarregamentoErro(string entidade, string mensagem)
        {
            return $"Erro ao carregar os dados do(a) {entidade}. Mensagem: {mensagem}";
        }

        //OPERAÇÃO
        public static string OperacaoSucesso(string entidade, string operacao)
        {
            return $"Sucesso ao {operacao} o(a) {entidade}";
        }
        public static string OperacaoProcessamento(string entidade, string operacao)
        {
            return $"{operacao} do(a) {entidade} em processamento";
        }
        public static string OperacaoErro(string entidade, string operacao)
        {
            return $"Erro ao {operacao} o(a) {entidade}";
        }
        public static string OperacaoErro(string entidade, string operacao, string mensagem)
        {
            return $"Erro ao {operacao} o(a) {entidade}. Mensagem: {mensagem}";
        }

        //RABBITMQ
        public static string SendMessage(string entidade, string operacao)
        {
            return $"{operacao} do(a) {entidade} em processamento";
        }

        //CONFLICTO
        public static string Conflito(string entidade)
        {
            return $"Já existe um(a) {entidade} com este nome";
        }
        public static string ConflitoUso(string entidade)
        {
            return $"Não é possível eliminar, porque o(a) {entidade} já se encontra em uso";
        }
        public static string ConflitoEmail(string email)
        {
            return $"Erro! Email {email} já se encontra registado";
        }

    }
}
