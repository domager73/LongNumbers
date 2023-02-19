using System.Text;

namespace LongNumbersLibrary;

public class LongNumber
{
    private int[] _digits;
    private bool minus;
    private string msg;

    #region CreateObgect
    public LongNumber()
    {
        _digits = new int[] { 0 };
        minus = false;
    }

    public LongNumber(string number)
    {
        if (number[0] == '-')
        {
            _digits = new int[number.Length - 1];

            minus = true;

            number = number.Remove(0, 1);
        }
        else
        {
            _digits = new int[number.Length];

            minus = false;
        }

        for (int i = 0; i < number.Length; i++)
        {
            _digits[i] = int.Parse(number[i].ToString());
        }
    }

    public LongNumber(string number, bool CheckMinus)
    {
        _digits = new int[number.Length];
        for (int i = 0; i < number.Length; i++)
        {
            _digits[i] = int.Parse(number[i].ToString());
        }

        minus = CheckMinus;
    }

    public LongNumber(int[] digits, bool CheckMinus, int i)
    {
        _digits = new int[digits.Length];
        Array.Copy(digits, _digits, digits.Length);

        minus = CheckMinus;
    }
    #endregion

    #region Inheritance
    public override string ToString()
    {
        StringBuilder stringBuilder = new StringBuilder();
        int dot = 0;

        for (int i = _digits.Length - 1; i >= 0; i--)
        {
            dot++;

            stringBuilder.Append(_digits[i].ToString());

            if (dot == 3 && i > 0)
            {
                stringBuilder.Append(".");
                dot = 0;
            }
        }

        char[] temp = stringBuilder.ToString().ToCharArray();
        Array.Reverse(temp);
        string strTemp = new string(temp);
        if (minus)
        {
            strTemp = '-' + strTemp;
        }

        return msg + strTemp;
    }
    #endregion

    #region Overload
    public static LongNumber operator +(LongNumber number1, LongNumber number2)
    {
        LongNumber resultNumber = new LongNumber();

        resultNumber.msg = CheckMsg(number1, number2);

        if (number1.minus && number2.minus)
        {
            resultNumber.minus = true;
        }
        else if (number1.minus || number2.minus)
        {
            return number1 - number2;
        }

        if (number1._digits[0] == 0 && number1._digits.Length == 1)
        {
            resultNumber._digits = number2._digits;

            return resultNumber;
        }
        else if (number2._digits[0] == 0 && number2._digits.Length == 1)
        {
            resultNumber._digits = number1._digits;

            return resultNumber;
        }

        int[] resultDigits = new int[Math.Max(number1._digits.Length, number2._digits.Length) + 1];
        Array.Fill(resultDigits, 0);

        for (int i = number1._digits.Length - 1, j = number2._digits.Length - 1, k = 0;
             i >= 0 || j >= 0;
             i--, j--, k++)
        {
            if (i >= 0 && j >= 0)
            {
                resultDigits[k] = number1._digits[i] + number2._digits[j];
            }
            else if (i >= 0)
            {
                resultDigits[k] = number1._digits[i];
            }
            else if (j >= 0)
            {
                resultDigits[k] = number2._digits[j];
            }
        }

        for (int i = 0; i < resultDigits.Length - 1; i++)
        {
            if (resultDigits[i] > 9)
            {
                resultDigits[i + 1] += 1;
                resultDigits[i] -= 10;
            }
        }

        Array.Reverse(resultDigits);

        if (resultDigits[0] == 0)
        {
            int[] cleanDigits = new int[resultDigits.Length - 1];

            for (int i = 1, j = 0; i < resultDigits.Length; i++, j++)
            {
                cleanDigits[j] = resultDigits[i];
            }

            resultDigits = cleanDigits;
        }

        resultNumber._digits = resultDigits;

        return resultNumber;
    }

    public static LongNumber operator -(LongNumber number1, LongNumber number2)
    {
        LongNumber resultNumber = new LongNumber();

        resultNumber.msg = CheckMsg(number1, number2);

        bool CheckMinus = number1 < number2;

        if (number1.minus && number2.minus)
        {
            LongNumber temp = number1 + number2;
            temp.minus = true;
        }

        resultNumber._digits = new int[Math.Max(number1._digits.Length, number2._digits.Length)];

        for (int i = number1._digits.Length - 1, j = number2._digits.Length - 1, k = 0;
             i >= 0 || j >= 0;
             i--, j--, k++)
        {
            if (i >= 0 && j >= 0)
            {
                resultNumber._digits[k] = number1._digits[i] - number2._digits[j];
            }
            else if (i >= 0)
            {
                resultNumber._digits[k] = number1._digits[i];
            }
            else if (j >= 0)
            {
                resultNumber._digits[k] = number2._digits[j];
            }
        }

        for (int i = 0; i < resultNumber._digits.Length - 1; i++)
        {
            if (resultNumber._digits[i] < 0)
            {
                resultNumber._digits[i + 1] -= 1;
                resultNumber._digits[i] += 10;
            }
        }

        int countZero = 0;

        for (int i = 0; i < resultNumber._digits.Length; i++)
        {
            if (resultNumber._digits[i] != 0)
            {
                countZero++;
            }
        }

        if (countZero == 0)
        {
            return new LongNumber(new int[1] { 0 }, false, 1);
        }


        Array.Reverse(resultNumber._digits);

        while (resultNumber._digits[0] == 0)
        {
            int[] cleanDigits = new int[resultNumber._digits.Length - 1];

            for (int i = 1, j = 0; i < resultNumber._digits.Length; i++, j++)
            {
                cleanDigits[j] = resultNumber._digits[i];
            }

            resultNumber._digits = cleanDigits;
        }

        return resultNumber;
    }

