using System;
using System.Collections.Generic;

namespace LabAB04
{
    class Program
    {

        public interface IBuilder
        {
            public void BuildStep1();
            public void BuildStep2();
        }

        public interface IHandler<T>
        {
            IHandler<T> SetNext(IHandler<T> handler);

            string Handle(T request, Func<T,object> selector);
        }


        public interface IValidate<T>
        {
            bool Validate(T request);
        }


        public class NameValidate : IValidate<string>
        {
            public bool Validate(string request)
            {
                
                if ((string) request == "Name")
                {
                    return true;
                    
                }
                else
                {
                    return false;
                    
                }
            }
        }

        public class CashValidate : IValidate<decimal>
        {
            public bool Validate(decimal request)
            {
                decimal cash = request;
                
                
                if (cash >= 1500.0M)
                {
                    return true;
                }
                else
                {

                    return false;
                }
            }
        }


        public struct MyStruct
        {
            public string name;
            public decimal cash;
        }

        //internal abstract class AbstractHandler : IHandler<MyStruct>
        //{
        //    public IHandler<MyStruct> _nextHandler;

            
        //    public IHandler<MyStruct> SetNext(IHandler<MyStruct> handler)
        //    {
        //        this._nextHandler = handler;

        //        return handler;
        //    }

            

        //    public  string Handle(MyStruct request, Func<MyStruct,object> selector)
        //    {
               
        //        if (this._nextHandler != null)
        //        {
        //            return this._nextHandler.Handle(request,selector);
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //}


        public delegate T MyDelegate<T,T1>(T t, T1 t1);

        //public class HandlersChain<T>
        //{
            
        //    private IHandler<T> _chain;

        //    public HandlersChain(IHandler<T> first)
        //    {
        //        _chain = first;
        //    }

        //    public void SetNext(IHandler<T> next)
        //    {
        //        _chain.SetNext(next);
        //    }

        //    public string Validate(T obj)
        //    {
        //        int i = 0;
        //        string res = string.Empty;
        //        var _c = _chain;
        //        while (_c != null)
        //        {
        //            res += _c.Handle(obj);
        //            i++;
        //        }

        //        return res;
        //    }
        //}



        public class AccountHandler : IHandler<MyStruct>
        {
            private IValidate<string> _validate;
            private IHandler<MyStruct> _nextHandler;
            public AccountHandler(IValidate<string> validate)
            {
                _validate = validate;
            }

            public IHandler<MyStruct> SetNext(IHandler<MyStruct> handl)
            {
                _nextHandler = handl;
                return handl;
            }
            public string Handle(MyStruct request, Func<MyStruct,object> f)
            {
                var req = f(request);

                if (req is string)
                {
                    if ((bool) _validate.Validate((string) req))
                    {
                        return "Correct name";
                    }
                    else
                    {
                        throw new Exception("Error name");
                    }
                }
                else
                {
                    return _nextHandler.Handle(request, f);
                    
                }


            }
        }


        public static object GetDec(MyStruct st)
        {
            return st.cash;
        }

        public static string GetStr(MyStruct st)
        {
            return st.name;
        }

        public class CashHandler : IHandler<MyStruct>
        {
            private IValidate<decimal> _validate;
            private IHandler<MyStruct> _nextHandler;

            public CashHandler(IValidate<decimal> validate)
            {
                _validate = validate;
            }

            public IHandler<MyStruct> SetNext(IHandler<MyStruct> handl)
            {
                _nextHandler = handl;
                return handl;
            }

            public  string Handle(MyStruct request, Func<MyStruct,object> f)
            {
                var req = f(request);
                if (req is decimal)
                {
                    if ((bool) _validate.Validate((decimal) req))
                    {
                        return "Correct cash count";
                    }
                    else
                    {
                        throw new Exception("incorrect cash");
                    }
                }
                else
                {
                    return this._nextHandler.Handle(request,f);
                }

            }
        }

        public class ValidateBuilder : IBuilder
        {
            private IHandler<MyStruct> _handler;

            public ValidateBuilder()
            {
                this._handler = null;
            }

            public void BuildStep1()
            {
                _handler = new AccountHandler(new NameValidate());
            }

            public void BuildStep2()
            {
                _handler.SetNext(new CashHandler(new CashValidate()));
            }

            public void BuildFull()
            {
                BuildStep1();
                BuildStep2();
            }

            public IHandler<MyStruct> GetHandler()
            {
                return _handler;
            }
        }

        //public class ValidateBuilderNameCash : IBuilder
        //{
        //    private IHandler _handler;

