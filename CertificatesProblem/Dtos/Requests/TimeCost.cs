namespace CertificatesProblem.Dtos.Requests
{
    /// <summary>
    /// Затраты времени на выдачу справки.
    /// </summary>
    public class TimeCost
    {
        /// <summary>
        /// Дни.
        /// </summary>
        public int Days { get; set; }

        /// <summary>
        /// Часы.
        /// </summary>
        public int Hours { get; set; }

        /// <summary>
        /// Минуты.
        /// </summary>
        public int Minutes { get; set; }
    }
}