using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace CoopMedica.Menus; 
public class Table<T> {
    List<T> objects = new();
    List<Column> columns = new();


    public void RegisterColumns(params Column[] cols) {
        columns.AddRange(cols);
    }

    public Table<T> RegisterColumn(string name, Func<T, string> function) {
        columns.Add(new Column(name, function));
        return this;
    }
 
    public void AddRow(T row) {
        objects.Add(row);
    }

    public void AddRows(IEnumerable<T> rows) {
        objects.AddRange(rows);
    }

    public void DisplayTable() {
        Dictionary<Column, List<string>> values = EvaluateColumns();
        Dictionary<Column, int> widths = EvaluateWidths(values);
        int w = widths.Values.Sum() + widths.Count-1; // 1 is for the dots
        int h = objects.Count + 2; // 4 is for the title, the two separators and the bottom border

        Console.Clear(); // previne estouro do limite de linhas do console
        DrawWindow(widths, w, h);
        DrawColumns(widths, w, h);
        DrawData(values, widths, w, h);

        Console.SetCursorPosition(0,h+2);
    }

    private Dictionary<Column, List<string>> EvaluateColumns() {
        Dictionary<Column, List<string>> columnValues = new();
        // initialize lists
        foreach(var col in columns) {
            List<string> values = new();

            foreach(var obj in objects) {
                values.Add(col.Function(obj));
            }

            columnValues.Add(col, values);
        }
        return columnValues;
    }

    private Dictionary<Column, int> EvaluateWidths(Dictionary<Column, List<string>> columnValues) {
        Dictionary<Column, int> columnWidths = new();
        foreach(var col in columnValues.Keys) {
            int width = columnValues[col].Max(x => x.Length) + 2; // for the data padding; 1 each side
            width = Math.Max(width, col.Name.Length); // for the title padding; 1 each side
            columnWidths.Add(col, width);
        }
        return columnWidths;
    }

    private void DrawWindow(Dictionary<Column, int> widths, int w, int h) {
        Console.SetCursorPosition(0, 0);
        //draw top
        Console.Write("╔");
        Console.Write(new string('═', w));
        Console.Write("╗");

        // draw sides
        for (int i = 0; i < h; i++) {
            Console.SetCursorPosition(0, i + 1);
            Console.Write("║");
            Console.SetCursorPosition(w + 1, i + 1);
            Console.Write("║");
        }

        // draw first separator on line 3
        Console.SetCursorPosition(0, 2);
        Console.Write("╠");
        Console.Write(new string('═', w));
        Console.Write("╣");

        // draw bottom
        Console.SetCursorPosition(0, h + 1);
        Console.Write("╚");
        Console.Write(new string('═', w));
        Console.Write("╝");

    }

    private void DrawColumns(Dictionary<Column, int> widths, int w, int h) {
        // foreach column we draw its separator and its title
        int currentLeft = 1;
        for(int i=0; i<columns.Count; i++) { 
            Column col = columns[i];
            int left = currentLeft+widths[col];

            if(i != columns.Count - 1) {
                // draw header
                Console.SetCursorPosition(left, 0);
                Console.Write("╦");
                Console.SetCursorPosition(left, 1);
                Console.Write("║");
                Console.SetCursorPosition(left, 2);
                Console.Write("╬");

                // loop to draw the vertical line
                for (int j = 0; j < h - 2; j++) {
                    Console.SetCursorPosition(left, j + 3);
                    Console.Write("║");
                }

                Console.SetCursorPosition(left, h+1);
                Console.Write('╩');

            }

            currentLeft += widths[col] + 1;

            // draw title
            int leftPad = (int)Math.Floor((widths[col] - (float)col.Name.Length) / 2.0f);
            int rightPad = (int)Math.Ceiling((widths[col] - (float)col.Name.Length) / 2.0f);

            string paddedTitle = new string(' ', leftPad) + col.Name + new string(' ', Math.Max(0,rightPad-1));
            Console.SetCursorPosition(currentLeft - widths[col]-1, 1);
            Console.Write(paddedTitle);
        }
    }
    
    private void DrawData(Dictionary<Column, List<string>> values, Dictionary<Column, int> widths, int w, int h) {
        int currentLeft = 1;
        foreach(var col in columns) {
            var vals = values[col];
            int colWidth = widths[col];
            //draw data
            for (int i = 0; i < vals.Count; i++) {
                Console.SetCursorPosition(currentLeft, i + 3);
                int leftPad = (int)Math.Floor((colWidth - (float)vals[i].Length) / 2.0f);
                int rightPad = (int)Math.Ceiling((colWidth - (float)vals[i].Length) / 2.0f);
                string paddedVal = new string(' ', leftPad) + vals[i] + new string(' ', rightPad);
                Console.Write(paddedVal);
            }

            currentLeft += widths[col] + 1;
        }
    }

    public class Column {
        public string Name { get; set; }

        public Func<T, string> Function { get; set; }

        public Column(string name, Func<T, string> function) {
            Name = name;
            Function = function;
        }
    }
}