        //    public ValidateBuilderNameCash()
        //    {
        //        this.Reset();
        //    }

        //    public void Reset()
        //    {
        //        this._handler = null;
        //    }
        //    public void BuildStep1()
        //    {
        //        _handler = new AccountHandler(new NameValidate());
        //    }

        //    public void BuildStep2()
        //    {
        //        _handler.SetNext(new CashHandler(new CashValidate()));
        //    }

        //    public void BuildFullVersion()
        //    {
        //        BuildStep1();
        //        BuildStep2();

                
        //    }

        //    public IHandler GetHandler()
        //    {
        //        IHandler handler = this._handler;
        //        this.Reset();
        //        return handler;
        //    }
        //}

        static void Main(string[] args)
        {
            ShowInfo();
            var builder = new ValidateBuilder();
            builder.BuildFull();
            var acc = builder.GetHandler();
            var ms = new MyStruct();
            ms.name = "Name";
            ms.cash = 1500.0M;
            try
            {
                Console.WriteLine("First try");
                var accc1 = new AccountHandler(new NameValidate());
                accc1.SetNext(new CashHandler(new CashValidate()));
                var next = accc1;
               
                var res = accc1.Handle(ms, GetStr);
                Console.WriteLine(res);
                var res1 = accc1.Handle(ms, GetDec);
                Console.WriteLine(res1);




            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                
            }

            try
            {
                Console.WriteLine("Second try");
                ms.cash = 100.0M;
                var accc1 = new AccountHandler(new NameValidate());
                accc1.SetNext(new CashHandler(new CashValidate()));
                var res = accc1.Handle(ms, GetStr);
                Console.WriteLine(res);
                var res1 = accc1.Handle(ms, GetDec);
                Console.WriteLine(res);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
            try
            {
                Console.WriteLine("Third try");
                ms.name = "Nmae";
                ms.cash = 100.0M;
                var accc1 = new AccountHandler(new NameValidate());
                accc1.SetNext(new CashHandler(new CashValidate()));
                var res = accc1.Handle(ms, GetStr);
                Console.WriteLine(res);
                var res1 = accc1.Handle(ms, GetDec);
                Console.WriteLine(res);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }

            //Console.WriteLine(ShowInfo());
            ////var builder = new ValidateBuilderNameCash();
            //builder.BuildFullVersion();
            //var account = builder.GetHandler();
            //try
            //{
            //    Console.WriteLine(@"Для первой проверки используется аккаут с правильным именем {0} и балансом {1}","Name","1500.0M");
            //foreach (var req in new List<object> {"Name",1500.0M})
            //{
            //    var res = account.Handle(req);
            //    if (res != null)
            //    {
            //        Console.Write($"   {res}");
            //    }
            //    else
            //    {
            //        Console.WriteLine($"   {req} was left untouched.");
            //    }
            //}
            //Console.WriteLine();

            //Console.WriteLine(@"Для второй проверки используется аккаут с правильным именем {0} и не правильным балансом балансом {1}", "Name", "1000.0M");
            //foreach (var req in new List<object> { "Name", 1000.0M })
            //{
            //    var res = account.Handle(req);
            //    if (res != null)
            //    {
            //        Console.Write($"   {res}");
            //    }
            //    else
            //    {
            //        Console.WriteLine($"   {req} was left untouched.");
            //    }
            //}
            //Console.WriteLine();
            //    Console.WriteLine(@"Для третьей проверки используется аккаут с не правильным именем {0} и не балансом {1}", "Admin", "1000.0M");
            //foreach (var req in new List<object> { "Admin", 1000.0M })
            //{
            //    var res = account.Handle(req);
            //    if (res != null)
            //    {
            //        Console.Write($"   {res}");
            //    }
            //    else
            //    {


            //        Console.WriteLine($"   {req} was left untouched.");
            //    }
            //}
            //Console.WriteLine();
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);

            //}

        }

        static string ShowInfo()
        {
            return string.Format(@"Грппа {0} + Студент {1} + Задание 4 {2}", "8-Т3О-402Б-16", "Карпов А.Б",
                @"Реализовать шаблонный валидатор на основе паттерна “цепочка
            обязанностей”. Создание валидатора(добавление правил в цепочку)
            реализовать при помощи паттерна “строитель”. Метод для добавления
                правил в цепочку должен принимать на вход делегат, в котором
                определяется, над чем производить валидацию(селектор) и предикат для
                валидируемых данных.При непройденной валидации должна
            генерироваться исключительная ситуация.Продемонстрировать работу
            валидатора");
        }
    }
}
