using System;

class StringArray
{
    private string[] array;
    private int stringLength;

    public StringArray(int size, int stringLength)
    {
        array = new string[size];
        this.stringLength = stringLength;
    }

    public string this[int index]
    {
        get
        {
            if (index < 0 || index >= array.Length)
            {
                throw new IndexOutOfRangeException("Индекс выходит за пределы массива.");
            }

            return array[index];
        }

        set
        {
            if (index < 0 || index >= array.Length)
            {
                throw new IndexOutOfRangeException("Индекс выходит за пределы массива.");
            }

            if (value.Length > stringLength)
            {
                throw new ArgumentException("Строка превышает заданную длину.");
            }

            array[index] = value;
        }
    }

    public StringArray Concatenate(StringArray other)
    {
        int size = Math.Min(array.Length, other.array.Length);

        StringArray result = new StringArray(size, stringLength);

        for (int i = 0; i < size; i++)
        {
            string newString = array[i] + other.array[i];

            if (newString.Length > stringLength)
            {
                newString = newString.Substring(0, stringLength);
            }

            result[i] = newString;
        }

        return result;
    }

    public StringArray Merge(StringArray other)
    {
        StringArray result = new StringArray(
            array.Length + other.array.Length,
            stringLength);

        int count = 0;

        for (int i = 0; i < array.Length; i++)
        {
            if (!Contains(result, count, array[i]))
            {
                result[count] = array[i];
                count++;
            }
        }

        for (int i = 0; i < other.array.Length; i++)
        {
            if (!Contains(result, count, other.array[i]))
            {
                result[count] = other.array[i];
                count++;
            }
        }

        StringArray finalResult = new StringArray(count, stringLength);

        for (int i = 0; i < count; i++)
        {
            finalResult[i] = result[i];
        }

        return finalResult;
    }

    private bool Contains(StringArray arr, int count, string value)
    {
        for (int i = 0; i < count; i++)
        {
            if (arr[i] == value)
            {
                return true;
            }
        }

        return false;
    }

    public void ShowElement(int index)
    {
        Console.WriteLine("Элемент с индексом " + index + ": " + this[index]);
    }

    public void ShowAll()
    {
        for (int i = 0; i < array.Length; i++)
        {
            Console.WriteLine(i + ": " + array[i]);
        }
    }
}

class Program
{
    static void Main()
    {
        StringArray array1 = new StringArray(3, 10);
        StringArray array2 = new StringArray(3, 10);

        array1[0] = "Привет";
        array1[1] = "Мир";
        array1[2] = "C#";

        array2[0] = "!";
        array2[1] = "Язык";
        array2[2] = "Мир";

        Console.WriteLine("Первый массив:");
        array1.ShowAll();

        Console.WriteLine();

        Console.WriteLine("Второй массив:");
        array2.ShowAll();

        Console.WriteLine();

        Console.WriteLine("Элемент первого массива с индексом 1:");
        array1.ShowElement(1);

        Console.WriteLine();

        StringArray concatenated = array1.Concatenate(array2);

        Console.WriteLine("Поэлементное сцепление:");
        concatenated.ShowAll();

        Console.WriteLine();

        StringArray merged = array1.Merge(array2);

        Console.WriteLine("Слияние без повторяющихся элементов:");
        merged.ShowAll();
    }
}