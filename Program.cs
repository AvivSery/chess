using System;

namespace chess
{
    class Program
    {

        static void Main(string[] args)
        {
            string[,] tools = new string[10, 10];
            tools = setBoard(tools);

            string[,] tools2 = new string[10, 10];//מערך לבדיקות
   
            string[][,] n = new string[1][,];//מערך לבדיקת לוח חוזר 3 פעמים
            n[0] = toolsForCheking(tools);
                        
            bool tie = false;
            bool move;
            bool whiteCoronation, blackCoronation;
            bool shahVsBlack, shahVsWhite;
            bool whiteMistake, blackMistake;
            bool math;
            bool hatzraha = false;
            bool wr1Moved = false, wr8Moved = false, br1Moved=false, br8Moved=false,
                wkMoved=false, bkMoved=false;
            bool goingRight;
            int PionBlackRow = 0;
            int PionBlackColumn = 0;
            bool optionToHakhaVsBlack = false, optionToHakhaVsWhite = false;
            bool attendToHakhaVsBlack, attendToHakhaVsWhite;
            int PionWhiteRow = 0, PionWhiteColumn = 0;
            string messageRow = "whice row do you want to move your object ? ";
            string messageColumn = "whice column do you want to move your object ?";
            int row, row2, column, column2;
            int count = 0;
            int countTill50 = 0;
            bool noPlayerCanDoMath ;
            bool sameBoardThreeTimes ;
            Tools tool;

            printBoard(tools);

            while (!tie)
            {

                Console.WriteLine("{0} turn, from " + messageRow
                    , count % 2 == 0 ? "White" : "Black");//בחירת משבצת מוצא
                row = chekRowInput(Console.ReadLine());
                Console.WriteLine("From " + messageColumn);
                column = changeColumn(Console.ReadLine());
                move = playOnlyWithYourTools(tools[row, column], count);//בדיקה                            
                if (!move)
                {
                    Console.WriteLine("You can't touce your opponent tools");
                    Console.WriteLine();
                    continue;
                }

                move = chekEmptySpace(tools[row, column]);//בדיקה
                if (!move)
                {
                    Console.WriteLine("You can not pick an empty space");
                    Console.WriteLine();
                    continue;
                }

                tool = new Tools(tools[row, column], row, column);

                Console.WriteLine("To " + messageRow); //בחירת משבצת תנועה
                row2 = chekRowInput(Console.ReadLine());
                Console.WriteLine("To " + messageColumn);
                column2 = changeColumn(Console.ReadLine());


                move = dontEatYoyrslef(tools[row, column], tools[row2, column2]);//בדיקה
                if (!move)
                {
                    Console.WriteLine("You can't eat yourself");
                    continue;
                }


                if (tools[row, column][1] == 'K' && row == row2 && (column2 == column + 2 ||
                    column2 == column - 2))//האם נעשה ניסיון להצרחה?
                    hatzraha = true;

                if (tools[row, column] == "WP" && row2 == PionBlackRow &&
                    column2 == PionBlackColumn && optionToHakhaVsBlack)
                    attendToHakhaVsBlack = true;//האם נעשה ניסיון להכאה דרך הילוכו?
                else
                    attendToHakhaVsBlack = false;

                if (tools[row, column] == "BP" && row2 == PionWhiteRow &&
                    column2 == PionWhiteColumn && optionToHakhaVsWhite)
                    attendToHakhaVsWhite = true;//האם נעשה ניסיון להכאה דרך הילוכו?
                else
                    attendToHakhaVsWhite = false;


                if (!hatzraha && !attendToHakhaVsBlack && !attendToHakhaVsWhite)
                {//רק אם אין ניסיון  להצרחה או הכאה דרך הילוכו יש לבדוק את תזוזת הכלי
                    
                    move = tool.moveTool(row, column, row2, column2, tools);//האם הכלי זז כחוק
                    if (!move)
                    {
                        Console.WriteLine("Illegal move ");
                        continue;
                    }
                }

                if (count % 2 == 0)//האם הלבן מכניס את עצמו לשח או לא יוצא משח?
                {
                    tools2 = toolsForCheking(tools);
                    tools2 = changeBoard(tools2, row, column, row2, column2);
                    whiteMistake = isThereShah(tools2, "WK", count);
                    if (whiteMistake)
                    {
                        Console.WriteLine("You can't get your self in to a Shah spot !");
                        continue;
                    }
                }

                else if (count % 2 != 0)//האם השחור מכניס את עצמו לשח או לא יוצא משח?
                {
                    tools2 = toolsForCheking(tools);
                    tools2 = changeBoard(tools2, row, column, row2, column2);
                    blackMistake = isThereShah(tools2, "BK", count);
                    if (blackMistake)
                    {
                        Console.WriteLine("You can't get your self in to a Shah spot !");
                        continue;
                    }
                }

                if (attendToHakhaVsBlack)//אכילת כלי שחור בהכאה דרך הילוכו
                    tools[row2 - 1, column2] = "--";


                if (attendToHakhaVsWhite)//אכילת כלי לבן בהכאה דרך הילוכו
                    tools[row2 + 1, column2] = "--";


                goingRight = column < column2 ? true : false; //משתנה עזר להצרחה


                if (hatzraha)//האם ההצרחה אפשרית בכלל
                {
                    bool king = count % 2 == 0 ? wkMoved : bkMoved;
                    bool r ;
                    if (goingRight)
                         r = count % 2 == 0 ? wr8Moved : br8Moved;
                    else
                        r = count % 2 == 0 ? wr1Moved : br1Moved;

                    hatzraha = isHatzrahaPossible(tools, row, column, row2, column2, king,
                        r, count);

                    if (!hatzraha)
                    {
                        Console.WriteLine("Illegal move");
                        continue;
                    }
                    if (hatzraha)
                    { 
                        
                        tools = changeBoard(tools, row, column, row2, column2);//מזיז מלך
                                                                       
                        if (count % 2 == 0 && !wkMoved)//עדכון שהמלך זז
                            wkMoved = haveBeenMoved("WK", tools[row2, column2]);

                        else if (count % 2 != 0 && !bkMoved)//עדכון שהמלך זז
                            bkMoved = haveBeenMoved("BK", tools[row2, column2]);

                        if (goingRight)//עדכון טורים כדי שיזוז גם הצריח
                        {
                            column = 8;
                            column2 = 5;
                        }
                        else
                        {
                            column = 1;
                            column2 = 3;
                        }
                    }
                }


                if (tools[row2, column2] != "--" || tools[row, column][1] == 'P')//ספירת 50 מסעים
                    countTill50 = 0;
                else
                    countTill50++;


                tools = changeBoard(tools, row, column, row2, column2);//עידכון לוח והזזת כלי               

                //בשלב זה הצעד עבר את כל הבדיקות ובוצע


                string[][,] m;
                m = new string[n.Length+1][,];//מערך שאוגר בתוכו את כל הלוחות

                for (int i = 0; i < n.Length ; i++)
                {
                    m[i] = toolsForCheking(n[i]);
                }
                m[n.Length] = toolsForCheking(tools);

                n = new string[m.Length][,];

                for (int i = 0; i < m.Length ; i++)//מערך נוסף שאוגר בתוכו את כל הלוחות
                {
                    n[i] = toolsForCheking(m[i]);
                }
                Console.WriteLine(m.Length);

                sameBoardThreeTimes = sameBoard3Times(m,count);//האם אותו לוח מופיע פעם שלישית?
                if (sameBoardThreeTimes)
                {
                    printBoard(tools);
                    Console.WriteLine("Same board 3 times, its a tie");
                    break;
                }


                if (countTill50 == 50)//50 מסעים ללא הכאה או תזוזת רגלי המשחק נגמר בתיקו
                {
                    printBoard(tools);
                    Console.WriteLine("Its a tie (50)");
                    break;
                }
                                   
                optionToHakhaVsBlack = false;//ביטול אופציה להכאה דרך הילוכו
                optionToHakhaVsWhite = false;//ביטול אופציה להכאה דרך הילוכו                


                if (count % 2 == 0 && !wkMoved)//האם צריחים ומלכים זזו
                    wkMoved = haveBeenMoved("WK", tools[row2, column2]);

                if (count % 2 != 0 && !bkMoved)
                    bkMoved = haveBeenMoved("BK", tools[row2, column2]);

                if (column == 1 && !wr1Moved)
                    wr1Moved = haveBeenMoved("WR", tools[row2, column2]);

                if (column == 1 && !br1Moved)
                    br1Moved = haveBeenMoved("BR", tools[row2, column2]);

                if (column == 8 && !wr8Moved)
                    wr8Moved = haveBeenMoved("WR", tools[row2, column2]);

                if (column == 8 && !br8Moved)
                    br8Moved = haveBeenMoved("BR", tools[row2, column2]);


                whiteCoronation = chekinCoronationWhite(tools);//הכתרה לבנה
                if (whiteCoronation)
                {
                    WPion wp = new WPion("WP", row2, column2);
                    tools[row2, column2] = wp.coronationWhite();
                }

                blackCoronation = chekinCoronationBlack(tools);//הכתרה שחורה
                if (blackCoronation)
                {
                    BPion bp = new BPion("BP", row2, column2);
                    tools[row2, column2] = bp.coronationBlack();
                }


                noPlayerCanDoMath = noPlayerCanDoMath1(tools);
                if (noPlayerCanDoMath)//תיקו - אף שחקן אינו יכול לבצע מט
                {
                    noPlayerCanDoMath = noPlayerCanDoMath2(tools);
                    if (noPlayerCanDoMath)
                    {
                        printBoard(tools);
                        Console.WriteLine("Its a tie, no player can do math");
                        break;
                    }
                }

                printBoard(tools);//הדפסת לוח


                count++;


                shahVsWhite = isThereShah(tools, "WK", count);//בדיקת שח נגד לבן
                if (shahVsWhite)
                {
                    math = isThereMath(tools, count);//מט
                    if (math)
                    {
                        Console.WriteLine("Game Over, Black Wins !");
                        break;
                    }
                    else
                        Console.WriteLine("Shah vs white king");
                }

                shahVsBlack = isThereShah(tools, "BK", count);//בדיקת שח נגד שחור
                if (shahVsBlack)
                {
                    math = isThereMath(tools, count);//מט
                    if (math)
                    {
                        Console.WriteLine("Game Over, White Wins !");
                        break;
                    }
                    else
                        Console.WriteLine("Shah vs black king");
                }

                tie = isThereMath(tools, count);//תיקו פט
                if (tie)
                {
                    Console.WriteLine("Game over, its a tie (pat)");
                    break;
                }

                hatzraha = false;

                if(tools[row2,column2] == "BP" && row2 - row == -2)//האם יש אופציה להכאה דרך הילוכו?
                {
                    PionBlackRow = row - 1;
                    PionBlackColumn = column ;
                    optionToHakhaVsBlack = true;
                }
                if (tools[row2, column2] == "WP" && row2 - row == 2)//האם יש אופציה להכאה דרך הילוכו?
                {
                    PionWhiteRow = row + 1;
                    PionWhiteColumn = column;
                    optionToHakhaVsWhite = true;
                }
            }
        }


