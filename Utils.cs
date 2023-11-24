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
            var color1 = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            var input = Console.ReadLine();
            Console.ForegroundColor = color1;
            ok = int.TryParse(input, out int result);
            if(!ok){
                var color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Valor inválido. Digite novamente: ");
                Console.ForegroundColor = color;
                continue;
            }

            if(!allowNegative && result < 0){
                var color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Valor inválido. Digite um número positivo. Digite novamente: ");
                Console.ForegroundColor = color;
                ok = false;
                continue;
            }

            if(range is not null && !range.Contains(result)){
                var color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"Valor inválido. Digite um número entre {range.Start} e {range.End}. Digite novamente: ");
                Console.ForegroundColor = color;
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
            var color1 = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            var input = Console.ReadLine() ?? "";
            Console.ForegroundColor = color1;
            if (!allowEmpty && string.IsNullOrWhiteSpace(input)) {
                var color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Valor inválido. Digite novamente: ");
                Console.ForegroundColor = color;
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
    /// <returns>A data lida</returns>
    public static DateOnly ReadDate(string prompt = "") {
        bool ok = false;
        Console.Write(prompt);
        while (!ok) {
            var color1 = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            var input = Console.ReadLine() ?? "";
            Console.ForegroundColor = color1;
            ok = DateOnly.TryParse(input, out var result);
            if (!ok) {
                var color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Valor inválido. Digite novamente: ");
                Console.ForegroundColor = color;
                continue;
            }

            return result;
        }
        return default;
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
            var color1 = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            var input = Console.ReadLine() ?? "";
            Console.ForegroundColor = color1;
            ok = double.TryParse(input, out var result);
            if (!ok) {
                var color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Valor inválido. Digite novamente: ");
                Console.ForegroundColor = color;
                continue;
            }

            if (!allowNegative && result < 0) {
                var color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Valor inválido. Digite um número positivo. Digite novamente: ");
                Console.ForegroundColor = color;
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
        Console.ForegroundColor = color.Value;
    }
}
