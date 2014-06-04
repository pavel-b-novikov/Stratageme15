using System;

namespace Stratageme15.Boudoir
{
    public class VeryMagicTest
    {
        public string Variable { get; set; }
       
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
        private string _privateVariable = null;
        
        public void OnButtonClick(object sender, EventArgs e)
        {
            bool b = false;
            bool c = true;
            var obj = new {A = "asdfasdf", B = new {C = 0, D = 1}};
        }

        private int _dependantField;
        private int _x, _y;
       // private float _privateFloatValue = new float(); todo type syntax identifiers
        private bool _privateBoolField;
        private bool _privateInitializedBooField = true;
        private VeryMagicTest _veryTest = new VeryMagicTest();
        public void OnSomethingElse(object sender,EventArgs e)
        {
            double doubleVariable;
            short shortVariable;
        }
    }
}

