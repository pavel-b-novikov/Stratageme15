using System;

namespace Stratageme15.Boudoir
{
    public class VeryMagicTest
    {
        public string Variable { get; set; }

        public VeryMagicTest()
        {
            int i, j, k;
        }

        private string ReturnSomeString()
        {
            return string.Empty;
        }

        public void ConstructSomething()
        {
            try
            {
                var s = "hello, world!";
                Console.WriteLine(ReturnSomeString(), s);
            }catch(NullReferenceException ex)
            {
                Console.WriteLine(ex.Message);
            }finally
            {
                Console.WriteLine("inside finally");
            }
        }

        public void OnButtonClick(object sender, EventArgs e)
        {
            bool b = false;
            bool c = true;
            var obj = new {A = "asdfasdf", B = new {C = 0, D = 1}};

            var obj2 = new VeryMagicTest();

            obj2.GetHashCode();

            obj2.GetType();
        }

        public void OnSomethingElse(object sender,EventArgs e)
        {
            double doubleVariable;
            short shortVariable;
        }
    }
}