        public static bool sameBoard3Times(string[][,] m,int count)
        {
            bool areTheSame = true;
            int SameBoards = 0;
           
            for (int i = 0; i < m.Length; i++)
            {
                for (int j = 0; j < m.Length; j++)
                {
                    if (i == j)
                        continue;
                    if (count % 2 == 0 && (i % 2 == 0 || j % 2 == 0))
                        continue;
                    if (count % 2 != 0 && (i % 2 != 0 || j % 2 != 0))
                        continue;

                    for (int x = 0; x < 10; x++)
                    {
                        for (int y = 0; y < 10; y++)
                        {
                            if (m[i][x, y] != m[j][x, y])
                                areTheSame = false;
                        }                 
                    }
                    if (areTheSame)
                        SameBoards++;
                    areTheSame = true;                   
                }
                if (SameBoards == 2)
                    return true;
                SameBoards = 0;
            }

            return false;
        }
        public static bool isHatzrahaPossible(string[,] tools, int row, int column, int row2,
            int column2, bool k, bool r, int count)
        {                      
            if (column2> column)
            {
                if (findShah(tools, row, column + 1, count))
                    return false;
                if (findShah(tools, row, column + 2, count))
                    return false;
                if (findShah(tools, row, column + 3, count))
                    return false;

                if (!k && !r && tools[row, column + 1] == "--" && tools[row, column + 2] == "--"
                    && tools[row, column + 3] == "--")
                    return true;
            }
            else if (column2 < column)
            {
                if (findShah(tools, row, column - 1, count))
                    return false;
                if (findShah(tools, row, column - 2, count))
                    return false;

                if (!k && !r && tools[row, column -1] == "--" && tools[row, column -2] == "--")
                    return true;
            }
            return false;
        }
        public static bool haveBeenMoved(string name, string name2)
        {
            if (name2 == name)
                return true;
            return false;
        }
        public static bool isThereMath(string[,] tools, int count)
        {
            string[,] tools2;
            bool shah = true ;
            bool move = true;
            string king = count % 2 == 0 ? "WK" : "BK";
            char jump = count % 2 == 0 ? 'B' : 'W';
            Tools tool;
            
            for(int i = 0;i<tools.GetLength(0); i++)
            {
                for(int j = 0; j<tools.GetLength(1); j++)
                {
                    for (int x = 0; x < tools.GetLength(0); x++)
                    {
                        for(int y = 0; y < tools.GetLength(1); y++)
                        {                            
                            move = chekEmptySpace(tools[i, j]);
                            if (!move)
                                continue;
                            tool = new Tools(tools[i, j], i, j);
                            move = dontEatYoyrslef(tools[i, j], tools[x, y]);
                            if (!move)
                                continue;
                            move = tool.moveTool(i, j, x, y, tools);
                            if (!move)
                                continue;
                            if (tools[i, j][0] == jump)
                                continue;
                            if (i == 0 || i == 9 || j == 0 || j == 9 ||
                                x == 0 || x == 9 || y == 0 || y == 9)
                                continue;
                            if (move)
                            {
                                tools2 = toolsForCheking(tools);//מערך חדש
                                tools2 = changeBoard(tools2, i, j, x, y);//שינוי המערך
                                shah = isThereShah(tools2, king, count);//האם יש שח נגד לבן עדיין?
                                if (!shah)
                                {
                                    return false;
                                }                                
                            }                       
                        }
                    }
                }
            }
            return true;
        }
        public static bool noPlayerCanDoMath2(string[,] tools)
        {
            int countBlackN = 0;
            int countBlackB = 0;
            int countWhiteN = 0;
            int countWhiteB = 0;

            for (int i = 0; i < tools.GetLength(0); i++)
            {
                for (int j = 0; j < tools.GetLength(1); j++)
                {
                    if (tools[i, j] == "BN")
                        countBlackN++;
                    if (tools[i, j] == "BB")
                        countBlackB++;
                    if (tools[i, j] == "WN")
                        countWhiteN++;
                    if (tools[i, j] == "WB")
                        countWhiteB++;
                }
            }
            if (countBlackN >= 2)
                return false;
            if (countBlackB >= 2)
                return false;
            if (countWhiteN >= 2)
                return false;
            if (countWhiteB >= 2)
                return false;

            return true;
        }
        public static bool noPlayerCanDoMath1(string[,] tools)
        {           
            for (int i = 0; i < tools.GetLength(0); i++)
            {
                for(int j = 0; j < tools.GetLength(1); j++)
                {
                    if (tools[i, j] == "WQ" || tools[i, j] == "BQ" || tools[i, j] == "WR" ||
                        tools[i, j] == "BR" || tools[i, j] == "WP" || tools[i, j] == "BP" )
                        return false;
                }
            }
            return true;
        }
        public static bool isThereShah(string[,] tools, string kingName, int count)
        {
            bool shah ;
            int kingRow, kingColumn;
            kingRow = whereIsKingRow(tools, kingName);
            kingColumn = whereIsKingColumn(tools, kingName);
            shah = findShah(tools, kingRow, kingColumn,count);
            return shah;
        }
        public static string[,] toolsForCheking(string[,] tools)
        {
            string[,] toolsForCheking = new string[10, 10];

            for(int i = 0; i<toolsForCheking.GetLength(0); i++)
            {
                for (int j =0; j < toolsForCheking.GetLength(1); j++)
                {
                    toolsForCheking[i, j] = tools[i, j];
                }
            }
            return toolsForCheking;
        }
        public static bool findShah(string[,] tools, int kingRow,
            int kingColumn, int count)
        {
            Tools tool;
            bool move = false;

            for(int i =0; i < tools.GetLength(0); i++)
            {
                for ( int j = 0; j < tools.GetLength(1); j++)
                {
                    move = playOnlyWithYourTools(tools[kingRow, kingColumn],count);
                    if (!move)
                       continue;
                    move = chekEmptySpace(tools[i, j]);
                    if (!move)
                        continue;
                    tool = new Tools(tools[i, j], i, j);
                    move = dontEatYoyrslef(tools[i, j], tools[kingRow, kingColumn]);
                    if (!move)
                        continue;
                    move = tool.moveTool(i, j, kingRow, kingColumn, tools);
                    if (!move)
                        continue;
                    move = playOnlyWithYourToolsShahCheking(tools[i, j], count);
                    if (!move)
                        continue;
                    if (move)
                        return move;
                }                
            }
            return move;
        }
        public static  bool playOnlyWithYourToolsShahCheking(string name, int count)
        {
            bool move = true;

            if ((count % 2 == 0 && name[0] == 'W') || (count % 2 != 0 && name[0] == 'B'))
                move = false;
            return move;
        }
        public static int whereIsKingRow(string[,] tools,string name)
        {
            int KingLine = 0;
            for(int i =0;i<tools.GetLength(0); i++)
            {
                for(int j=0 ;j< tools.GetLength(1); j++)
                {
                    if (tools[i, j] == name)
                    {
                        KingLine = i;
                        break;
                    }                    
                }
            }
            return KingLine;
        }
        public static int whereIsKingColumn(string[,] tools, string name)
        {
            int KingRow = 0;
            for (int i = 0; i < tools.GetLength(0); i++)
            {
                for (int j = 0; j < tools.GetLength(1); j++)
                {
                    if (tools[i, j] == name)
                    {
                        KingRow = j;
                        break;
                    }                    
                }
            }
            return KingRow;
        }
        public static bool chekinCoronationBlack(string[,] tools)
        {
            bool coronation = false;

            for (int i = 0; i <= 9; i++)
            {
                if (tools[1, i] == "BP")
                    coronation = true;
            }
            return coronation;
        }
        public static bool chekinCoronationWhite(string[,] tools)
        {
            bool coronation = false;

            for(int i = 0; i<=9; i++)
            {
                if (tools[8, i] == "WP")
                    coronation = true;
            }
            return coronation;
        }
        public static bool dontEatYoyrslef(string first, string second)
        {
            bool move = true;

            if (first[0] == second[0])
            {                
                move = false;
            }
            return move;
        }
        static bool playOnlyWithYourTools (string space, int count)
        {
            bool move = true;

            if ((count % 2 == 0 && space[0] == 'B') || (count % 2 != 0 && space[0] == 'W'))
                move = false;
                return move;
        }
        static bool chekEmptySpace(string space)
        {
            bool move = true ;
            if (space == "--")
                move = false;
                return move;                         
        }
        static string [,] changeBoard(string[,] tools, int row, int column, int row2,
            int column2)
        {
            tools[row2, column2] = tools[row, column];
            tools[row, column] = "--";
            return tools;
        }
        static int changeColumn(string letter)
        {
            letter = letter.Trim();
            for (int x = 0; x<1; x++)
            {
                switch (letter)
                {
                    case "A": case "a": return 1; case "B": case "b": return 2;
                    case "C": case "c": return 3; case "D": case "d": return 4;
                    case "E": case "e": return 5; case "F": case "f": return 6;
                    case "G": case "g": return 7; case "H": case "h": return 8;
                    default: Console.WriteLine("Illegal input, please try again");
                        letter = Console.ReadLine(); x--;  break;
                }
            }
            return 0;          
        }
        static int chekRowInput(string input)
        {
            input = input.Trim();
            bool rowInput = false;
            while (!rowInput)
            {
                switch (input)
                {
                    case "1":case "2":case "3":case "4":case "5":case "6":case "7":
                    case "8": rowInput = true; break;
                    default: Console.WriteLine("Illegal input, please try again");
                        input = Console.ReadLine(); break;
                }
            }
            return int.Parse(input);
        }
        static void printBoard (string[,] tools )
        {
            for (int i =0; i<tools.GetLength(0); i++)
            {
                for (int j = 0; j<tools.GetLength(1); j++)
                {
                    Console.Write(tools[i, j] + (i == 0||i==9 ? "   ": "  "));
                }
                Console.WriteLine();
                Console.WriteLine();
            }            
        }
        static string[,] setBoard(string[,] tools)
        {                      
            for (int i=0; i < tools.GetLength(0); i++)
            {
                for (int j=0; j < tools.GetLength(1); j++)
                {
                    if (i == 0 || i == 9)
                        tools[i, j] = returnChar(j) + "";
                    else if (j == 0 || j == 9)
                        tools[i, j] = (i  + "");
                    else if (i == 1)
                        tools[i, j] = returnWhiteTools(j) + "";
                    else if (i == 2)
                        tools[i, j] = "WP";
                    else if (i == 7)
                        tools[i, j] = "BP";
                    else if (i == 8)
                        tools[i, j] = returnBlackTools(j) + "";
                    else
                        tools[i, j] = "--";
                }                
            }
                                              
            string[,] toolss = new string[10, 10]
            { {" ", "A",  "B",   "C",   "D",   "E",   "F",   "G",   "H", " " },
            { "1", "--",  "--",  "WK",  "--",  "--",  "--",  "--",  "--", "1" },
            { "2", "--",  "--",  "--",  "--",  "--",  "--",  "--",  "--", "2" },
            { "3", "--",  "--",  "--",  "--",  "--",  "--",  "--",  "--", "3" },
            { "4", "--",  "WB",  "--",  "WN",  "--",  "--",  "--",  "--", "4" },
            { "5", "BP",  "--",  "--",  "--",  "--",  "--",  "--",  "--", "5" },
            { "6", "--",  "--",  "BR",  "--",  "--",  "--",  "--",  "--", "6" },
            { "7", "--",  "--",  "--",  "--",  "--",  "--",  "--",  "--", "7" },
            { "8", "BB",  "--",  "BB",  "--",  "--",  "BK",  "--",  "--", "8" },
            { " ", "A",  "B",  "C",  "D",  "E",  "F",  "G",  "H", " " },
            };
                                                
            return tools;            
        }
        static string returnBlackTools(int j)
        {
            string blackhTool = " ";
            switch (j)
            {
                default: blackhTool = " "; break;
                case 1: case 8: blackhTool = "BR"; break;
                case 2: case 7: blackhTool = "BN"; break;
                case 3: case 6: blackhTool = "BB"; break;
                case 4: blackhTool = "BK"; break;
                case 5: blackhTool = "BQ"; break;
            }
            return blackhTool;
        }
        static char returnChar (int j)
        {
            char returnChar;
            switch (j)
            {
                default :returnChar = ' '; break; case 1: returnChar = 'A'; break;
                case 2: returnChar = 'B'; break;  case 3: returnChar = 'C'; break;
                case 4: returnChar = 'D'; break;  case 5: returnChar = 'E'; break;
                case 6: returnChar = 'F'; break;  case 7: returnChar = 'G'; break;
                case 8: returnChar = 'H'; break;
            }
            return returnChar;
        }
        static string returnWhiteTools(int j)
        {
            string whiteTool ;
            switch (j)
            {
                default: whiteTool = " "; break;
                case 1: case 8: whiteTool = "WR"; break;
                case 2: case 7: whiteTool = "WN"; break;
                case 3: case 6: whiteTool = "WB"; break;
                case 4: whiteTool = "WK"; break; case 5: whiteTool = "WQ"; break;
            }
            return whiteTool;
        }
    }
    interface ITool
    {
        public bool moveTool(Tools tool,int row, int column, int row2, int column2, string[,] tools);
    }
    class Tools
    {
        protected string name;
        protected int row;
        protected int column;
        public Tools(string name, int row , int column)
        {
            this.name = name;
            this.row = row;
            this.column = column;
        }

