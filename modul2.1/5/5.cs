using System;

class TemperatureSensor
{
    public event Action<double>? TemperatureChanged;

    private double temperature;

    public void SetTemperature(double newTemperature)
    {
        if (newTemperature != temperature)
        {
            temperature = newTemperature;
            TemperatureChanged?.Invoke(temperature);
        }
    }
}

class Thermostat
{
    public void OnTemperatureChanged(double temperature)
    {
        Console.WriteLine("Температура: " + temperature);

        if (temperature < 10)
        {
            Console.WriteLine("Отопление включено.");
        }
        else
        {
            Console.WriteLine("Отопление выключено.");
        }
    }
}

class Program
{
    static void Main()
    {
        TemperatureSensor sensor = new TemperatureSensor();
        Thermostat thermostat = new Thermostat();

        sensor.TemperatureChanged += thermostat.OnTemperatureChanged;

        sensor.SetTemperature(8);

        Console.WriteLine();

        sensor.SetTemperature(12);

        Console.WriteLine();

        sensor.SetTemperature(5);
    }
}