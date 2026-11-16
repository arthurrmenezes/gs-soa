namespace LuzVital.Pontos.Domain;

public class DomainException : Exception
{
    public DomainException(string mensagem) : base(mensagem) { }
}

public class NaoEncontradoException : DomainException
{
    public NaoEncontradoException(string mensagem) : base(mensagem) { }

    public static NaoEncontradoException Ponto(int id) => new($"Não existe ponto com o id {id}.");
}

public class ConflitoException : DomainException
{
    public ConflitoException(string mensagem) : base(mensagem) { }
}
