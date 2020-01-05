using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace staticTest
{

    public class Person {

        public static int a = 3;
        public static void show() { 

        }
    }
    class Program
    {

        static void Main(string[] args)
        {
            Person p1 = new Person();
            Person.a = 1;


        }
    }
}
