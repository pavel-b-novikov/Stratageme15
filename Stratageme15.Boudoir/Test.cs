using System;

namespace Stratageme15.Boudoir
{

    public class AnotherSmallClass
    {
        public string Name { get; set; }
        public int Id { get; set; }

        public AnotherSmallClass(string name, int id)
        {
            Name = name;
            Id = id;
        }
    }

    public class VeryMagicTest
    {
        private string _privateVariable = null;
        private AnotherSmallClass _anotherSmallClassInstance = new AnotherSmallClass("Hello!", 15);

        public VeryMagicTest(string variable)
        {
            _variable = variable;
        }

        public string Variable
        {
            get { return _variable; }
            set { _variable = value; }
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
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.WriteLine("inside finally");
            }
        }

        public void OnButtonClick(object sender, EventArgs e)
        {
            _anotherSmallClassInstance.Name = "World";
            for (int i = 0; i < 10; i++)
            {
                _anotherSmallClassInstance.Id += 5;
            }
            if (_anotherSmallClassInstance!=null&&_anotherSmallClassInstance.Id>3)
            {
                Console.WriteLine("Successfully modified!");
                    
            }
            bool b = false;
            bool c = true;
            var obj = new { A = "asdfasdf", B = new { C = 0, D = 1 } };
        }

        private int _dependantField;
        private int _x, _y;
        // private float _privateFloatValue = new float(); todo type syntax identifiers
        private bool _privateBoolField;
        private bool _privateInitializedBooField = true;
        private VeryMagicTest _veryTest = new VeryMagicTest(null);
        private string _variable;

        public void OnSomethingElse(object sender, EventArgs e)
        {
            double doubleVariable;
            short shortVariable;
        }
    }

}

