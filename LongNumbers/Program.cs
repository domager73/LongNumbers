using LongNumbersLibrary;

LongNumber num1 = new LongNumber("12341234");
LongNumber num2 = new LongNumber("12341234134");
LongNumber num3 = new LongNumber("123123");

LongNumber num4 = num2 / num3;

Console.WriteLine(num4);