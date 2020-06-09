namespace CertificatesProblem.Dtos.Requests
{
    /// <summary>
    /// Справка которую необходимо получить.
    /// </summary>
    public class Certificate
    {
        /// <summary>
        /// Идентификатор справки, например "c0"
        /// </summary>
        public string CertificateId { get; set; }
    }
}