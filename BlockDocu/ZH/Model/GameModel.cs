using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ZH.Model
{
    public class GameModel
    {
        private ModelTable _table;
        private ModelTable _tableForObj;
        private Int32 _gameStepCount;
        private Int32 _gameTime;
        public Int32 GameStepCount { get { return _gameStepCount; } }
        public Int32 GameTime { get { return _gameTime; } }
        public ModelTable Table { get { return _table; } }
        public ModelTable TableForObj { get { return _tableForObj; } }

        public Boolean IsGameOver { get { return (_gameTime == 99); } }

        public event EventHandler<ModelEventArgs> GameAdvanced;
        public event EventHandler<ModelEventArgs> GameOver;
        public event EventHandler<ModelEventArgs> GameCreated;

        public GameModel()
        {
            _table = new ModelTable();
            _tableForObj = new ModelTable();
        }

        public void NewGame(int size)
        {
            _table = new ModelTable(size);
            _tableForObj = new ModelTable(2);
            _gameStepCount = 0;
            _gameTime = 0;
            GenerateFields();
            OnGameCreated();
        }

        public void AdvanceTime()
        {
            if (IsGameOver)
                return;

            _gameTime++;
            OnGameAdvanced();

            if (_gameTime == 99) 
                OnGameOver(false);
        }
        /*
         1 0 -- 0 0 -- 1 0 -- 1 1
         1 0 -- 1 1 -- 1 1 -- 0 1
         */
        public void Step(Int32 x, Int32 y) // ha tikre mozog a pálya előre akkor hasznos és akkor paraméter se kell
        {
            if (IsGameOver) // ha már vége a játéknak, nem játszhatunk
                return;

            Debug.Write(_tableForObj.CurrentObjN() + "\n");
            Debug.Write(_table.OnlyWhite(x, y, _tableForObj.CurrentObjN()) + " ezaz only\n");
            switch (_tableForObj.CurrentObjN())
            {
                
                case 1:
                    if (x + 1 >= _table.Size || !_table.OnlyWhite(x,y, _tableForObj.CurrentObjN())) return;
                    _table.setFirst(x, y);
                    break;
                case 2:
                    if (y + 1 >= _table.Size || !_table.OnlyWhite(x, y, _tableForObj.CurrentObjN())) return;
                    _table.setSecond(x, y);
                    break;
                case 3:
                    if ((x + 1 >= _table.Size || y + 1 >= _table.Size) || !_table.OnlyWhite(x, y, _tableForObj.CurrentObjN())) return;
                    _table.setThird(x,y);
                    break;
                case 4:
                    if ((x + 1 >= _table.Size || y + 1 >= _table.Size) || !_table.OnlyWhite(x, y, _tableForObj.CurrentObjN())) return;
                    _table.setForth(x, y);
                    break;
            }
            Debug.Write("ujat generalok\n");
            _table.CheckRowClomn();
            _tableForObj.PutObj2x2();
            _gameStepCount = _table.CleardRowColmn * 4;
            Debug.Write(_tableForObj.CurrentObjN() + "\n");
             // lépésszám növelés

            OnGameAdvanced();
        }

        private void GenerateFields()
        {
            for(Int32 i = 0; i < _table.Size; i++)
            {
                for (Int32 j = 0; j < _table.Size; j++)
                {
                    if (i < 2 && j < 2) _tableForObj.SetValue(i, j, 0);
                    _table.SetValue(i, j, 0);
                }
            }

            _tableForObj.PutObj2x2();

            
        }

        private void OnGameAdvanced()
        {
            if (GameAdvanced != null)
                GameAdvanced(this, new ModelEventArgs(false, _gameStepCount, _gameTime));
        }
        private void OnGameOver(Boolean isWon)
        {
            if (GameOver != null)
                GameOver(this, new ModelEventArgs(isWon, _gameStepCount, _gameTime));
        }
        private void OnGameCreated()
        {
            if (GameCreated != null)
                GameCreated(this, new ModelEventArgs(false, _gameStepCount, _gameTime));
        }
    }
}
