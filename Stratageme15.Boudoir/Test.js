function Stratageme15Boudoir$AnotherSmallClass(name, id) {
        {
            this._Name = null;
            this._Id = 0;
        }
        this.setName(name);
        this.setId(id);
    }

Stratageme15Boudoir$AnotherSmallClass.prototype.setName = function (value) { this._Name = value; };

Stratageme15Boudoir$AnotherSmallClass.prototype.getName = function () { return this._Name; };

Stratageme15Boudoir$AnotherSmallClass.prototype.setId = function (value) { this._Id = value; };

Stratageme15Boudoir$AnotherSmallClass.prototype.getId = function () { return this._Id; };

Stratageme15Boudoir$AnotherSmallClass.prototype.constructor = Stratageme15Boudoir$AnotherSmallClass;

;

function Stratageme15Boudoir$VeryMagicTest(variable) {
        {
            this._privateVariable = null;
            this._anotherSmallClassInstance = new Stratageme15Boudoir$AnotherSmallClass("Hello!", 15);
            this._dependantField = 0;
            this._y = 0;
            this._x = 0;
            this._privateBoolField = false;
            this._privateInitializedBooField = true;
            this._veryTest = new Stratageme15Boudoir$VeryMagicTest(null);
            this._variable = null;
        }
        this._variable = variable;
    }

Stratageme15Boudoir$VeryMagicTest.prototype.setVariable = function (value) { this._variable = value; };

Stratageme15Boudoir$VeryMagicTest.prototype.getVariable = function () { return this._variable; };

Stratageme15Boudoir$VeryMagicTest.prototype.ReturnSomeString = function () { return Empty; };

Stratageme15Boudoir$VeryMagicTest.prototype.ConstructSomething = function () {
        try 
        {
            var s = "hello, world!";
            Console.WriteLine(this.ReturnSomeString(), s);
        }
        catch (ex) { Console.WriteLine(ex.getMessage()); }
        finally { Console.WriteLine("inside finally"); }
    };

Stratageme15Boudoir$VeryMagicTest.prototype.OnButtonClick = function (sender, e) {
        _anotherSmallClassInstance.setName("World");
        for (var i = 0; i < 10; i++) { _anotherSmallClassInstance.setId(_anotherSmallClassInstance.getId() + 5); }
        if (this._anotherSmallClassInstance != null && _anotherSmallClassInstance.getId() > 3) { Console.WriteLine("Successfully modified!"); }
        
        var b = false;
        var c = true;
        var obj = {
            A: "asdfasdf",
            B: {
                C: 0,
                D: 1
            }
        };
    };

Stratageme15Boudoir$VeryMagicTest.prototype.OnSomethingElse = function (sender, e) {
        var doubleVariable;
        var shortVariable;
    };

Stratageme15Boudoir$VeryMagicTest.prototype.constructor = Stratageme15Boudoir$VeryMagicTest;

;

