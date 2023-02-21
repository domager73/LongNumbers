using LongNumbersLibrary;

Console.WriteLine("Введите задачу: ");
string[] exemple = Console.ReadLine().Split(" ");

LongNumber num1 = new LongNumber($"{exemple[0]}");
LongNumber num2 = new LongNumber($"{exemple[2]}");
string sing = exemple[1];

LongNumber result = num1.Decision(num2, sing);

Console.WriteLine(result);