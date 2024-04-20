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

            Console.WriteLine($"Is good Sudoku valid? {ValidateSudoku(goodSudoku1)}");
            Console.WriteLine($"Is good Sudoku valid? {ValidateSudoku(goodSudoku2)}");

            Console.WriteLine($"Is bad Sudoku valid? {!ValidateSudoku(badSudoku1)}");
            Console.WriteLine($"Is bad Sudoku valid? {!ValidateSudoku(badSudoku2)}");
        }

        static bool ValidateSudoku(int[][] puzzle)
        {
            if (puzzle == null)
            {
                throw new ArgumentNullException("Sudoku board can not be null.");
            }
            var sudoku = puzzle.ToList();

            int count = sudoku.Count();

            bool isValid = false;

            //Validate rows
            isValid = sudoku.All(r => r.Distinct().Count() == count);

            //Validate cols
            isValid = isValid && Enumerable.Range(0, count)
                .All(c =>
                    sudoku.Select(r => r.ElementAt(c)).Distinct().Count() == count);

            //Validate little squares
            //No need to check the little squares if rows or cols validation fails
            if (isValid)
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
                isValid = littleSqrs.All(lg => lg.Distinct().Count() == count);
            }

            return isValid;
        }
    }
}