        public bool moveTool(int row, int column, int row2, int column2, string[,] tools)
        {
            bool move = true;
            switch (tools[row, column])
            {
                case "WP": WPion wp = new WPion("WP", row2, column2);
                   move =  wp.moveTool(wp,row,column, row2, column2, tools); break;

                case "BP": BPion bp = new BPion("BP", row2, column2);
                    move = bp.moveTool(bp,row,column, row2, column2, tools); break;

                case "BR" : case "WR": R r = new R("R", row2, column2);
                    move = r.moveTool(r, row, column, row2, column2, tools); break;

                case "BN": case "WN":N n = new N("N", row2, column2);
                    move = n.moveTool(n, row, column, row2, column2, tools); break;

                case "BB":case "WB": B b = new B("B", row2, column2);
                    move = b.moveTool(b, row, column, row2, column2, tools); break;

                 case "BK":case "WK": K k = new K("K", row2, column2);
                   move = k.moveTool(k, row, column, row2, column2, tools); break;

                case "BQ": case "WQ": Q q = new Q("Q", row2, column2);
                    move = q.moveTool(q, row, column, row2, column2, tools); break;

                case "--":case " ":case "A":case "B": case "C":case "D":case "E":
                case "F":case "G":case "H":case "1":case "2":case "3":case "4":
                case "5":case "6": case "7":case "8":case "9":

                    Empty e = new Empty("--", row2, column2);
                    move = e.moveTool(e, row, column, row2, column2, tools); break;

            }            
            return move;
        }  
        
