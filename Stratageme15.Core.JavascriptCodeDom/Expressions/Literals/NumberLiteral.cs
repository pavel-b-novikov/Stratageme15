using System.Globalization;
using Stratageme15.Core.JavascriptCodeDom.Markers;

namespace Stratageme15.Core.JavascriptCodeDom.Expressions.Literals
{
    public class NumberLiteral : LiteralExpression, IDictionaryKey
    {
        public NumberLiteral(decimal number)
        {
            Number = number;
            Parsed = true;
            InitialNumberString = number.ToString();
        }

        public NumberLiteral(string number)
        {
            InitialNumberString = number;
            decimal nm = 0;
            Parsed = decimal.TryParse(number,out nm);
            Number = nm;
        }

        public bool Parsed { get; private set; }

        public string InitialNumberString { get; private set; }

        public decimal Number { get; set; }

        public override string ToString()
        {
            return string.Format("{1}{0}", Parsed?Number.ToString():InitialNumberString, Parsed?string.Empty:"(not parsed)");
        }

        public override string Literal
        {
            get { return Parsed ? Number.ToString("#0.#", CultureInfo.InvariantCulture) : InitialNumberString; }
        }
    }
}