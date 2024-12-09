using System.Reflection;
using System.Text;
using app.Queries.TableObjects;
namespace app.Menus;


public static class MenuHelpers
{

    public static string CenterInString(string text, int totalWidth, char padChar = ' ')
    {
        if (text.Length >= totalWidth)
        {
            return text;
        }
        int leftPadding = (totalWidth - text.Length) / 2 + text.Length;
        return text.PadLeft(leftPadding, padChar).PadRight(totalWidth);
    }

    public static string[] CreateHeaderStrings(string text, int totalWidth, char topChar = '-', char sideChar = '|')
    {
        List<string> outputStrings = new List<string>();
        outputStrings.Add(new string(topChar, totalWidth));
        if (text.Length < totalWidth - 2)
        {
            outputStrings.Add(sideChar + CenterInString(text, totalWidth) + sideChar);
        }
        else
        {
            //insåg efteråt att jag kunde ha splittat vid varje space och
            //sen gjort min radbrytning vid index halva längden av listan ord
            //detta är dock inte utan egan problem att lösa som tex olika storlek på ord
            //but this was already completed 
            int splitCount = text.Length / totalWidth;
            
            while (text.Length > totalWidth - 2)
            {
                int index = totalWidth - 3;
                bool loop = true;
                while (loop)
                {
                    if (text[index] == ' ')
                    {
                        outputStrings.Add(sideChar + CenterInString(text.Substring(0, index + 1), totalWidth - 2) + sideChar);
                        text = text.Remove(0, index + 1);
                        splitCount--;
                        loop = false;
                    }
                    index--;
                }
            }
            
            outputStrings.Add(sideChar + CenterInString(text, totalWidth - 2) + sideChar);
            
        }
        outputStrings.Add(new string(topChar, totalWidth));
        return outputStrings.ToArray();
    }

    public static string[] createOptionFooterStrings(List<string> options, int totalWidth, int spacing = 5,char topChar = '-', char sideChar = '|')
    {
        List<string> newString = new List<string>();
        int optionCount = options.Count;
        int optionTextLength = options.Sum(s => s.Length);
        int textWithSpacingSize = optionTextLength + (optionCount * spacing * 2) + (spacing * 2);
        StringBuilder stringBuilder = new StringBuilder();
        
        newString.Add(new String(topChar, totalWidth));

        if (textWithSpacingSize < totalWidth - 2)
        {

            stringBuilder.Append(' ', spacing);
            stringBuilder.Append(fullStringWithSpacing(' '));
            stringBuilder.Append(' ', spacing);

            newString.Add(sideChar + CenterInString(stringBuilder.ToString(), totalWidth - 2) + sideChar);
        }
        else
        {
            stringBuilder.Append(fullStringWithSpacing('\u25a0', true));
            stringBuilder.Remove(0, 1);
            string currentString = stringBuilder.ToString();
            textWithSpacingSize = currentString.Length + (spacing * 2);
            while (textWithSpacingSize > totalWidth - 12)
            {
                int index = totalWidth - 3;
                bool loop = true;
                while (loop)
                {
                    if (currentString[index] == '|')
                    {
                        stringBuilder.Replace('\u25a0', ' ', 0, index);
                        stringBuilder.Remove(index, 1);
                        newString.Add(sideChar + CenterInString(new string(' ', spacing) + 
                                                                stringBuilder.ToString(0, index - 1).Replace('|', '\0') + 
                                                                new string(' ', spacing), totalWidth - 2) + sideChar);
                        stringBuilder.Remove(0, index);
                        loop = false;
                    }
                    index--;
                }
                currentString = stringBuilder.ToString();
                textWithSpacingSize = currentString.Length + (spacing * 2);
            }
            stringBuilder.Replace('|', '\0');
            stringBuilder.Replace('\u25a0', ' ');
            stringBuilder.Insert(0, new string(' ', spacing));
            stringBuilder.Append(' ', spacing);
            newString.Add(sideChar + CenterInString(stringBuilder.ToString(), totalWidth) + sideChar);

            
        }
        string fullStringWithSpacing(char c, bool seperateSpacingWithChar = false)
        {
            StringBuilder newStr = new StringBuilder();
            foreach (var option in options)
            {
                if (seperateSpacingWithChar) stringBuilder.Append('|');
                stringBuilder.Append(c, spacing);
                stringBuilder.Append(option);
                stringBuilder.Append(c, spacing);
            }
            return newStr.ToString();
        }
        newString.Add(new String(topChar, totalWidth));
        return newString.ToArray();
    }
    
    public static Dictionary<PropertyInfo, int> CalculateMaxWidthOfAllProperties<T>(List<T> objects)
    {
        var result = new Dictionary<PropertyInfo, int>();
        object? obj = objects[0];
        if (obj == null) return result;
        var properties = obj.GetType().GetProperties();
        foreach (var property in properties)
        {
            result.Add(property, CalculateMaxWidthOfProperty(property, objects));
            Console.WriteLine(result[property].ToString());
        }

        Console.ReadLine();
        
        return result;
    }

    
    private static int CalculateMaxWidthOfProperty<T>(PropertyInfo property, List<T> objects)
    {
        int maxWidth = 0;
        foreach (object obj in objects)
        {
            int val = property.GetValue(obj)?.ToString()?.Length ?? 0;
            maxWidth = (val > maxWidth) ? val : maxWidth;
        }

        return maxWidth;
    }