        public bool chekMove(int row, int column, int row2, int column2,
         string[,] tools, bool move)//נועד למחלקת מלכה וצריח
        {
            bool move2 = move;
            int biggest;
            int smallest;

            if (row == row2)
            {
                biggest = column > column2 ? column : column2;
                smallest = column2 > column ? column : column2;
                smallest++;
                while (biggest > smallest)
                {
                    if (tools[row, smallest] != "--")
                    {
                        move2 = false;
                        return move2;
                    }
                    smallest++;
                }
            }
            else if (column == column2)
            {
                biggest = row > row2 ? row : row2;
                smallest = row2 > row ? row : row2;
                smallest++;
                while (biggest > smallest)
                {
                    if (tools[smallest, column] != "--")
                    {
                        move2 = false;
                        return move2;
                    }
                    smallest++;
                }
            }
            return move2;
        }        
    }
    class WPion: Tools, ITool
    {
        public WPion(string name ,int row, int column) : base(name, row, column){}
        public string coronationWhite()
        {
            string input;
            Console.WriteLine("Whice tool do you want to get ? ");
            input = Console.ReadLine();
            input = input.Trim();
            bool llegalInput = false;

            while (!llegalInput )
            {
                switch (input)
                {
                    case "WR":case "WN":case "WB":case "WQ":
                    case "wr":case "wn":case "wb":case "wq":
                        llegalInput = true; break;

                    default: Console.WriteLine("Try again");
                        input = Console.ReadLine().Trim(); break;
                }
            }
            input = input.ToUpper();
            return input;
        }
        public bool moveTool(Tools tool,int row, int column, int row2, int column2, string [,] tools)
        {
            bool move = false;

            if ((column == column2 && row2 - row == 1 && tools[row2,column] == "--")
                || (column == column2 && row == 2 && row2 == 4 && tools[row2, column] == "--"
                && tools[3,column] == "--"))
                move = true;
            else if (column2 == column - 1 && row2 == row + 1//מאפשר אלכסון
                 && tools[row2, column2][0] == 'B')
                move = true;
            else if (column2 == column + 1 && row2 == row + 1//מאפשר אלכסון
                && tools[row2, column2][0] == 'B')
                move = true;
            else if (tools[row2, column2] != "--")
                move = false;
                
            return move ;
        }
    }
    class BPion : Tools, ITool
    {
        public BPion(string name,int row,int column) : base(name, row, column) { }

