using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ZH.Model
{
    public class ModelTable
    {
        private Int32[,] _fieldValues;
        private Int32 CurrentObjNumber;
        private Int32 _cleardRowColmn;
        public Int32 Size { get { return _fieldValues.GetLength(0); } }
        public Int32 this[Int32 x, Int32 y] { get { return GetValue(x, y); } }
        public Int32 CleardRowColmn { get { return _cleardRowColmn; } }
        public ModelTable() : this(4) { }
        public ModelTable(Int32 tableSize)
        {
            if (tableSize < 0)
                throw new ArgumentOutOfRangeException("The table size is less than 0.", "tableSize");
            _cleardRowColmn = 0;
            _fieldValues = new Int32[tableSize, tableSize];
        }
        public Boolean IsEmpty(Int32 x, Int32 y)
        {
            if (x < 0 || x >= _fieldValues.GetLength(0))
                throw new ArgumentOutOfRangeException("x", "The X coordinate is out of range.");
            if (y < 0 || y >= _fieldValues.GetLength(1))
                throw new ArgumentOutOfRangeException("y", "The Y coordinate is out of range.");

            return _fieldValues[x, y] == 0;
        }
        public Int32 GetValue(Int32 x, Int32 y)
        {
            if (x < 0 || x >= _fieldValues.GetLength(0))
                throw new ArgumentOutOfRangeException("x", "The X coordinate is out of range.");
            if (y < 0 || y >= _fieldValues.GetLength(1))
                throw new ArgumentOutOfRangeException("y", "The Y coordinate is out of range.");

            return _fieldValues[x, y];
        }
        public void SetValue(Int32 x, Int32 y, Int32 value) //érték felülírás
        {
            if (x < 0 || x >= _fieldValues.GetLength(0))
                throw new ArgumentOutOfRangeException("x", "The X coordinate is out of range.");
            if (y < 0 || y >= _fieldValues.GetLength(1))
                throw new ArgumentOutOfRangeException("y", "The Y coordinate is out of range.");
           
            _fieldValues[x, y] = value;
        }

        public void StepValue(Int32 x, Int32 y) // ha csak egy irányba kell elküldeni
        {
            if (x < 0 || x >= _fieldValues.GetLength(0))
                throw new ArgumentOutOfRangeException("x", "The X coordinate is out of range.");
            if (y < 0 || y >= _fieldValues.GetLength(1))
                throw new ArgumentOutOfRangeException("y", "The Y coordinate is out of range.");


        }
        /*
        1 0 -- 0 0 -- 1 0 -- 1 1
        1 0 -- 1 1 -- 1 1 -- 0 1
        */
        public bool OnlyWhite(int x, int y, int currObj)
        {
            Debug.Write("ez az onlyWhite belseje " + x + 1 + " " + y + 1 + "\n");
            switch(currObj)
            {
                case 1:
                    if (x + 1 < Size)
                        if(_fieldValues[x, y] == 0 && _fieldValues[x + 1, y] == 0) return true;
                    break;
                case 2:
                    if (y + 1 < Size)
                        if(_fieldValues[x, y] == 0 && _fieldValues[x, y + 1] == 0) return true;
                    break;
                case 3:
                    if (x + 1 < Size && y + 1 < Size)
                        if(_fieldValues[x, y] == 0 && _fieldValues[x + 1, y + 1] == 0 && _fieldValues[x + 1, y] == 0) return true;
                    break;
                case 4:
                    if (x + 1 < Size  && y + 1 < Size)
                        if(_fieldValues[x, y] == 0 && _fieldValues[x + 1, y + 1] == 0 && _fieldValues[x, y + 1] == 0) return true;
                    break;
            }
            return false;
        }

        public void CheckRowClomn()
        {
            for(int i = 0; i < Size; i++)
            {
                Boolean isFullColumn = true;
                Boolean isFullRow = true;
                for (int j = 0; j < Size; j++)
                {
                    if (_fieldValues[i, j] == 0) isFullColumn = false;
                    if (_fieldValues[j, i] == 0) isFullRow = false;

                }
                
                if(isFullColumn)
                {
                    for (int k = 0; k < Size; k++)
                    {
                        _fieldValues[i, k] = 0;
                    }
                    _cleardRowColmn++;
                }
                if (isFullRow)
                {
                    for (int k = 0; k < Size; k++)
                    {
                        _fieldValues[k, i] = 0;
                    }
                    _cleardRowColmn++;
                }
            }
        }

        public int CurrentObjN()
        {
            return CurrentObjNumber;
        }
        public void PutObj2x2()
        {
            Clear();
            Random rand = new Random();
            double random = rand.NextDouble();
            if (random <= 0.25)
            {
                CurrentObjNumber = 1;
                First();
            }
            else if(random <= 0.5)
            {
                CurrentObjNumber = 2;
                Second();
            }
            else if (random <= 0.75)
            {
                CurrentObjNumber = 3;
                Third();
            }
            else
            {
                CurrentObjNumber = 4;
                Forth();
            }

        }
       
        private void Clear()
        {
            _fieldValues[0, 0] = 0;
            _fieldValues[0, 1] = 0;
            _fieldValues[1, 0] = 0;
            _fieldValues[1, 1] = 0;
        }
        public void setFirst(int x, int y)
        {
            _fieldValues[x, y] = 1;
            _fieldValues[x + 1, y] = 1;
        }
        private void First()
        {
            _fieldValues[0, 0] = 1;
            _fieldValues[0, 1] = 0;
            _fieldValues[1, 0] = 1;
            _fieldValues[1, 1] = 0;
        }
        public void setSecond(int x, int y)
        {
            _fieldValues[x, y] = 1;
            _fieldValues[x, y + 1] = 1;
        }
        private void Second()
        {
            _fieldValues[0, 0] = 0;
            _fieldValues[0, 1] = 0;
            _fieldValues[1, 0] = 1;
            _fieldValues[1, 1] = 1;
        }
        public void setThird(int x, int y)
        {
            _fieldValues[x, y] = 1;
            _fieldValues[x + 1, y] = 1;
            _fieldValues[x + 1, y + 1] = 1;
        }
        private void Third()
        {
            _fieldValues[0, 0] = 1;
            _fieldValues[0, 1] = 0;
            _fieldValues[1, 0] = 1;
            _fieldValues[1, 1] = 1;
        }
        public void setForth(int x, int y)
        {
            _fieldValues[x, y] = 1;
            _fieldValues[x, y + 1] = 1;
            _fieldValues[x + 1, y + 1] = 1;
        }
        private void Forth()
        {
            _fieldValues[0, 0] = 1;
            _fieldValues[0, 1] = 1;
            _fieldValues[1, 0] = 0;
            _fieldValues[1, 1] = 1;
        }
    }
}
