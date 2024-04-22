using System.Diagnostics;

namespace SudokuTest
{
    class SudokuPuzzleValidator
    {
        static void Main(string[] args)
        {
            int[][] goodSudoku1 = {
                new int[] {7,8,4,  1,5,9,  3,2,6},
                new int[] {5,3,9,  6,7,2,  8,4,1},
                new int[] {6,1,2,  4,3,8,  7,5,9},

                new int[] {9,2,8,  7,1,5,  4,6,3},
                new int[] {3,5,7,  8,4,6,  1,9,2},
                new int[] {4,6,1,  9,2,3,  5,8,7},

                new int[] {8,7,6,  3,9,4,  2,1,5},
                new int[] {2,4,3,  5,6,1,  9,7,8},
                new int[] {1,9,5,  2,8,7,  6,3,4}
            };


            int[][] goodSudoku2 = {
                new int[] {1,4, 2,3},
                new int[] {3,2, 4,1},

                new int[] {4,1, 3,2},
                new int[] {2,3, 1,4}
            };

            int[][] badSudoku1 =  {
                new int[] {1,2,3, 4,5,6, 7,8,9},
                new int[] {1,2,3, 4,5,6, 7,8,9},
                new int[] {1,2,3, 4,5,6, 7,8,9},

                new int[] {1,2,3, 4,5,6, 7,8,9},
                new int[] {1,2,3, 4,5,6, 7,8,9},
                new int[] {1,2,3, 4,5,6, 7,8,9},

                new int[] {1,2,3, 4,5,6, 7,8,9},
                new int[] {1,2,3, 4,5,6, 7,8,9},
                new int[] {1,2,3, 4,5,6, 7,8,9}
            };

            int[][] badSudoku2 = {
                new int[] {1,2,3,4,5},
                new int[] {1,2,3,4},
                new int[] {1,2,3,4},
                new int[] {1}
            };

            Console.WriteLine($"Is good Sudoku valid? {ValidateSudoku(goodSudoku1).Result}");
            Console.WriteLine($"Is good Sudoku valid? {ValidateSudoku(goodSudoku2).Result}");

            Console.WriteLine($"Is bad Sudoku valid? {!ValidateSudoku(badSudoku1).Result}");
            Console.WriteLine($"Is bad Sudoku valid? {!ValidateSudoku(badSudoku2).Result}");
        }

        static async Task<bool> ValidateSudoku(int[][] sudoku)
        {
            if (sudoku == null)
            {
                throw new ArgumentNullException("Sudoku board can not be null.");
            }
            
            int count = sudoku.Count();

            var isRowsValidTask = IsRowsValid(sudoku, count);
            var isColsValidTask = IsColsValid(sudoku, count);
            var isLittleSqrsValidTask = IsLittleSqrsValid(sudoku, count);

            await Task.WhenAll(isRowsValidTask, isColsValidTask, isLittleSqrsValidTask);

            return isRowsValidTask.Result && isColsValidTask.Result && isLittleSqrsValidTask.Result;
        }

        private static async Task<bool> IsRowsValid(int[][] sudoku, int count)
        {
            return await Task.Run(() =>
            {
                return sudoku.All(r => r.Where(c=>c > 0).Distinct().Count() == count);
            });
        }

        private static async Task<bool> IsColsValid(int[][] sudoku, int count)
        {
            return await Task.Run(() =>
            {
                return Enumerable.Range(0, count)
                .All(c =>
                    sudoku.Select(r => r.Where(c => c > 0).ElementAt(c)).Distinct().Count() == count);
            });
        }

        private static async Task<bool> IsLittleSqrsValid(int[][] sudoku, int count)
        {
            return await Task.Run(() =>
            {
                int littSqrSize = (int)Math.Sqrt(count);
                var littleSqrs = Enumerable.Range(0, littSqrSize)
                    .SelectMany(r =>
                        Enumerable.Range(0, littSqrSize)
                        .Select(c => sudoku
                                    .Skip(r * littSqrSize)
                                    .Take(littSqrSize)
                                    .SelectMany(lr => lr
                                                      .Skip(c * littSqrSize)
                                                      .Take(littSqrSize))
                                    )
                        );
                return littleSqrs.All(lg => lg.Where(c => c > 0).Distinct().Count() == count);
            });
        }
    }
}