    public static void PrintWithColor(string text, ConsoleColor textColor, string prefix = "",
                                      ConsoleColor prefixColor = ConsoleColor.White, 
                                      string suffix = "", ConsoleColor suffixColor = ConsoleColor.White)
    {
        if (!String.IsNullOrEmpty(prefix))
        {
            Console.ForegroundColor = prefixColor;
            Console.Write(prefix);
        }
        
        Console.ForegroundColor = textColor;
        Console.Write(text);
        
        if (!String.IsNullOrEmpty(suffix))
        {
            Console.ForegroundColor = suffixColor;
            Console.Write(suffix);
        }
        
        Console.ResetColor();
    }
    
    
    
    //                 Console.Write("Name: ");
    //                 Console.ForegroundColor = ConsoleColor.DarkCyan;
    //                 Console.Write((customer.Name ?? "null").PadRight(maxNameLength));
    //                 Console.ForegroundColor = ConsoleColor.DarkMagenta;
    //                 Console.Write("  |  ");
    
    // private async Task ResultMenuHandler()
    // {
    //     int extraTextLength = "|  Id:   |  Name:   |  Email:   |  PhoneNumber:   |  BirthYear:   |".Length;
    //     int totalWidth = maxIdLength + maxNameLength + maxEmailLength + maxPhonenumberLength + maxBirthyearLength + extraTextLength;
    //     int customersStart = 0;
    //     int customersEnd = 10;
    //     ConsoleKeyInfo key;
    //     bool test = true;
    //     int row = 0;
    //     Console.Clear();
    //     (int left, int top) = Console.GetCursorPosition();
    //     
    //     
    //
    //     Console.CursorVisible = false;
    //     
    //     while (test)
    //     {
    //         Console.SetCursorPosition(left, top);
    //         
    //         for (int i = customersStart; i < customersEnd && i < customers.Count; i++)
    //         {
    //             Customer customer = customers[i];
    //
    //             Console.ForegroundColor = (i == row) ? ConsoleColor.Green : ConsoleColor.Gray;
    //
    //             if (i == row)
    //             {
    //                 Console.ForegroundColor = ConsoleColor.Green;
    //                 Console.WriteLine($"|  Id: {customer.Id.ToString().PadRight(maxIdLength)}  |  " +
    //                                   $"Name: {(customer.Name ?? "null").PadRight(maxNameLength)}  |  " +
    //                                   $"Email: {(customer.Email ?? "null").PadRight(maxEmailLength)}  |  " +
    //                                   $"PhoneNumber: {(customer.PhoneNumber ?? "null").PadRight(maxPhonenumberLength)}  |  " +
    //                                   $"BirthYear: {customer.Birthyear.ToString().PadRight(maxBirthyearLength)}" + "  <--");
    //             }
    //         }
    //         Console.ResetColor();
    
    //         Console.WriteLine(new string('-', totalWidth));
    //         // if (_menuOptions.TryGetValue(_menuState, out string[]? options))
    //         // {
    //         //     string optionsText = "";
    //         //     foreach (var option in options)
    //         //     {
    //         //         optionsText = optionsText + option + "   ";
    //         //     }
    //         //     Console.WriteLine("|" + PadBoth(optionsText, totalWidth - 2) + "|");
    //         // }
    //         // else
    //         // {
    //         //     Console.WriteLine("no options for this state");
    //         // }
    //         Console.WriteLine(new string('-', totalWidth));
    //         
    //         
    //         key = Console.ReadKey(true);
    //         
    //         switch (key.Key)
    //         {
    //             case ConsoleKey.DownArrow:
    //                 if (row == customers.Count - 1)
    //                 {
    //                     row = 0;
    //                     customersStart = 0;
    //                     customersEnd = 10;
    //                 }
    //                 else
    //                 {
    //                     if (row >= customers.Count - 5)
    //                     {
    //                         customersStart = customers.Count - 10;
    //                         customersEnd = customers.Count;
    //                     }
    //                     else if (row >= 4)
    //                     {
    //                         customersStart++;
    //                         customersEnd++;
    //                     }
    //
    //                     row++;
    //                 }
    //                 break;
    //             case ConsoleKey.UpArrow:
    //                 if (row == 0)
    //                 {
    //                     row = customers.Count - 1;
    //                     customersStart = customers.Count - 10;
    //                     customersEnd = customers.Count;
    //                 }
    //                 else
    //                 {
    //                     if (row <= 4)
    //                     {
    //                         customersStart = 0;
    //                         customersEnd = 10;
    //                     }
    //                     else if (row < customers.Count - 5)
    //                     {
    //                         customersStart--;
    //                         customersEnd--;
    //                     }
    //                     
    //                     row --;
    //                 }
    //                 
    //                 break;
    //             case ConsoleKey.Enter:
    //                 test = false;
    //                 break;
    //                 
    //         }
    //         
    //     }
    //}
}