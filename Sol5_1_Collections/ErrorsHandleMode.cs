﻿namespace Sol5_1_Collections
{
    /// <summary>
    /// Набор статусов отвечающий за поведение MyAsyncEnumerable при возникновении ошибок в процессе получения результатов,
    /// переданных в него функций.
    /// </summary>
    public enum ErrorsHandleMode
    {
        /// <summary>
        /// Режим в котором при перечислении енумератора все поучаемые ошибки будут проигнорированы
        /// без прекращения процесса перечисления получаемых данных.
        /// </summary>
        IgnoreErrors,
        /// <summary>
        /// Режим в котором все получаемые ошибки будут собраны в AggregateException, который 
        /// будет выброшен в конце перечисления всех полученныех данных.
        /// </summary>
        ReturnAllErrors,
        /// <summary>
        /// Режим в котором перечисление остановливается, при получении первой ошибки. И она будет проброшена в вызывающий метод.
        /// </summary>
        EndAtFirstError
    }
}