        public string coronationBlack()
        {
            string input;
            Console.WriteLine("Whice tool do you want to get ? ");
            input = Console.ReadLine();
            input = input.Trim();
            bool llegalInput = false;

            while (!llegalInput)
            {
                switch (input)
                {
                    case "BR":case "BN":case "BB":case "BQ":
                    case "br":case "bn":case "bb":case "bq":
                        llegalInput = true; break;

                    default: Console.WriteLine("Try again");
                        input = Console.ReadLine().Trim(); break;
                }
            }
            input = input.ToUpper();
            return input;
        }

        public bool moveTool(Tools tool,int row, int column, int row2, int column2, string[,] tools)
        {
            bool move = false;

            if ((column == column2 && row - row2 == 1 && tools[row2, column] == "--")
                || (column == column2 && row == 7 && row2 == 5 && tools[row2, column] == "--"
                && tools[6, column] == "--"))
                move = true;
            else if (column2 == column - 1 && row2 == row - 1
                && tools[row2, column2][0] == 'W')
                move = true;
            else if (column2 == column + 1 && row2 == row - 1
                && tools[row2, column2][0] == 'W')
                move = true;
            else if (tools[row2, column2] != "--")
                move = false;          
            return move;
        }
    }
    class R : Tools, ITool
    {
        public R(string name, int row, int column) : base(name, row, column) { }

