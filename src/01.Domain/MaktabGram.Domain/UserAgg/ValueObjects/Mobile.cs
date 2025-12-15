using System.Text.RegularExpressions;

namespace MaktabGram.Domain.Core.UserAgg.ValueObjects;
public class Mobile
{
    public string Value { get; set; }

    public Mobile(string value)
    {
        Value = Normalize(value);
        Validate(Value);
    }

    public static Mobile Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new Exception("شماره موبایل نمی‌تواند خالی باشد");
        }

        var normalized = Normalize(value);
        Validate(normalized);

        return new Mobile(normalized);
    }

    private static string Normalize(string input)
    {
        var digitsOnly = Regex.Replace(input, @"[^\d]", "");

        if (digitsOnly.StartsWith("98"))
        {
            digitsOnly = "0" + digitsOnly.Substring(2);
        }
        else if (digitsOnly.StartsWith("0098"))
        {
            digitsOnly = "0" + digitsOnly.Substring(4);
        }

        return digitsOnly;
    }

    private static void Validate(string mobile)
    {
        if (!Regex.IsMatch(mobile, @"^09\d{9}$"))
        {
            throw new Exception("شماره موبایل نامعتبر است.");
        }
    }

    public override string ToString() => Value;

    private Mobile() { }
}

