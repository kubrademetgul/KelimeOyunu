using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kelime_Oyunu
{
    class GameLogic
    {
        public Boolean isWordRegular(List<String> wordList, String inputText)
        {
            bool isJokerUsing = false;
            String tempSortedText = String.Concat(inputText.OrderBy(i => i));
            String ruleWords = String.Join("",wordList.OrderBy(i => i).ToList());

            
            for (int c=0;c< tempSortedText.Length;c++)
            {
                if (ruleWords.IndexOf(tempSortedText[c])>-1)
                {
                    ruleWords = ruleWords.Remove(ruleWords.IndexOf(tempSortedText[c]),1);
                }
                else
                {
                    if (!isJokerUsing)
                    {
                        isJokerUsing = true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }


            return true;
        }

        public int calculatePoint(String word)
        {
            switch (word.Length)
            {
                
                case 3:
                    return 3;
                case 4:
                    return 4;
                case 5:
                    return 5;
                case 6:
                    return 7;
                case 7:
                    return 9;
                case 8:
                    return 11;
                case 9:
                    return 15;
                default:
                    return 0;
            }
        }
    }
}
