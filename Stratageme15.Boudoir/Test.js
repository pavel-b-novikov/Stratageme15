function Stratageme15Boudoir$VeryMagicTest() {
        this._privateVariable = null;
        this._y = 0;
        this._x = 0;
        this._privateFloatValue = 0;
        this._privateBoolField = false;
        this._privateInitializedBooField = true;
    }

Stratageme15Boudoir$VeryMagicTest.prototype.ReturnSomeString = function () { return Empty; };

Stratageme15Boudoir$VeryMagicTest.prototype.ConstructSomething = function () {
        try 
        {
            var s = "hello, world!";
            Console.WriteLine(ReturnSomeString(), s);
        }
        catch (ex) { Console.WriteLine(ex.Message); }
        finally { Console.WriteLine("inside finally"); }
    };

Stratageme15Boudoir$VeryMagicTest.prototype.OnButtonClick = function (sender, e) {
        var b = false;
        var c = true;
        var obj = {
            A: "asdfasdf",
            B: {
                C: 0,
                D: 1
            }
        };
        var obj2 = new Stratageme15Boudoir$VeryMagicTest();
        obj2.GetHashCode();
        obj2.GetType();
    };

Stratageme15Boudoir$VeryMagicTest.prototype.OnSomethingElse = function (sender, e) {
        var doubleVariable;
        var shortVariable;
    };

Stratageme15Boudoir$VeryMagicTest.prototype.constructor = Stratageme15Boudoir$VeryMagicTest;

;

