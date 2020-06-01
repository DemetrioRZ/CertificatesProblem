namespace CertificatesProblem.Interfaces.Mappers
{
    public interface IMapper<in TIn, out TOut>
    {
        TOut Map(TIn source);
    }
}