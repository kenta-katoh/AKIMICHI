using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akimichi.Game
{
    public class PlayerData
    {
        public string Name { get; set; } = "";
        public int Weight { get; set; } = PlayerConst.InitWeight;
        public int DiceCount { get; set; } = 0;
        public int PlusCount {  get; set; } = 0;
        public int MinusCount {  get; set; } = 0;
        public int EventCount { get; set; } = 0;
        public int MoveValue { get; set; } = 0;
        public int PracticeCount { get; set; } = 0;

        /// <summary>
        /// 現在のレベル取得
        /// </summary>
        /// <returns></returns>
        public int GetLevel()
        {
            int result = 0;
            if(1 <= this.Weight &&  this.Weight <= 9) result = 0;
            else if(10 <= this.Weight && this.Weight <= 70) result = 1;
            else if (71 <= this.Weight && this.Weight <= 100) result = 2;
            else if (101 <= this.Weight && this.Weight <= 200) result = 3;
            else if (201 <= this.Weight && this.Weight <= 500) result = 4;
            else if (501 <= this.Weight && this.Weight <= 999) result = 5;
            else if (1000 <= this.Weight) result = 6;
            return result;
        }
    }
}
