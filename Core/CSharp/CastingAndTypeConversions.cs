using System;
using System.Collections.Generic;
using System.Text;

namespace Core.CSharp
{
    public static class CastingAndTypeConversions
    {
        /// <summary>
        /// Só pode declarar uma variavel uma unica vez. 
        /// Não pode fazer re-declarar.
        /// </summary>
        public static void Declare()
        {
            //int explicitDeclaration;
            //explicitDeclaration = "Hello"; // error CS0029: Cannot implicitly convert type 'string' to 'int'

            //var implicitDeclaration = 1;
            //implicitDeclaration = "Hello"; // error CS0029: Cannot implicitly convert type 'string' to 'int'

            //explicitDeclaration = implicitDeclaration;
        }

        /// <summary>
        /// Quando conversão não tem chance de dar erro.
        /// Quando não tem risco de perder informação.
        /// </summary>
        public static void ImplicitConversions()
        {
            // Implicit conversion. A long can
            // hold any value an int can hold, and more!
            int number = 2147483647;
            long bigNumber = number;

            Derived d = new Derived();
            Base b = d; // Always OK. 
        }

        /// <summary>
        /// Chamado de Cast ou Casting.
        /// Quando tem chance de perder informação na conversão.
        /// Quando pode dar pau.
        /// </summary>
        public static void ExplicitConversions()
        {
            double x = 1234.7;
            int a;
            // Cast double to int.
            a = (int)x;
            System.Console.WriteLine(a);// Output: 1234


            // Create a new derived type.  
            Giraffe girafa = new Giraffe();

            // Implicit conversion to base type is safe.  
            Animal animal = g;

            // Explicit conversion is required to cast back  
            // to derived type. Note: This will compile but will  
            // throw an exception at run time if the right-side  
            // object is not in fact a Giraffe.  
            Giraffe outraGirafa = (Giraffe)animal;
        }

        private class Base { }
        private class Derived : Base { }


        private class Animal
        {
        }

        private class Giraffe : Animal
        {
        }
    }
}
