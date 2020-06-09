namespace CertificatesProblem.Dtos.Requests
{
    /// <summary>
    /// Существующая на руках справка на момент запроса.
    /// </summary>
    public class ExistingCertificate
    {
        /// <summary>
        /// Идентификатор справки, например "c1"
        /// </summary>
        public string CertificateId { get; set; }
    }
}