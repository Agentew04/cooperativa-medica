using MySqlX.XDevAPI.Common;
using System.Text;

namespace CoopMedica;

/// <summary>
/// Classe que contém métodos variados que auxiliam majoritariamente com 
/// a interação com o usuário.
/// </summary>
public static class Utils {
    /// <summary>
    /// Le e retorna um inteiro do usuario. Implementa validacao.
    /// </summary>
    /// <param name="prompt">Mensagem a ser exibida ao usuario</param>
    /// <param name="allowNegative">Se true, permite numeros negativos</param>
    /// <returns>O inteiro lido</returns>
    public static int? ReadInt(string prompt = "", 
        bool allowNegative = false, 
        Range? range = null, 
        int? defaultValue = null,
        bool allowEmpty = false){

        bool ok = false;
        Console.Write(prompt);
        string content = "";
        int? result = null;
        if (defaultValue is not null) {
            content = defaultValue.Value.ToString();
        }

        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write(content);
        ConsoleKeyInfo key;
        do {
            string error = "";
            key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.Enter) {
                bool canParse = int.TryParse(content, out var res);
                result = res;

                if(!canParse) {
                    error = "Valor inválido";
                }

                if(!allowNegative && result < 0) {
                    error = "Valor inválido. Digite um número positivo.";
                }

                if(range is not null && !range.Contains(result.Value)) {
                    error = $"Valor fora do intervalo [{range.Start},{range.End}]";
                }

                ok = canParse && error == "";
                if(content == "" && allowEmpty) {
                    ok = true;
                    error = "";
                    result = null;
                }
            } else if (key.Key == ConsoleKey.Backspace && content.Length > 0) {
                content = content[..^1];
            } else {
                if (key.KeyChar >= '0' && key.KeyChar <= '9'
                                       && content.Length < 10) {
                    content += key.KeyChar;
                }
            }

            Console.CursorLeft = prompt.Length;
            Console.Write(new string(' ', 10));
            Console.CursorLeft = prompt.Length;
            Console.ForegroundColor = error == "" ? ConsoleColor.Blue : ConsoleColor.Red;
            Console.Write(content);
            if(error != "") {
                Console.Write(" ");
                Console.Write(error);
            }
            Console.ResetColor();
        }while(!ok);
        Console.WriteLine();
        