    public static LongNumber operator *(LongNumber number1, LongNumber number2)
    {
        LongNumber resultNumber = new LongNumber();

        resultNumber.msg = CheckMsg(number1, number2);

        resultNumber.minus = CheckMinus(number1, number2);

        number1.minus = false;
        number2.minus = false;

        resultNumber._digits = new int[number1._digits.Length + number2._digits.Length];

        double doubleLenght = 0;
        for (int i = 0; i < number2._digits.Length; i++)
        {
            doubleLenght = Convert.ToDouble(number2._digits[number2._digits.Length - i - 1]) * Math.Pow(10, i) + doubleLenght;
        }

        int intLenght = Convert.ToInt32(doubleLenght);
        for (int i = 0; i < intLenght; i++)
        {
            resultNumber = resultNumber + number1;
        }

        for (int i = 0; i < resultNumber._digits.Length - 1; i++)
        {
            if (resultNumber._digits[i] > 9)
            {
                resultNumber._digits[i + 1] += 1;
                resultNumber._digits[i] -= 10;
            }
        }

        if (resultNumber._digits[0] == 0)
        {
            int[] cleanDigits = new int[resultNumber._digits.Length - 1];

            for (int i = 1, j = 0; i < resultNumber._digits.Length; i++, j++)
            {
                cleanDigits[j] = resultNumber._digits[i];
            }

            resultNumber._digits = cleanDigits;
        }

        return resultNumber;
    }

    public static LongNumber operator /(LongNumber number1, LongNumber number2)
    {
        LongNumber resultNumber = new LongNumber();

        resultNumber.msg = CheckMsg(number1, number2);

        resultNumber.minus = CheckMinus(number1, number2);

        number1.minus = false;
        number2.minus = false;

        if (number2._digits[0] == 1 && number2._digits.Length == 1)
        {
            return number1;
        }
        else
        {
            resultNumber._digits = new int[number1._digits.Length];
        }

        if (number2 > number1)
        {
            resultNumber._digits = new int[] { 0 };
            resultNumber.minus = false;
            resultNumber.msg = "примерно: ";

            return resultNumber;
        }
        else if (number1 == number2)
        {
            resultNumber._digits = new int[] { 1 };

            return resultNumber;
        }

        if (number2._digits[0] == 0)
        {
            resultNumber._digits = new int[1] { 0 };

            resultNumber.msg = "на ноль делить нельзя";

            return resultNumber;
        }

        LongNumber number = new LongNumber();

        int quotient = 0;

        while (number1 >= number + number2)
        {
            number = number + number2;
            quotient++;
        }

        if (number1 != number)
        {
            resultNumber.msg = "примерно: ";
        }

        string str = quotient.ToString();

        for (int i = 0; i < str.Length; i++)
        {
            resultNumber._digits[i] = Convert.ToInt32(str[str.Length - i - 1]) - 48;
        }

        Array.Reverse(resultNumber._digits);

        while (resultNumber._digits[0] == 0)
        {
            int[] cleanDigits = new int[resultNumber._digits.Length - 1];

            for (int i = 1, j = 0; i < resultNumber._digits.Length; i++, j++)
            {
                cleanDigits[j] = resultNumber._digits[i];
            }

            resultNumber._digits = cleanDigits;
        }

        return resultNumber;
    }

    public static bool operator ==(LongNumber longNumber1, LongNumber longNumber2) => CompareTo(longNumber1, longNumber2) == 0;

    public static bool operator !=(LongNumber longNumber1, LongNumber longNumber2) => CompareTo(longNumber1, longNumber2) != 0;

    public static bool operator >(LongNumber longNumber1, LongNumber longNumber2) => CompareTo(longNumber1, longNumber2) == 1;

    public static bool operator <(LongNumber longNumber1, LongNumber longNumber2) => CompareTo(longNumber1, longNumber2) == -1;

    public static bool operator >=(LongNumber longNumber1, LongNumber longNumber2) => CompareTo(longNumber1, longNumber2) >= 0;

    public static bool operator <=(LongNumber longNumber1, LongNumber longNumber2) => CompareTo(longNumber1, longNumber2) <= 0;
    #endregion

    #region SupportFunction
    private static int CompareTo(LongNumber number1, LongNumber number2)
    {
        if (number1._digits.Length > number2._digits.Length)
        {
            return 1;
        }
        else if (number1._digits.Length < number2._digits.Length)
        {
            return -1;
        }
        else
        {
            for (int i = 0; i < number1._digits.Length; i++)
            {
                if (number1._digits[i] > number2._digits[i])
                {
                    return 1;
                }
                else if (number1._digits[i] < number2._digits[i])
                {
                    return -1;
                }
            }

            return 0;
        }
    }

    private static string CheckMsg(LongNumber number1, LongNumber number2)
    {
        if (number1.msg != null)
        {
            return number1.msg;
        }
        else if (number2.msg != null)
        {
            return number2.msg;
        }
        else
        {
            return "";
        }

    }

    private static bool CheckMinus(LongNumber number1, LongNumber number2)
    {
        if (number1.minus || number2.minus)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion
}