        public bool moveTool(Tools tool, int row, int column, int row2, int column2,
            string[,] tools)
        {
            bool move = false;
            
            if ((row2 != row && column2 == column) || (row2 == row && column2 != column))
                move = true;

            move = chekMove(row, column, row2, column2, tools,move);//בדיקה נוספת לצריח

            return move;
        }       
    } 
    class N :Tools, ITool
    {
        public N (string name, int row, int column): base(name, row, column) { }

        public bool moveTool(Tools tool, int row, int column, int row2, int column2,
            string[,] tools)
        {
            bool move = false;

            if (row2 == row-1 && (column2 == column - 2 || column2 == column + 2))
                move = true; 
            if (row2 == row+1 && (column2 == column - 2 || column2 == column + 2))
                move = true; 
            if (row2 == row-2 && (column2 == column - 1 || column2 == column + 1))
                move = true; 
            if (row2 == row+2 && (column2 == column - 1 || column2 == column + 1))
                move = true;

            return move;
        }
    }
    class B : Tools, ITool
    {
        public B(string name, int row, int column ): base(name, row, column) { }

        public bool moveTool(Tools tool, int row, int column, int row2, int column2,
            string[,] tools)
        {
            bool move = false;

            int x = row - row2;
            int y = column - column2;
            if (x < 0)
                x = -x;
            if (y < 0)
                y = -y;
            if (y == x)
                move = true;

            while (row != row2)
            {
                if (row2 < row)
                    row2++;
                else
                    row2--;
                if (column2 < column)
                    column2++;
                else
                    column2--;
                if (tools[row2, column2] != "--" && tools[row, column]
                    != tools[row2, column2])
                    move = false;
            }            
            return move;
        }
    }
    class K :Tools, ITool
    {
        public K(string name, int row, int column) : base(name, row, column) { }       
        public bool moveTool(Tools tool, int row, int column, int row2, int column2,
            string[,] tools)
        {
            bool move = false;
            int x = row2 - row;
            int y = column2 - column;

            if (x > -2 && x < 2 && y > -2 && y < 2)
                move = true;
            
            return move;
        }
    }
    class Q:Tools,ITool
    {
        public Q(string name, int row, int column) : base(name, row, column) { }

        public bool moveTool(Tools tool, int row, int column, int row2, int column2,
            string[,] tools)
        {
            bool move = false;

            if (row == row2 || column == column2)//תנועת צריח
                move = true;

            int x = row - row2;//תנועת רץ
            int y = column - column2;
            if (x < 0)
                x = -x;
            if (y < 0)
                y = -y;
            if (y == x)
                move = true;

            while (row != row2 && column != column2)
            {
                if (row2 < row)
                    row2++;
                else
                    row2--;
                if (column2 < column)
                    column2++;
                else
                    column2--;
                if (tools[row2, column2] != "--" && tools[row, column]
                    != tools[row2, column2])
                {
                    move = false;
                    return move;
                }
            }
            move = chekMove(row, column, row2, column2, tools, move);//בדיקה נוספת למלכה
            return move;             
        }
        
    }   
    class Empty : Tools , ITool
    {
        public Empty(string name, int row, int column):base (name,row,column){}

        public bool moveTool(Tools tool, int row, int column, int row2, int column2,
            string[,] tools)
        {
            return false;
        }
    }
}
