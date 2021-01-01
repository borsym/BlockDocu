﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using ZH.Model;

namespace ZH.ViewModel
{
    public class GameViewModel : ViewModelBase
    {
        private GameModel _model;
        public DelegateCommand NewGameCommand { get; private set; }
  

        public DelegateCommand ExitCommand { get; private set; }
        public ObservableCollection<ModelField> Fields { get; set; }
        public ObservableCollection<ModelField> Fields2 { get; set; }
        public Int32 GameStepCount { get { return _model.GameStepCount; } }
        public String GameTime { get { return TimeSpan.FromSeconds(_model.GameTime).ToString("g"); } }
        public Int32 GridSize { get; private set; }

        public event EventHandler<int> NewGame;
        public event EventHandler ExitGame;

        public GameViewModel(GameModel model)
        {
            GridSize = 4;
            // játék csatlakoztatása
            _model = model;
            _model.GameAdvanced += new EventHandler<ModelEventArgs>(Model_GameAdvanced);
            _model.GameOver += new EventHandler<ModelEventArgs>(Model_GameOver);
            _model.GameCreated += new EventHandler<ModelEventArgs>(Model_GameCreated);

            // parancsok kezelése
            NewGameCommand = new DelegateCommand(param => OnNewGame(GridSize));



            // játéktábla létrehozása
            Fields = new ObservableCollection<ModelField>();
            for (Int32 i = 0; i < _model.Table.Size; i++) // inicializáljuk a mezőket
            {
                for (Int32 j = 0; j < _model.Table.Size; j++)
                {
                    Fields.Add(new ModelField
                    {
                        Text = String.Empty,
                        X = i,
                        Y = j,
                        Number = i * _model.Table.Size + j,
                        StepCommand = new DelegateCommand(param => StepGame(Convert.ToInt32(param)))
                        
                    });
                }
            }

            Fields2 = new ObservableCollection<ModelField>();
            for (Int32 i = 0; i < 2; i++) // inicializáljuk a mezőket
            {
                for (Int32 j = 0; j < 2; j++)
                {
                    Fields2.Add(new ModelField
                    {
                        Text = String.Empty,
                        X = i,
                        Y = j,
                        Number = i * _model.Table.Size + j,
                       // StepCommand = new DelegateCommand(param => StepGame(Convert.ToInt32(param)))

                    });
                }
            }
            RefreshTable();
        }

        private void RefreshTable()
        {
            foreach (ModelField field in Fields) // inicializálni kell a mezőket is
            {
                //field.Text = !_model.Table.IsEmpty(field.X, field.Y) ? _model.Table[field.X, field.Y].ToString() : String.Empty;
                field.Type = _model.Table[field.X, field.Y];
            }

            foreach (ModelField field in Fields2) // inicializálni kell a mezőket is
            {
                //field.Text = !_model.TableForObj.IsEmpty(field.X, field.Y) ? _model.Table[field.X, field.Y].ToString() : String.Empty;
                field.Type = _model.TableForObj[field.X, field.Y];
            }

            OnPropertyChanged("GameTime");
        }

        private void StepGame(int index)
        {
            ModelField field = Fields[index];

            _model.Step(field.X, field.Y);
            RefreshTable();
            for(int i = 0; i < _model.Table.Size; i++)
            {
                for(int j = 0;  j < _model.Table.Size; j++) {
                    Debug.Write(_model.Table[i, j] + " ");
                }
                Debug.Write("\n");
            }
            //field.Text = _model.Table[field.X, field.Y] > 0 ? _model.Table[field.X, field.Y].ToString() : String.Empty; // visszaírjuk a szöveget
            OnPropertyChanged("GameStepCount"); // jelezzük a lépésszám változást
           // field.Type = _model.Table[field.X, field.Y];
            //field.Text = !_model.Table.IsEmpty(field.X, field.Y) ? _model.Table[field.X, field.Y].ToString() : String.Empty;
           
        }


       
        private void OnNewGame(int size)
        {
            Debug.Write("ramkattolt" + size +"\n");
            Fields.Clear();
            GridSize = size;
            OnPropertyChanged("GridSize");
            if (NewGame != null)
                NewGame(this, size);
            for (Int32 i = 0; i < size; i++) // inicializáljuk a mezőket
            {
                for (Int32 j = 0; j < size; j++)
                {
                    Fields.Add(new ModelField
                    {
                        Text = String.Empty,
                        X = i,
                        Y = j,
                        Number = i * size + j,
                        StepCommand = new DelegateCommand(param => StepGame(Convert.ToInt32(param)))
                    });
                }
            }
            RefreshTable();

        }

        private void Model_GameCreated(object sender, ModelEventArgs e)
        {
            RefreshTable();
        }

        private void Model_GameOver(object sender, ModelEventArgs e)
        {
            Debug.Write("vege");
        }

        private void Model_GameAdvanced(object sender, ModelEventArgs e)
        {
            OnPropertyChanged("GameTime");
        }
        private void OnExitGame()
        {
            if (ExitGame != null)
                ExitGame(this, EventArgs.Empty);
        }
    }
}