        return result;
    }

    /// <summary>
    /// Le uma string do usuario. Implementa validacao para strings vazias.
    /// </summary>
    /// <param name="prompt">O prompt a ser mostrado antes</param>
    /// <param name="allowEmpty">Se o usuario pode inserir strings vazias</param>
    /// <returns>A string lida</returns>
    public static string ReadString(string prompt = "", bool allowEmpty = false, string defaultValue = "") {
        bool ok = false;
        Console.Write(prompt);
        string content = defaultValue;
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write(content);
        ConsoleKeyInfo key;
        do {
            string error = "";
            key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.Enter) {
                if(content == "" && !allowEmpty) {
                    error = "Valor inválido!";
                }

                ok = true;
            } else if (key.Key == ConsoleKey.Backspace && content.Length > 0) {
                content = content[..^1];
            } else {    
                content += key.KeyChar;
            }

            Console.CursorLeft = prompt.Length;
            Console.Write(new string(' ', Console.BufferWidth-prompt.Length-5));
            Console.CursorLeft = prompt.Length;
            Console.ForegroundColor = error == "" ? ConsoleColor.Blue : ConsoleColor.Red;
            Console.Write(content);
            if (error != "") {
                Console.Write(" ");
                Console.Write(error);
            }
            Console.ResetColor();
        } while (!ok);
        Console.WriteLine();

        return content;
    }
    
    /// <summary>
    /// Le uma data do usuario.
    /// </summary>
    /// <param name="prompt">O prompt a ser mostrado</param>
    /// <param name="validation">Uma funcao de validacao para o input</param>
    /// <returns>A data lida</returns>
    public static DateOnly ReadDate(string prompt = "", Predicate<DateOnly>? validation = null, DateOnly? defaultValue = null) {
        bool ok = false;
        Console.Write(prompt);
        string pattern = "  /  /    ";
        string content = "";
        if(defaultValue is not null) {
            content = defaultValue.Value.ToString("ddMMyyyy");
        }
        Console.ForegroundColor = ConsoleColor.Blue;
        PrintMaskedContent(pattern, content);
        Console.CursorLeft = prompt.Length + content.Length + pattern[..(content.Length + 1)].Count(x => x != ' ');
        while (!ok) {
            ConsoleKeyInfo key;
            do {
                key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Enter) {
                    continue;
                }
                if (key.Key == ConsoleKey.Backspace) {
                    if (content.Length > 0)
                        content = content[..^1];
                } else {
                    if (key.KeyChar >= '0' && key.KeyChar <= '9'
                        && content.Length < pattern.Count(x => x == ' '))
                        content += key.KeyChar;
                }

                Console.CursorLeft = prompt.Length;
                PrintMaskedContent(pattern, content);
                Console.CursorLeft = prompt.Length + content.Length + pattern[..(content.Length + 1)].Count(x => x != ' ');
            } while (key.Key != ConsoleKey.Enter);

            StringBuilder result = new();
            for (int i = 0; i < pattern.Length; i++) {
                if (pattern[i] != ' ') {
                    result.Append(pattern[i]);
                } else {
                    if (content.Length > 0) {
                        result.Append(content[0]);
                        content = content[1..];
                    }
                }
            }
            ok = DateOnly.TryParse(result.ToString(), out var date);
            if (ok) {
                if ((validation is not null && validation(date)) || validation is null) {
                    Console.ResetColor();
                    Console.WriteLine();
                    return date;
                }
            }
        }
        return default;
    }

    private static void PrintMaskedContent(string pattern, string content) {
        Queue<char> contentStack = new(content);
        for (int i = 0; i < pattern.Length; i++) {
            if (pattern[i] != ' ') {
                Console.Write(pattern[i]); //pattern char
            } else {
                if (contentStack.Count > 0) {
                    Console.Write(contentStack.Dequeue()); //content char
                } else {
                    Console.Write(' ');//empty space
                }
            }
        }
    }

    /// <summary>
    /// Le uma string do usuario, com uma mascara.
    /// </summary>
    /// <param name="prompt">O prompt a ser mostrado antes do input</param>
    /// <param name="pattern">O padrao/mascara a ser utilizado</param>
    /// <returns>A string lida, com a mascara incluida</returns>
    public static string ReadMaskedString(string prompt, string pattern, string defaultValue = "") {
        bool ok = false;
        Console.Write(prompt);
        pattern
            .Where(x => x != ' ')
            .ToList()
            .ForEach(x => defaultValue = defaultValue.Replace(x.ToString(), ""));
        string content = defaultValue;
        Console.ForegroundColor = ConsoleColor.Blue;
        PrintMaskedContent(pattern, content);
        Console.CursorLeft = prompt.Length + content.Length + pattern[..(content.Length + 1)].Count(x => x != ' ');
        while (!ok) {
            ConsoleKeyInfo key;
            do {
                key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Enter) {
                    continue;
                }
                if (key.Key == ConsoleKey.Backspace) {
                    if (content.Length > 0)
                        content = content[..^1];
                } else {
                    if (key.KeyChar >= '0' && key.KeyChar <= '9'
                        && content.Length < pattern.Count(x => x == ' '))
                        content += key.KeyChar;
                }

                Console.CursorLeft = prompt.Length;
                PrintMaskedContent(pattern, content);
                Console.CursorLeft = prompt.Length + content.Length + pattern[..(content.Length + 1)].Count(x => x != ' ');
            } while (key.Key != ConsoleKey.Enter);

            StringBuilder result = new();
            for (int i = 0; i < pattern.Length; i++) {
                if (pattern[i] != ' ') {
                    result.Append(pattern[i]);
                } else {
                    if (content.Length > 0) {
                        result.Append(content[0]);
                        content = content[1..];
                    }
                }
            }
            if(result.Length == pattern.Length) {
                Console.ResetColor();
                Console.WriteLine();
                return result.ToString();
            }
        }
        return string.Empty;
    }

    /// <summary>
    /// Le um numero real do usuario.
    /// </summary>
    /// <param name="prompt"></param>
    /// <param name="allowNegative"></param>
    /// <returns></returns>
    public static double ReadDouble(string prompt = "", bool allowNegative = false, float? defaultValue = null) {
        bool ok = false;
        Console.Write(prompt);
        string content = "";
        float result = default;
        if (defaultValue is not null) {
            content = defaultValue.Value.ToString();
        }

        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write(content);
        ConsoleKeyInfo key;
        do {
            string error = "";
            key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.Enter) {
                bool canParse = float.TryParse(content, out result);

                if (!canParse) {
                    error = "Valor inválido";
                }

                if (!allowNegative && result < 0) {
                    error = "Valor inválido. Digite um número positivo.";
                }

                ok = canParse && error == "";
            } else if (key.Key == ConsoleKey.Backspace && content.Length > 0) {
                content = content[..^1];
            } else {
                if ((key.KeyChar >= '0' && key.KeyChar <= '9')
                    || (key.KeyChar == '.' || key.KeyChar == ',')) {
                    content += key.KeyChar;
                }
            }

            Console.CursorLeft = prompt.Length;
            Console.Write(new string(' ', 10));
            Console.CursorLeft = prompt.Length;
            Console.ForegroundColor = error == "" ? ConsoleColor.Blue : ConsoleColor.Red;
            Console.Write(content);
            if (error != "") {
                Console.Write(" ");
                Console.Write(error);
            }
            Console.ResetColor();
        } while (!ok);
        Console.WriteLine();

        return result;
    }

    /// <summary>
    /// Representa um intervalo de inteiros. Inclusivo em ambos os lados
    /// </summary>
    public sealed class Range {
        public int Start { get; set; }
        public int End { get; set; }

        /// <summary>
        /// Cria um intervalo de inteiros. Inclusivo em ambos os lados.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public Range(int start, int end){
            Start = start;
            End = end;
        }        

        public bool Contains(int value){
            return value >= Start && value <= End;
        }
    }

    public static void Print(string msg, ConsoleColor? color = null){
        color ??= Console.ForegroundColor;
        Console.ForegroundColor = color.Value;
        Console.WriteLine(msg);
        Console.ResetColor();
    }
   
}