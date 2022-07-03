using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sol5_1_Collections;
using System.Threading;

namespace MyEnumTesting
{
    public class ListMakers
    {
        public static List<Func<CancellationToken, Task<Post>>> MakeList1()
        {
            List<Func<CancellationToken, Task<Post>>> funcs = new();

            for (int i = 1; i <= 100; i++)
            {
                var count = i;
                funcs.Add(async (CancellationToken token) => await Methods.GetPostAsync1(count, token));
            }
            return funcs;
        }
        public static List<Func<CancellationToken, Task<Post>>> MakeList2()
        {
            List<Func<CancellationToken, Task<Post>>> funcs = new();

            for (int i = 1; i <= 100; i++)
            {
                var count = i;
                funcs.Add(async (CancellationToken token) => await Methods.GetPostAsync2(count, token));
            }
            return funcs;
        }
        public static List<Func<CancellationToken, Task<Post>>> MakeList3()
        {
            List<Func<CancellationToken, Task<Post>>> funcs = new();

            for (int i = 1; i <= 100; i++)
            {
                var count = i;
                funcs.Add(async (CancellationToken token) => await Methods.GetPostAsync3(count, token));
            }
            return funcs;
        }
        public static List<Func<CancellationToken, Task<Post>>> MakeList4()
        {
            List<Func<CancellationToken, Task<Post>>> funcs = new();

            for (int i = 1; i <= 100; i++)
            {
                var count = i;
                funcs.Add(async (CancellationToken token) => await Methods.GetPostAsync4(count, token));
            }
            return funcs;
        }
        public static List<Func<CancellationToken, Task<Post>>> MakeList5()
        {
            List<Func<CancellationToken, Task<Post>>> funcs = new();
            funcs.Add(async (CancellationToken token) => await Methods.GetPostAsync1(1, token));
            return funcs;
        }
        public static List<Func<CancellationToken, Task<Post>>> MakeList6()
        {
            List<Func<CancellationToken, Task<Post>>> funcs = new();
            funcs.Add(async (CancellationToken token) => await Methods.GetPostAsync2(1, token));
            return funcs;
        }
        public static List<Func<CancellationToken, Task<Post>>> MakeList7()
        {
            List<Func<CancellationToken, Task<Post>>> funcs = new();
            funcs.Add(async (CancellationToken token) => await Methods.GetPostAsync3(1, token));
            return funcs;
        }
        public static List<Func<CancellationToken, Task<Post>>> MakeList8()
        {
            List<Func<CancellationToken, Task<Post>>> funcs = new();
            funcs.Add(async (CancellationToken token) => await Methods.GetPostAsync4(1, token));
            return funcs;
        }
    }
}
