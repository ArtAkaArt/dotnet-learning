﻿namespace Sol5_1_Collections
{
    // не уверен, что его надо внутри класса объявлять
    /// <summary>
    /// Набор статусов отвечающий за поведение MyAsyncEnumerable при возникновении ошибок в процессе получения результатов, переданных в него функций.
    /// В режиме IgnoreErrors при перечислении енумератора все поучаемые ошибки будут проигнорированы без прекращения процесса перечисления получаемых данных.
    /// В режиме ReturnAllErrors все получаемые ошибки будут собраны в AggregateException, который будет выброшен в конце перечисления всех полученныех данных.
    /// В режиме EndAtFirstError перечисление остановится, при получении первой ошибки. И она будет проброшена в вызывающий метод.
    /// </summary>
    public enum ErrorsHandleMode
    {
        IgnoreErrors,
        ReturnAllErrors,
        EndAtFirstError
    }
}