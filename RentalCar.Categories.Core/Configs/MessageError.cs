namespace RentalCar.Categories.Core.Configs;

public class MessageError
{
    //NOTFOUND
    public static string NotFound(string entidade)
    {
        return $"{entidade} não encontrado";
    }
    public static string NotFound(string entidade, string key)
    {
        return $"{entidade} com a chave {key} não encontrado";
    }

    //LISTAR
    public static string CarregamentoSucesso(string entidade)
    {
        return $"Dados do {entidade} carregado com sucesso";
    }
    public static string CarregamentoSucesso(string entidade, int size)
    {
        return $"Dados do {entidade} carregados com sucesso. Total Itens: {size}";
    }
    public static string CarregamentoErro(string entidade)
    {
        return $"Erro ao carregar os dados do {entidade}";
    }
    public static string CarregamentoErro(string entidade, string mensagem)
    {
        return $"Erro ao carregar os dados do {entidade}. Mensagem: {mensagem}";
    }

    //OPERAÇÃO
    public static string OperacaoSucesso(string entidade, string operacao)
    {
        return $"Sucesso ao {operacao} o {entidade}";
    }
    public static string OperacaoProcessamento(string entidade, string operacao)
    {
        return $"{operacao} do {entidade} em processamento";
    }
    public static string OperacaoErro(string entidade, string operacao)
    {
        return $"Erro ao {operacao} o {entidade}";
    }
    public static string OperacaoErro(string entidade, string operacao, string mensagem)
    {
        return $"Erro ao {operacao} o {entidade}. Mensagem: {mensagem}";
    }

    //CONFLICTO
    public static string Conflito(string entidade)
    {
        return $"Já existe um {entidade} com este nome";
    }
    public static string ConflitoUso(string entidade)
    {
        return $"Não é possível eliminar, porque a {entidade} já se encontra em uso";
    }
    
    // CONSUMIR MENSAGEM
    public static string ConsumirMensagemSucesso(string titulo)
    {
        return $"Suceso ao consumir a mensagem do {titulo}.";
    }
    public static string ConsumirMensagemErro(string titulo, string message)
    {
        return $"Erro ao consumir a mensagem do {titulo}. Mensagem: {message}";
    }

    // PUBLICAR MENSAGEM
    public static string PublicarMensagemSucesso(string titulo)
    {
        return $"Suceso ao publicar a mensagem do {titulo}.";
    }
    public static string PublicarMensagemErro(string titulo, string message)
    {
        return $"Erro ao publicar a mensagem do {titulo}. Mensagem: {message}";
    }

    // CONEXÃO
    public static string AbrirConexão(string titulo)
    {
        return $"Suceso ao abrir a conexão com o servidor rabbitmq";
    }
    public static string FecharConexão(string titulo)
    {
        return $"Sucesso ao fechar do(a) {titulo} a conexão e o canal com o servidor rabbitmq.";
    }
    public static string FecharConexãoErro(string message)
    {
        return $"Erro ao fechar a conexão e o canal com o servidor rabbitmq. Mensagem: {message}";
    }
    
}