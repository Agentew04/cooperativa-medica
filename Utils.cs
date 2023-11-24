using MySqlX.XDevAPI.Common;
using System.Text;

namespace CoopMedica;

public static class Utils {
    /// <summary>
    /// Le e retorna um inteiro do usuario. Implementa validacao.
    /// </summary>
    /// <param name="prompt">Mensagem a ser exibida ao usuario</param>
    /// <param name="allowNegative">Se true, permite numeros negativos</param>
    /// <returns>O inteiro lido</returns>
    public static int ReadInt(string prompt = "", bool allowNegative = false, Range? range = null){
        bool ok = false;
        Console.Write(prompt);
        while(!ok){
            Console.ForegroundColor = ConsoleColor.Blue;
            var input = Console.ReadLine();
            Console.ResetColor();
            ok = int.TryParse(input, out int result);
            if(!ok){
                var color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Valor inválido. Digite novamente: ");
                Console.ResetColor();
                continue;
            }

            if(!allowNegative && result < 0){
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Valor inválido. Digite um número positivo. Digite novamente: ");
                Console.ResetColor();
                ok = false;
                continue;
            }

            if(range is not null && !range.Contains(result)){
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"Valor inválido. Digite um número entre {range.Start} e {range.End}. Digite novamente: ");
                Console.ResetColor();
                ok = false;
                continue;
            }
            return result;
        }
        return -1;
    }

    /// <summary>
    /// Le uma string do usuario. Implementa validacao para strings vazias.
    /// </summary>
    /// <param name="prompt">O prompt a ser mostrado antes</param>
    /// <param name="allowEmpty">Se o usuario pode inserir strings vazias</param>
    /// <returns>A string lida</returns>
    public static string ReadString(string prompt = "", bool allowEmpty = false) {
        bool ok = false;
        Console.Write(prompt);
        while (!ok) {
            Console.ForegroundColor = ConsoleColor.Blue;
            var input = Console.ReadLine() ?? "";
            Console.ResetColor();
            if (!allowEmpty && string.IsNullOrWhiteSpace(input)) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Valor inválido. Digite novamente: ");
                Console.ResetColor();
                ok = false;
                continue;
            }

            return input;
        }
        return "";
    }
    
    /// <summary>
    /// Le uma data do usuario.
    /// </summary>
    /// <param name="prompt">O prompt a ser mostrado</param>
    /// <param name="validation">Uma funcao de validacao para o input</param>
    /// <returns>A data lida</returns>
    public static DateOnly ReadDate(string prompt = "", Predicate<DateOnly>? validation = null) {
        bool ok = false;
        Console.Write(prompt);
        string pattern = "  /  /    ";
        string content = "";
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

    public static string ReadMaskedString(string prompt, string pattern) {
        bool ok = false;
        Console.Write(prompt);
        string content = "";
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
    public static double ReadDouble(string prompt = "", bool allowNegative = false) {
        bool ok = false;
        Console.Write(prompt);
        while (!ok) {
            Console.ForegroundColor = ConsoleColor.Blue;
            var input = Console.ReadLine() ?? "";
            Console.ResetColor();
            ok = double.TryParse(input, out var result);
            if (!ok) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Valor inválido. Digite novamente: ");
                Console.ResetColor();
                continue;
            }

            if (!allowNegative && result < 0) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Valor inválido. Digite um número positivo. Digite novamente: ");
                Console.ResetColor();
                ok = false;
                continue;
            }

            return result;
        }
        return -1;

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
